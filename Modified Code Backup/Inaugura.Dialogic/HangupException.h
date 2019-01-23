#include "DialogicException.h"
#using <mscorlib.dll>

#pragma once

using namespace System;

namespace Inaugura
{
namespace Dialogic
{
	public __sealed __gc class HangupException : public DialogicException
	{
	public:
		HangupException(String __gc* message);
	};
}
}