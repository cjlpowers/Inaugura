#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using OrbisSoftware.Accounting.Transactions;
#endregion

namespace Inaugura.Accounting.Transactions
{
	public class MonthlyInvoice : OrbisSoftware.Accounting.Transactions.Invoice
	{
		public enum MonthlyInvoiceStatus
		{
			NotIssued,
			Outstanding,
			Paid
		}

		#region Variables
		private MergingItemList mItemList = new MergingItemList();	// the item list
		private MonthlyInvoiceStatus mStatus = MonthlyInvoiceStatus.NotIssued; 
		private float mPreviousBalance = 0.0f;		// The previous balance before the new charges
		private float mAdjustments = 0.0f;			// Any adjustments
		private float mPayments = 0.0f;				// Payments made since last invoice
		private float mOverdueAmount = 0.0f;		// Overdue charges
		#endregion

		#region Properties
		/// <summary>
		/// The items contained in this invoice
		/// </summary>
		/// <value></value>
		public override ItemList Items
		{
			get
			{
				return this.mItemList;
			}			
		}

		/// <summary>
		/// The status of the invoice
		/// </summary>
		/// <value></value>
		public MonthlyInvoiceStatus Status
		{
			get
			{
				return this.mStatus;
			}
			set
			{
				this.mStatus = value;
			}
		}

		/// <summary>
		/// The date that the payment for this invoice is due
		/// </summary>
		/// <value></value>
		public DateTime DueDate
		{
			get
			{
				return this.Terms.GetDueDate(this.Date);
			}
		}

		/// <summary>
		/// Adjustments to be made 
		/// </summary>
		/// <value></value>
		public float Adjustments
		{
			get
			{
				return this.mAdjustments;
			}
			set
			{
				this.mAdjustments = value;
			}
		}

		/// <summary>
		/// Overdue amount
		/// </summary>
		/// <value></value>
		public float OverdueAmount
		{
			get
			{
				return this.mOverdueAmount;
			}
			set
			{
				this.mOverdueAmount = value;
			}
		}

		/// <summary>
		/// Payments
		/// </summary>
		/// <value></value>
		public float Payments
		{
			get
			{
				return this.mPayments;
			}
			set
			{
				this.mPayments = value;
			}
		}


		/// <summary>
		/// Previous Balance
		/// </summary>
		/// <value></value>
		public float PreviousBalance
		{
			get
			{
				return this.mPreviousBalance;
			}
			set
			{
				this.mPreviousBalance = value;
			}
		}

		/// <summary>
		/// Total amount due
		/// </summary>
		/// <value></value>
		public float TotalAmountDue
		{
			get
			{
				return this.Amount + this.PreviousBalance + this.Adjustments;
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

				pe.SetAttribute("status", this.Status.ToString());
				if(this.PreviousBalance != 0.0f)
					pe.SetAttribute("previousBalance", this.PreviousBalance.ToString());
				if (this.Adjustments != 0.0f)
					pe.SetAttribute("adjustments",this.Adjustments.ToString());
				if (this.Payments != 0.0f)
					pe.SetAttribute("payments",this.Payments.ToString());


				pe.AppendChild(xmlDoc.ImportNode(this.Items.Xml, true));

				return pe;
			}
			set
			{
				base.Xml = value;

				XmlNode node = value;
				if (node != null)
				{
					XmlAttribute a;
					if((a = node.Attributes["status"]) != null)
						this.Status = (MonthlyInvoice.MonthlyInvoiceStatus)Enum.Parse(typeof(MonthlyInvoice.MonthlyInvoiceStatus),a.Value);

					if ((a = node.Attributes["previousBalance"]) != null)
						this.PreviousBalance = float.Parse(a.Value);

					if ((a = node.Attributes["adjustments"]) != null)
						this.Adjustments = float.Parse(a.Value);

					if ((a = node.Attributes["payments"]) != null)
						this.Payments = float.Parse(a.Value);

					if (node["Items"] != null)
						this.mItemList = (MergingItemList)ItemList.FromXml(node["Items"]);
				}
			}
		}
			#endregion

		#endregion

		/// <summary>
		/// Constructor (sets the date to last day in the current month
		/// </summary>
		public MonthlyInvoice() : this(new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.DaysInMonth(DateTime.Now.Year,DateTime.Now.Month)))
		{				
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="date">The last day of the month which this invoice covers</param>
		public MonthlyInvoice(DateTime date) : base(OrbisSoftware.Accounting.Terms.Net30Days)
		{
			this.Date = date;
			this.Taxes.Add(OrbisSoftware.Accounting.Tax.GST);
			this.Taxes.Add(OrbisSoftware.Accounting.Tax.PST);
		}
	}
}
