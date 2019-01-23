#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

using Inaugura;
using Inaugura.Data;
using Inaugura.Telephony;
using Inaugura.Telephony.Services;
using Inaugura.RealLeads;

#endregion

namespace OrbisTel.Services.RealLeads
{

	public class RealLeadsIncommingLine: IncommingServiceLine
	{
		#region Variables			
		private Language mLanguage = Language.English;
		#endregion

		#region Properties
		public Language Language
		{
			get
			{
				return this.mLanguage;
			}
			set
			{
				this.mLanguage = value;
			}
		}
		#endregion

		/// <summary>
		/// Constrcutor
		/// </summary>
		/// <param name="service">The RealLeadsService</param>
		/// <param name="line"></param>
		public RealLeadsIncommingLine(Service service, TelephonyLine line) : base(service,line)
		{			
		}
		
		protected override void ProcessIncommingCall()
        {
            Agent agent = null;
            CallLog details = null;
			CallHandler handler = null;			
			try
			{
				details = new CallLog(DateTime.Now);
				details.SwitchID = Switch.ActiveSwitch.Id;
				details.CallerID = this.CallerID.ToString();

                #region DEBUG Limit Calls
                // TODO remove this code in release
                Inaugura.Log.AddLog(this.CallerID.Name);

                //Validate the calller
                this.PlayPrompt("Tone.wav", false);
                string key = Line.GetDigits(1, TimeSpan.FromSeconds(4), TimeSpan.FromSeconds(4));
                if (key != "*")
                    return;


				//if (this.CallerID.Name != "VANDAL P")
				//	throw new CallerException("Incomming call was not from VANDAL P", null);
                #endregion                

				// Is the caller an agent
				if (this.Line.CallerID.Status == "OK" && 
                    this.Line.CallerID.PhoneNumber != "PRIVATE" && 
                    this.Line.CallerID.PhoneNumber != "OUT OF AREA" && 
                    this.Line.CallerID.PhoneNumber != string.Empty)
				{
					try
					{
						agent = ((RealLeadsService)this.Service).RealLeadsData.AgentStore.GetAgentByPhoneNumber(this.Line.CallerID.PhoneNumber);
					}
					catch (ConnectionException ex) // database error
					{
						RealLeadsService.LogException(ex);
						agent = null;
					}
				}

				if (agent != null) // agent call
				{
                    Listing[] listings = ((RealLeadsService)this.Service).GetListingsByAgent(agent);
					if (listings != null && listings.Length > 0)
					{
						handler = new AgentCallHandler(this, details, agent, listings[0]);
						handler.Agent = agent;
					}
					else
						handler = new ClientCallHandler(this, details);
				}
				else // non agent call
					handler = new ClientCallHandler(this, details);

				handler.StatusChanged += new StatusHandler(handler_StatusChanged);
				handler.ProcessCall();

			}
			catch (HangupException ex) // caller hung up
			{
				string str = ex.Message;
			}
			catch (CallerException ex) // generic error caused by the caller
			{
				try
				{
					this.PlayMultiLingualPrompt("Thankyou.wav", false);
				}
				catch (CallerException e)
				{
					// do nothing the exception was expected
					string msg = e.Message;
				}			
				catch (Exception e)
				{
					RealLeadsService.LogException(e);
				}
			}									
			catch (Exception ex)
			{
				RealLeadsService.LogException(ex);
				// play the error message
				try
				{
					this.PlayMultiLingualPrompt("Error.wav", false);
				}
				catch (CallerException e)
				{
					// do nothing the exception was expected
					string msg = e.Message;
				}
				catch (Exception e)
				{
					RealLeadsService.LogException(e);
				}
			}
			finally
			{
				try
				{
					if (details != null)
					{
						details.Duration = DateTime.Now - details.Time;

						if (handler != null)
						{
							if (handler.Agent != null)
								details.AgentID = handler.Agent.ID;
							if (handler.Listing != null)
								details.ListingID = handler.Listing.ID;
						}
						((RealLeadsService)this.Service).LogCall(details);
					}
				}
				catch (Exception ex)
				{
					RealLeadsService.LogException(ex);
				}
			}
		}

		void handler_StatusChanged(object sender, StatusEventArgs e)
		{
			this.Status = e.StatusSoruce.Status;
		}

		#region Prompt Playing

