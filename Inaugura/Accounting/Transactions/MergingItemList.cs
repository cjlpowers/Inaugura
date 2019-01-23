#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Inaugura.Accounting.Transactions
{
	public class MergingItemList : OrbisSoftware.Accounting.Transactions.ItemList
	{
		public MergingItemList()
		{
		}

		/// <summary>
		/// Adds a item to the list
		/// </summary>
		/// <param name="customerType">The item to be added</param>
		public override void Add(OrbisSoftware.Accounting.Transactions.Item item)
		{
			if (item is ServiceUsage)
			{
				ServiceUsage newServiceUsage = (ServiceUsage)item;
				if (!this.List.Contains(item))
				{
					// try to find the service usage with the same name
					foreach (OrbisSoftware.Accounting.Transactions.Item i in this)
					{
						if (i is ServiceUsage)
						{
							ServiceUsage existingServiceUsage = (ServiceUsage)i;
							if (existingServiceUsage.Name == newServiceUsage.Name && existingServiceUsage.PerMinuteRate == newServiceUsage.PerMinuteRate)
							{
								existingServiceUsage.CallDuration = existingServiceUsage.CallDuration.Add(newServiceUsage.CallDuration);
								existingServiceUsage.Amount += newServiceUsage.Amount;
								return;
							}
						}
					}
				}
			}

			
			base.Add(item);			
		}
	}
}
