#include "stdafx.h"
#include "InvalidInputException.h"

namespace Inaugura
{
namespace Dialogic
{
	DialogicInvalidUserInputException::DialogicInvalidUserInputException(String __gc* message, String __gc* userInput) : Exception(message)
	{
		this->mUserInput = userInput;
	}

	String __gc* DialogicInvalidUserInputException::get_UserInput()
	{
		return this->mUserInput;
	}
}
}