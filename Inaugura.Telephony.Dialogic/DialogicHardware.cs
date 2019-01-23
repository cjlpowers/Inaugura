using System;
using System.Xml;

using Inaugura;
using Inaugura.Dialogic;
using Inaugura.Telephony;

namespace Inaugura.Telephony.Dialogic
{
	/// <summary>
	/// 
	/// </summary>
	public class DialogicHardware : Inaugura.Telephony.TelephonyHardware
	{
		#region Variables
		private Inaugura.Dialogic.DialogicHardware mHardware;
		#endregion

		#region Properties
		public override string Name
		{
			get
			{
				return "Dialogic Hardware (VoiceAPI)";
			}
		}
		#endregion

		public override TelephonyLine[] Start()
		{
            mHardware = new Inaugura.Dialogic.DialogicHardware();
            DialogicLine[] lines = null;
			Inaugura.Dialogic.DialogicLine[] dialogicLines = null;

			try
			{
				if(!Inaugura.Dialogic.DialogicHardware.IsDialogicServiceStarted())
				{
					Log.AddLog("Starting Dialogic Service");
					Inaugura.Dialogic.DialogicHardware.StartDialogicService();
					Log.AddLog("Dialogic Service Started");
				}

				int numberOfBoards = Inaugura.Dialogic.DialogicHardware.GetBoardCount();
				Log.AddLog("Number of Boards: "+numberOfBoards.ToString());
			
				string channelNames = Inaugura.Dialogic.DialogicHardware.GetChannelNames();
				
				//string[] channels = Inaugura.Dialogic.DialogicHardware.ParseChannelNames(channelNames);
		
				/*
				dialogicLines = new Inaugura.Dialogic.DialogicLine[channels.Length];
				for(int i = 0; i < channels.Length; i++)
				{
					Log.AddLog("Creating Line: "+channels[i]);
					Inaugura.Dialogic.DialogicLine l = new Inaugura.Dialogic.DialogicLine(channels[i]);
					dialogicLines[i] = l;
				}
				*/		
				
				string[] channels = Inaugura.Dialogic.DialogicHardware.ParseChannelNames(channelNames);
		
				dialogicLines = new Inaugura.Dialogic.DialogicLine[channels.Length];
				for(int i = 0; i < channels.Length;i++)//channels.Length; i++)
				{
					Log.AddLog("Creating Line: "+channels[i]);
					Inaugura.Dialogic.DialogicLine l = new Inaugura.Dialogic.DialogicLine(channels[i]);
					dialogicLines[i] = l;
				}
						



				lines = new DialogicLine[dialogicLines.Length];
				for(int i = 0; i < dialogicLines.Length; i++)
				{	
					lines[i] = new DialogicLine(this,dialogicLines[i]);

					// TESTING
					//lines[i] = new DialogicFakeCallerIdLine(dialogicLines[i]);
				}
			}
			catch(Exception ex)
			{
				Log.AddLog(ex.Message);
				throw ex;
			}
			return lines;    			
		}	

		public override void Stop()
		{	
			/*
			if(Inaugura.Dialogic.DialogicHardware.IsDialogicServiceStarted())
			{
				Log.AddLog("Stopping Dialogic Service");
				Inaugura.Dialogic.DialogicHardware.StopDialogicService();
				Log.AddLog("Dialogic Service Stopped");
			}
			*/
		}

		public DialogicHardware()
		{
			//this.FileReferences.Add(new FileReference("OrbisTel.Telephony.Dialogic.dll", "TelephonyHardware", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));
			//this.FileReferences.Add(new FileReference("Inaugura.Dialogic.dll", "TelephonyHardware", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));
		}

		public DialogicHardware(XmlNode node) : base(node)
		{
		}

		public override string ToString()
		{
			return "Dialogic Hardware";
		}
	}
}
