using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Xml;

namespace Inaugura.Telephony.Emulation
{
	/// <summary>
	/// Summary description for WindowsLine.
	/// </summary>
	public class WindowsLine : Telephony.TelephonyLine
	{
		private delegate string StringDelegate();
		private WindowsLineDlg mDlg;
		protected XmlDocument mXmlDoc;
		protected XmlElement mElement;
		protected string mPromptPath = string.Empty;
		private string mTermDigits = string.Empty;
		private bool mTerminated = false;
		private string mTermDigit = "";

		public override int NumberOfDigitsInBuffer
		{
			get
			{
				return this.mDlg.DigitBuffer.CurrentBuffer.Length;
			}
		}

		
		internal WindowsLine(TelephonyHardware hardware, WindowsLineDlg dlg) : base(hardware,"EMU_LINE")
		{			
			mDlg = dlg; 
			mDlg.Line = this;		
			mXmlDoc = new XmlDocument();
			mElement = mXmlDoc.CreateElement("Call");
		}
		
		#region Telephony
		[DllImport("winmm.dll", EntryPoint="PlaySound",CharSet=CharSet.Auto)]
		private static extern int PlaySound(String pszSound, int hmod, int falgs);

		internal enum SND
		{
			SND_SYNC            = 0x0000  ,/* play synchronously (default) */
			SND_ASYNC           = 0x0001 , /* play asynchronously */
			SND_NODEFAULT       = 0x0002 , /* silence (!default) if sound not found */
			SND_MEMORY          = 0x0004 , /* pszSound points to a memory file */
			SND_LOOP            = 0x0008 , /* loop the sound until next sndPlaySound */
			SND_NOSTOP          = 0x0010 , /* don't stop any currently playing sound */
			SND_NOWAIT			= 0x00002000, /* don't wait if the driver is busy */
			SND_ALIAS			= 0x00010000,/* name is a registry alias */
			SND_ALIAS_ID		= 0x00110000, /* alias is a pre d ID */
			SND_FILENAME		= 0x00020000, /* name is file name */
			SND_RESOURCE		= 0x00040004, /* name is resource name or atom */
			SND_PURGE           = 0x0040,  /* purge non-static events for task */
			SND_APPLICATION     = 0x0080  /* look for application specific association */
		}

		public override void ClearDigitBuffer()
		{
			this.mDlg.DigitBuffer.Clear();			
			this.mTermDigits = string.Empty;
		}		

		public override void Dial(string phoneNumber)
		{
			Log.AddLog("Dialing: " + phoneNumber);
			//System.Windows.Forms.MessageBox.Show(phoneNumber);
		}	
		
		public override DialAnalysis Dial(string  phoneNumber, int numberOfRings)
		{
			Log.AddLog("Dialing (with Analysis): " + phoneNumber);

            DialAnalysisDlg dlg = new DialAnalysisDlg();
            dlg.ShowDialog();
            return dlg.Result;
		}