		/// <summary>
		/// Gets the prompts directory for a specific language
		/// </summary>
		/// <param name="language">The language</param>
		/// <returns>The directory path</returns>
		private string GetPromptDirectoryForLanguage(Language language)
		{
			if (language == Language.Unknown)
				throw new ArgumentException("Cannot generate a prompt path for an unknown language");
			else
				return RealLeadsService.PromptDirectory + language.ToString() + "\\";
		}

		/// <summary>
		/// Plays a prompt
		/// </summary>
		/// <param name="prompt">The prompt file to play</param>
		/// <param name="terminate">True to terminate when a key is pressed, false otherwise</param>
		/// <returns>The telephony result of the operation</returns>
		public TelephonyResult PlayPrompt(string prompt, bool terminate)
		{
			string path = RealLeadsService.PromptDirectory + prompt;
			if (!System.IO.File.Exists(path))
				throw new System.IO.FileNotFoundException(string.Format("The specified prompt could not be found [{1}].", path), prompt);
			return this.Line.PlayFile(path, terminate);
		}



		/// <summary>
		/// Plays a multilingual prompt
		/// </summary>
		/// <param name="prompt">The prompt file to play</param>
		/// <param name="terminate">True to terminate when a key is pressed, false otherwise</param>
		/// <param name="language">The language of the prompt</param>
		/// <returns>The telephony result of the operation</returns>
		public TelephonyResult PlayMultiLingualPrompt(string prompt, bool terminate, Language language)
		{
			string path = GetPromptDirectoryForLanguage(language) + prompt;
			if (!System.IO.File.Exists(path))
				throw new System.IO.FileNotFoundException(string.Format("The specified prompt could not be found [{0}].", path), prompt);

			return this.Line.PlayFile(path, terminate);
		}

		/// <summary>
		/// Plays a multilingual prompt
		/// </summary>
		/// <param name="prompt">The prompt file to play</param>
		/// <param name="terminate">True to terminate when a key is pressed, false otherwise</param>
		/// <param name="languages">The language of the prompt</param>
		/// <returns>The telephony result of the operation</returns>
		public TelephonyResult PlayMultiLingualPrompt(string prompt, bool terminate, LanguageList languages)
		{
			TelephonyResult result = TelephonyResult.Completed;
			foreach (Language language in languages)
			{
				result = this.PlayMultiLingualPrompt(prompt, terminate, language);

				if (result == TelephonyResult.Terminated)
					break;
			}
			return result;
		}


		/// <summary>
		/// Plays a multilingual prompt using the active language if supported, otherwise the language settings for the service
		/// </summary>
		/// <param name="prompt">The prompt file to play</param>
		/// <param name="terminate">True to terminate when a key is pressed, false otherwise</param>
		/// <returns>The telephony result of the operation</returns>
		public TelephonyResult PlayMultiLingualPrompt(string prompt, bool terminate)
		{
			if(this.Language == Language.Unknown)
				return this.PlayMultiLingualPrompt(prompt,terminate,Switch.ActiveSwitch.Languages);
			else if(Switch.ActiveSwitch.Languages.Contains(this.Language)) // language not supported by the service
				return this.PlayMultiLingualPrompt(prompt, terminate, this.Language);
			else // play the first language supported by the service
				return this.PlayMultiLingualPrompt(prompt, terminate, Switch.ActiveSwitch.Languages[0]);
		}


		/// <summary>
		/// Plays a number
		/// </summary>
		/// <param name="number">The number to play</param>
		/// <param name="terminate">True to terminate when a key is pressed, false otherwise</param>
		/// <returns>The telephony result of the operation</returns>
		public TelephonyResult PlayNumber(int number, bool terminate)
		{
			if (number > 20)
			{
				// todo
				return TelephonyResult.Completed;
			}

			string prompt = number.ToString() + ".wav";
			return this.PlayMultiLingualPrompt(prompt, terminate);
		}

		/// <summary>
		/// Plays digits
		/// </summary>
		/// <param name="digits">The digits to play</param>
		/// <param name="terminate">True to terminate when a key is pressed, false otherwise</param>
		/// <returns>The telephony result of the operation</returns>
		public TelephonyResult PlayDigits(string digits, bool terminate)
		{
			TelephonyResult result = TelephonyResult.Completed;
			for (int i = 0; i < digits.Length; i++)
			{
				result = this.PlayMultiLingualPrompt(digits[i] + ".wav", terminate);
				if (result == TelephonyResult.Terminated)
					break;
			}
			return result;
		}


