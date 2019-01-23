#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mail;
using System.Xml;

using Inaugura;
using Inaugura.Telephony;
using Inaugura.RealLeads;


#endregion

namespace OrbisTel.Services.RealLeads
{
    class ClientCallHandler : CallHandler
    {
        private enum CallResult
        {
            NoAnswer,
            CallAccepted,
            CallRejected
        }

        public Language mLanguage = Language.Unknown;

        public Language PrimaryLanguage
        {
            get
            {
                return Language.English;
            }
            set
            {
                this.mLanguage = value;
            }
        }

        public ClientCallHandler(RealLeadsIncommingLine line, CallLog details) : base(line,details)
        {
        }

        public override void ProcessCall()
        {
            this.Line.ClearDigitBuffer();
			// play the welcome prompt
			this.Line.PlayMultiLingualPrompt("Welcome.wav", true);

			string propertyNumber = this.GetPropertyNumber();
            Inaugura.Log.AddLog("Listing Number: " + propertyNumber);

            this.CallDetails.AddActivityItem(string.Format("Entered listing number '{0}'", propertyNumber));
			this.Status = "Listing Number = " + propertyNumber;
			
            // try to get the listing
			//this.Listing = this.Service.RealLeadsData.ListingStore.GetListingByPinCode(this.Service.ZoneID, propertyNumber);			
			this.Listing = this.Service.GetListingByCode(propertyNumber);
			if (this.Listing == null)
            {
                this.CallDetails.AddActivityItem("Listing did not exist");
				this.Status = "Property number (" + propertyNumber + ") does not exist";
                this.Line.PlayMultiLingualPrompt("PropertyNumberEnterFail.wav", false);
				throw new InvalidInputException(propertyNumber, "Property number did not exist");
			}		

			// else the listing exists
			this.Agent = this.Service.GetAgent(Listing.AgentID);
			//this.Company = RealLeadsService.RealLeadsData.CompanyStore.GetCompany(this.Agent.CompanyId);

			// see if the listing has been setup already
			if (this.Listing.IsSetup == false)
			{
				ListingSetupCallHandler callHandler = new ListingSetupCallHandler(this.Line, this.CallDetails);
				callHandler.Listing = this.Listing;
				callHandler.Agent = this.Agent;
				callHandler.StatusChanged += new StatusHandler(this.handler_StatusChanged);
				callHandler.ProcessCall();
                ((RealLeadsService)this.Service).PreLoadListing((RealEstateListing)this.Listing);
				// now play the listing as tho the caller were a client
			}

            this.Line.ClearDigitBuffer();
            // play the listing greeting
			string termDigit = string.Empty;
            this.CallDetails.AddActivityItem("Playing the listing greeting");
			if (this.Line.PlayListingFile(this.Listing.AgentID, this.Listing.ID, this.Listing.GreetingPrompt, "*", out termDigit) == TelephonyResult.Terminated)
			{
				this.ProcessListingManagement();
				return;
			}			

			// now play the listing menu
            this.ProcessListingMenu();
        }

		protected void ProcessListingMenu()
        {
            System.Threading.Thread.Sleep(300);
            while (true)
            {
				string result = string.Empty;
                RealEstateListing realListing = (RealEstateListing)this.Listing;
                if(realListing.InformationPrompt != null && realListing.InformationPrompt != string.Empty)
                {
				    if (realListing.OpenHousePrompt != null && realListing.OpenHousePrompt != string.Empty)
    					result = this.Line.PlayMenu("Client_PropertyMenu03.wav", 3, "1", "2", "3","*");
	    			else
		    			result = this.Line.PlayMenu("Client_PropertyMenu02.wav", 3, "1", "2","*");
                }
                else
                    	result = this.Line.PlayMenu("Client_PropertyMenu01.wav", 3, "1", "*");


                    if (result == "1") // contact the owner
                    {
                        this.CallDetails.AddActivityItem("Caller selected 'Contact Property Owner'");
                        this.ContactAgent();
                        return;
                    }
                    else if (result == "2") // hear detailed information
                    {
                        this.CallDetails.AddActivityItem("Caller listening to Property Description");
                        this.Line.PlayListingFile(this.Listing, this.Listing.InformationPrompt, true);
                    }
                    else if (result == "3") // hear open house information
                    {
                        this.CallDetails.AddActivityItem("Caller listening to Open House Information");
                        this.Line.PlayListingFile(this.Listing, ((RealEstateListing)this.Listing).OpenHousePrompt, true);
                    }
                    else if (result == "*") // Login as the agent
                    {
                        this.CallDetails.AddActivityItem("Caller attempting to access Listing Management");
                        this.ProcessListingManagement();
                    }
                    else
                        throw new NotSupportedException("The selected option was not supported");
			}
        }

