#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mail;
using System.Xml;

using Inaugura.Telephony;
using Inaugura.RealLeads;


#endregion

namespace OrbisTel.Services.RealLeads
{
	/// <summary>
	/// This handler is responsible for handling calls the first time an Agent calls to setup their listing
	/// </summary>
    class ListingSetupCallHandler : CallHandler
    {
		protected delegate void SaveListingDelegate(RealEstateListing listing);

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

		public ListingSetupCallHandler(RealLeadsIncommingLine line, CallLog details) : base(line, details)
		{
		}

        public override void ProcessCall()
        {
			// check that the caller is actually the agent
			if(!AgentCallHandler.ValidateAgent(this.Line,this.Agent))
			{				
				throw new HangupException("The caller could not verify their identity as an Agent. Call was terminated");
			}

			// start the listing setup process
			if (this.Listing is RealEstateListing)
			{
				RealEstateListing listing = this.Listing as RealEstateListing;
				this.Line.PlayMultiLingualPrompt("RealEstateListingSetup_Welcome.wav", false);

				this.SetupGreeting();
				this.SetupDescription();
				this.SetupOpenHouse();
				this.SetupVoiceMail();

				this.Line.PlayMultiLingualPrompt("RealEstateListingSetup_Complete.wav", false);

                // now save the listing to the database
                this.SaveListing(listing);				
				//this.Service.ThreadPool.QueueWorkItem(new SaveListingDelegate(this.SaveListing), listing );
			}
		}

		protected void SetupGreeting()
		{
			this.Line.PlayMultiLingualPrompt("RealEstateListingSetup_GreetingInstructions.wav", false);
			this.Line.PlayPrompt("Tone.wav", false);
			this.Line.PlayMultiLingualPrompt("RealEstateListingSetup_GreetingSample.wav", false);
			this.Line.PlayPrompt("Tone.wav", false);

			string greetingFile = Guid.NewGuid().ToString() + ".wav";
			while (true)
			{
				string option = this.Line.PlayMenu("RealEstateListingSetup_GreetingMenu.wav", 3, "1","2");
				if (option == "1") // play the sample greeting again
				{
					this.Line.PlayPrompt("Tone.wav", false);
					this.Line.PlayMultiLingualPrompt("RealEstateListingSetup_GreetingSample.wav", false);
					this.Line.PlayPrompt("Tone.wav", false);
				}
				else if (option == "2")
				{
					this.Line.PlayMultiLingualPrompt("RealEstateListingSetup_ToneStartRec.wav", false);
					this.Line.Record(RealLeadsService.TempFilesDirectory + greetingFile, true);
					this.Listing.GreetingPrompt = greetingFile;
					break;
				}
				else
					throw new NoResponseException("The caller failed to select a valid menu option");
			}

			this.Line.PlayMultiLingualPrompt("RealEstateListingSetup_GreetingCreated.wav", false);
		}

		protected void SetupDescription()
		{
			this.Line.PlayMultiLingualPrompt("RealEstateListingSetup_DescriptionInstructions.wav", false);			

			string descriptionFile = Guid.NewGuid().ToString() + ".wav";			
			while (true)
			{
				string option = this.Line.PlayMenu("RealEstateListingSetup_DescriptionMenu.wav", 3, "1","2");
				if (option == "1") // play the sample description again
				{
					this.Line.PlayPrompt("Tone.wav", false);
					this.Line.PlayMultiLingualPrompt("RealEstateListingSetup_DescriptionSample.wav", false);
					this.Line.PlayPrompt("Tone.wav", false);
				}
				else if (option == "2")
				{
					System.Threading.Thread.Sleep(300);
					this.Line.Record(RealLeadsService.TempFilesDirectory + descriptionFile, true);
					this.Listing.InformationPrompt = descriptionFile;
					break;
				}
				else
					throw new NoResponseException("The caller failed to select a valid menu option");
			}
		}

		protected void SetupOpenHouse()
		{
			this.Line.PlayMultiLingualPrompt("RealEstateListingSetup_OpenHouseInstructions.wav", false);

			string openHouseFile = Guid.NewGuid().ToString() + ".wav";
			while (true)
			{
				string option = this.Line.PlayMenu("RealEstateListingSetup_OpenHouseMenu.wav", 3, "1","2","3");
				if (option == "1") // sample
				{
					this.Line.PlayPrompt("Tone.wav", false);
					this.Line.PlayMultiLingualPrompt("RealEstateListingSetup_OpenHouseSample.wav", false);
					this.Line.PlayPrompt("Tone.wav", false);
				}				
				else if (option == "2")
				{
					System.Threading.Thread.Sleep(300);
					this.Line.Record(RealLeadsService.TempFilesDirectory + openHouseFile, true);
					((RealEstateListing)this.Listing).OpenHousePrompt = openHouseFile;
					break;
				}
				else if (option == "3") // skip the step
				{
					break;
				}
				else
					throw new NoResponseException("The caller failed to select a valid menu option");
			}
		}

