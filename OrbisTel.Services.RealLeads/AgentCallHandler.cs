#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

using Inaugura;
using Inaugura.Telephony;
using Inaugura.RealLeads;

#endregion

namespace OrbisTel.Services.RealLeads
{
	internal class AgentCallHandler : CallHandler
	{
		#region Variables
		private VoiceMailCollection mVoiceMails;
		#endregion

		#region Properties
		/// <summary>
		/// The voice mail of the current agent
		/// </summary>
		public VoiceMailCollection VoiceMails
		{
			get
			{
				return this.mVoiceMails;
			}			
		}
		#endregion

		public AgentCallHandler(RealLeadsIncommingLine line, CallLog details, Agent agent, Listing listing) : base(line,details)
		{
			if (agent == null)
				throw new ArgumentNullException("agent", "The agent can not be null");
			if(listing == null)
				throw new ArgumentNullException("listing", "The listing can not be null");


			this.Listing = listing;
			this.Agent = agent;
			this.mVoiceMails = new VoiceMailCollection();
		}

		public override void ProcessCall()
		{
            // throw an exception if the listing or agent have originated from cache
            if (this.Agent.FromCache || this.Listing.FromCache)
                throw new NotSupportedException("The agent management menu is not supported when either the agent or listing has originated from cache");

            Inaugura.Log.AddLog("Accessing Administration Menu");
			if (!this.Agent.Details.ContainsKey("HasUsedPhoneManagement") || this.Agent.Details["HasUsedPhoneManagement"] != "true")
			{
				this.Line.PlayMultiLingualPrompt("Agent_ManagementWelcome.wav", false);

				if (this.Agent.Details.ContainsKey("HasUsedPhoneManagement"))
					this.Agent.Details["HasUsedPhoneManagement"] = "true";
				else
					this.Agent.Details.Add("HasUsedPhoneManagement", "true");

				this.Service.UpdateAgent(this.Agent);
			}

			// Check the phone number association
			this.CheckCallerIDAssociation();

			// see if the agent has any new voice mails
			if (this.CheckVoiceMail())
			{
				this.PlayVocieMail(this.VoiceMails.GetVoiceMailOfStatus(VoiceMail.VoiceMailStatus.New));
			}
			
			RealEstateListing listing = (RealEstateListing)this.Listing;
			// play the main menu
			while (true)
			{
				string result = string.Empty;
				if (listing.OpenHousePrompt != null && listing.OpenHousePrompt != string.Empty)
					result = this.Line.PlayMenu("Agent_ManagementMenu02.wav", 3, "1", "2", "3", "4");
				else
					result = this.Line.PlayMenu("Agent_ManagementMenu01.wav", 3, "1", "2", "3", "4");

				if (result == "1") // voice mail
					this.ProcessVoiceMailMenu();
				else if (result == "2")
					this.ProcessModifyGreeting();
				else if (result == "3")
					this.ProcessModifyDescription();
				else if(result == "4")
					this.ProcessModifyOpenHouse();
				else
					throw new NoResponseException("The caller failed to select a valid menu option");
			}							
		}

		private void ProcessModifyGreeting()
		{
            this.CallDetails.AddActivityItem("Accessing Greeting Menu");
			RealEstateListing listing = this.Listing as RealEstateListing;
			while (true)
			{
				string result = this.Line.PlayMenu("Agent_GreetingModifyMenu.wav", 3, "1", "2", "*");
                if (result == "1")
                {
                    this.CallDetails.AddActivityItem("Playing existing Greeting");
                    this.Line.PlayListingFile(listing, listing.GreetingPrompt, true);
                }
                else if (result == "2")
                {
                    this.CallDetails.AddActivityItem("Recording new Greeting");
                    string fileName = Guid.NewGuid().ToString() + ".wav";
                    string filePath = RealLeadsService.TempFilesDirectory + fileName;
                    try
                    {
                        this.Line.PlayMultiLingualPrompt("Agent_GreetingBeginRecording.wav", false);
                        this.Line.Record(filePath, true);
                        File file = File.Load(filePath);

                        // save the file so we dont have to download it 
                        string finalPath = RealLeadsService.GetListingPromptPath(this.Agent.ID, this.Listing.ID, file.ID);
                        file.Save(finalPath);

                        // now update the listing
                        this.Listing.GreetingPrompt = file.ID;
                        this.Service.UpdateListing(this.Listing, file);
                    }
                    finally
                    {
                        if (System.IO.File.Exists(filePath))
                            System.IO.File.Delete(filePath);
                    }
                }
                else if (result == "*")
                    return;
                else
                    throw new NoResponseException("The caller failed to select a valid menu option");
			}
		}