        private void ContactAgent()
        {
            bool playedPromo = false;
            // make sure the agent can take calls
            ContactSchedule[] activeSchedules = this.Agent.ContactSchedules[DateTime.Now];
            if (activeSchedules.Length > 0)
            {
                for(int i =0; i < 2 && i < activeSchedules.Length; i++)
                {
                    ContactSchedule schedule = activeSchedules[i];
                    if (schedule.ContactNumber != null && schedule.ContactNumber != string.Empty)
                    {
                        if (!playedPromo)
                        {
                            this.Line.PlayMultiLingualPrompt("WebPromo.wav", false);
                            playedPromo = true;
                        }
                        this.Line.PlayMultiLingualPrompt("Client_Connecting.wav", false);
                        CallResult result = ContactAgent(schedule);
                        if (result == CallResult.CallAccepted)
                            return;
                        else if (result == CallResult.CallRejected)
                            break;
                    }
                }
            }                            
            // call needs to be sent to voice mail
            this.ProcessVoiceMail();
        }

        private CallResult ContactAgent(ContactSchedule schedule)
        {
            if (schedule.ContactNumber != null && schedule.ContactNumber != string.Empty)
            {
                this.CallDetails.AddActivityItem(string.Format("Dialing Property Owner @ {0} [schedule {1}]", schedule.ContactNumber, schedule.Name));

                // flash to get a new line
                this.Line.Dial("&");

                // attempt to call the agent
                Inaugura.Log.AddLog(string.Format("Dialing Agent @ {0} Number of rings = {1}", schedule.ContactNumber, schedule.VoiceMailRings));
                DialAnalysis anal = DialAnalysis.NoAnswer;
                try
                {
                    anal = this.Line.Dial("L" + schedule.ContactNumber, schedule.VoiceMailRings);
                    Inaugura.Log.AddLog(string.Format("Result of dialing = {0}", anal.ToString()));
                }
                catch (Exception ex)
                {
                    Inaugura.Log.AddLog(string.Format("Exception thrown during dialing analysis: {0}", ex.ToString()));
                }

                if (anal == DialAnalysis.CallConnected)
                {
                    string result = string.Empty;
                    try
                    {
                        this.Line.PlayMultiLingualPrompt("Agent_IncommingCall.wav", false);
                        result = this.Line.PlayMenu("Agent_IncommingCallMenu.wav", 1, "1", "2");
                    }
                    catch (CallerException ex)
                    {
                        Inaugura.Log.AddLog(ex.ToString());
                        this.Line.Dial("&");
                        this.Line.Dial("&");
                        return CallResult.NoAnswer;
                    }

                    if (result == "1")
                    {
                        this.Line.Dial("&");
                        this.Line.PlayPrompt("Ring.wav", false);

                        this.CallDetails.AddActivityItem("Property Owner accepted call");

                        this.CallDetails.AgentAccepted = true;
                        return CallResult.CallAccepted;
                    }
                    else if (result == "2") // agent wants call sent to voice mail
                    {
                        this.Line.Dial("&");
                        this.Line.Dial("&");
                        return CallResult.CallRejected;
                    }
                }
                // not answered
                Inaugura.Log.AddLog("Not Answered");
                Inaugura.Log.AddLog("Here");
                System.Threading.Thread.Sleep(100);
                this.Line.Dial("&");
                this.Line.Dial("&");
                return CallResult.NoAnswer;
            }
            else
            {
                return CallResult.NoAnswer;
            }
        }

		private void ProcessListingManagement()
		{
            if (AgentCallHandler.ValidateAgent(this.Line, this.Agent))
            {
                AgentCallHandler handler = new AgentCallHandler(this.Line, this.CallDetails, this.Agent, this.Listing);
                handler.StatusChanged += new StatusHandler(handler_StatusChanged);
                handler.ProcessCall();
                return;
            }
            else
            {
                this.CallDetails.AddActivityItem("Caller faild to login to Listing Management");
            }
		}