		public override string GetDigits(int maxDigits, TimeSpan timeOut, TimeSpan digitTimeOut, string termDigits, out string termDigit)
		{
			termDigit = string.Empty;
			this.mTermDigits = termDigits;
			System.Drawing.Color oldColor = this.mDlg.DigitBufferControl.BackColor;
			this.mDlg.DigitBufferControl.BackColor = System.Drawing.Color.Red;

			int digitsToGet = maxDigits;
			string allDigits = string.Empty;
			DateTime startDigitTime = DateTime.Now;
			DateTime lastDigitTime = DateTime.Now;            
			
			while(digitsToGet > 0)
			{
				if(!this.mDlg.OffHook)
				{
					this.mDlg.DigitBufferControl.BackColor = oldColor;
					throw new Telephony.HangupException("Caller Hang Up");
				}

				string digit = this.mDlg.DigitBuffer.GrabDigit();

				if(digit != string.Empty)
				{
					digitsToGet--;
					lastDigitTime = DateTime.Now;
					if(termDigits.IndexOf(digit) != -1)
					{
						termDigit = digit;
						break;
					}				
	
					allDigits+=digit;
				}
				else // no digit was pressed so check the digit time out
				{
					TimeSpan totalDigitSpan = DateTime.Now-lastDigitTime;
					if (digitTimeOut.TotalSeconds != 0 && totalDigitSpan.TotalSeconds > digitTimeOut.TotalSeconds)
						break;
				}

				TimeSpan totalTimeSpan = DateTime.Now-startDigitTime;
				if (timeOut.TotalSeconds != 0 && totalTimeSpan.TotalSeconds > timeOut.TotalSeconds)
					break;		
		
				Thread.Sleep(100);

			}
			
			this.mDlg.DigitBufferControl.BackColor = oldColor;
			LogInput(allDigits);
			return allDigits;
			
			/*

			DateTime startDigitTime = DateTime.Now;
			DateTime lastDigitTime = DateTime.Now;
			string lastBuffer = mDlg.DigitBuffer;
			string buffer = mDlg.DigitBuffer;
			
			this.mTermDigit = "";
			//this.mDlg.DigitBuffer = "";
                        
			do
			{	
				if(!this.mDlg.OffHook)
				{
					this.mDlg.DigitBufferControl.BackColor = oldColor;
					throw new Telephony.HangupException("Caller Hang Up");
				}

				// check 
				for(int i = 0; i < termDigits.Length; i++)
				{
					string buffer = mDlg.DigitBuffer;
					for(int b = 0; b < buffer.Length; b++)
					{
						if(buffer[b] == termDigits[i])						
						{
							termDigit = termDigits.Substring(i,1);
							this.mDlg.DigitBufferControl.BackColor = oldColor;
							return buffer;
						}
					}					
				}			

				if(lastBuffer != mDlg.DigitBuffer)
				{
					lastBuffer = mDlg.DigitBuffer;
					lastDigitTime = DateTime.Now;
				}
				
				TimeSpan totalTimeSpan = DateTime.Now-startDigitTime;				
				if(timeOutSec!= 0 && totalTimeSpan.TotalSeconds > timeOutSec)
					break;

				TimeSpan totalDigitSpan = DateTime.Now-lastDigitTime;
				if(digitTimeOutSec != 0 && totalDigitSpan.TotalSeconds > digitTimeOutSec)
					break;

				if(this.mDlg.DigitBuffer.Length == maxDigits)
				{
					string str = this.mDlg.DigitBuffer;
					this.mDlg.DigitBuffer = "";
					this.mDlg.DigitBufferControl.BackColor = oldColor;
					
					

					return str;					
				}
			}while(true);

			this.mDlg.DigitBufferControl.BackColor = oldColor;
			LogInput(this.mDlg.DigitBuffer);
			return this.mDlg.DigitBuffer;
			*/
		}
				

		public override void GiveDialTone()
		{			
		}
		
		public override void OffHook()
		{	
			this.mDlg.DigitBuffer.Clear();
			this.mDlg.OffHook = true;
		}
		
		public override void OnHook()
		{
			this.mDlg.OffHook = false;
			this.mDlg.DigitBuffer.Clear();
		}

		public override void OpenLine()
		{		
		}

		public override void CloseLine()
		{		
		}

		/*
		public override void PlayText(string voice, string text)
		{
			//OrbisSoftware.Speech.Engine.Speak(voice,text);
			OrbisSoftware.Speech.Engine.SaveWav(voice,text,this.Name+".wav");
			this.PlayFile(this.Name+".wav");
		}

		public override TelephonyResult PlayText(string voice, string text, bool terminate)
		{
			OrbisSoftware.Speech.Engine.SaveWav(voice,text,this.Name+".wav");
			return this.PlayFile(this.Name+".wav",terminate);
		}

		public override TelephonyResult PlayText(string voice, string text, string termDigits, ref string termDigit)
		{
			OrbisSoftware.Speech.Engine.SaveWav(voice,text,this.Name+".wav");
			return this.PlayFile(this.Name+".wav",termDigits,ref termDigit);			
		}
				public override string Reco()
		{
			this.PlayFile("Beep.wav");
			string str = "";
			
			str = OrbisSoftware.Speech.Engine.Reco();
						
			return str;	
		}

		
		public override string Reco(string grammerXml)
		{
			this.PlayFile("Beep.wav");
			string str = "";
			
			str = OrbisSoftware.Speech.Engine.Reco(grammerXml);
						
			return str;	
		}
		*/