		private void ProcessModifyDescription()
		{
            this.CallDetails.AddActivityItem("Accessing Listing Description Menu");
			RealEstateListing listing = this.Listing as RealEstateListing;
			while (true)
			{
				string result = this.Line.PlayMenu("Agent_DescriptionModifyMenu.wav", 3, "1", "2", "*");
                if (result == "1")
                {
                    this.CallDetails.AddActivityItem("Playing existing Listing Description");
                    this.Line.PlayListingFile(listing, listing.InformationPrompt, true);
                }
                else if (result == "2")
                {
                    this.CallDetails.AddActivityItem("Recording new Listing Description");
                    string fileName = Guid.NewGuid().ToString() + ".wav";
                    string filePath = RealLeadsService.TempFilesDirectory + fileName;
                    try
                    {
                        this.Line.PlayMultiLingualPrompt("Agent_DescriptionBeginRecording.wav", false);
                        this.Line.Record(filePath, true);
                        File file = File.Load(filePath);

                        // save the file so we dont have to download it 
                        string finalPath = RealLeadsService.GetListingPromptPath(this.Agent.ID, this.Listing.ID, file.ID);
                        file.Save(finalPath);

                        // now update the listing
                        this.Listing.InformationPrompt = file.ID;
                        this.Service.UpdateListing(this.Listing, file);
                    }
                    finally
                    {
                        if (System.IO.File.Exists(filePath))
                            System.IO.File.Delete(filePath);
                    }
                }
                else if (result == "*")
                    return;
                else
                    throw new NoResponseException("The caller failed to select a valid menu option");
			}
		}

		private void ProcessModifyOpenHouse()
		{
            this.CallDetails.AddActivityItem("Accessing Open House Menu");
			RealEstateListing listing = this.Listing as RealEstateListing;
			while (true)
			{
				if (listing.OpenHousePrompt != null && listing.OpenHousePrompt != string.Empty)
				{
					string result = this.Line.PlayMenu("Agent_OpenHouseModifyMenu.wav", 3, "1", "2", "3", "*");
                    if (result == "1")
                    {
                        this.CallDetails.AddActivityItem("Playing existing Open House Information");
                        this.Line.PlayListingFile(listing, listing.OpenHousePrompt, true);
                    }
                    else if (result == "2")
                    {
                        this.CallDetails.AddActivityItem("Reording new Open House Information");
                        string fileName = Guid.NewGuid().ToString() + ".wav";
                        string filePath = RealLeadsService.TempFilesDirectory + fileName;
                        try
                        {
                            this.Line.PlayMultiLingualPrompt("Agent_OpenHouseBeginRecording.wav", false);
                            this.Line.Record(filePath, true);
                            File file = File.Load(filePath);

                            // save the file so we dont have to download it 
                            string finalPath = RealLeadsService.GetListingPromptPath(this.Agent.ID, listing.ID, file.ID);
                            file.Save(finalPath);

                            // now update the listing
                            listing.OpenHousePrompt = file.ID;
                            this.Service.UpdateListing(listing, file);
                        }
                        finally
                        {
                            if (System.IO.File.Exists(filePath))
                                System.IO.File.Delete(filePath);
                        }
                    }
                    else if (result == "3")
                    {
                        this.CallDetails.AddActivityItem("Removing Open House Information");
                        listing.OpenHousePrompt = string.Empty;
                        this.Service.UpdateListing(listing);
                        return;
                    }
                    else if (result == "*")
                        return;
                    else
                        throw new NoResponseException("The caller failed to select a valid menu option");
				}
				else // create open house info
				{
                    this.CallDetails.AddActivityItem("Creating Open House Information");
					string fileName = Guid.NewGuid().ToString() + ".wav";
					string filePath = RealLeadsService.TempFilesDirectory + fileName;
					try
					{
						this.Line.PlayMultiLingualPrompt("Agent_OpenHouseBeginRecording.wav", false);
						this.Line.Record(filePath, true);
						File file = File.Load(filePath);

						// save the file so we dont have to download it 
						string finalPath = RealLeadsService.GetListingPromptPath(this.Agent.ID, listing.ID, file.ID);
						file.Save(finalPath);

						// now update the listing
						listing.OpenHousePrompt = file.ID;
						this.Service.UpdateListing(listing, file);
					}
					finally
					{
						if (System.IO.File.Exists(filePath))
							System.IO.File.Delete(filePath);
					}
					return;
				}
			}
		}		

