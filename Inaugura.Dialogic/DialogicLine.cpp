// This is the main DLL file.

#include "stdafx.h"
#include "DialogicLegacy.h"
#include "DialogicLine.h"
#include "HangupException.h"
#include <stdio.h>

/*
 * Call Analysis termination type.
 */
#define CR_BUSY               7        /* Line busy */
#define CR_NOANS              8        /* No answer */
#define CR_NORB               9        /* No ringback */
#define CR_CNCT               10       /* Call connected */
#define CR_CEPT               11       /* Operator intercept */
#define CR_STOPD              12       /* Call analysis stopped */
#define CR_NODIALTONE         17       /* No dialtone detected */
#define CR_FAXTONE            18       /* Fax tone detected */
#define CR_ERROR              0x100    /* Call analysis error */

/*
 * Connection types ( ATDX_CONNTYPE() )
 */
#define CON_CAD               1  /* Cadence Break */
#define CON_LPC               2  /* Loop Current Drop */
#define CON_PVD               3  /* Positive Voice Detect */
#define CON_PAMD              4  /* Positive Answering Machine Detect */
#define CON_DIGITAL	

//#using <SpeechLib.dll>

//using namespace SpeechLib;

namespace Inaugura
{
namespace Dialogic
{	
	DialogicLine::DialogicLine(String __gc* channelName)
	{
		mpDialogicLine = new DialogicLegacy(channelName);		
	}

	DialogicLine::~DialogicLine(void)
	{
		delete mpDialogicLine;
	}

	// PROPERTIES 
	int DialogicLine::get_NumberOfDigitsInBuffer()
	{
		try
		{
			return mpDialogicLine->GetNumberOfDigitsInBuffer();
		}
		catch(char* str)
		{
			DialogicException __gc *ex = new DialogicException(str);
			throw ex;			
		}
	}

	String __gc* DialogicLine::get_ChannelName()
	{
		try
		{
			return (mpDialogicLine->ChannelName());
		}
		catch(char* str)
		{
			DialogicException __gc *ex = new DialogicException(str);
			throw ex;			
		}
	}

	int DialogicLine::GetHookState()
	{
		try
		{
			return mpDialogicLine->GetHookState();
		}
		catch(char* str)
		{
			DialogicException __gc *ex = new DialogicException(str);
			throw ex;			
		}
	}

	void DialogicLine::set_ChannelName(String __gc* channelName)
	{		
		try
		{
			mpDialogicLine->ChannelName(channelName);
		}
		catch(char* str)
		{
			DialogicException __gc *ex = new DialogicException(str);
			throw ex;			
		}
	}

	bool DialogicLine::get_EnableCallerId()
	{
		try
		{
			return mpDialogicLine->EnableCallerId();
		}
		catch(char* str)
		{
			DialogicException __gc *ex = new DialogicException(str);
			throw ex;			
		}
	}

	void DialogicLine::set_EnableCallerId(bool  enableCallerId)
	{	
		try
		{
			mpDialogicLine->EnableCallerId(enableCallerId);
		}
		catch(char* str)
		{
			DialogicException __gc *ex = new DialogicException(str);
			throw ex;			
		}
	}

	String __gc* DialogicLine::get_CallerId()
	{
		try
		{
			return (mpDialogicLine->GetCallerIdXml());
		}
		catch(char* str)
		{
			DialogicException __gc *ex = new DialogicException(str);
			throw ex;			
		}
	}

	// METHODS
	void DialogicLine::OpenLine()
	{
		try
		{
			mpDialogicLine->OpenLine();
		}
		catch(char* str)
		{
			DialogicException __gc *ex = new DialogicException(str);
			throw ex;			
		}
	}


	void DialogicLine::CloseLine()
	{
		try
		{
			mpDialogicLine->CloseLine();
		}
		catch(char* str)
		{
			DialogicException __gc *ex = new DialogicException(str);
			throw ex;			
		}
	}

	void DialogicLine::ClearDigitBuffer()
	{
		try
		{
			mpDialogicLine->ClearDigitBuffer();
		}
		catch(char* str)
		{
			DialogicException __gc *ex = new DialogicException(str);
			throw ex;			
		}
	}

	void DialogicLine::Dial(String __gc *phoneNumber)
	{
		try
		{
			mpDialogicLine->Dial(phoneNumber);
		}
		catch(char* str)
		{
			DialogicException __gc *ex = new DialogicException(str);
			throw ex;			
		}
	}

