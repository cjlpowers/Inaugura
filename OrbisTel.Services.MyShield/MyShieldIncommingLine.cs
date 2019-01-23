using System;
using System.Collections.Generic;
using System.Text;

using Inaugura.Telephony;
using Inaugura.Telephony.Services;


namespace OrbisTel.Services.MyShield
{
    internal class MyShieldIncommingLine: IncommingServiceLine
    {
        public MyShieldIncommingLine(Service service, TelephonyLine line)
            : base(service, line)
        {
        }

        protected override void ProcessIncommingCall()
        {
            try
            {
                // START OF CALL


                // PAT PUT CODE HERE
                this.Line.GiveDialTone();
                this.Line.GetDigits(5,TimeSpan.FromSeconds(7),TimeSpan.FromSeconds(3));

                // END OF CALL
            }
            catch (Exception ex)
            {
                // some error occured.
            }
        }
    }
}
