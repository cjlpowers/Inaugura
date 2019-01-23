using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO.Ports;

namespace Inaugura.LCD
{
	/// <summary>
	/// Summary description for LCD.
	/// </summary>
	public class LCD
	{

		private const int ControlAddress = 890;
		private const int DataAddress = 888;

		/* For sending to the ports */
		[DllImport("inpout32.dll", EntryPoint="Out32")]
		private static extern void Output(int adress, int value);
		/* For receiving from the ports */
		[DllImport("inpout32.dll", EntryPoint="Inp32")]
		private static extern int Input(int adress);

		#region Properties
		public bool RS
		{
			get
			{
				int val = LCD.Input(LCD.ControlAddress);
				return ((val & 0x04) > 0);
			}
			set
			{
				if(this.RS == value) // is the value already set
					return;

				int val = LCD.Input(LCD.ControlAddress);
				if((val & 0x04) > 0)
					val -= 0x04;
				else
					val = val | 0x04;

				LCD.Output(LCD.ControlAddress,val);
			}
		}

		public bool EnabledFlag
		{
			get
			{
				int val = LCD.Input(LCD.ControlAddress);
				return !((val & 0x01) > 0);
			}
			set
			{
				if(this.EnabledFlag == value) // is the value already set
					return;

				int val = LCD.Input(LCD.ControlAddress);
				if((val & 0x01) > 0)
					val -= 0x01;
				else
					val = val | 0x01; 

				LCD.Output(LCD.ControlAddress,val);
			}
		}

		public int Data
		{
			get
			{
				int val = LCD.Input(LCD.DataAddress);
				return val;
			}
			set
			{				
				LCD.Output(LCD.DataAddress,value);
			}
		}

		public int Control
		{
			get
			{
				int val = LCD.Input(LCD.ControlAddress);
				return val;
			}
			set
			{				
				LCD.Output(LCD.ControlAddress,value);
			}
		}


		public string Line1
		{
			set
			{
				char[] ch_buffer = value.ToCharArray();						
				this.MoveToPosition(1,1);
		
				// Loop for sending data continuesly
				for(int a=0; a<ch_buffer.Length; a++)
				{
					this.Data = (int)ch_buffer[a];
					Thread.Sleep(1); //You can disable this little delay but your controller needs some time for execution, Look for the datasheet mine is "KSC0066"

					this.EnabledFlag = true;
					this.RS = true;
					Thread.Sleep(1); //You can disable this little delay but your controller needs some time for execution, Look for the datasheet mine is "KSC0066"

					this.EnabledFlag = false;
					this.RS = true;
					Thread.Sleep(1); //You can disable this little delay but your controller needs some time for execution, Look for the datasheet mine is "KSC0066"
				}								
			}
		}

		public string Line2
		{
			set
			{
				char[] ch_buffer = value.ToCharArray();						
				this.MoveToPosition(2,1);
		
				// Loop for sending data continuesly
				for(int a=0; a<ch_buffer.Length; a++)
				{
					this.Data = (int)ch_buffer[a];
					Thread.Sleep(1); //You can disable this little delay but your controller needs some time for execution, Look for the datasheet mine is "KSC0066"

					this.EnabledFlag = true;
					this.RS = true;
					Thread.Sleep(1); //You can disable this little delay but your controller needs some time for execution, Look for the datasheet mine is "KSC0066"

					this.EnabledFlag = false;
					this.RS = true;
					Thread.Sleep(1); //You can disable this little delay but your controller needs some time for execution, Look for the datasheet mine is "KSC0066"
				}								
			}
		}
		#endregion

		public LCD()
		{	
			InitDisplay();
		}

		private void InitDisplay()
		{	
			this.Data = 12;
			this.Flush();
		
			this.Data = 1;
			this.Flush();

			this.Data = 56;
			this.Flush();
		}

		public void Clear()
		{
			this.Data = 1;
			this.Flush();			
		}

		private void Flush()
		{
			this.EnabledFlag = true;
			this.RS = false;
			Thread.Sleep(1);

			this.EnabledFlag = false;
			this.RS = false;
			Thread.Sleep(1);
		}

		public void MoveToPosition(int line, int col)
		{
			this.RS = false;			
			if(line == 1)
			{
				/* Sets RAM address so that the cursor is positioned at a specific column of the 1st line. */
				this.Data = 127+col;
				this.Flush();
			}
			if(line == 2)
			{
				/* Sets RAM address so that the cursor is positioned at a specific column of the 2nd line. */
				this.Data = 191+col;
				this.Flush();
			}
		}
	}
}
