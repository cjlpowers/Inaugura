using System;
using System.Collections;


namespace Inaugura.Telephony
{
	/// <summary>
	/// 
	/// </summary>
	public class TelephonyLineList : CollectionBase
	{
		#region Properties

		#endregion

		#region Accessors

		public TelephonyLine this [int index]
		{
			get
			{
				return (TelephonyLine)this.List[index];				
			}
		}		

		public TelephonyLine this [string lineName]
		{
			get
			{
				foreach(TelephonyLine l in this)
				{
					if(l.Name == lineName)
						return l;
				}				

				return null;
			}
		}		
		#endregion
				
		#region Methods
		public TelephonyLineList()
		{			
		}

		public void Add(TelephonyLine line)
		{
			if(!this.Contains(line))
				this.List.Add(line);
		}

		public void Remove(TelephonyLine line)
		{
			this.List.Remove(line);
		}

		public bool Contains(TelephonyLine line)
		{
			return this.List.Contains(line);
		}
		
		#endregion
		
	}
}

