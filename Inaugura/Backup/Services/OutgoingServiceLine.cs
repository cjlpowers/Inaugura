using System;
using System.Threading;

using Inaugura.Telephony;

namespace Inaugura.Telephony.Services
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class OutgoingServiceLine : ServiceLine
	{	
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="service">The Service</param>
		/// <param name="line">The Telephony Line</param>
		public OutgoingServiceLine(Service service, TelephonyLine line) : base(service,line)
		{		
		}			


		/*
		protected bool IsUserListening(OrbisTel.Clients.IUser u)
		{			
			if (u.Language == Language.English)
			{
				this.Line.PlayText("ATT DTNV 1.3 Mike", "To verify you have received this call, please press star on the key pad now", true);
			}
			else if (u.Language == Language.French)
				this.Line.PlayText("ATT DTNV1.3 Alain", "Pour vous vérifier ont reçu cet appel serrent svp l'étoile maintenant", true);									

			string termDigit = "";
			this.Line.GetDigits(10,5,3,"*",ref termDigit);

			if(termDigit == "*")
				return true;
			else
				return false;
		}
		*/
	}
}
