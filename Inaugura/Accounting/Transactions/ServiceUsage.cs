#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#endregion

namespace Inaugura.Accounting.Transactions
{
	public class ServiceUsage : OrbisSoftware.Accounting.Transactions.Item
	{
		#region Variables 
		private TimeSpan mCallDuration = new TimeSpan();
		private float mPerMinuteRate = 0.0f;
		#endregion

		#region Properties
		/// <summary>
		/// The per minute price
		/// </summary>
		/// <value></value>
		public float PerMinuteRate
		{
			get
			{
				return this.mPerMinuteRate;
			}
			set
			{
				this.mPerMinuteRate = value;
			}
		}

		/// <summary>
		/// The call duration
		/// </summary>
		/// <value></value>
		public TimeSpan CallDuration
		{
			get
			{
				return this.mCallDuration;
			}
			set
			{
				this.mCallDuration = value;
			}
		}

		#region IXmlable Members
		/// <summary>
		/// Conversion to and from xml
		/// </summary>
		/// <value></value>
		public override System.Xml.XmlNode Xml
		{
			get
			{
				XmlElement pe = (XmlElement)base.Xml;
				XmlDocument xmlDoc = pe.OwnerDocument;

				pe.SetAttribute("perMinuteRate", this.PerMinuteRate.ToString());
				pe.SetAttribute("seconds", ((int)this.CallDuration.TotalSeconds).ToString());
				
				return pe;
			}
			set
			{
				base.Xml = value;

				XmlNode node = value;
				if (node != null)
				{
					XmlAttribute a;
					if ((a = node.Attributes["perMinuteRate"]) != null)
						this.PerMinuteRate = float.Parse(a.Value);

					if ((a = node.Attributes["seconds"]) != null)
						this.CallDuration = new TimeSpan(0,0,int.Parse(a.Value));					
				}
			}
		}
		#endregion
		#endregion

		public ServiceUsage() : this("",new TimeSpan(),0.0f)
		{
		}

		public ServiceUsage(string name, TimeSpan callDuration, float perMinuteRate) : base(name,(float)Math.Round(callDuration.TotalMinutes*perMinuteRate,2))
		{
			this.PerMinuteRate = perMinuteRate;
			this.CallDuration = callDuration;
		}
	}
}
