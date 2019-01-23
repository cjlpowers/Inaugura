#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using OrbisSoftware.Accounting.Transactions;

#endregion

namespace Inaugura.Accounting
{
	public class Payment : OrbisSoftware.Accounting.Transactions.Payment
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="transactions">The transactions that are specified in this </param>
		public Payment() : base()
		{
			this.Date = DateTime.Now;
		}
	}
}
