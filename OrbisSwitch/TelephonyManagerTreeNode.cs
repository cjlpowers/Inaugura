#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

using Inaugura.Telephony;

namespace OrbisSwitch
{

	class TelephonyManagerTreeNode: StatusTreeNode
	{
		public TelephonyManagerTreeNode(TelephonyManager manager) : base(manager)
		{
			this.ImageIndex = 0;
		}
	}

}