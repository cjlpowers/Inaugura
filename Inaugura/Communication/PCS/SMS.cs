using System;
using System.Web.Mail;

namespace Inaugura.Communication.PCS
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class SMS
	{
		public SMS()
		{			
		}

		static public void SendTextMessage(string phoneNumber, string title, string textMessage, string network, string fromAddress, string smtpServer)
		{
			string email = "";
			if(network == "Rogers AT&T Wireless")
			{
                email = phoneNumber+"@pcs.rogers.com";
			}
			else if(network == "Bell Mobility")
			{
				email = phoneNumber+"@txt.bellmobility.ca";
			}	
			else if(network == "Telus Mobility")
			{
				email = phoneNumber+"@msg.telus.com";
			}	
			else
				throw new ApplicationException("Network ("+network+") not supported");

			SmtpMail.SmtpServer = smtpServer;
			MailMessage m = new MailMessage();
			m.To = email;
			m.From = fromAddress;
			m.Subject = title;
			m.Body = textMessage;
			SmtpMail.Send(m);
		}
	}
}
