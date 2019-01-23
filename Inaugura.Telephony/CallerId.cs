using System;
using System.Xml;

namespace Inaugura.Telephony
{
	/// <summary>
	/// 
	/// </summary>
	public class CallerID
	{        
		#region Variables
		private string mPhoneNumber;
		private string mName;
		private string mFrameType;
        private string mDialedNumber;
        private DateTime mDateTime;
        private string mStatus;        
		#endregion

		#region Properties

        public string Status
        {
            get
            {
                return this.mStatus;
            }
        }

        public string Name
		{
			get
			{
				return this.mName;
			}
		}

		public string PhoneNumber
		{
			get
			{
				return mPhoneNumber;
			}
		}			

		public string FrameType
		{
			get
			{
				return mFrameType;
			}
		}			

		public DateTime DateTime
		{
			get
			{
				return mDateTime;
			}
		}			
		#endregion

		#region Get/Set Xml
		#endregion

		#region Methods
		public CallerID(string status, string name, string number, string dialedNumber, DateTime dateTime, string frameType)
		{
            this.mStatus = status;
            this.mName = name;
			this.mPhoneNumber = number;
            this.mDialedNumber = dialedNumber;
            this.mDateTime = dateTime;
			this.mFrameType = frameType;
		}

		public override string ToString()
		{
			if (this.Name != string.Empty && this.Name != null && this.PhoneNumber != null && this.mPhoneNumber != string.Empty)
				return string.Format("{0} [{1}]", this.Name, this.PhoneNumber);
			else
				return string.Empty;			
		}
		#endregion
	}
}
