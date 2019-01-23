#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

using Inaugura.Telephony.Services;

namespace OrbisSwitch
{

	class ServiceManagerTreeNode: StatusTreeNode
	{
		public ServiceManagerTreeNode(ServiceManager manager) : base(manager)
		{
			this.ImageIndex = 0;
		}
	}

}