	CallAnalysis DialogicLine::Dial(String __gc *phoneNumber, int numberOfRings)
	{
		try
		{
			int cares = mpDialogicLine->Dial(phoneNumber,numberOfRings);
			switch(cares)
			{
				case CR_CNCT:				
					return CallAnalysis::CallConnected;
				case CR_CEPT:
					return CallAnalysis::OperatorIntercept;
				case CR_BUSY:
					return CallAnalysis::Busy;			
				case CR_FAXTONE:
					return CallAnalysis::FaxTone;			
				case CR_NODIALTONE:
					return CallAnalysis::NoDialTone;			
				case CR_STOPD:
					return CallAnalysis::CallAnalysisStopped;			
				case CR_NOANS:		
					return CallAnalysis::NoAnswer;			
				case CR_NORB:
					return CallAnalysis::NoRingBack;			
				case CR_ERROR:				
					return CallAnalysis::Error;			
			}				
		}
		catch(char* str)
		{
			DialogicException __gc *ex = new DialogicException(str);
			throw ex;			
		}
	}

	String __gc* DialogicLine::GetCallerId()
	{
		try
		{
			return mpDialogicLine->GetCallerIdXml();
		}
		catch(char* str)
		{
			DialogicException __gc *ex = new DialogicException(str);
			throw ex;			
		}
	}

	String __gc* DialogicLine::GetDigits(int maxDigits, double  timeOutSec, String __gc* termDigits, double digitTimeOutSec, double silenceTimeOutSec, String __gc* __gc* termDigit)
	{				
			int cppMaxDigits = maxDigits;
			CString cppTermDigits = termDigits;
			float cppTimeOutSec = (float)timeOutSec;
			float cppDigitTimeOutSec = (float)digitTimeOutSec;
			float cppSilenceTimeOutSec = (float)silenceTimeOutSec;
			CString cppTermDigit = (*termDigit);		
			
			String __gc* str = String::Empty;
			try
			{
				str = mpDialogicLine->GetDigits(cppMaxDigits,cppTimeOutSec,cppTermDigits,cppDigitTimeOutSec,cppSilenceTimeOutSec,cppTermDigit);
				
				(*termDigit) = cppTermDigit;
				return str;	
			}
			catch(char* exStr)
			{
				DialogicException __gc *ex = new DialogicException(exStr);
				throw ex;		
			}
			if(String::Compare(str,"HANGUP")==0)
				throw new HangupException("Hang Up Termination");		
	}

	String __gc* DialogicLine::GetDigits(int maxDigits, double  timeOutSec, String __gc* termDigits, double digitTimeOutSec)
	{		
		String __gc* returnString = new String("");
		String __gc* str = GetDigits(maxDigits,timeOutSec,termDigits,digitTimeOutSec,-1,&returnString);			
		return str;	
	}

	String __gc* DialogicLine::GetDigits(int maxDigits, double  timeOutSec, double digitTimeOutSec)
	{		
		String __gc* returnString = new String("");
		String __gc* str = GetDigits(maxDigits,timeOutSec,"",digitTimeOutSec,-1,&returnString);			
		return str;	
	}

	int DialogicLine::DetectRingback(double timeOutSec)
	{				
			float cppTimeOutSec = (float)timeOutSec;			
			int result = 0;
			try
			{
				result = mpDialogicLine->DetectRingback(cppTimeOutSec);
				return result;
			}
			catch(char* exStr)
			{
				DialogicException __gc *ex = new DialogicException(exStr);
				throw ex;		
			}
	}


	void DialogicLine::GiveDialTone()
	{
		try
		{
			mpDialogicLine->GiveDialTone();			
		}
		catch(char* str)
		{
			DialogicException __gc *ex = new DialogicException(str);
			throw ex;			
		}
	}

	void DialogicLine::OnHook()
	{
		try
		{
			mpDialogicLine->OnHook();			
		}
		catch(char* str)
		{
			DialogicException __gc *ex = new DialogicException(str);
			throw ex;			
		}
	}

	void DialogicLine::OffHook()
	{
		try
		{
			mpDialogicLine->OffHook();			
		}
		catch(char* str)
		{
			DialogicException __gc *ex = new DialogicException(str);
			throw ex;			
		}
	}

	int DialogicLine::PlaySpecial(String __gc* fileName)
	{
		CString str2 = fileName;
		int result = 0;
		try
		{
			result = mpDialogicLine->PlaySpecial(str2);			
		}
		catch(char* str)
		{
			DialogicException __gc *ex = new DialogicException(str);
			throw ex;			
		}			
		catch(Exception __gc *ex)
		{
			DialogicException __gc *dialogicEx = new DialogicException("Exception thrown by internal component",ex);
			throw dialogicEx;			
		}
		
		if(result == HANGUP)
			throw new HangupException("Hang Up Termination");
		else if(result == TONETERM)
			throw new HangupException("Hang Up via tone detection");
		
		return result;
	}

	int DialogicLine::PlayFile(String __gc* fileName, String __gc* termDigits, bool terminate)
	{
		CString str2 = fileName;
		int result = 0;
		try
		{
			result = mpDialogicLine->PlayFile(str2,termDigits,terminate?1:0);			
		}
		catch(char* str)
		{
			DialogicException __gc *ex = new DialogicException(str);
			throw ex;			
		}			
		catch(Exception __gc *ex)
		{
			DialogicException __gc *dialogicEx = new DialogicException("Exception thrown by internal component",ex);
			throw dialogicEx;			
		}
		
		if(result == HANGUP)
			throw new HangupException("Hang Up Termination");			
		else
		{
			if(result == DIGITTERM)
				return 1;
			else 
				return 0;
		}				
	}