		/// <summary>
		/// Plays a menu
		/// </summary>
		/// <param name="prompt">The menu prompt</param>
		/// <param name="maxNumberOfTries">The maximum number of times to reprompt the user before failing</param>
		/// <param name="options">The valid options to be accepted</param>
		/// <returns>The selected option or an empty string if the user failed to enter any of the valid options</returns>		
        public string PlayMenu(string prompt, int maxNumberOfTries, params string[] options)
        {
            if (options.Length == 0)
                throw new ArgumentException("You must specifiy at least one menu option");

            string optionKeys = string.Empty;
            foreach (string str in options)
                optionKeys += str;

            while (maxNumberOfTries > 0)
            {
                this.ClearDigitBuffer();
                this.PlayMultiLingualPrompt(prompt, true);

                if (this.Line.NumberOfDigitsInBuffer == 0 && maxNumberOfTries > 1)
                {
                    //Log.AddLog("Playing 'Hear Options Again' prompt");
                    this.PlayMultiLingualPrompt("HearOptionsAgain.wav", true);
                }
                else
                {
                    //Log.AddLog(string.Format("Not playing 'Hear Options Again' prompt. Digits in buffer {0}, number of tries {1}",Line.NumberOfDigitsInBuffer,maxNumberOfTries));
                }

                string pressedTermDigit = string.Empty;
                string str = this.Line.GetDigits(10, TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3), optionKeys, out pressedTermDigit);

                if (pressedTermDigit.Length != 0)
                {
                    System.Threading.Thread.Sleep(1500);
                    return pressedTermDigit;
                }
                maxNumberOfTries--;
            }

            string allOptions = string.Empty;
            for (int i = 0; i < options.Length; i++)
            {
                if (i < options.Length - 1)
                    allOptions += options[i] + ", ";
                else
                    allOptions += options[i];
            }
            // throw an exception since the user did not select a menu option
            throw new NoResponseException(string.Format("The caller failed to select one of the following options {0} when prompted with {1}", allOptions, prompt));
        }

		/// <summary>
		/// Plays a listing file
		/// </summary>
		/// <param name="agentID">The ID of the Agent</param>
		/// <param name="listingID">The ID of the Listing</param>
		/// <param name="fileID">The ID of the File</param>
		/// <param name="terminate">True to terminate when a key is pressed, false otherwise</param>
		/// <returns>The telephony result of the operation</returns>
		public TelephonyResult PlayListingFile(string agentID, string listingID, string fileID, bool terminate)
		{
			((RealLeadsService)this.Service).WaitForResource(fileID);			
			string promptPath = RealLeadsService.GetListingPromptPath(agentID, listingID, fileID);
			return this.Line.PlayFile(promptPath, terminate);
		}

		/// <summary>
		/// Plays a listing file
		/// </summary>
		/// <param name="agentID">The ID of the Agent</param>
		/// <param name="listingID">The ID of the Listing</param>
		/// <param name="fileID">The ID of the File</param>
		/// <param name="termDigits">The terminating digits</param>
		/// <param name="termDigit">The terminating digit (if any) which ended the playing process</param>
		/// <returns>The telephony result of the operation</returns>
		public TelephonyResult PlayListingFile(string agentID, string listingID, string fileID, string termDigits, out string termDigit)
		{
			((RealLeadsService)this.Service).WaitForResource(fileID);			
			string promptPath = RealLeadsService.GetListingPromptPath(agentID, listingID, fileID);
			return this.Line.PlayFile(promptPath, termDigits, out termDigit);
		}

		public void PlayVoiceMailGreeting(Listing listing)
		{
                string fileID = listing.VoiceMailGreetingPrompt;
                ((RealLeadsService)this.Service).WaitForResource(fileID);
                string promptPath = RealLeadsService.GetListingPromptPath(listing.AgentID, listing.ID, fileID);
                this.Line.PlaySpecial(promptPath);
		}

		/// <summary>
		/// Plays a listing file
		/// </summary>
		/// <param name="listing">The listing</param>
		/// <param name="fileID">The ID of the file</param>
		/// <param name="terminate">True to terminate when a key is pressed, false otherwise</param>		
		/// <returns>The telephony result of the operation</returns>
		public TelephonyResult PlayListingFile(Listing listing, string fileID, bool terminate)
		{
			return this.PlayListingFile(listing.AgentID, listing.ID, fileID, terminate);
		}