		protected void SetupVoiceMail()
		{
			this.Line.PlayMultiLingualPrompt("RealEstateListingSetup_VoiceMailInstructions.wav", false);

			string voiceMailGreetingFile = Guid.NewGuid().ToString() + ".wav";
			while (true)
			{
				string option = this.Line.PlayMenu("RealEstateListingSetup_VoiceMailMenu.wav", 3, "1","2","3");
				if (option == "1") // sample
				{
					this.Line.PlayPrompt("Tone.wav", false);
					this.Line.PlayMultiLingualPrompt("RealEstateListingSetup_VoiceMailSample.wav", false);
					this.Line.PlayPrompt("Tone.wav", false);
				}
				else if (option == "2")
				{
					System.Threading.Thread.Sleep(300);
					this.Line.Record(RealLeadsService.TempFilesDirectory + voiceMailGreetingFile, true);
					this.Listing.VoiceMailGreetingPrompt = voiceMailGreetingFile;
					break;
				}
				else
					throw new NoResponseException("The caller failed to select a valid menu option");
			}
		}

		protected void SaveListing(RealEstateListing listing)
		{
			// try and save the listing
			try
			{				
				Inaugura.File greetingFile = Inaugura.File.Load(RealLeadsService.TempFilesDirectory + listing.GreetingPrompt);
				greetingFile.FileName = listing.GreetingPrompt;
				listing.GreetingPrompt = greetingFile.ID;
				this.Service.RealLeadsData.ListingStore.AddFile(listing.ID, greetingFile);

				Inaugura.File descriptionFile = Inaugura.File.Load(RealLeadsService.TempFilesDirectory + listing.InformationPrompt);				
				descriptionFile.FileName = listing.InformationPrompt;
				listing.InformationPrompt = descriptionFile.ID;
				this.Service.RealLeadsData.ListingStore.AddFile(listing.ID, descriptionFile);

				if (listing.OpenHousePrompt != null && listing.OpenHousePrompt != string.Empty)
				{
					Inaugura.File openHouseFile = Inaugura.File.Load(RealLeadsService.TempFilesDirectory + listing.OpenHousePrompt);
					openHouseFile.FileName = listing.OpenHousePrompt;
					listing.OpenHousePrompt = openHouseFile.ID;
					this.Service.RealLeadsData.ListingStore.AddFile(listing.ID, openHouseFile);
				}

				Inaugura.File voiceMailGreetingFile = Inaugura.File.Load(RealLeadsService.TempFilesDirectory + listing.VoiceMailGreetingPrompt);
				voiceMailGreetingFile.FileName = listing.VoiceMailGreetingPrompt;
				listing.VoiceMailGreetingPrompt = voiceMailGreetingFile.ID;
				this.Service.RealLeadsData.ListingStore.AddFile(listing.ID, voiceMailGreetingFile);

				listing.IsSetup = true;
				this.Service.RealLeadsData.ListingStore.Update(listing);

			}
			catch (Exception ex)
			{
				/////////////////////////////////////////////////////////////////
				// some exception occured so save the files for processing later

				string pendingDirectory = RealLeadsService.PendingDirectory + "Listing " + Listing.ID + "\\";
				if (!System.IO.Directory.Exists(pendingDirectory))
					System.IO.Directory.CreateDirectory(pendingDirectory);

				// copy all the agents recordings
				if (System.IO.File.Exists(RealLeadsService.TempFilesDirectory + listing.GreetingPrompt))
					System.IO.File.Copy(RealLeadsService.TempFilesDirectory + listing.GreetingPrompt, pendingDirectory + listing.GreetingPrompt);

				if (System.IO.File.Exists(RealLeadsService.TempFilesDirectory + listing.InformationPrompt))
					System.IO.File.Copy(RealLeadsService.TempFilesDirectory + listing.InformationPrompt, pendingDirectory + listing.InformationPrompt);

				if (System.IO.File.Exists(RealLeadsService.TempFilesDirectory + listing.OpenHousePrompt))
					System.IO.File.Copy(RealLeadsService.TempFilesDirectory + listing.OpenHousePrompt, pendingDirectory + listing.OpenHousePrompt);

				if (System.IO.File.Exists(RealLeadsService.TempFilesDirectory + listing.VoiceMailGreetingPrompt))
					System.IO.File.Copy(RealLeadsService.TempFilesDirectory + listing.VoiceMailGreetingPrompt, pendingDirectory + listing.VoiceMailGreetingPrompt);

				// now save the listing xml
				listing.Xml.OwnerDocument.Save(pendingDirectory + listing.ID + ".xml");

				// save the exception info
				System.IO.File.WriteAllText(pendingDirectory + "Exception.txt", ex.ToString());
			}
			finally
			{
				// delete the temporary files
				if (System.IO.File.Exists(RealLeadsService.TempFilesDirectory + listing.GreetingPrompt))
					System.IO.File.Delete(RealLeadsService.TempFilesDirectory + listing.GreetingPrompt);

				if (System.IO.File.Exists(RealLeadsService.TempFilesDirectory + listing.InformationPrompt))
					System.IO.File.Delete(RealLeadsService.TempFilesDirectory + listing.InformationPrompt);

				if (System.IO.File.Exists(RealLeadsService.TempFilesDirectory + listing.OpenHousePrompt))
					System.IO.File.Delete(RealLeadsService.TempFilesDirectory + listing.OpenHousePrompt);

				if (System.IO.File.Exists(RealLeadsService.TempFilesDirectory + listing.VoiceMailGreetingPrompt))
					System.IO.File.Delete(RealLeadsService.TempFilesDirectory + listing.VoiceMailGreetingPrompt);
			}

		}

	}
}
