using System;
using System.Xml;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

namespace Inaugura.Xml
{
	/// <summary>
	/// Summary description for IXmlable.
	/// </summary>
    /// <remarks>Any object which implements this interface should also provide a constructor which accepts a xml node as a single parameter</remarks>
	public interface IXmlable
	{
		[Browsable(false)]
		XmlNode Xml
		{
			get;
		}
	}
}
