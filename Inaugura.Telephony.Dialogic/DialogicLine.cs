using System;
using System.Xml;

using Inaugura.Dialogic;
using Inaugura.Telephony;


using System.Diagnostics;

namespace Inaugura.Telephony.Dialogic
{
	/// <summary>
	/// 
	/// </summary>
	public class DialogicLine : TelephonyLine
	{
		#region Variables
		protected Inaugura.Dialogic.DialogicLine mLine;
		#endregion

		#region Properties
		public override int NumberOfDigitsInBuffer
		{
			get
			{
				return this.mLine.NumberOfDigitsInBuffer;
			}
		}

		public override CallerID CallerID
		{
			get
			{
                try
                {
                    string callerIdXml = this.mLine.CallerId;

                    if (callerIdXml == null || callerIdXml == string.Empty)
                        return new CallerID("NOTSET", "", "", "", DateTime.Now, "");

                    //Log.AddLog(callerIdXml);

					XmlDocument xmlDoc = new XmlDocument();
					xmlDoc.LoadXml(mLine.CallerId);

					XmlElement node = (XmlElement)xmlDoc.FirstChild;

                    string status = "";
                    string frameType = "";
                    string name = "";
                    string number = "";
                    string dialedNumber = "";
                    string strDateTime = "";

					if (node != null)
					{
						DateTime dateTime = DateTime.Now;

						status = node.GetAttribute("status");
						frameType = node.GetAttribute("frameType");
						name = node.GetAttribute("sourceName");
						number = node.GetAttribute("sourceNumber");
						dialedNumber = node.GetAttribute("dialedNumber");
						strDateTime = node.GetAttribute("dateTime");

						if (strDateTime != string.Empty)
						{
							int month = int.Parse(strDateTime.Substring(0, 2));
							int day = int.Parse(strDateTime.Substring(2, 2));
							int hour = int.Parse(strDateTime.Substring(4, 2));
							int minute = int.Parse(strDateTime.Substring(6, 2));
							dateTime = new DateTime(dateTime.Year, month, day, hour, minute, 0, 0);
						}
						return new CallerID(status, name, number, dialedNumber, dateTime, frameType);
					}

					return null;

				}
                catch (DialogicException ex)
                {
					throw new TelephonyException(ex.Message,ex);
                }                
            }
		}	

		public override HookState HookState
		{
			get
			{
				try
				{
					if (this.mLine.GetHookState() == 1)
						return HookState.OnHook;
					else
						return HookState.OffHook;
				}
				catch (DialogicException ex)
				{
					throw new TelephonyException(ex.Message, ex);
				}
			}
		}
		#endregion

		#region Methods
        /// <summary>
        /// Clears all digits from the digit buffer
        /// </summary>
		public override void ClearDigitBuffer()
		{
			try
			{
                
                mLine.ClearDigitBuffer();
			}
			catch (DialogicException ex)
			{
				throw new TelephonyException(ex.Message, ex);
			}
		}
			
		public override void Dial(string  phoneNumber)
		{
			if(phoneNumber == null)
				throw new ArgumentNullException("phoneNumber","The phone number can not be null");

			try
			{
				mLine.Dial(phoneNumber);
			}
			catch(Inaugura.Dialogic.HangupException ex)
			{
				throw new HangupException(ex.Message,ex);
			}
			catch (DialogicException ex)
			{
				throw new TelephonyException(ex.Message, ex);
			}
		}

		public override DialAnalysis Dial(string  phoneNumber, int numberOfRings)
		{
			if (phoneNumber == null)
				throw new ArgumentNullException("phoneNumber", "The phone number can not be null");
			else if (numberOfRings <= 0)
				throw new ArgumentOutOfRangeException("numberOfRings", "numberOfRings must be greater then zero");

			CallAnalysis c = CallAnalysis.Error;

			try
			{
				c = mLine.Dial(phoneNumber,numberOfRings);
			}
			catch(Inaugura.Dialogic.HangupException ex)
			{
				throw new HangupException(ex.Message,ex);
			}
			catch (DialogicException ex)
			{
				throw new TelephonyException(ex.Message, ex);
			}

			switch(c)
			{
				case CallAnalysis.Busy:
					return DialAnalysis.Busy;
				case CallAnalysis.OperatorIntercept:
					return DialAnalysis.OperatorIntercept;				
				case CallAnalysis.CallConnected:
					return DialAnalysis.CallConnected;
				case CallAnalysis.Error:
					return DialAnalysis.Error;
				case CallAnalysis.FaxTone:
					return DialAnalysis.FaxTone;
				case CallAnalysis.NoAnswer:
					return DialAnalysis.NoAnswer;
				case CallAnalysis.NoDialTone:
					return DialAnalysis.NoDialTone;
				case CallAnalysis.NoRingBack:
					return DialAnalysis.NoRingBack;
				default:
					return DialAnalysis.Error;					
			}
		}

