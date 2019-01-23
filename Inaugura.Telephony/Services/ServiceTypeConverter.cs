using System;
using System.Xml;
using System.ComponentModel;
using System.Globalization;


namespace Inaugura.Telephony.Services
{
	/// <summary>
	/// Summary description for PhoneNumberTypeConverter.
	/// </summary>
	public class ServiceTypeConverter : System.ComponentModel.ExpandableObjectConverter
	{

		public ServiceTypeConverter()
		{			
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)		
		{
			Type t = typeof(Service);
			if(destinationType == t)
				return true;
			
			return base.CanConvertTo(context,destinationType);												 
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			Type t = typeof(XmlNode);
			if(sourceType == t)
				return true;
			
			return base.CanConvertFrom(context,sourceType);												 
		}

		public override object ConvertTo(ITypeDescriptorContext context,CultureInfo culture, object value, Type destinationType)
		{
			Type t = typeof(XmlNode);
			if(destinationType == t && value is Service)
			{
				return ((Service)value).Xml;
			}
			
			
			return base.ConvertTo(context, culture, value, destinationType);	
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture,	object value)
		{			
			if(value is string)
			{				
				XmlNode node = (XmlNode)value;
				return Service.FromXml(node);				
			}			
			return base.ConvertFrom(context, culture, value);	
		}
	}
}