		/// <summary>
		/// Checks the Agents CallerID association (If the caller has called from the number 3 times they will be prompted
		/// to associate the number with their account)
		/// </summary>
		private void CheckCallerIDAssociation()
		{
			try
			{
				string callingNumber = this.Line.CallerID.PhoneNumber;
				if(callingNumber == null)
					return;
				callingNumber = callingNumber.ToUpper();
				if (callingNumber != "PRIVATE" && callingNumber != "OUT OF AREA" && callingNumber != string.Empty)
				{
					string[] numbers = this.Service.RealLeadsData.AgentStore.GetPhoneNumbersForAgent(this.Agent.ID);
					foreach (string number in numbers)
					{
						if (callingNumber == number)
							return;
					}

					// the number does not exist in the agents phone list
					// see if the caller has called from that number before and if so
					// ask if they want ot associate it with their account
					if (this.Agent.Details.ContainsKey(callingNumber))
					{
						if (this.Agent.Details[callingNumber] !=  null && this.Agent.Details[callingNumber] != string.Empty)
						{
							int numberTimesCalled = int.Parse(this.Agent.Details[callingNumber]);
							numberTimesCalled++; // this call
							if (numberTimesCalled >= 3)
							{
								// ask the agent if they want to associate the number
								this.Line.PlayMultiLingualPrompt("Agent_PhoneAssociation.wav", true);
								string pressedTermDigit = string.Empty; ;
								this.Line.GetDigits(10, TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3), "*", out pressedTermDigit);
								if (pressedTermDigit == "*")
									this.Service.AddPhoneAssociation(this.Agent.ID, callingNumber);

								this.Agent.Details[callingNumber] = string.Empty;
							}
							else
								this.Agent.Details[callingNumber] = numberTimesCalled.ToString();
						}
					}
					else
					{
						// its the first call from this number
						this.Agent.Details.Add(callingNumber, "1");
					}
					this.Service.UpdateAgent(this.Agent);
				}
			}
			catch (Exception ex)
			{
				RealLeadsService.LogException(ex);
			}
		}