        #region Regular Call
        /// <summary>
        /// Get the listing code
        /// </summary>
        /// <returns></returns>
        private string GetPropertyNumber()
        {
			const int PropertyNumberLength = 4;            	

			this.Line.PlayMultiLingualPrompt("PropertyNumberEnter01.wav", true);

			// Try getting the property number
            string pressedTermDigit = string.Empty;
			string propertyNumber = this.Line.GetDigits(PropertyNumberLength, TimeSpan.FromSeconds(8), TimeSpan.FromSeconds(4), "*#", out pressedTermDigit);
            Inaugura.Log.AddLog(string.Format("Property Number: {0}", propertyNumber));

			if (pressedTermDigit.Contains("*"))
                return "*";			

			if (propertyNumber.Length == PropertyNumberLength)
				return propertyNumber;
			
            // the number was not valid 
            this.Line.ClearDigitBuffer();

            if (propertyNumber.Length == 0)
				this.Line.PlayMultiLingualPrompt("PropertyNumberEnter02.wav", true);
			else if (propertyNumber.Length != PropertyNumberLength)
			{
				this.Line.PlayMultiLingualPrompt("PropertyNumberIncorrectDigits.wav", false);
				this.Line.PlayMultiLingualPrompt("PropertyNumberEnter02.wav", true);
			}

			// Try getting the listing code
            pressedTermDigit = string.Empty;
			propertyNumber = this.Line.GetDigits(PropertyNumberLength, TimeSpan.FromSeconds(8), TimeSpan.FromSeconds(3), "*#", out pressedTermDigit);
			if (pressedTermDigit.Contains("*"))
                return "*";

			if (propertyNumber.Length == PropertyNumberLength)
				return propertyNumber;
			else
            {
                // play the failure to enter listing code prompt
				this.Line.PlayMultiLingualPrompt("PropertyNumberEnterFail.wav", false);
				throw new InvalidInputException(propertyNumber, "Caller faild to enter a valid property number");
			}
        }

		protected void ProcessVoiceMail()
		{
            this.CallDetails.AddActivityItem("Call sent to voice mail");

			System.Threading.Thread.Sleep(700);
			this.CallDetails.SentToVoiceMail = true;
			string fileName = Guid.NewGuid().ToString() + ".wav";
			string filePath = RealLeadsService.TempFilesDirectory + fileName;
            DateTime start = DateTime.Now;
			try
			{
				//this.Line.PlayListingFile(this.Listing, this.Listing.VoiceMailGreetingPrompt, true);
				this.Line.PlayVoiceMailGreeting(this.Listing);
                start = DateTime.Now;
				this.Line.Record(filePath, false, TimeSpan.FromSeconds(120),TimeSpan.FromSeconds(0),false);
				//TODO ensure that only messages of a least a certain length are saved		                
			}		
			finally
			{
                DateTime stop = DateTime.Now;
                TimeSpan messageDuration = stop - start;
                this.CallDetails.AddActivityItem(string.Format("Caller has left a {0} second voice message", Convert.ToInt32(messageDuration.TotalSeconds)));

				if (fileName != string.Empty && System.IO.File.Exists(filePath))
				{
					byte[] data = System.IO.File.ReadAllBytes(filePath);
					Inaugura.File file = new Inaugura.File();
					file.Data = data;
					file.FileName = fileName;
					
					VoiceMail vm = new VoiceMail(this.Agent.ID, file.ID, this.Line.CallerID.ToString() );
					this.Service.SaveVoiceMail(vm, file);

					System.IO.File.Delete(filePath);
				}
			}

			/*
			// try and send a email notification to the agent
			if (this.Agent.VoiceMailEmail && this.Customer != null && this.Customer.Email != string.Empty)
			{
				// send email
				string link = "https://secure.inaugura.ca/Services/RealLeads/VoiceMailRedirect.aspx?data=" + System.Web.HttpUtility.UrlEncode(vm.ID);

				MailMessage objEmail = new MailMessage();
				objEmail.To = this.Customer.Email;
				objEmail.From = "realleads@inaugura.ca";
				objEmail.Cc = "";
				objEmail.Subject = "Voice Mail";
				objEmail.Body = "You have received a new voice mail. Click the following link to hear it now...\n" + link;
				objEmail.Priority = MailPriority.High;
				SmtpMail.SmtpServer = this.Service.Details["SMTP"];
				try
				{
					SmtpMail.Send(objEmail);
				}
				catch (Exception ex)
				{
					Log.AddError("Unable to send voice mail email: " + ex.ToString());
				}
			}
			*/
		}
        #endregion

        void handler_StatusChanged(object sender, StatusEventArgs e)
        {
            this.Status = e.StatusSoruce.Status;
        }
    }
}