		/// <summary>
		/// Plays a voice mail file
		/// </summary>
		/// <param name="voiceMail">The preloaded voice mail to play</param>
		/// <param name="terminate">True to terminate when a key is pressed, false otherwise</param>		
		/// <returns>The telephony result of the operation</returns>
		public TelephonyResult PlayVoiceMailFile(VoiceMail voiceMail, bool terminate)
		{
			((RealLeadsService)this.Service).WaitForResource(voiceMail.FileID);
			string promptPath = RealLeadsService.GetVoiceMailPath(voiceMail.AgentID,voiceMail.FileID);
			return this.Line.PlayFile(promptPath, terminate);
		}
		#endregion

		#region Prompt Recording
		/// <summary>
		/// Records
		/// </summary>
		/// <param name="file">The path of the file to record to</param>
		/// <param name="mutlipleAttempts">If true the user will be allowed to attempt the recording again if they do not like the previous attempt</param>
		public void Record(string file, bool mutlipleAttempts, TimeSpan timeOut, TimeSpan silenceTimeOut, bool terminate)
		{
            Inaugura.Log.AddLog("Recording " + file);
			this.Line.Record(file,timeOut,silenceTimeOut,terminate);
			while (true)
			{
				if (mutlipleAttempts)
				{
					// allow the caller to re record the prompt if needed
					string option = this.PlayMenu("RecordingOptions.wav", 3, "1", "2", "3");
					if (option == "1") // play the recording
					{
						this.Line.PlayFile(file, true);
					}
					else if (option == "2") // do the recording again
					{
						this.Line.Record(file, timeOut, silenceTimeOut, terminate);
					}
					else if (option == "3") // save the recording
					{
						return;
					}
					else
						throw new NoResponseException("The caller failed to select a valid menu option");
				}
				else // dont allow to re-record
					return;
			}
		}

		public void Record(string file, bool mutlipleAttempts)
		{
			this.Record(file, mutlipleAttempts, TimeSpan.FromSeconds(60), TimeSpan.FromSeconds(2), true);
		}
		#endregion

		#region Telephony
		/// <summary>
		/// Gets digits
		/// </summary>
		/// <param name="maxDigits">The maximum number of digits to get</param>
		/// <param name="timeOut">The timeout</param>
		/// <param name="digitTimeOut">The digit timeout</param>
		/// <returns>The entered digits</returns>
		public string GetDigits(int maxDigits, TimeSpan timeOut, TimeSpan digitTimeOut)
		{
			return this.Line.GetDigits(maxDigits, timeOut, digitTimeOut);
		}

		/// <summary>
		/// Gets digits
		/// </summary>
		/// <param name="maxDigits">The maximum number of digits to get</param>
		/// <param name="timeOut">The timeout</param>
		/// <param name="digitTimeOut">The digit timeout</param>
		/// <param name="termDigits">The termination digits</param>
		/// <returns>The entered digits</returns>
		public string GetDigits(int maxDigits, TimeSpan timeOut, TimeSpan digitTimeOut, string termDigits)
		{
			return this.Line.GetDigits(maxDigits, timeOut, digitTimeOut, termDigits);
		}

		/// <summary>
		/// Gets digits
		/// </summary>
		/// <param name="maxDigits">The maximum number of digits to get</param>
		/// <param name="timeOut">The timeout</param>
		/// <param name="digitTimeOut">The digit timeout</param>
		/// <param name="termDigits">The termination digits</param>
		/// <param name="termDigit">The terminating digit</param>
		/// <returns>The entered digits</returns>
		public string GetDigits(int maxDigits, TimeSpan timeOut, TimeSpan digitTimeOut, string termDigits, out string termDigit)
		{
			return this.Line.GetDigits(maxDigits, timeOut, digitTimeOut, termDigits, out termDigit);
		}

		/// <summary>
		/// Dials a number
		/// </summary>
		/// <param name="phoneNumber">The number to dial</param>
		public void Dial(string phoneNumber)
		{
			this.Line.Dial(phoneNumber);
		}

		/// <summary>
		/// Dials a number
		/// </summary>
		/// <param name="phoneNumber">The phone number</param>
		/// <param name="numberOfRings">The number of rings</param>
		/// <returns>The dial analysis of the operation</returns>
		public DialAnalysis Dial(string phoneNumber, int numberOfRings)
		{
			return this.Line.Dial(phoneNumber, numberOfRings);
		}

		/// <summary>
		/// The caller ID
		/// </summary>
		/// <value></value>
		public CallerID CallerID
		{
			get
			{
				return this.Line.CallerID;
			}
		}

        public void ClearDigitBuffer()
        {
            this.Line.ClearDigitBuffer();
        }

		public bool DetectRingback(TimeSpan timeOut)
		{
			return this.Line.DetectRingback(timeOut);
		}
		#endregion
	}
}
