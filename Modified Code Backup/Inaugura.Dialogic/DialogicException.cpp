#include "stdafx.h"
#include "dialogicexception.h"

namespace Inaugura
{
namespace Dialogic
{
	DialogicException::DialogicException(String __gc* message) : Exception(message)
	{			
	}

	DialogicException::DialogicException(String __gc* message, Exception __gc* innerException) : Exception(message, innerException)
	{
	}
}
}
