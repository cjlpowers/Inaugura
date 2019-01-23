#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Inaugura.RealLeads
{
	/// <summary>
	/// An exception that is thrown when a schedule being added overlaps with an existing schedule
	/// </summary>
	public class ScheduleOverlapException : ArgumentException
	{
		public ScheduleOverlapException()
		{
		}

		public ScheduleOverlapException(string message) : base(message)
		{
		}
	}
}