		#region Voice Mail
		/// <summary>
		/// Checks the Agents voice mail and prompts them if there are new messages.
		/// </summary>
		/// <returns>True if the agent has decided to hear the new messages, false otherwise</returns>
		private bool CheckVoiceMail()
		{
			this.VoiceMails.AddRange(this.Service.GetVoiceMail(this.Agent.ID));
			if (this.VoiceMails.Count > 0)
				this.Service.PreLoadVoiceMail(this.VoiceMails.ToArray());

			int newCount = this.VoiceMails.GetVoiceMailOfStatus(VoiceMail.VoiceMailStatus.New).Length;
			if (newCount > 0)
			{
				if (newCount == 1)
					this.Line.PlayMultiLingualPrompt("Agent_VoiceMail_01New.wav", true);
				else if (newCount == 2)
					this.Line.PlayMultiLingualPrompt("Agent_VoiceMail_02New.wav", true);
				else if (newCount == 3)
					this.Line.PlayMultiLingualPrompt("Agent_VoiceMail_03New.wav", true);
				else if (newCount == 4)
					this.Line.PlayMultiLingualPrompt("Agent_VoiceMail_04New.wav", true);
				else if (newCount == 5)
					this.Line.PlayMultiLingualPrompt("Agent_VoiceMail_05New.wav", true);
				else if (newCount == 6)
					this.Line.PlayMultiLingualPrompt("Agent_VoiceMail_06New.wav", true);
				else if (newCount == 7)
					this.Line.PlayMultiLingualPrompt("Agent_VoiceMail_07New.wav", true);
				else if (newCount == 8)
					this.Line.PlayMultiLingualPrompt("Agent_VoiceMail_08New.wav", true);
				else if (newCount == 9)
					this.Line.PlayMultiLingualPrompt("Agent_VoiceMail_09New.wav", true);
				else if (newCount == 10)
					this.Line.PlayMultiLingualPrompt("Agent_VoiceMail_10New.wav", true);
				else
					this.Line.PlayMultiLingualPrompt("Agent_VoiceMail_MultiNew.wav", true);

				string pressedTermDigit = string.Empty; ;
				this.Line.GetDigits(10, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2), "*", out pressedTermDigit);

				if (pressedTermDigit == "*")
					return true;
			}
			return false;
		}

		private void ProcessVoiceMailMenu()
		{
            this.CallDetails.AddActivityItem("Accessing Voice Mail Menu");
			while (true)
			{
				string result = string.Empty;
				result = this.Line.PlayMenu("Agent_VoiceMailMenu.wav", 3, "1", "2", "*");

				if (result == "1")                
					this.PlayVocieMail(this.VoiceMails.ToArray());
				else if (result == "2")
					this.ProcessModifyVoiceMailGreeting();
				else if(result == "*")
					return;
				else
					throw new NoResponseException("The caller failed to select a valid menu option");
			}
		}

		private void ProcessModifyVoiceMailGreeting()
		{
            this.CallDetails.AddActivityItem("Accessing Voice Mail Greeting Menu");
			RealEstateListing listing = this.Listing as RealEstateListing;
			while (true)
			{
				string result = this.Line.PlayMenu("Agent_VoiceMailGreetingModifyMenu.wav", 3, "1", "2", "*");
                if (result == "1")
                {
                    this.CallDetails.AddActivityItem("Playing existing Voice Mail Greeting");
                    this.Line.PlayListingFile(listing, listing.VoiceMailGreetingPrompt, true);
                }
                else if (result == "2")
                {
                    this.CallDetails.AddActivityItem("Recording a new Voice Mail Greeting");
                    string fileName = Guid.NewGuid().ToString() + ".wav";
                    string filePath = RealLeadsService.TempFilesDirectory + fileName;
                    try
                    {
                        this.Line.PlayMultiLingualPrompt("Agent_VoiceMailGreetingBeginRecording.wav", false);
                        this.Line.Record(filePath, true);
                        File file = File.Load(filePath);

                        // save the file so we dont have to download it 
                        string finalPath = RealLeadsService.GetListingPromptPath(this.Agent.ID, this.Listing.ID, file.ID);
                        file.Save(finalPath);

                        // now update the listing
                        this.Listing.VoiceMailGreetingPrompt = file.ID;
                        this.Service.UpdateListing(this.Listing, file);
                    }
                    finally
                    {
                        if (System.IO.File.Exists(filePath))
                            System.IO.File.Delete(filePath);
                    }
                }
                else if (result == "*")
                    return;
                else
                    throw new NoResponseException("The caller failed to select a valid menu option");
			}
		}

		private void PlayVocieMail(VoiceMail[] voiceMail)
		{
            this.CallDetails.AddActivityItem("Playing Voice Mail messages");

			List<VoiceMail> newItemsNowOld = new List<VoiceMail>();
			List<VoiceMail> deletedItems = new List<VoiceMail>();
			bool allNew = true;

			try
			{
				for (int i = 0; i < voiceMail.Length; i++)
				{
					VoiceMail message = voiceMail[i] as VoiceMail;
					if (message.Status != VoiceMail.VoiceMailStatus.New)
						allNew = false;

					if(i ==0 && message.Status == VoiceMail.VoiceMailStatus.New)
						this.Line.PlayMultiLingualPrompt("Agent_VoiceMailFirstNewMessage.wav", false);
					else if(i ==0 && message.Status == VoiceMail.VoiceMailStatus.Old)
						this.Line.PlayMultiLingualPrompt("Agent_VoiceMailFirstMessage.wav", false);
					else if(message.Status == VoiceMail.VoiceMailStatus.New)
						this.Line.PlayMultiLingualPrompt("Agent_VoiceMailNextNewMessage.wav", false);
					else
						this.Line.PlayMultiLingualPrompt("Agent_VoiceMailNextMessage.wav", false);

					// play the message
					this.Service.WaitForResource(message.FileID);

					while (true)
					{
						this.Line.PlayVoiceMailFile(message, true);

						string result = string.Empty;					
						this.Line.GetDigits(10, TimeSpan.FromSeconds(.2), TimeSpan.FromSeconds(.2) , "179*", out result);
						if(result == string.Empty)
							result = this.Line.PlayMenu("Agent_VoiceMailItemMenu.wav", 4, "1", "7", "9", "*");

						if (result == "1")
							continue;
						else if (result == "7")
						{
							this.Line.PlayMultiLingualPrompt("Agent_VoiceMailMessageErased.wav", false);
							deletedItems.Add(message);
							break;
						}
						else if (result == "9")
						{
							this.Line.PlayMultiLingualPrompt("Agent_VoiceMailMessageSaved.wav", false);
							if (message.Status == VoiceMail.VoiceMailStatus.New)
								newItemsNowOld.Add(message);
							break;
						}
						else if (result == "*")
							return;
						else
							throw new NoResponseException("The caller failed to select a valid menu option");
					}				
				}
				if (allNew)
					this.Line.PlayMultiLingualPrompt("Agent_VoiceMailNoMoreNewMessages.wav", false);
				else
					this.Line.PlayMultiLingualPrompt("Agent_VoiceMailNoMoreMessages.wav", false);
			}
			finally
			{
				// update any of the mail messages that need updating or removing
				foreach (VoiceMail message in deletedItems)
				{
					this.VoiceMails.Remove(message);
					this.Service.RemoveVoiceMail(message);
				}
				foreach (VoiceMail message in newItemsNowOld)
				{
					message.Status = VoiceMail.VoiceMailStatus.Old;
					this.Service.UpdateVoiceMail(message);
				}
			}
		}
		#endregion


		/// <summary>
		/// Validates an agent
		/// </summary>
		/// <param name="line">The line</param>
		/// <param name="agent">The agent to validate</param>
		/// <returns>True if the caller validated their idenity as the agent, false otherwise</returns>
		public static bool ValidateAgent(RealLeadsIncommingLine line, Agent agent)
		{
            bool finalAttempt = false;
			if(line == null)
				throw new ArgumentNullException("line","The line argument must not be null");
			if(agent == null)
				throw new ArgumentNullException("agent","The agent argument must not be null");

			const int PinCodeLength = 4;
            bool result = false;
            for(int attempt = 1; attempt <=2 && result == false; attempt++)
            {
                if (attempt == 1)
                    line.PlayMultiLingualPrompt("AgentLogin_EnterPincode01.wav", true);
                else                
                    line.PlayMultiLingualPrompt("AgentLogin_EnterPincode02.wav", true);
                
                string pressedTermDigit = string.Empty; ;
                string pinCode = line.GetDigits(PinCodeLength, TimeSpan.FromSeconds(7), TimeSpan.FromSeconds(4), "*#", out pressedTermDigit);

                if (pinCode == string.Empty)
                    result = false;

                if (pinCode == agent.PinCode)
                   result = true;
                else
                {
                    if (attempt == 1)
                        line.PlayMultiLingualPrompt("AgentLogin_InvalidPinCode.wav", false);
                    else
                        line.PlayMultiLingualPrompt("AgentLogin_InvalidPinCodeCallAgain.wav", false);
                    
                    result = false;
                }
            }
            return result;
		}
	}
}

