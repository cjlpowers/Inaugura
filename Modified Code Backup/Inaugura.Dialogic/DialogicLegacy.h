//////////////////////////////////////////////////////////////////////
// Dialogic Line
//////////////////////////////////////////////////////////////////////
//
//
//
//
//
//
//
//////////////////////////////////////////////////////////////////////
// Copyright © 1999-2002 Orbis Software Solutions 
// All Rights Reserved
//////////////////////////////////////////////////////////////////////

#include "afx.h"

#ifndef DIALOGIC_LINE
#define DIALOGIC_LINE

const int HANGUP = -9;
const int MAXTIME = -10;
const int DIGITTERM = -11;
const int MAXDTMF = -12;
const int NORMTERM = -13;
const int MAXSILENCE = -14;
const int MAXNONSILENCE = -15;
const int INTERDIGITDELAY = -16;
const int PATTERNTERM = -17;
const int USERSTOP = -18;
const int EODTERM = -19;
const int TONETERM = -20;
const int ERRORTERM = -21;
const int BARGEIN = -22;
const int SUCCESS = 1;

namespace Inaugura
{
	namespace Dialogic
	{

		class DialogicLegacy
		{
			public:
				// Constructor/Destructor
				DialogicLegacy(CString channelName);

				// Methods
				int OpenLine();
				void CloseLine();
				void OffHook();
				void OnHook();
				void ClearDigitsBuffer();
				void Stop();
				bool WaitRing(int numofrings, int waittimer);
				void GetCallerId();
				int GiveDialTone();
				int GetNumberOfDigitsInBuffer();
				static int GetBoardCount();
				static CString GetChannelNames();
				CString GetDigits( int maxDigits, float timeOutSec, CString termDigits, float digitTimeOutSec, float silenceTimeOutSec, CString& termDigit);
				int PlayFile(CString Filename, CString TermDigits, int Terminate);
				int PlayBuffer(char* buffer, CString TermDigits, int Terminate);
				void RecordFile(CString Filename);
				int Dial(CString PhoneNumber, int numberOfRings);
				void Dial(CString PhoneNumber);
				void SendFax(CString number_to_dial, CString file_to_send);
				void OpenFax();
				void StopFax();

				// Properties
				int GetHookState();
				int LineNum();
				bool EnableCallerId();
				void EnableCallerId(bool enableCallerId);
				CString ChannelName();
				void ChannelName(CString channelName);
				void SoundFilePath(CString soundFilePath);
				CString SoundFilePath();
				CString GetCallerIdXml();

			protected:
				// Methods
				int GetDigitsBitMask(CString termDigits);

				// Variables
				int mChannelHandle;
				//int mLineNumber;
				CString mChannelName;
				CString mCallerIdXml;
				CString mSoundFilePath;
				bool mEnableCallerId;
		};
	}
}
#endif