	void DialogicLine::PlayFile(String __gc* fileName)
	{
		this->PlayFile(fileName,"",false);		
	}

	void DialogicLine::RecordWav(String __gc* fileName, double  timeOutSec, double  silenceTimeOutSec, bool terminate )
	{
		int result = 0;
		try
		{
			result = mpDialogicLine->RecordFile(fileName,timeOutSec,silenceTimeOutSec,terminate?1:0);
		}
		catch(char* str)
		{			
			throw new DialogicException(str);				
		}
		catch(Exception __gc *ex)
		{
			DialogicException __gc *dialogicEx = new DialogicException("Exception thrown by internal component",ex);
			throw dialogicEx;			
		}		
		if(result == HANGUP)
			throw new HangupException("Hang Up Termination");
	}

	/*
	int DialogicLine::PlayText(String __gc* text, String __gc* termDigits, bool terminate)
	{
		SpVoiceClass *sp = NULL;
		SpWaveFormatEx *wf = NULL;
		SpAudioFormatClass *sf = NULL;
		SpMemoryStreamClass *buffer = NULL;

		try
		{
			SpVoiceClass *sp = new SpVoiceClass();			
			SpWaveFormatExClass *wf = new SpWaveFormatExClass();			
			wf->set_BitsPerSample(8);
			wf->set_Channels(1);
			wf->set_SamplesPerSec(11000);
			wf->set_BlockAlign(1);
			wf->set_AvgBytesPerSec(11000);
			wf->set_FormatTag(0x0007);

			SpAudioFormatClass *sf = new SpAudioFormatClass();
			sf->SetWaveFormatEx(wf);

			SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags::SVSFlagsAsync;
			SpeechStreamFileMode SpFileMode = SpeechStreamFileMode::SSFMCreateForWrite;
			SpMemoryStreamClass *buffer = new SpMemoryStreamClass();
			buffer->set_Format(sf);
			sp->set_AudioOutputStream((ISpeechBaseStream __gc *)buffer);
			sp->Speak(text, SpFlags);
			sp->WaitUntilDone(-1);
			System::Object __gc* data = buffer->GetData();
			System::Array * array = (System::Array *)data;			
   
			unsigned char *buf = (unsigned char *)malloc(array->get_Length() * sizeof(unsigned char));
			FILE *fp = fopen("test.wav","wb");			
			for(int i = 0; i < array->get_Length(); i++)
			{   				
				buf[i] = System::Byte::Parse(array->get_Item(i)->ToString());							
				fwrite(&buf[i],sizeof(unsigned char),1,fp);
			}
			fclose(fp);

			SpFileStreamClass __gc* SpFileStream = new SpFileStreamClass();
			SpFileStream->set_Format(sf);
			SpFileStream->Open("working.wav", SpFileMode, false);
			sp->set_AudioOutputStream((ISpeechBaseStream __gc *)SpFileStream);
			sp->Speak(text, SpFlags);
			sp->WaitUntilDone(-1);
			SpFileStream->Close();
            
			int result = mpDialogicLine->PlayFile("working.wav",termDigits,terminate?1:0);

			if(result == HANGUP)
				throw new DialogicHangupException("Hang Up Termination");			
			else
			{
				if(result != DIGITTERM)
					return 1;
				else
					return 0;
			}				
		}
		catch(char* str)
		{
			ApplicationException __gc *ex = new ApplicationException(str);
			throw ex;			
		}		
	}

	void DialogicLine::PlayText(String __gc* text)
	{
		this->PlayText(text,"",false);		
	}
	*/
	
	bool DialogicLine::WaitRing(int numberOfRings, int waitTimer)
	{
		bool result = false;
		try
		{
			result = mpDialogicLine->WaitRing(numberOfRings,waitTimer);
		}
		catch(char* str)
		{
			DialogicException __gc *ex = new DialogicException(str);
			throw ex;			
		}
			
		if(result && this->get_EnableCallerId())
		{
			try
			{
				mpDialogicLine->GetCallerId();			
			}
			catch(char* str)
			{
				DialogicException __gc *ex = new DialogicException(str);
				throw ex;			
			}							
			return result;					
		}

		return false;
	}

	float DialogicLine::TimeOut(int numberOfDigits)
	{
		return numberOfDigits * DialogicLine::TimeOutPerDigit;
	}	

	String __gc* DialogicLine::ToString()
	{
		try
		{
			return mpDialogicLine->ChannelName();
		}
		catch(char* str)
		{
			DialogicException __gc *ex = new DialogicException(str);
			throw ex;			
		}
	}
}
}