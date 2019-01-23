#include "stdafx.h"
#include "HangupException.h"

namespace Inaugura
{
namespace Dialogic
{
	HangupException::HangupException(String __gc* message) : DialogicException(message)
	{
	}
}
}