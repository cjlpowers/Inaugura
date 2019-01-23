// DialogicNet.h
#include "stdafx.h"
#include "DialogicLine.h"

#using <mscorlib.dll>

using namespace System;

namespace Inaugura
{
namespace Dialogic
{
	public __sealed __gc class DialogicHardware
	{
	public:
		DialogicHardware(){};
		DialogicLine* Start()[];
		void Stop();
		static int GetBoardCount();
		static String __gc* GetChannelNames();
		static void StartDialogicService();
		static void StopDialogicService();
		static String* ParseChannelNames(String __gc* channelNames)[];		
		static bool IsDialogicServiceStarted();
		
	};
}
}


