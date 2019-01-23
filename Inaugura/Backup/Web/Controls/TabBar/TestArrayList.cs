using System;

namespace Inaugura.Web.Controls
{
	/// <summary>
	/// Summary description for TestArrayList.
	/// </summary>
	public class TestArrayList : System.Collections.ArrayList
	{
		public TestArrayList()
		{			
		}

		public override int Add(object value) 
		{
			return base.Add(value);
		}
	}
}
