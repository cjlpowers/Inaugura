// DialogicNet.h
#include "stdafx.h"
#include "DialogicLegacy.h"

#using <mscorlib.dll>

using namespace System;


#ifndef DIALOGIC_NET
#define DIALOGIC_NET


namespace Inaugura
{
namespace Dialogic
{

	public __value enum CallAnalysis
	{	
		Busy = 0,
		OperatorIntercept = 1,
	    CallConnected = 2,
		Error = 3,
		FaxTone = 4,
		NoAnswer = 5,
		NoDialTone = 6,
		NoRingBack =  7,
		CallAnalysisStopped= 8
	};	

	public __value enum Termination
	{	
		Tone = 0x02000		
	};	

public __sealed __gc class DialogicLine
{
	static public float TimeOutPerDigit = 3.0;
	public:
		// METHODS
		DialogicLine(String __gc* channelName);
		~DialogicLine(void);
		void OpenLine();
		void CloseLine();
		void ClearDigitBuffer();
		void Dial(String __gc* phoneNumber);
		CallAnalysis Dial(String __gc* phoneNumber, int numberOfRings);
		String __gc* GetCallerId();
		String __gc* GetDigits(int  maxDigits, double  timeOutSec, String __gc* termDigits, double  digitTimeOutSec, double  silenceTimeOutSec, String __gc* __gc* termDigit);
		String __gc* GetDigits(int  maxDigits, double  timeOutSec, String __gc* termDigits, double  digitTimeOutSec);
		String __gc* GetDigits(int  maxDigits, double  timeOutSec, double  digitTimeOutSec);
		
		int DetectRingback(double timeOutSec);

		int GetHookState();
		void GiveDialTone();
		void OffHook();
		void OnHook();
		int PlaySpecial(String __gc* fileName);
		int PlayFile(String __gc* fileName, String __gc* termDigits, bool terminate);
		void PlayFile(String __gc* fileName);
		//int PlayText(String __gc* text, String __gc* termDigits, bool terminate);
		//void PlayText(String __gc* text);

		void RecordWav(String __gc* fileName, double  timeOutSec, double  silenceTimeOutSec, bool terminate );


		bool WaitRing(int numberOfRings, int waitTimer);
		String __gc* ToString();
		
		static float TimeOut(int numberOfDigits);
	

		// PROPERTIES
		__property String __gc* get_ChannelName();		
		__property void set_ChannelName(String __gc* channelName);		
		__property bool get_EnableCallerId();	
		__property int get_NumberOfDigitsInBuffer();
		__property void set_EnableCallerId(bool enableCallerId);	
		__property String __gc* get_CallerId();		
		
	private:
		DialogicLegacy *mpDialogicLine;
	};
}
}

#endif