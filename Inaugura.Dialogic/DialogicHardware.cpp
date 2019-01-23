// This is the main DLL file.

#include "stdafx.h"
#include "DialogicHardware.h"
#include "DialogicLine.h"

#using <System.DLL>
#using <System.ServiceProcess.DLL>

using namespace System;
using namespace System::Threading;
using namespace System::Collections;
using namespace System::ComponentModel;
using namespace System::ServiceProcess;

namespace Inaugura
{
namespace Dialogic
{	
	DialogicLine* DialogicHardware::Start() []
    {
		int numberOfBoards = DialogicHardware::GetBoardCount();			
		Console::WriteLine("Number of Bards Detected: {0}",numberOfBoards.ToString()); 
								
		String __gc* channelNames = DialogicHardware::GetChannelNames();				
		Console::WriteLine("Lines Detected: {0}",channelNames); 

		String* channels[] = DialogicHardware::ParseChannelNames(channelNames);
		DialogicLine* lines[] = new DialogicLine* [channels->Length];

		for(int i = 0; i < channels->Length; i++)
		{
			DialogicLine __gc* l = new DialogicLine(channels[i]);
			lines[i] = l;
		}		

		return lines;    
    }

	void DialogicHardware::StartDialogicService()
	{
		ServiceController __gc *dialogicService = new ServiceController("Dialogic");		
		if(dialogicService->Status != ServiceControllerStatus::Running)
		{
			Console::WriteLine("Starting {0}",dialogicService->DisplayName);
			dialogicService->Start();
			while(dialogicService->Status != ServiceControllerStatus::Running)
			{
				Console::Write(".");
				Thread::Sleep(1000);
				dialogicService->Refresh();
			}					
			Console::WriteLine("");
		}
	}

	void DialogicHardware::StopDialogicService()
	{
		ServiceController __gc *dialogicService = new ServiceController("Dialogic");		
		if(dialogicService->Status == ServiceControllerStatus::Running)
		{
			Console::WriteLine("Stoping {0}",dialogicService->DisplayName);
			dialogicService->Stop();
			while(dialogicService->Status == ServiceControllerStatus::Running)
			{
				Console::Write(".");
				Thread::Sleep(1000);
				dialogicService->Refresh();
			}					
			Console::WriteLine("");
		}	
	}

	void DialogicHardware::Stop()
    {
		this->StopDialogicService();		
    }

	bool DialogicHardware::IsDialogicServiceStarted()
	{
		ServiceController __gc *dialogicService = new ServiceController("Dialogic");		
			if(dialogicService->Status == ServiceControllerStatus::Running)
				return true;

			return false;
	}

	String* DialogicHardware::ParseChannelNames(String __gc* channelNames)[]
	{
		ArrayList __gc* a = new ArrayList();

			while(true)
			{
				int index = channelNames->IndexOf(";");
				if(index == -1)
					break;
				
				String __gc* channel = channelNames->Substring(0,index);
				a->Add(channel);				
				channelNames = channelNames->Substring(index+1);
			}

			String* channels[] = new String*[a->Count];
			for(int i = 0; i < channels->Length; i++)
			{
				channels[i]=a->get_Item(i)->ToString();
			}

			return channels;
	}

	int DialogicHardware::GetBoardCount()
	{
		try
		{
			return DialogicLegacy::GetBoardCount();
		}
		catch(char* str)
		{
			ApplicationException __gc *ex = new ApplicationException(str);
			throw ex;			
		}
	}

	String __gc* DialogicHardware::GetChannelNames()
	{
		
		try
		{
			String __gc* str = DialogicLegacy::GetChannelNames();
			return str;
		}
		catch(const char* str)
		{
			ApplicationException __gc *ex = new ApplicationException(str);
			throw ex;
		}
		return "";
	}	
}
}