		#region GetDigits
		public override string GetDigits(int maxDigits, TimeSpan timeOut, TimeSpan digitTimeOut, string termDigits, out string termDigit)
		{			
			if (termDigits == null)
				termDigits = String.Empty;
			try
			{
				string termDigitTemp = string.Empty;
				string result = mLine.GetDigits(maxDigits, timeOut.TotalSeconds, termDigits, digitTimeOut.TotalSeconds, 0, ref termDigitTemp);
				termDigit = termDigitTemp;
				return result;
			}
			catch (Inaugura.Dialogic.HangupException ex)
			{
				throw new HangupException(ex.Message, ex);
			}
			catch (DialogicException ex)
			{
				throw new TelephonyException(ex.Message, ex);
			}
		}

		#endregion

		#region PlayFile
		public override void PlaySpecial(string fileName)
		{
			try
			{
				//Log.AddLog(string.Format("Playing(0): {0}", fileName));
				int result = mLine.PlaySpecial(fileName);
				Log.AddLog(string.Format("Result of playing file (with tone detection) = {0}", result));
			}
			catch (Inaugura.Dialogic.HangupException ex)
			{
				throw new HangupException(ex.Message, ex);
			}
			catch (DialogicException ex)
			{
				//Log.AddError(ex.ToString());
				throw new TelephonyException(string.Format("Error playing '{0}'", fileName), ex);
			}
			catch
			{
				//Log.AddError("Some other exception");
				throw new Exception("Some other exception");
			}
		}

		public override void PlayFile(string fileName)
		{
			try
			{
				//Log.AddLog(string.Format("Playing(0): {0}", fileName));
				mLine.PlayFile(fileName);
			}
			catch (Inaugura.Dialogic.HangupException ex)
			{
				//Log.AddError(ex.ToString());
				throw new HangupException(ex.Message, ex);
			}
			catch (DialogicException ex)
			{
				//Log.AddError(ex.ToString());
				throw new TelephonyException(string.Format("Error playing '{0}'", fileName), ex);
			}
			catch
			{
				//Log.AddError("Some other exception");
				throw new Exception("Some other exception");
			}
		}
		
		public override TelephonyResult PlayFile(string fileName, bool terminate)
		{
			try
			{
				//Log.AddLog(string.Format("Playing(1): {0}", fileName));
				if (terminate)
				{
					//Log.AddLog("Here");
					int result = mLine.PlayFile(fileName, "1234567890*#", true);
					//Log.AddLog(string.Format("Result: {0}",result));
					if (result == 1)
					{
						//Log.AddLog("Here 4");
						return TelephonyResult.Terminated;
					}
					else
					{
						//Log.AddLog("Here 5");
						return TelephonyResult.Completed;
					}
				}
				else
				{
					mLine.PlayFile(fileName);
					return TelephonyResult.Completed;
				}
			}
			catch (Inaugura.Dialogic.HangupException ex)
			{
				//Log.AddLog("Here 1");
				//Log.AddError(ex.ToString());
				throw new HangupException(ex.Message, ex);
			}
			catch (DialogicException ex)
			{
				//Log.AddLog("Here 2");
				//Log.AddError(ex.ToString());
				throw new TelephonyException(string.Format("Error playing '{0}'",fileName), ex);
			}
			catch
			{
				//Log.AddLog("Here 3");
				//Log.AddError("Some other exception");
				throw new Exception("Some other exception");
			}
		}

		public override TelephonyResult PlayFile(string fileName, string termDigits, out string termDigit)
		{
			try
			{
				termDigit = string.Empty;
				//Log.AddLog(string.Format("Playing(2): {0}", fileName));
				int result = mLine.PlayFile(fileName, termDigits, false);
				//Log.AddLog(string.Format("Result: {0}", result));
				if (result == 0) // no term digit
					return TelephonyResult.Completed;
				else // terminated because of a digit
				{
					string refTermDigit = string.Empty;
					mLine.GetDigits(mLine.NumberOfDigitsInBuffer, 0, termDigits, 0, 0, ref refTermDigit);
					termDigit = refTermDigit;
					return TelephonyResult.Terminated;
				}
			}
			catch (Inaugura.Dialogic.HangupException ex)
			{
				//Log.AddError(ex.ToString());
				throw new HangupException(ex.Message, ex);
			}
			catch (DialogicException ex)
			{
				//Log.AddError(ex.ToString());
				throw new TelephonyException(string.Format("Error playing '{0}'", fileName), ex);
			}
			catch
			{
				//Log.AddError("Some other exception");
				throw new Exception("Some other exception");
			}
		}	
		#endregion

