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

#include "DialogicLegacy.h"
#include "stdafx.h"
#include "fcntl.h"
#include "stdio.h"
#include "time.h"
#include <srllib.h>
#include <dxxxlib.h>



namespace Inaugura
{
namespace Dialogic
{	

DialogicLegacy::DialogicLegacy(CString channelName="")
{
	ChannelName(channelName);
	mEnableCallerId = true;
	mChannelHandle = 0;
	mCallerIdXml = "";
}

bool DialogicLegacy::EnableCallerId()
{
	return mEnableCallerId;
}

int DialogicLegacy::GetHookState()
{
	if(ATDX_HOOKST(this->mChannelHandle) == DX_OFFHOOK)
		return 0;
	else
		return 1;
}

int DialogicLegacy::GetNumberOfDigitsInBuffer()
{
	return ATDX_BUFDIGS(this->mChannelHandle);		
}

void DialogicLegacy::EnableCallerId(bool enableCallerId)
{
	mEnableCallerId = enableCallerId;
}

CString DialogicLegacy::ChannelName()
{
	return mChannelName;
}

void DialogicLegacy::ChannelName(CString channelName)
{
	mChannelName = channelName;
}


int DialogicLegacy::OpenLine()
{
	unsigned short parmval;

	// Open Channel
	if ((mChannelHandle = dx_open(mChannelName,NULL)) == -1)
	{
		throw "Could Not Open Line";
	}

	//set the volume
	if (dx_adjsv( mChannelHandle, SV_VOLUMETBL, SV_ABSPOS, SV_ADD8DB ) == -1 ) 
	{
		
		throw "Unable to set the volume";
	}

	if(mEnableCallerId)
	{
		parmval = DX_CALLIDENABLE; //enable callerid
		if (dx_setparm(mChannelHandle, DXCH_CALLID, (void *)&parmval) == -1)
		{
			throw "Could Not Enable Caller Id";				
		}
	
		/*
		if (dx_setevtmsk(mChannelHandle, DM_RINGS) == -1) 
		{            
			throw "Could Not Enable WAIT RINGS Event";
		}
		*/

		/* NOTE THIS CREATED A HANGUP PROBLEM AT THE WHEN CALLING
		// set start or end of ring edge
		parmval = ET_RON; //start of ring
		if (dx_setparm(mChannelHandle, DXBD_R_EDGE, (void *)&parmval) == -1) 
		{            
			throw "Could Not set the ring edge to start of ring";
		}
		*/

		/*
	    // set ring detection duration
		parmval = 1; //start of ring
		if (dx_setparm(mChannelHandle, DXBD_R_ON, (void *)&parmval) == -1) 
		{            
			throw "Could Not set the ring detection duration";
		}
		*/

		parmval = 2; //number of rings
		if (dx_setparm(mChannelHandle, DXCH_RINGCNT, (void *)&parmval) == -1)
		{ 
			throw "Cound Not Set The Ring Count";
		}

		//parmval = 1;            
		// Removed to allow multi rings
		// if (dx_setparm(chid, DXBD_R_IRD, (void *)&parmval) == -1)
		// { 
		//	    Tracestring.Format("ERROR occured in CallerID numrings AllocateLine");
		//	    Log(Tracestring);
		//      return 0;
		// }
	}
	else //non callerID
	{
		parmval = 1; //number of rings
		if (dx_setparm(mChannelHandle, DXCH_RINGCNT, (void *)&parmval) == -1)
		{ 
			throw "Could Not Set The Ring Count";
		}			
		//parmval = 1; 
		//clear ring counter after every ring so on a busy switch you
		//dpn't get channels not picking up when starting the program
		//if (dx_setparm(chid, DXBD_R_IRD, (void *)&parmval) == -1)
		//{ 
		//	Tracestring.Format("ERROR occured in CallerID numrings AllocateLine");
		//	Log(Tracestring);
		//	return 0;
		//}
	}

	
	
	// Delete the tones
	if ( dx_deltones(mChannelHandle) == -1 ) 
	{
		throw "Unable to delete tones";
	}		
	// Initialize Perfect Call Analysis
	if (dx_initcallp( mChannelHandle ) == -1) 
	{
		throw "Unable to initialize Perfect Call Analysis";	
	}
	
		

	return mChannelHandle;
}

void DialogicLegacy::CloseLine()
{    
	// Close Channel
	if ((dx_close(mChannelHandle)) == -1)
	{
		throw "Could Not Stop Line";
	}   
}

void DialogicLegacy::Stop()
{
	if ( dx_stopch(mChannelHandle, EV_ASYNC) == -1)
	{
		throw "Could Not Close Line";	
	} 
}

void DialogicLegacy::OffHook()
{
	if (dx_sethook(mChannelHandle,DX_OFFHOOK,EV_SYNC) == -1)
	{
		throw "Could Not Set Line Off Hook";
	} 	
}

void DialogicLegacy::OnHook()
{
	if (dx_sethook(mChannelHandle,DX_ONHOOK,EV_SYNC) == -1)
	{
		throw "Could Not Set Line On Hook";
	}  
}

void DialogicLegacy::ClearDigitsBuffer()
{
	if (dx_clrdigbuf(mChannelHandle) == -1) //clear channel buffer on card
	{
		throw "Could Not Clear Digits Buffer";		
	}
}

bool DialogicLegacy::WaitRing(int numofrings, int waittimer)
{	
	if (dx_setparm(mChannelHandle, DXCH_RINGCNT, &numofrings) == -1)
	{
		throw "Unable to set the Ring Event Param";
	}

	if(dx_wtring(mChannelHandle,numofrings,DX_ONHOOK,waittimer) == -1)
		return false;
	else 
		return true;	
}

void DialogicLegacy::GetCallerId()
{
	CString frameType = "";
	CString sourceNum = "";
	CString name = "";
	CString dateTime = "";
	CString dialedNumber = "";
    CString status = "OK";

	if(!mEnableCallerId)
		return;

	unsigned char buffer[100];
	memset(buffer,0,sizeof(buffer));

	// get the frame type
	if(dx_gtextcallid(mChannelHandle,CLIDINFO_FRAMETYPE, buffer) != -1) 
	{
		if(buffer[0] == CLASSFRAME_MDM)  // MULTIPLE DATA MESSAGE
		{
            frameType = "MDM";
			if (dx_gtextcallid(mChannelHandle, MCLASS_DN, buffer) != -1) 
			{
				sourceNum = buffer;
			}			
			else if (dx_gtextcallid(mChannelHandle, CLIDINFO_CALLID, buffer) != -1) /* This is another way to obtain Caller ID (regardless of frame type)*/
			{
				sourceNum = buffer;
			}
			// why was the caller Name not available
			else if(dx_gtextcallid(mChannelHandle, MCLASS_ABSENCE1, buffer) != -1) 
			{
				CString temp = "";
				temp = buffer;
				if(temp == "P")
					sourceNum = "PRIVATE";
				if(temp == "O")
					sourceNum = "OUT OF AREA";
			}
			else /* print the reason for the Absence of Caller ID */
			{
				switch(ATDV_LASTERR(mChannelHandle))
				{
					case EDX_CLIDBLK:
                        status = "BLOCKED";
						break;
					case EDX_CLIDOOA:
                        status = "OUT OF AREA";
						break;
					case EDX_BADPARM:
                        status = "INVALID PARAMETER";						
						break;
					case EDX_BUSY:
						status = "BUSY";
						break;
					case EDX_CLIDINFO:
                        status = "INFORMATION NOT SENT";
						break;
					case EDX_SYSTEM:
                        status = "OPERATING SYSTEM ERROR";
						break;
					case EDX_TIMEOUT:       
                        status = "TIMEOUT REACHED";
						break;
					default:
                        status = "UNKNOWN ERROR";
						break;
				}
			}
			
            /* Get and print the Caller Name */
			if (dx_gtextcallid(mChannelHandle, MCLASS_NAME, buffer) != -1) 
			{
				name = buffer;				
			}
			// why was the caller Name not available
			else if(dx_gtextcallid(mChannelHandle, MCLASS_ABSENCE2, buffer) != -1) 
			{
				CString temp = "";
				temp = buffer;
				if(temp == "P")
					name = "PRIVATE";
				if(temp == "O")
					name = "OUT OF AREA";
			}
			/* Get and print the Date and Time */
			if (dx_gtextcallid(mChannelHandle, MCLASS_DATETIME, buffer) != -1) 
			{
				dateTime = buffer;
			}
			/* Get and print the Dialed Number */
			if (dx_gtextcallid(mChannelHandle, MCLASS_DDN, buffer) != -1) 
			{
                dialedNumber = buffer;
			}
			
			sourceNum.Replace(" ","");
			
			name.Replace("'","&apos;");
			name.Replace("&","&amp;");
			name.Replace("\"","&quot;");
			name.Replace("<","&lt;");
			name.Replace(">","&gt;");			
		}
		else // SINGLE DATA MESSAGE
		{

			frameType = "SDM";
			if (dx_gtextcallid(mChannelHandle, CLIDINFO_GENERAL, buffer) == -1) 
			{
				switch(ATDV_LASTERR(mChannelHandle))
				{
					case EDX_CLIDBLK:
                        status = "BLOCKED";
						break;
					case EDX_CLIDOOA:
                        status = "OUT OF AREA";
						break;
					case EDX_BADPARM:
                        status = "INVALID PARAMETER";						
						break;
					case EDX_BUSY:
						status = "BUSY";
						break;
					case EDX_CLIDINFO:
                        status = "INFORMATION NOT SENT";
						break;
					case EDX_SYSTEM:
                        status = "OPERATING SYSTEM ERROR";
						break;
					case EDX_TIMEOUT:       
                        status = "TIMEOUT REACHED";
						break;
					default:
                        status = "UNKNOWN ERROR";
						break;
				}				
			}

			CString temp;
			temp = buffer;
			name = temp.Mid(40,temp.GetLength()-40);
			sourceNum = temp.Mid(20,20);		

			// filter the values
			for(int i = 0; i < name.GetLength(); i++)
			{
				if(!isalnum(name[i]) && !ispunct(name[i]) && name[i]!=' ')
				{			
					name = "";
				}
			}
			for(i = 0; i < sourceNum.GetLength(); i++)
			{
				if(!isalnum(sourceNum[i]) && !ispunct(sourceNum[i]) && sourceNum[i]!=' ')
				{				
					sourceNum = "";
				}
			}

			// replace the name with the proper value 
			if(name == "P")
				name = "PRIVATE";
			else if(name == "O")
				name = "OUT OF AREA";

			sourceNum.Replace(" ","");

			// replace the source num with the proper value 
			if(sourceNum == "P")
				sourceNum = "PRIVATE";
			else if(sourceNum == "O")
				sourceNum = "OUT OF AREA";

			name.Replace("'","&apos;");
			name.Replace("&","&amp;");
			name.Replace("\"","&quot;");
			name.Replace("<","&lt;");
			name.Replace(">","&gt;");
        }		
	}
	else // error
	{
		switch(ATDV_LASTERR(mChannelHandle))
		{
			case EDX_CLIDBLK:
                status = "BLOCKED";
				break;
			case EDX_CLIDOOA:
                status = "OUT OF AREA";
				break;
			case EDX_BADPARM:
                status = "INVALID PARAMETER";						
				break;
			case EDX_BUSY:
				status = "BUSY";
				break;
			case EDX_CLIDINFO:
                status = "INFORMATION NOT SENT";
				break;
			case EDX_SYSTEM:
                status = "OPERATING SYSTEM ERROR";
				break;
			case EDX_TIMEOUT:       
                status = "TIMEOUT REACHED";
				break;
			default:
                status = "UNKNOWN ERROR";
				break;
		}	
	}

    CString result = "";
    result.Format("<CallerId status=\"OK\" frameType=\"%s\" sourceNumber=\"%s\" sourceName=\"%s\" dialedNumber=\"%s\" dateTime=\"%s\"/>",frameType,sourceNum,name,dialedNumber,dateTime);
	this->mCallerIdXml = result;
}

int DialogicLegacy::GiveDialTone()
{
	TN_GEN  tngen;
	long	term;

	//build the tone
	dx_bldtngen( &tngen, 440, 350, -30, -30, 500 );

	//set up DV_TPT with a termmask
	DV_TPT *tpt = new DV_TPT[1];
	
		// create the TPT structure
		tpt[0].tp_type		= IO_EOT;
		tpt[0].tp_termno	= DX_MAXDTMF;
		tpt[0].tp_length	= 1;
		tpt[0].tp_flags		= TF_MAXDTMF;

		/*
		tpt[1].tp_type		= IO_EOT;
		tpt[1].tp_termno	= DX_MAXTIME;
		tpt[1].tp_length	= 100;
		tpt[1].tp_flags		= TF_MAXTIME;
		*/

	//play the tone
	if(dx_playtone( mChannelHandle, &tngen, tpt, EV_SYNC ) == -1)
	{ 
		// delete the tpt struct
		if(tpt)
		{
			delete[] tpt;
			tpt = NULL;
		}
		throw "Could Not Play Dial Tone";
	}

	// delete the tpt struct
	if(tpt)
	{
		delete[] tpt;
		tpt = NULL;
	}

	
	// Examine bitmap to determine if digits caused termination	
	if((term = ATDX_TERMMSK(mChannelHandle)) == AT_FAILURE)
	{ 
		throw ATDV_ERRMSGP(mChannelHandle);
	}
	if(term == TM_MAXDTMF) //terminated because digit pressed
	{ 
		return 1;
	}
	if(term == TM_LCOFF) //customer hungup
	{ 
		return HANGUP;
	}



	return 1;
}		

int DialogicLegacy::GetDigitsBitMask(CString termDigits)
{
	// make upper
	termDigits.MakeUpper();
	int returnCode = 0;
	for(int i =0; i < termDigits.GetLength(); i++)
	{		
		switch(termDigits.GetAt(i))
		{
		case '0':
			returnCode |= DM_0;
			break;
		case '1':
			returnCode |= DM_1;
			break;
		case '2':
			returnCode |= DM_2;
			break;
		case '3':
			returnCode |= DM_3;
			break;
		case '4':
			returnCode |= DM_4;
			break;
		case '5':
			returnCode |= DM_5;
			break;
		case '6':
			returnCode |= DM_6;
			break;
		case '7':
			returnCode |= DM_7;		
			break;
		case '8':
			returnCode |= DM_8;
			break;
		case '9':
			returnCode |= DM_9;
			break;
		case '*':
			returnCode |= DM_S;
			break;
		case '#':
			returnCode |= DM_P;
			break;
		case 'A':
			returnCode |= DM_A;
			break;
		case 'B':
			returnCode |= DM_B;
			break;
		case 'C':
			returnCode |= DM_C;
			break;
		case 'D':
			returnCode |= DM_D;			
			break;
		}
	}

	return returnCode;
}


CString DialogicLegacy::GetDigits( int maxDigits, float timeOutSec, CString termDigits, float digitTimeOutSec, float silenceTimeOutSec, CString& termDigit)
{
	const static float _100MS_PER_SEC = 10;
	int digitBitMask		= 0;
	int optionsCount		= 0;
	int optionIndex			= 0;
	int numDigitsReceived	= 0;
	long term;
	DV_DIGIT digitBuffer; 
	CString tempstring;
	CString returnstring;
	DV_TPT *tpt = NULL;
	int i;

	try
	{
		// count how many options we are using
		if(termDigits.GetLength() != 0)
			optionsCount++;
		if(maxDigits != -1)
			optionsCount++;
		if(timeOutSec != float(-1))
			optionsCount++;
		if(digitTimeOutSec != float(-1))
			optionsCount++;
		if(silenceTimeOutSec != float(-1))
			optionsCount++;
		
		// Clear the digit buffer
		//dx_clrdigbuf(m_channelID);
		
		// create a new tpt struct with enough space to hold all the options
		tpt = new DV_TPT[optionsCount+1];	// +1 since we must also process hang ups
		if(termDigits.GetLength() != 0) // check for termination digits
		{
			digitBitMask = GetDigitsBitMask(termDigits);
			// create the TPT structure
			tpt[optionIndex].tp_type	= IO_CONT;
			tpt[optionIndex].tp_termno	= DX_DIGMASK;
			tpt[optionIndex].tp_length	= digitBitMask;
			tpt[optionIndex].tp_flags	= TF_DIGMASK;
			optionIndex++;
		}
		if(maxDigits != -1) // watch for max digits
		{		
			// create the TPT structure
			tpt[optionIndex].tp_type	= IO_CONT;
			tpt[optionIndex].tp_termno	= DX_MAXDTMF;
			tpt[optionIndex].tp_length	= maxDigits;
			tpt[optionIndex].tp_flags	= TF_MAXDTMF;
			optionIndex++;
		}
		if(timeOutSec != float(-1)) // watch for time out
		{		
			int time = int(timeOutSec * _100MS_PER_SEC);
			// create the TPT structure
			tpt[optionIndex].tp_type	= IO_CONT;
			tpt[optionIndex].tp_termno	= DX_MAXTIME;
			tpt[optionIndex].tp_length	= time;
			tpt[optionIndex].tp_flags	= TF_MAXTIME;
			optionIndex++;
		}
		if(digitTimeOutSec != float(-1)) // watch for digit time out
		{		
			int time = int(digitTimeOutSec * _100MS_PER_SEC);
			// create the TPT structure
			tpt[optionIndex].tp_type	= IO_CONT;
			tpt[optionIndex].tp_termno	= DX_IDDTIME;
			tpt[optionIndex].tp_length	= time;
			tpt[optionIndex].tp_flags	= TF_IDDTIME;
			optionIndex++;
		}
		if(silenceTimeOutSec != float(-1)) // watch for max digits
		{		
			int time = int(silenceTimeOutSec * _100MS_PER_SEC);
			// create the TPT structure
			tpt[optionIndex].tp_type	= IO_CONT;
			tpt[optionIndex].tp_termno	= DX_MAXSIL;
			tpt[optionIndex].tp_length	= time;
			tpt[optionIndex].tp_flags	= TF_MAXSIL;
			optionIndex++;
		}
		
		// we always want to handle hang up
		tpt[optionIndex].tp_type	= IO_EOT;	
		tpt[optionIndex].tp_termno	= DX_LCOFF;	
		tpt[optionIndex].tp_length	= 3;
		tpt[optionIndex].tp_flags	= TF_LCOFF;			
		
		// Start playback 
		if( (numDigitsReceived = dx_getdig(mChannelHandle,tpt, &digitBuffer, EV_SYNC)) == -1) 
		{			
			// delete the tpt struct
			if(tpt)
			{
				delete[] tpt;
				tpt = NULL;
			}			

			char buffer[1000];
			sprintf(buffer,"Could Not Get Digits %s",ATDV_ERRMSGP(mChannelHandle));
			throw buffer;		
		}

		//printf("Number of digits received: %d\n",numDigitsReceived);

		// delete the tpt struct
		if(tpt)
		{
			delete[] tpt;
			tpt = NULL;
		}
		
		if((term = ATDX_TERMMSK(mChannelHandle)) == AT_FAILURE)
		{			
			throw ATDV_ERRMSGP(mChannelHandle);
		}

		if(term & TM_DIGIT) //terminated because specified digit received
		{
			returnstring = "";
			for (i=0; i < (numDigitsReceived-2); i++)
			{
				tempstring.Format("%s%c",returnstring,digitBuffer.dg_value[i]);
				returnstring = tempstring; //copy buffer to string
			}
			if(termDigit)
			{
				termDigit = digitBuffer.dg_value[numDigitsReceived-2];
			}
			return returnstring;
		}
		else if(term & TM_MAXDTMF) //received all of the digits successfully
		{
			returnstring = "";
			for (i=0; i < numDigitsReceived; i++)
			{
				tempstring.Format("%s%c",returnstring,digitBuffer.dg_value[i]);
				returnstring = tempstring; //copy buffer to string
			}
			return returnstring;
		}
		else if(term & TM_LCOFF) //customer hungup
		{
			return "HANGUP";
		}
		else if(term & TM_MAXTIME || term & TM_IDDTIME) //timed out waiting for digits
		{
			returnstring = "";
			for (i=0; i < (numDigitsReceived-1); i++)
			{
				tempstring.Format("%s%c",returnstring,digitBuffer.dg_value[i]);
				returnstring = tempstring; //copy buffer to string
			}
			return returnstring;
		}
	}
	catch(char *str)
	{
		if(tpt)
		{
			delete[] tpt;
			tpt = NULL;
		}
		throw str;
	}
	catch(...)
	{
		// delete the tpt struct
		if(tpt)
		{
			delete[] tpt;
			tpt = NULL;
		}
		throw "Some Error In Get Digits";
	}	
	return returnstring;
}

int DialogicLegacy::PlayFile(CString Filename, CString TermDigits, int Terminate)
{	
	long term;
	unsigned short TermMaskDigit;
	unsigned short sTerminate = Terminate;
	CString tempstring;
	CString debugstring;
	CString Tracestring;	

	// add the path onto front of Filename
	dx_clrdigbuf(mChannelHandle);

	if(TermDigits != "")
	{
		//convert the received string terminating digit to unsigned short
		TermMaskDigit = GetDigitsBitMask(TermDigits);

		if(sTerminate) // terminate on any key
		{
			//set up DV_TPT with a termmask
			DV_TPT tpt[] = {{IO_CONT, DX_MAXDTMF, sTerminate, TF_MAXDTMF},
			{IO_CONT, DX_LCOFF, 3, TF_LCOFF|TF_10MS},
			{IO_EOT,  DX_DIGMASK, TermMaskDigit, TF_DIGMASK}};

			//start playing
			if (dx_playwav(mChannelHandle,Filename,tpt,EV_SYNC)==-1)
			{			
				throw "Error Playing File ("+Filename+")";						
			}
		}
		else // dont terminate on just any key
		{
			//convert the received string terminating digit to unsigned short
			TermMaskDigit = GetDigitsBitMask(TermDigits);

			//set up DV_TPT with a termmask
			DV_TPT tpt[] = {{IO_CONT, DX_LCOFF, 3, TF_LCOFF|TF_10MS},
			{IO_EOT,  DX_DIGMASK, TermMaskDigit, TF_DIGMASK}};

			//start playing
			if (dx_playwav(mChannelHandle,Filename,tpt,EV_SYNC)==-1)
			{			
				throw "Error Playing File ("+Filename+")";						
			}
		}
	}
	else
	{
		//set up DV_TPT without a termmask
		DV_TPT tpt[] = {{IO_CONT, DX_MAXDTMF, sTerminate, TF_MAXDTMF},
		{IO_EOT, DX_LCOFF, 3, TF_LCOFF|TF_10MS}};

		//start playing
		if (dx_playwav(mChannelHandle,Filename,tpt,EV_SYNC)==-1)
		{						
			throw "Error Playing File ("+Filename+")";							
		}
	}

	CString errMSg = ATDV_ERRMSGP(mChannelHandle);

	// Examine bitmap to determine if digits caused termination
	if((term = ATDX_TERMMSK(mChannelHandle)) == AT_FAILURE)//error retrieving mask info
	{
		throw ATDV_ERRMSGP(mChannelHandle);		
	} 
	else if(term & TM_LCOFF)//customer hungup
	{
		//debugstring.Format("Customer Hungup return -9");
		return HANGUP;
	}
	else if(term & TM_DIGIT)//terminated with digit
	{		
		return DIGITTERM;
	}
	else if(term & TM_MAXTIME)
	{
		//debugstring.Format("Maximum function time return -8");
		return MAXTIME;
	}
	else if(term & TM_MAXDTMF)
	{
		//debugstring.Format("Maximum DTMF count return -8");
		return MAXDTMF;
	}
	else if(term & TM_NORMTERM)
	{
		//debugstring.Format("Normal Termination (for dx_dial( ), dx_sethook( )) return -8");
		return NORMTERM;
	}
	else if(term & TM_MAXSIL)
	{
		//debugstring.Format("Maximum period of silence return -8");
		return MAXSILENCE;
	}
	else if(term & TM_MAXNOSIL)
	{
		//debugstring.Format("Maximum period of non-silence return -8");
		return MAXNONSILENCE;
	}
	else if(term & TM_IDDTIME)
	{
		//debugstring.Format("Inter-digit delay return -8");
		return INTERDIGITDELAY;
	}
	else if(term & TM_PATTERN)
	{
		//debugstring.Format("Pattern matched silence off return -8");
		return PATTERNTERM;
	}
	else if(term & TM_USRSTOP)
	{
		//debugstring.Format("Function stopped by user return -8");
		return USERSTOP;
	}
	else if(term & TM_EOD)
	{
		//debugstring.Format("End of Data reached on playback return -10");
		return EODTERM;
	}
	else if(term & TM_TONE)
	{
		//debugstring.Format("Tone-on/off event return -8");
		return TONETERM;
	}
	else if(term & TM_ERROR)
	{
		return ERRORTERM;
	}
	else if(term & TM_BARGEIN)
	{
		return BARGEIN;
	}
	else
		return SUCCESS;	
}

int DialogicLegacy::PlayBuffer(char* buffer, CString TermDigits, int Terminate)
{
   unsigned short TermMaskDigit;
   unsigned short sTerminate = Terminate;
   long term;
   DX_IOTT iott;  
   DV_TPT  tpt;  
      
   TermMaskDigit = GetDigitsBitMask(TermDigits);
    
   /* set up DX_IOTT */  
   iott.io_type = IO_MEM|IO_EOT;  
   iott.io_bufp = buffer;  
   iott.io_offset = 0;  
   iott.io_length = -1;  /* play till end of file */  
   iott.io_fhandle = 0;   
   
   /* set up DV_TPT */  
   dx_clrtpt(&tpt,1);  
   tpt.tp_type   = IO_EOT;          /* only entry in the table */  
   tpt.tp_termno = DX_MAXDTMF;      /* Maximum digits */  
   tpt.tp_length = 4;               /* terminate on four digits */  
   tpt.tp_flags  = TF_MAXDTMF;      /* Use the default flags */  
   
   if(TermDigits != "")
	{
		//convert the received string terminating digit to unsigned short
		TermMaskDigit = GetDigitsBitMask(TermDigits);

		//set up DV_TPT with a termmask
		DV_TPT tpt[] = {{IO_CONT, DX_MAXDTMF, sTerminate, TF_MAXDTMF},
		{IO_CONT, DX_LCOFF, 3, TF_LCOFF|TF_10MS},
		{IO_EOT,  DX_DIGMASK, TermMaskDigit, TF_DIGMASK}};

		//start playing
		if (dx_play(mChannelHandle,&iott,tpt,EV_SYNC) == -1)		
		{			
			throw "Error Playing Buffer";						
		}
	}
	else
	{
		//set up DV_TPT without a termmask
		DV_TPT tpt[] = {{IO_CONT, DX_MAXDTMF, sTerminate, TF_MAXDTMF},
		{IO_EOT, DX_LCOFF, 3, TF_LCOFF|TF_10MS}};

		//start playing
		if (dx_play(mChannelHandle,&iott,tpt,EV_SYNC) == -1)		
		{			
			throw "Error Playing Buffer";						
		}
	}

		// Examine bitmap to determine if digits caused termination
	if((term = ATDX_TERMMSK(mChannelHandle)) == AT_FAILURE)//error retrieving mask info
	{
		throw ATDV_ERRMSGP(mChannelHandle);
	} 
	else if(term == TM_LCOFF)//customer hungup
	{
		//debugstring.Format("Customer Hungup return -9");
		return HANGUP;
	}
	else if(term == TM_DIGIT)//terminated with digit
	{
		//debugstring.Format("Specific digit received return -8");
		return DIGITTERM;
	}
	else if(term == TM_MAXTIME)
	{
		//debugstring.Format("Maximum function time return -8");
		return MAXTIME;
	}
	else if(term == TM_MAXDTMF)
	{
		//debugstring.Format("Maximum DTMF count return -8");
		return MAXDTMF;
	}
	else if(term == TM_NORMTERM)
	{
		//debugstring.Format("Normal Termination (for dx_dial( ), dx_sethook( )) return -8");
		return NORMTERM;
	}
	else if(term == TM_MAXSIL)
	{
		//debugstring.Format("Maximum period of silence return -8");
		return MAXSILENCE;
	}
	else if(term == TM_MAXNOSIL)
	{
		//debugstring.Format("Maximum period of non-silence return -8");
		return MAXNONSILENCE;
	}
	else if(term == TM_IDDTIME)
	{
		//debugstring.Format("Inter-digit delay return -8");
		return INTERDIGITDELAY;
	}
	else if(term == TM_PATTERN)
	{
		//debugstring.Format("Pattern matched silence off return -8");
		return PATTERNTERM;
	}
	else if(term == TM_USRSTOP)
	{
		//debugstring.Format("Function stopped by user return -8");
		return USERSTOP;
	}
	else if(term == TM_EOD)
	{
		//debugstring.Format("End of Data reached on playback return -10");
		return EODTERM;
	}
	else if(term == TM_TONE)
	{
		//debugstring.Format("Tone-on/off event return -8");
		return TONETERM;
	}
	else if(term == TM_ERROR)
	{
		return ERRORTERM;
	}
	else
		return SUCCESS;	
}


/*
int DialogicLegacy::PlayFile(CString Filename, CString TermDigits, int Terminate)
{	
	long term;
	unsigned short TermMaskDigit;
	unsigned short sTerminate = Terminate;
	CString tempstring;
	CString debugstring;
	CString Tracestring;	


	// Set up DX_IOTT for file input/output
	DX_IOTT iott = {IO_DEV | IO_EOT, 0, 0, 0, 0, -1};    // I/O xfer table
	// universal DX_XPB used for VOX file playing
	//DX_XPB xpbVox = {FILE_FORMAT_VOX, DATA_FORMAT_DIALOGIC_ADPCM, DRT_6KHZ, 4};
	DX_XPB xpbWav = {FILE_FORMAT_WAVE, DATA_FORMAT_MULAW, DRT_11KHZ, 4};

	// add the path onto front of Filename
	tempstring.Format("%s%s",mSoundFilePath,Filename);

	if((iott.io_fhandle = dx_fileopen(tempstring, O_RDONLY|O_BINARY))	== -1) //open file
	{
		char buff[1000];
		sprintf(buff,"Could Not Open %s For Playing",tempstring);
		throw buff;	
	}

	dx_clrdigbuf(mChannelHandle);

	if(TermDigits != "")
	{
		//convert the received string terminating digit to unsigned short
		TermMaskDigit = GetDigitsBitMask(TermDigits);
		
		//set up DV_TPT with a termmask
		DV_TPT tpt[] = {{IO_CONT, DX_MAXDTMF, sTerminate, TF_MAXDTMF},
		{IO_CONT, DX_LCOFF, 3, TF_LCOFF|TF_10MS},
		{IO_EOT,  DX_DIGMASK, TermMaskDigit, TF_DIGMASK}};

		//start playing
		if (dx_play(mChannelHandle,&iott,tpt,EV_SYNC)==-1)
		{
			dx_fileclose(iott.io_fhandle); //close file				
			throw "Error Playing File ("+Filename+")";						
		}

	}
	else
	{
		//set up DV_TPT without a termmask
		DV_TPT tpt[] = {{IO_CONT, DX_MAXDTMF, sTerminate, TF_MAXDTMF},
		{IO_EOT, DX_LCOFF, 3, TF_LCOFF|TF_10MS}};

		//start playing
		if (dx_play(mChannelHandle,&iott,tpt,EV_SYNC)==-1)
		{				
			dx_fileclose(iott.io_fhandle); //close file					
			throw "Error Playing File ("+Filename+")";							
		}
	}

	//close the file
	if ((dx_fileclose(iott.io_fhandle)) == -1)
	{
		throw "Error Closing File For Play ("+Filename+")";			
	}

	// Examine bitmap to determine if digits caused termination
	if((term = ATDX_TERMMSK(mChannelHandle)) == AT_FAILURE)//error retrieving mask info
	{
		throw "Error Retreiveing Mask Info";
	} 
	else if(term == TM_LCOFF)//customer hungup
	{
		//debugstring.Format("Customer Hungup return -9");
		return HANGUP;
	}
	else if(term == TM_DIGIT)//terminated with digit
	{
		//debugstring.Format("Specific digit received return -8");
		return DIGITTERM;
	}
	else if(term == TM_MAXTIME)
	{
		//debugstring.Format("Maximum function time return -8");
		return MAXTIME;
	}
	else if(term == TM_MAXDTMF)
	{
		//debugstring.Format("Maximum DTMF count return -8");
		return MAXDTMF;
	}
	else if(term == TM_NORMTERM)
	{
		//debugstring.Format("Normal Termination (for dx_dial( ), dx_sethook( )) return -8");
		return NORMTERM;
	}
	else if(term == TM_MAXSIL)
	{
		//debugstring.Format("Maximum period of silence return -8");
		return MAXSILENCE;
	}
	else if(term == TM_MAXNOSIL)
	{
		//debugstring.Format("Maximum period of non-silence return -8");
		return MAXNONSILENCE;
	}
	else if(term == TM_IDDTIME)
	{
		//debugstring.Format("Inter-digit delay return -8");
		return INTERDIGITDELAY;
	}
	else if(term == TM_PATTERN)
	{
		//debugstring.Format("Pattern matched silence off return -8");
		return PATTERNTERM;
	}
	else if(term == TM_USRSTOP)
	{
		//debugstring.Format("Function stopped by user return -8");
		return USERSTOP;
	}
	else if(term == TM_EOD)
	{
		//debugstring.Format("End of Data reached on playback return -10");
		return EODTERM;
	}
	else if(term == TM_TONE)
	{
		//debugstring.Format("Tone-on/off event return -8");
		return TONETERM;
	}
	else if(term == TM_ERROR)
	{
		return ERRORTERM;
	}
	else
		return SUCCESS;	
}
*/

void DialogicLegacy::RecordFile(CString Filename)
{
    CString debugstring;
	CString tempstring;
	CString	Tracestring;
	long term;

	//set up DV_TPT with a termmask
	DV_TPT tpt[] = {{IO_CONT, DX_MAXDTMF, 1, TF_MAXDTMF},
	{IO_CONT, DX_LCOFF, 3, TF_LCOFF|TF_10MS},
	{IO_EOT,  DX_MAXSIL, 20, TF_MAXSIL|TF_SETINIT,40}};
	
	
	//FOR DOING LENGTHLY RECORDINGS
	/*DV_TPT tpt[] = {{IO_EOT,  DX_MAXSIL, 80, TF_MAXSIL|TF_SETINIT,40}};*/

	// add the path onto front of Filename
	tempstring.Format("%s%s",mSoundFilePath,Filename);

	//now record the file
	if (dx_recwav(mChannelHandle,tempstring,tpt, (DX_XPB *)NULL,PM_TONE|EV_SYNC) == -1)
	{			
		throw "Error Recording file "+tempstring;			
	}
	// Examine bitmap to determine if digits caused termination
	if((term = ATDX_TERMMSK(mChannelHandle)) == AT_FAILURE)
	{
		throw ATDV_ERRMSGP(mChannelHandle);	
	} 
	else if(term == TM_LCOFF)
	{
		throw "HANGUP";
	}	
}


int DialogicLegacy::Dial(CString PhoneNumber, int numberOfRings)
{
	DX_CAP capp;

	/* Clear DX_CAP structure */
	dx_clrcap(&capp);
	capp.ca_nbrdna = numberOfRings;
	capp.ca_dtn_pres = 100;
    capp.ca_dtn_npres = 300;
    capp.ca_dtn_deboff = 10;
    capp.ca_noanswer = (numberOfRings-1)*(300);
    capp.ca_intflg = DX_PVDENABLE;
	capp.ca_cnosig = 2000;

	/*//DOING THIS STUFF IN INIT LINE NOW
	if ( dx_deltones(mChannelHandle) == -1 ) 
	{
		throw "Unable to delete tones";
	}	
	// Initialize Perfect Call Analysis
	if (dx_initcallp( mChannelHandle ) == -1) 
	{
		throw "Unable to initialize Perfect Call Analysis";	
	}
	*/
	

	int car = 0;
	if ((car = dx_dial( mChannelHandle, PhoneNumber,&capp, DX_CALLP|EV_SYNC))==-1) 
	{
		throw "Error Dialing "+PhoneNumber;	
	}
		
	return car;
}

void DialogicLegacy::Dial(CString PhoneNumber)
{

	if((dx_dial(mChannelHandle,PhoneNumber,(DX_CAP*)NULL, EV_SYNC)) == -1)
		throw "Error Dialing "+PhoneNumber;			

}

//******************************************************************//
//***							SendFax		                     ***//
//******************************************************************//
void DialogicLegacy::SendFax(CString number_to_dial, CString file_to_send)
{

	/*
	CString Tracestring;
	//Tracestring.Format("Sending Fax to %s",number_to_dial);
	//Trace(Tracestring);
	
	BOOL completed = FALSE;
	int status;
	int number_of_events = 0;
	GFQRECORD qrec;
	GRT_EVENT grt_event;
	GRT_INFO_DATA info;
	int		returncode;

	status = grtInit( m_nLineNum, m_nLineNum, GRT_CALL_TERM_ENABLE);
	if ( status != GRT_SUCCESS ) {
		Tracestring.Format( "GRT_INIT failed to init channel %d: status: %d\n", m_nLineNum, status );
		Trace(Tracestring);
		return -1;
	}                                       

	gfqClearRec(&qrec);
	
	qrec.operation = GFQDIAL_SEND;
	qrec.notify = GFQNOTIFY_ONERROR | GFQNOTIFY_ONSUCCESS | GFQNOTIFY_ONATTEMPT;
	qrec.retry_counter=0;
	qrec.rate=GFQMAX_RATE;
	qrec.number_calls=1;
	qrec.cd_timeout=45;
	qrec.source_type=GFQSINGLE_DOC;

	char *csid_name;
	csid_name = "IntelliFax";
	char testfax[22];

	strncpy(testfax,number_to_dial,21);	
	
	char testfile[64];
	
	strncpy(testfile,file_to_send,63);	
	
	strcpy((char *)qrec.fn_send,(char *)testfile);
	strcpy((char *)qrec.phone_no,(char *)testfax);
	strcpy((char *)qrec.csid,(char *)csid_name);
	
	status = grtSubmitFax( m_nLineNum, &qrec );
	if ( status != GRT_SUCCESS )
	{
		Tracestring.Format("Fax Status: %d   grtSubmitFax Failure", status);
		Trace(Tracestring);
		Sleep(2000);
		return -1;
	}

	//START ANOTHER THREAD HERE TO PROCESS ALL THE EVENTS
	while( ! completed )
	{
		if( grtGetEvent( m_nLineNum, m_nLineNum, &grt_event ) == GRT_SUCCESS ) 
		{
			
			switch ( grt_event.event_type )
			{
				case GRT_CALL_TERM:
					number_of_events++;
					grtProcessCallTermEvent( &grt_event, &qrec );
					Tracestring.Format("Received CSID: %s", qrec.received_csid);
					Trace(Tracestring);
					Sleep(2000);
					completed = TRUE;
					break;
			
				case GRT_INFO_EXCHANGE:
					number_of_events = 1;
					if( ( grtProcessInfoEvent( &grt_event, &info ) ) != GRT_SUCCESS )
					{
						Tracestring.Format("Could not process Info Exchange");
						Trace(Tracestring);
						Sleep(2000);
					}
					else
					{
						Tracestring.Format("\nCSID: %s", info.rcsid);
						Trace(Tracestring);
						if(sizeof(info.nsf > 0))
						{
							Tracestring.Format("NSF: %s", info.nsf);
							Trace(Tracestring);
						}
					}
					break;

				default:
					Tracestring.Format("Unknown Event %d returned", grt_event.event_type);
					Trace(Tracestring);
					Sleep(2000);
					completed = TRUE;
			}
		}

		Sleep( 500 );
	}       
	//STOP THE THREAD HERE, DONE PROCESSING

	returncode = StopFax();

	if(returncode == -1)
		return -2;
	else
		return 1;
	*/
}

void DialogicLegacy::OpenFax()
{
	/*
	GFQRECORD qrec;
	char GFAX_Queue_File[ 256 ];
	int channel = -1;
	int status = -1;
	CString Tracestring;

	Tracestring.Format("Allocating FaxLine");
	Trace(Tracestring);
	Sleep(2000);

	// check to see if Dispatcher is running
	status = gfdQueryStatus( 0 );
	if ( status )
	{
		Tracestring.Format( "GammaLink service is not started.\n" );
		Trace(Tracestring);
		Sleep(2000);
		return -1;
	}

	sprintf( GFAX_Queue_File, "%sgfax.$qu",databasepath);
	while ( GFQSUCCESS == gfqFindFirst( GFAX_Queue_File, &qrec, GFQCTRL_LIST, GFQLIST_START, "" ) )
	{
		Sleep( 500 );
	}
		
	return 1;
	*/
}

void DialogicLegacy::StopFax()
{
	/*
	CString Tracestring;
	int status;

	status = grtStop( m_nLineNum, m_nLineNum );
	if ( status != GRT_SUCCESS )
	{
		Tracestring.Format("Status: %d   Error stopping GRT channel %d: %d", m_nLineNum, status );
		Trace(Tracestring);
		Sleep(2000);
		return -1;
	}
	else
	{
		Tracestring.Format("GRT successfully stopped.");
		Trace(Tracestring);
		Sleep(2000);
	}

	return 1;
	*/
}

CString DialogicLegacy::GetCallerIdXml()
{
	return mCallerIdXml;
}

CString DialogicLegacy::GetChannelNames()
{
		
		int allboardnames[500];
		int bddev; //board number
        int IRQ;
        int i;
        int j;
        int tempi=0;
        long baseaddress;
        char **chnames; //channel name
        long subdevs; //number of total lines
        CString tempboardname;
		CString channelNames;
        
        CString Tracestring;
        
        //get the number of boards in system
		int numBoards = GetBoardCount();
		if (numBoards != -1)
        {             
            //loop as many times as there are boards
            //and build an array of line and channel numbers
            for (j = 1; j <= numBoards; j++)
            {
                // build the board name and open the board device to get number of channels
                tempboardname.Format("dxxxB%d",j);
                
                if ((bddev = dx_open(tempboardname,NULL)) == -1) //open the board
                {  
					CString str;				
					str.Format("Unable to get channels, for board: %s, board not started or failure",tempboardname);
					throw (const char *)str;                    
                }
                
                allboardnames[j] = bddev; //get board name
                chnames = ATDX_CHNAMES(bddev); //get channel names
                subdevs = ATDV_SUBDEVS(bddev); //get phoneline total
                IRQ = ATDV_IRQNUM( bddev ); //get the irq
                baseaddress = ATDX_PHYADDR( bddev );
                
				for(i=0; i < subdevs; i++)
                {
					channelNames+=*(chnames +i);
					channelNames+=";";
				}
                dx_close(allboardnames[j]);
            }
		}
	return channelNames;
}



int DialogicLegacy::GetBoardCount()
{
	int numvoxbrds = 0;

	//get the number of boards in system
        if ( sr_getboardcnt(DEV_CLASS_VOICE, &numvoxbrds) == -1)
        { 
            // error retrieving number of voice boards
			throw "No voice boards found";            
        }

		return numvoxbrds;
}

}
}