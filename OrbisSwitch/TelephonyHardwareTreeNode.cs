#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

using Inaugura.Telephony;

namespace OrbisSwitch
{

	class TelephonyHardwareTreeNode: StatusTreeNode
	{
		public TelephonyHardwareTreeNode(TelephonyHardware hardware) : base(hardware)
		{
			this.Tag = hardware;
			this.ImageIndex = 1;
			this.SelectedImageIndex = 1;			
		}
	}

}