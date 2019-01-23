using System;
using System.Collections.Generic;
using System.Text;

using Inaugura.Telephony;
using Inaugura.Telephony.Services;

namespace OrbisTel.Services.MyShield
{
    public class MyShieldService : IncommingService
    {
        #region Properties
        public override string Id
        {
            get 
            {
                return "MyShieldService";
            }
        }

        public override string Name
        {
            get 
            {
                return "MyShield DTMF Service";
            }
        }
        #endregion

        public MyShieldService(System.Xml.XmlNode node) : base(node)
		{	
		}


        protected override void ProcessService()
        {            
        }

        public override void SupplyLine(TelephonyLine line)
        {
            MyShieldIncommingLine myShieldLine = new MyShieldIncommingLine(this, line);
            this.Add(myShieldLine);
        }
    }
}
