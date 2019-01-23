#using <mscorlib.dll>

#pragma once

using namespace System;

namespace Inaugura
{
namespace Dialogic
{
	public __gc class DialogicException : public Exception
	{
	public:
		DialogicException(String __gc* message);
		DialogicException(String __gc* message, Exception __gc* innerException);
	};
}
}