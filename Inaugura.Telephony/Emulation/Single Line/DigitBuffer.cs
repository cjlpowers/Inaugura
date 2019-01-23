using System;

namespace Inaugura.Telephony.Emulation
{
	/// <summary>
	/// Summary description for DigitBuffer.
	/// </summary>
	internal class DigitBuffer
	{
		public event System.EventHandler BufferChanged;

		private string mBuffer = string.Empty;

		public string CurrentBuffer
		{
			get
			{
				return this.mBuffer;
			}
		}

		public DigitBuffer()
		{		
		}

		public void PlaceDigit(string digit)
		{
			lock(this.mBuffer)
			{
				this.mBuffer+=digit;
			}
			this.CallBufferChanged();
		}

		public string GrabDigit()
		{
			string digit = string.Empty;
			lock(this.mBuffer)
			{
				if(this.mBuffer.Length == 0)
					return string.Empty;

				digit = mBuffer.Substring(0,1);				
				mBuffer = mBuffer.Substring(1);
			}
            			
			this.CallBufferChanged();
			return digit;
		}

		public string GrabDigits(int count)
		{
			string digits = string.Empty;

			for(int i =0; i < count; i++)
			{
				digits+= this.GrabDigit();
			}

			return digits;
		}

		protected void CallBufferChanged()
		{
			if(this.BufferChanged != null)
			{
				this.BufferChanged(this,null);
			}
		}

		public void Clear()
		{
			lock(this.mBuffer)
			{
				this.mBuffer = string.Empty;
			}
			this.CallBufferChanged();				
		}
	}
}
