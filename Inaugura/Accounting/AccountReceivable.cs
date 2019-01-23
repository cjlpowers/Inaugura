#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using OrbisSoftware.Accounting.Transactions;
using Inaugura.Accounting.Transactions;
#endregion

namespace Inaugura.Accounting
{
	public class AccountReceivable : OrbisSoftware.Accounting.Account
	{
		#region Variables
		private float mBalance = 0.0f;
		#endregion

		#region Properties	
		public override float Balance
		{
			get
			{
				return this.mBalance;
			}
			protected set
			{
				this.mBalance = value;
			}
		}
		#endregion

		#region IXmlable
		
		public override System.Xml.XmlNode Xml
		{
			get
			{
				XmlElement pe = (XmlElement)base.Xml;
				XmlDocument xmlDoc = pe.OwnerDocument;

				pe.SetAttribute("balance", this.Balance.ToString());
				
				return pe;
			}
			set
			{
				base.Xml = value;

				XmlNode node = value;
				if (node != null)
				{
					XmlAttribute a;

					if ((a = node.Attributes["balance"]) != null)
						this.Balance = float.Parse(a.Value);				
				}
			}
		}
		#endregion

		public AccountReceivable(float openingBalance, float balance) : base(openingBalance)
		{
			this.Balance = balance;
		}

		public AccountReceivable() : this(0.0f,0.0f)
		{

		}
	}
}
