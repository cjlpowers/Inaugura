using System;
using System.Collections;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Interop.SpeechLib;

namespace Inaugura.Telephony.Emulator
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	internal class Engine
	{
		private string mRecoText = "";
		private Dlg mDlg;		
		static private Mutex mMutex = new Mutex();
		
		//static SpVoice sp = null;

		[DllImport("winmm.dll", EntryPoint="PlaySound",CharSet=CharSet.Auto)]
		private static extern int PlaySound(String pszSound, int hmod, int falgs);
		
		public static string[] VoiceNames
		{
			get
			{				
				ArrayList list = new ArrayList();
				SpVoiceClass sp = new SpVoiceClass();
				foreach(ISpeechObjectToken o in sp.GetVoices("",""))
				{
					list.Add(o.GetDescription(0));
				}

				string[] strList = new string[list.Count];
				for(int i = 0; i<list.Count; i++)
				{
					strList[i] = (string)list[i];
				}

				return strList;
			}
		}

		public Engine()
		{			
		}		

		protected static ISpObjectToken GetVoice(string voiceName)
		{
			SpVoiceClass sp = new SpVoiceClass();
			foreach(ISpeechObjectToken o in sp.GetVoices("",""))
			{
				if(o.GetDescription(0) == voiceName)
					return (ISpObjectToken)o;			
			}

			throw new ApplicationException("Voice: "+voiceName+" was not found");
		}

		public static void SaveWav(string voice, string text, string fileName)
		{
			//SpVoice sp = null;
			SpAudioFormat sf = null;			 
			SpWaveFormatExClass wf = null;
			SpFileStream SpFileStream = null;

			mMutex.WaitOne();
			SpVoice sp = null;

			try
			{	
				SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;			

				sp = new SpVoice();
				
				sp.Voice = (SpObjectToken)GetVoice(voice);							
		
				wf = new SpWaveFormatExClass();
				wf.BitsPerSample = 8;
				wf.Channels = 1;
				wf.SamplesPerSec = 11000;
				wf.BlockAlign = 1;
				wf.AvgBytesPerSec = 11000;
				wf.FormatTag = 0x0007;			
			
				sf = new SpAudioFormat();
				sf.SetWaveFormatEx(wf);

				SpeechStreamFileMode SpFileMode = SpeechStreamFileMode.SSFMCreateForWrite;			
				SpFileStream = new SpFileStream();
				SpFileStream.Format = sf;
				SpFileStream.Open(fileName, SpFileMode, false);

				sp.AllowAudioOutputFormatChangesOnNextSet = false;

				sp.AudioOutputStream = SpFileStream;
				sp.Speak(text, SpFlags);				
				sp.WaitUntilDone(-1);	
				sp.Speak(null,SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
				SpFileStream.Close();				
			}
			finally
			{
				sp = null;
				System.GC.Collect();
				mMutex.ReleaseMutex();
			}
		}
		
		public static void Speak(string voice, string text)
		{
			
			try
			{
				mMutex.WaitOne();

				SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;			
				SpVoice sp = new SpVoice();
				
				sp.Voice = (SpObjectToken)GetVoice(voice);

				sp.Speak(text,SpFlags);
				sp.WaitUntilDone(-1);
			}
			finally
			{
				// MUST DO GC TO SOLVE PROBLEM WITH AT&T NATURAL VOICES EATING UP MEMORY
				//System.GC.Collect();
				mMutex.ReleaseMutex();
			}
		}

		public static string Reco()
		{
			Engine eng = new Engine();
			eng.mDlg = new Dlg();				

			eng.RecoWav();

			eng.mDlg.Visible = false;
			eng.mDlg.ShowDialog();
			
			return eng.mRecoText;
		}

		public static string Reco(string xml)
		{
			Engine eng = new Engine();
			eng.mDlg = new Dlg();

			eng.RecoWav(xml);

			eng.mDlg.Visible = false;
			eng.mDlg.ShowDialog();
			
			return eng.mRecoText;
		}

		private void RecoWav()
		{				
			SpInprocRecognizer sr = new SpInprocRecognizer();
					
			SpInProcRecoContext sc = (SpInProcRecoContext)sr.CreateRecoContext();
			sc.Recognition += new _ISpeechRecoContextEvents_RecognitionEventHandler(this.OnRecog);
			sc.EndStream += new _ISpeechRecoContextEvents_EndStreamEventHandler(this.OnStreamEnd);
			sc.SoundEnd += new _ISpeechRecoContextEvents_SoundEndEventHandler(sc_SoundEnd);
			sc.SoundStart += new _ISpeechRecoContextEvents_SoundStartEventHandler(sc_SoundStart);

			ISpRecoGrammar myGrammer = (ISpRecoGrammar)sc.CreateGrammar(null);
			myGrammer.LoadDictation(null,SPLOADOPTIONS.SPLO_STATIC);	
			
			myGrammer.SetDictationState(SPRULESTATE.SPRS_ACTIVE);			
		}

		private void RecoWav(string xml)
		{				
			// save the cmd xml
			string xmlFileName = System.Guid.NewGuid().ToString()+".xml";
			System.IO.StreamWriter sw = System.IO.File.CreateText(xmlFileName);
			sw.Write(xml);
			sw.Close();

			//SpInprocRecognizer sr = new SpInprocRecognizer();
		
			//SpInProcRecoContext sc = (SpInProcRecoContext)sr.CreateRecoContext();
			SpSharedRecoContext sc = new SpSharedRecoContext();
			sc.Recognition += new _ISpeechRecoContextEvents_RecognitionEventHandler(this.OnRecog);
			sc.EndStream += new _ISpeechRecoContextEvents_EndStreamEventHandler(this.OnStreamEnd);
			sc.SoundEnd += new _ISpeechRecoContextEvents_SoundEndEventHandler(sc_SoundEnd);
			sc.SoundStart += new _ISpeechRecoContextEvents_SoundStartEventHandler(sc_SoundStart);
			sc.Hypothesis+=new _ISpeechRecoContextEvents_HypothesisEventHandler(sc_Hypothesis);
			sc.FalseRecognition +=new _ISpeechRecoContextEvents_FalseRecognitionEventHandler(sc_FalseRecognition);

			ISpRecoGrammar myGrammer = (ISpRecoGrammar)sc.CreateGrammar(null);
			myGrammer.LoadCmdFromFile(xmlFileName,SPLOADOPTIONS.SPLO_STATIC);

			myGrammer.SetRuleIdState(0,SPRULESTATE.SPRS_ACTIVE);			
			System.IO.File.Delete(xmlFileName);
		}

		

		private void InitReco(string xml)
		{	
			/*
			// save the cmd xml
			string xmlFileName = System.Guid.NewGuid().ToString()+".xml";
			System.IO.StreamWriter sw = System.IO.File.CreateText(xmlFileName);
			sw.Write(xml);
			sw.Close();

			SpInprocRecognizer sr = new SpInprocRecognizer();
			sr.AudioInputStream = SpFileStream;
		
			SpeechLib.ISpRecoContext sc = (ISpRecoContext)sr.CreateRecoContext();
			sc.SetInterest(SpeechLib.SPEVENTENUM.SPEI_PHRASE_START | SpeechLib.SPEVENTENUM.SPEI_RECOGNITION| SpeechLib.SPEI_FALSE_RECOGNITION),SpeechLib.SPEVENTENUM.SPEI_PHRASE_START | SpeechLib.SPEVENTENUM.SPEI_RECOGNITION| SpeechLib.SPEI_FALSE_RECOGNITION);
			sc.SetAudioOptions(SPAUDIOOPTIONS.SPAO_RETAIN_AUDIO,null,null);
			ISpRecoGrammar grammar;
			sc.CreateGrammar(0,out grammer);
			grammar.LoadDictation(null,SPLOADOPTIONS.SPLO_STATIC);

			ISpVoice voice;
			sc.GetVoice(out voice);
			sc.






			sc.Recognition += new _ISpeechRecoContextEvents_RecognitionEventHandler(this.OnRecog);
			sc.EndStream += new _ISpeechRecoContextEvents_EndStreamEventHandler(this.OnStreamEnd);
			
			ISpRecoGrammar myGrammer = (ISpRecoGrammar)sc.CreateGrammar(null);
			myGrammer.LoadCmdFromFile(xmlFileName,SPLOADOPTIONS.SPLO_STATIC);

			myGrammer.SetRuleIdState(0,SPRULESTATE.SPRS_ACTIVE);			
			mOpenStream = SpFileStream;
			System.IO.File.Delete(xmlFileName);
			*/
		}

		public static ISpVoice InitVoice()
		{	
			ISpVoice sp = new SpVoiceClass();
			return sp;
		}


		private void OnRecog(int i, object o,SpeechRecognitionType type, ISpeechRecoResult result)
		{
			this.mRecoText = result.PhraseInfo.GetText(0,result.PhraseInfo.Elements.Count,false);			
			this.mDlg.Close();
           	
		}

		private void OnStreamEnd(int StreamNumber, object StreamPosition, bool StreamReleased)
		{
			if(this.mDlg.Visible)
			{
				this.mDlg.Close();			
			}
		}

		private void sc_SoundEnd(int StreamNumber, object StreamPosition)
		{			
		}

		private void sc_SoundStart(int StreamNumber, object StreamPosition)
		{
		}

		private void sc_FalseRecognition(int StreamNumber, object StreamPosition, ISpeechRecoResult Result)
		{
			if(this.mDlg.Visible)
			{
				this.mDlg.Close();			
			}
		}

		private void sc_Hypothesis(int StreamNumber, object StreamPosition, ISpeechRecoResult Result)
		{
				
			OrbisTel.Telephony.Emulator.WindowsLineDlg.TextBox.Text = "Hypothesis: " + 
                Result.PhraseInfo.GetText(0, -1, true) + ", " +
                StreamNumber + ", " + StreamPosition;
		}
	}	
}

