#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#endregion

namespace Inaugura.Communication.Messaging
{
	//public delegate XmlResponse RequestProcessor(XmlRequest request);
    //public delegate void ResponseCallback(XmlResponse response);

    public interface IMessageProcessor
	{
		bool SupportsRequest(XmlMessage request);
		XmlMessage ProcessRequest(XmlMessage request);
	}
}
