#using <mscorlib.dll>

#pragma once

using namespace System;

namespace Inaugura
{
namespace Dialogic
{
	public __gc class InvalidUserInputException : public DialogicException
	{
	public:
		InvalidUserInputException(String __gc* message, String __gc* userInput);
		__property String __gc* get_UserInput();		
	protected:
		String __gc* mUserInput;
	};
}
}