		#region PlayText
		/*
		public override void PlayText(string voice, string text)
		{
			string fileName = this.Name;
			try
			{				
				Inaugura.Speech.Engine.SaveWav(voice,text,fileName+".wav");								
				mLine.PlayFile(fileName+".wav");				
			}
			catch(DialogicHangupException ex)
			{
				throw new HangupException(ex.Message);
			}			
		}

		public override TelephonyResult PlayText(string voice, string text, bool terminate)
		{
			string fileName = this.Name;
			try
			{				
				Inaugura.Speech.Engine.SaveWav(voice,text,fileName+".wav");				
				if(mLine.PlayFile(fileName+".wav","",terminate) == 1)
					return TelephonyResult.Completed;
				else
					return TelephonyResult.Terminated;
			}
			catch(DialogicHangupException ex)
			{
				throw new HangupException(ex.Message);
			}			
		}

		public override TelephonyResult PlayText(string voice, string text, string termDigits, ref string termDigit)
		{
			string fileName = this.Name;
			try
			{				
				Inaugura.Speech.Engine.SaveWav(voice,text,fileName+".wav");				
				if(mLine.PlayFile(fileName+".wav",termDigits,true) == 1)
					return TelephonyResult.Completed;
				else
				{
					termDigit = mLine.GetDigits(1,0,0);
					return TelephonyResult.Terminated;
				}
			}
			catch(DialogicHangupException ex)
			{
				throw new HangupException(ex.Message);
			}			
		}
		*/
		#endregion		

		public override void GiveDialTone()
		{
			mLine.GiveDialTone();
		}

		public override void OffHook()
		{
			if(this.HookState == HookState.OnHook)
				mLine.OffHook();
		}

		public override void OnHook()
		{
			if(this.HookState == HookState.OffHook)			
				mLine.OnHook();
		}

		public override void OpenLine()
		{	
			mLine.OpenLine();							
		}

		public override void CloseLine()
		{
			mLine.CloseLine();						
		}

		public override void Record(string fileName, TimeSpan timeOut, TimeSpan silenceTimeOut, bool terminate)
		{
			try
			{
				mLine.RecordWav(fileName,timeOut.TotalSeconds,silenceTimeOut.TotalSeconds,terminate);		
			}
			catch (Inaugura.Dialogic.HangupException ex)
			{
				throw new HangupException(ex.Message, ex);
			}
			catch (DialogicException ex)
			{
				throw new TelephonyException(ex.Message, ex);
			}
		}

		/*
		public override string Reco()
		{
			string str = "";
			try
			{
				this.Record(this.Name+".wav");
				str = Inaugura.Speech.Engine.Reco(this.Name+".wav");
				
			}
			catch (Inaugura.Dialogic.HangupException ex)
			{
				throw new HangupException(ex.Message, ex);
			}
			catch (DialogicException ex)
			{
				throw new TelephonyException(ex.Message, ex);
			}

			return str;			
		}
				
		public override string Reco(string grammerXml)
		{
			string str = "";
			try
			{
				this.Record(this.Name+".wav");
				str = Inaugura.Speech.Engine.Reco(this.Name+".wav",grammerXml);
				
			}
			catch (Inaugura.Dialogic.HangupException ex)
			{
				throw new HangupException(ex.Message, ex);
			}
			catch (DialogicException ex)
			{
				throw new TelephonyException(ex.Message, ex);
			}

			return str;			
		}
		*/



		public override bool WaitRing(int numberOfRings, int waitTimer)
		{
			return mLine.WaitRing(numberOfRings,waitTimer);
		}

		public DialogicLine(TelephonyHardware hardware, Inaugura.Dialogic.DialogicLine line) : base(hardware, line.ChannelName)
		{
			mLine = line;
		}

		public override bool DetectRingback(TimeSpan timeOut)
		{
			Log.AddLog(string.Format("Attempting to detect ringback for the next {0} seconds",timeOut.TotalSeconds));
			int result = mLine.DetectRingback(timeOut.TotalSeconds);
			if (result == (int)Inaugura.Dialogic.Termination.Tone)
			{
				Log.AddLog(string.Format("Result of ringback detection = Tone Detected", result));
				return true;
			}
			else
			{
				Log.AddLog(string.Format("Result of ringback detection = {0}", result));
				return false;
			}
		}
		#endregion				
	}
}