		public override bool DetectRingback(TimeSpan timeOut)
		{
			return false;
		}


		public override void Record(string fileName, TimeSpan timeOut, TimeSpan silenceTimeOut, bool terminate)
		{
            System.Windows.Forms.MessageBox.Show(string.Format("Recording {0}", fileName));
		}

		public override void PlaySpecial(string fileName)
		{
            this.PlayFile(fileName);
		} 

		public override void PlayFile(string  fileName)
		{		
			this.PlayFile(fileName,false);			
		}

		public override TelephonyResult PlayFile(string fileName, bool terminate)
		{
			string str = "";
			if (terminate)
			{
				TelephonyResult result = this.PlayFile(fileName, "1234567890*#", out str);
				if (str != string.Empty)
				{
					this.mDlg.DigitBuffer.PlaceDigit(str);
				}
				return result;
			}
			else
			{
				this.PlayFile(fileName,string.Empty,out str);
				return TelephonyResult.Completed;
			}

		}	

		public override TelephonyResult PlayFile(string fileName, string termDigits, out string termDigit)
		{
			this.mTerminated = false;
			this.mTermDigits = termDigits;
			this.mTermDigit = "";

			if(!this.mDlg.OffHook)
			{
				throw new Telephony.HangupException("Caller Hang Up");
			}
			if(System.IO.File.Exists(this.mPromptPath+fileName))
			{
				PlaySound(this.mPromptPath+fileName,0,(int) (SND.SND_ASYNC | SND.SND_FILENAME | SND.SND_NOWAIT));

				while(this.IsSoundPlaying())
				{
					if(!this.mDlg.OffHook)
					{
						this.StopPlayingSound();
						throw new Telephony.HangupException("Caller Hang Up");
					}
				}
				
				termDigit = this.mTermDigit;
				
				if(this.mTerminated)
					return TelephonyResult.Terminated;
				else
                    return TelephonyResult.Completed;
			}
			else
				throw new ApplicationException("File "+this.mPromptPath+fileName+" did not exist");
		}	

		public override bool WaitRing(int numberOfRings, int waitTimer)
		{
			if(this.mDlg.OffHook)
				return true;

			for(int i = 0; i < waitTimer; i++)
			{
				Thread.Sleep(1000);

				if(this.mDlg.OffHook)
					return true;
			}
			return false;
		}

		protected void LogInput(string input)
		{
			XmlElement e = this.mXmlDoc.CreateElement("Input");
			e.InnerText = input;
			this.mElement.AppendChild(e);            
		}

		public override CallerID CallerID
		{
			get
			{

				string callerName = (string)this.mDlg.Invoke(new StringDelegate(this.mDlg.GetCallerName));
				string callerNumber = (string)this.mDlg.Invoke(new StringDelegate(this.mDlg.GetCallerNumber));

				CallerID id = new CallerID("OK",callerName,callerNumber,"",DateTime.Now,"");
				return id;
			}
		}

		public string CallXml
		{
			get
			{
				return this.mElement.OuterXml;				
			}
		}

		public string PromptPath
		{
			get
			{
				return mPromptPath;
			}
			set
			{
				mPromptPath = value;
			}
		}
		public override HookState HookState
		{
			get
			{
				if(this.mDlg.OffHook)
					return HookState.OffHook;
				else
					return HookState.OnHook;
			}
		}

		public void DigitPressed(string digit)
		{
			if(this.mTermDigits.IndexOf(digit)!= -1)
			{				
				this.mTerminated = true;			
				this.StopPlayingSound();
				this.mTermDigit = digit;
			}
			else
			{
				this.mDlg.DigitBuffer.PlaceDigit(digit);
			}
		}

		private void StopPlayingSound()
		{
			PlaySound(null,0,(int) (SND.SND_ASYNC | SND.SND_FILENAME | SND.SND_NOWAIT));
		}

		private bool IsSoundPlaying()
		{
			Thread.Sleep(500);
			int result = PlaySound(null,0,(int) (SND.SND_ASYNC | SND.SND_FILENAME | SND.SND_NOSTOP));		

			if(result == 1)
				return false;
			else
				return true;
		}
		#endregion

		
	}
}
