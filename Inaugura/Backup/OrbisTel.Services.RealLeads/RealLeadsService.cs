#region Using directives

using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Inaugura;
using Inaugura.Data;
using Inaugura.Telephony;
using Inaugura.Telephony.Services;
using Inaugura.RealLeads.Data;
using Inaugura.RealLeads;

#endregion

namespace OrbisTel.Services.RealLeads
{
	public class RealLeadsService : IncommingService
	{
		#region Variables
		private Language mActiveLanguage = Language.Unknown;
		private ResourceLoader mResourceLocker;
		private const int DefaultThreadPoolSize = 3;
        private Inaugura.Threading.ManagedThreadPool mThreadPool;
        private Inaugura.Data.SQLCachedAdaptor mInauguraData;
        private Inaugura.RealLeads.Data.SQLCachedAdaptor mRealLeadsData;
		#endregion

		#region Properties
		public Inaugura.RealLeads.Data.IRealLeadsDataAdaptor RealLeadsData
		{
			get
			{
                return this.mRealLeadsData;
			}
		}

		public Inaugura.Data.IDataAdaptor InauguraData
		{
			get
			{
                return this.mInauguraData;
			}
		}

		public Inaugura.Threading.ManagedThreadPool ThreadPool
        {
            get
            {
                return this.mThreadPool;
            }
        }

        public override string Name
		{
			get
			{
				return "RealLeads";
			}
		}

		public override string Id
		{
			get 
			{
				return "RealLeads";
			}
		}

		/// <summary>
		/// The zone in which this service operates
		/// </summary>
		/// <value></value>
        public string ZoneID
        {
            get
            {
					return this.Details["Zone"];
			}
        }

		private int ThreadPoolSize
		{
			get
			{
				string str = this.Details["ThreadPoolSize"];
				if (str != null)
				{
					int result;
					if (int.TryParse(str, out result))
						return result;
				}
				// other wise return the default
				return DefaultThreadPoolSize;
			}
		}
		#endregion

		#region Methods
		public RealLeadsService()
		{	
			this.Details.Add("Zone", "");
			this.Details.Add("ConnectionString", "");
			this.Details.Add("SMTP", "smtp.istop.com");
			this.Details.Add("LocalAreaCode", "");
			this.Details.Add("ThreadPoolSize", "3");            

			this.mResourceLocker = new ResourceLoader();
			this.mThreadPool = new Inaugura.Threading.ManagedThreadPool(this.ThreadPoolSize);
		}

		public RealLeadsService(XmlNode node) : base(node)
		{
			this.mResourceLocker = new ResourceLoader();
			this.mThreadPool = new Inaugura.Threading.ManagedThreadPool(this.ThreadPoolSize);
		}

		public override void SupplyLine(TelephonyLine line)
		{
			RealLeadsIncommingLine realLine = new RealLeadsIncommingLine(this, line);
			this.Add(realLine);
		}

		protected override void ProcessService()
		{
			// make sure that the directories exist
			if (!System.IO.Directory.Exists(RealLeadsService.RealLeadsDirectory))
				System.IO.Directory.CreateDirectory(RealLeadsService.RealLeadsDirectory);

			if (!System.IO.Directory.Exists(RealLeadsService.PromptDirectory))
				System.IO.Directory.CreateDirectory(RealLeadsService.PromptDirectory);
            	
			if (!System.IO.Directory.Exists(RealLeadsService.TempFilesDirectory))
				System.IO.Directory.CreateDirectory(RealLeadsService.TempFilesDirectory);

			if (!System.IO.Directory.Exists(RealLeadsService.PendingDirectory))
				System.IO.Directory.CreateDirectory(RealLeadsService.PendingDirectory);
		}

        public override void Start()
        {
            // create the data adaptors
            string connectionString = this.Details["ConnectionString"];
            string cacheDirectory = RealLeadsService.CacheDirectory;

            this.mInauguraData = new Inaugura.Data.SQLCachedAdaptor(connectionString,cacheDirectory);
            this.mRealLeadsData = new Inaugura.RealLeads.Data.SQLCachedAdaptor(connectionString, cacheDirectory);
            base.Start();
        }


		public override void Stop()
		{
			base.Stop();

			if (!this.ThreadPool.Idle)
			{
                Inaugura.Log.AddLog(string.Format("Waiting for RealLeads Thread Pool to complete queued work items ({0} active threads, {1} tasks queued", this.ThreadPool.ActiveThreads, this.ThreadPool.QueuedCount));
				while (!this.ThreadPool.Idle)
				{
					System.Threading.Thread.Sleep(100);
				}
			}

            // dispose the data adaptors
            this.mRealLeadsData.Dispose();
            this.mInauguraData.Dispose();
		}

		#region Data Access
		/// <summary>
		/// Gets a listing from the data source
		/// </summary>
		/// <param name="code">The property code</param>
		/// <param name="useCache">If true the cache will be used to retreive listings in the event of an error connecting to the data source</param>
		/// <returns>The listing or null if not found</returns>
		public Listing GetListingByCode(string code, bool useCache)
		{
			Listing listing = null;
			listing = this.RealLeadsData.ListingStore.GetListingByCode(code);                

			if (listing != null)
			{				
				// pre load the listing resources
				this.PreLoadListing(listing as RealEstateListing);			
			}
			return listing;
		}

		/// <summary>
		/// Gets a listing from the data source or in the event of a communications error, the cache
		/// </summary>
		/// <param name="propertyNumber">The property code</param>
		/// <returns>The listing or null if not found</returns>
		public Listing GetListingByCode(string code)
		{
			return this.GetListingByCode(code, true);
		}

		/// <summary>
		/// Gets listings for a specific agent
		/// </summary>
		/// <param name="agent">The agent</param>
		/// <returns>The listings owned by that agent</returns>
		public Listing[] GetListingsByAgent(Agent agent)
		{
			if (agent == null)
				throw new ArgumentNullException("agent", "The agent can not be null");

			Listing[] listings = null;
			try
			{
				listings = this.RealLeadsData.ListingStore.GetListings(agent.ID);
			}
			catch (ConnectionException ex) // could not establish connection with database
			{
				RealLeadsService.LogException(ex);				
			}

			foreach (Listing listing in listings)
			{
				this.PreLoadListing(listing as RealEstateListing);
			}
			return listings;
		}

		public void UpdateListing(Listing listing)
		{
			UpdateListingDelegate del = new UpdateListingDelegate(this.ProcessUpdateListing);
			this.ThreadPool.QueueWorkItem(del, listing);
		}

		private delegate void UpdateListingDelegate(Listing listing);
		private void ProcessUpdateListing(Listing listing)
		{
			try
			{
				this.RealLeadsData.ListingStore.Update(listing);
			}
			catch (Exception ex)
			{
				// todo log this process for future processing
				RealLeadsService.LogException(ex);
			}
		}

		public void UpdateListing(Listing listing, File listingFile)
		{
			UpdateListingFileDelegate del = new UpdateListingFileDelegate(this.ProcessUpdateListingFIle);
			this.ThreadPool.QueueWorkItem(del, listing, listingFile);
		}

		private delegate void UpdateListingFileDelegate(Listing listing, File file);
		private void ProcessUpdateListingFIle(Listing listing, File file)
		{
			try
			{
				this.RealLeadsData.ListingStore.AddFile(listing.ID, file);
				this.RealLeadsData.ListingStore.Update(listing);				
			}
			catch (Exception ex)
			{
				// todo log this process for future processing
				RealLeadsService.LogException(ex);
			}
		}

		/// <summary>
		/// Gets an agent from the data source
		/// </summary>
		/// <param name="agentID">The ID of the agent</param>
		/// <param name="useCache">If true the cache will be used to retreive agents in the event of an error connecting to the data source</param>
		/// <returns>The agent or null if not found</returns>
		public Agent GetAgent(string agentID, bool useCache)
		{
			Agent agent = null;		
			agent = this.RealLeadsData.AgentStore.GetAgent(agentID);
        	return agent;
		}

		/// <summary>
		/// Gets an agent from the data source or in the event of a communications error, the cache
		/// </summary>
		/// <param name="agentID">The ID of the agent</param>
		/// <returns>The agent or null if not found</returns>
		public Agent GetAgent(string agentID)
		{
			return this.GetAgent(agentID, true);
		}

		/// <summary>
		/// Gets an agent by phone number
		/// </summary>
		/// <param name="phoneNumber">The agents phone number</param>
		/// <returns>The agent or null if not found</returns>
		public Agent GetAgentByPhoneNumber(string phoneNumber)
		{
			Agent agent = this.RealLeadsData.AgentStore.GetAgentByPhoneNumber(phoneNumber);
			return agent;
		}


		/// <summary>
		/// Updates an agent Asyncronusly
		/// </summary>
		/// <param name="agent">The agent to update</param>
		public void UpdateAgent(Agent agent)
		{
			UpdateAgentDelegate del = new UpdateAgentDelegate(this.ProcessUpdateAgent);
			this.ThreadPool.QueueWorkItem(del, agent);
		}

		private delegate void UpdateAgentDelegate(Agent agent);
		private void ProcessUpdateAgent(Agent agent)
		{
			try
			{
				this.RealLeadsData.AgentStore.Update(agent);
			}
			catch (Exception ex)
			{
				RealLeadsService.LogException(ex);
			}
		}

		public VoiceMail[] GetVoiceMail(string agentID)
		{
			VoiceMail[] messages = this.RealLeadsData.VoiceMailStore.GetVoiceMails(agentID);
			return messages;
		}

		public void SaveVoiceMail(VoiceMail voiceMail, File file)
		{
			this.RealLeadsData.VoiceMailStore.Add(voiceMail);
			this.RealLeadsData.VoiceMailStore.AddFile(voiceMail.ID, file);
		}

		public void UpdateVoiceMail(VoiceMail voiceMail)
		{
			VoiceMailDelegate del = new VoiceMailDelegate(this.ProcessUpdateVoiceMail);
			this.ThreadPool.QueueWorkItem(del, voiceMail);
		}

		public void RemoveVoiceMail(VoiceMail voiceMail)
		{
			VoiceMailDelegate del = new VoiceMailDelegate(this.ProcessRemoveVoiceMail);
			this.ThreadPool.QueueWorkItem(del, voiceMail);
		}

		private delegate void VoiceMailDelegate(VoiceMail voiceMail);
		private void ProcessUpdateVoiceMail(VoiceMail voiceMail)
		{
			try
			{
				this.RealLeadsData.VoiceMailStore.Update(voiceMail);
			}
			catch (Exception ex)
			{
				RealLeadsService.LogException(ex);
			}
		}

		private void ProcessRemoveVoiceMail(VoiceMail voiceMail)
		{
			try
			{
				this.RealLeadsData.VoiceMailStore.Remove(voiceMail.ID);
			}
			catch (Exception ex)
			{
				RealLeadsService.LogException(ex);
			}
		}

		/// <summary>
		/// Associates a phone number with an agent asyncronusly
		/// </summary>
		/// <param name="agentID">The ID of the Agent</param>
		/// <param name="phoneNumber">The phone number to associate</param>
		public void AddPhoneAssociation(string agentID, string phoneNumber)
		{
			AddPhoneAssociationDelegate del = new AddPhoneAssociationDelegate(this.ProcessAddPhoneAssociation);
			this.ThreadPool.QueueWorkItem(del, agentID, phoneNumber);
		}

		private delegate void AddPhoneAssociationDelegate(string agentID, string phoneNumber);
		private void ProcessAddPhoneAssociation(string agentID, string phoneNumber)
		{
			try
			{
				this.RealLeadsData.AgentStore.AddPhone(agentID, phoneNumber);
			}
			catch (Exception ex)
			{
				RealLeadsService.LogException(ex);
			}
		}
		#endregion

		#region Resource Loading
		/// <summary>
		/// Causes the executing thread to wait until a resource undergoing preloading is loaded
		/// </summary>
		/// <param name="resourceID">The id of the resource currently being preloaded</param>
		public void WaitForResource(string resourceID)
		{
			this.mResourceLocker.Wait(resourceID);
		}

		#region Listing Resources
		private delegate void PreLoadListingResourceDelegate(string fileID, string path);
		/// <summary>
		/// Preloads all resources needed to process a listing
		/// </summary>
		/// <param name="listing">The listing</param>
		public void PreLoadListing(RealEstateListing listing)
		{
			if (listing.GreetingPrompt != null && listing.GreetingPrompt != string.Empty)
				this.PreLoadListingResource(listing.AgentID, listing.ID, listing.GreetingPrompt);

			if (listing.InformationPrompt != null && listing.InformationPrompt != string.Empty)
				this.PreLoadListingResource(listing.AgentID, listing.ID, listing.InformationPrompt);

			if (listing.OpenHousePrompt != null && listing.OpenHousePrompt != string.Empty)
				this.PreLoadListingResource(listing.AgentID, listing.ID, listing.OpenHousePrompt);

			if (listing.VoiceMailGreetingPrompt != null && listing.VoiceMailGreetingPrompt != string.Empty)
				this.PreLoadListingResource(listing.AgentID, listing.ID, listing.VoiceMailGreetingPrompt);
		}

		private void PreLoadListingResource(string agentID, string listingID, string fileID)
		{
			if (fileID != null && fileID != string.Empty)
			{
				// determin the path to the file
				string path = RealLeadsService.GetListingPromptPath(agentID, listingID, fileID);
				
				// if the file does not exist load it
				if (!System.IO.File.Exists(path))
				{
					#region Create the directories
					string agentDirectory = RealLeadsService.GetAgentDirectory(agentID);
					if (!System.IO.Directory.Exists(agentDirectory))
						System.IO.Directory.CreateDirectory(agentDirectory);

					string listingDirectory = RealLeadsService.GetListingDirectory(agentID,listingID);
					if (!System.IO.Directory.Exists(listingDirectory))
						System.IO.Directory.CreateDirectory(listingDirectory);
					#endregion

					this.mResourceLocker.LockResource(fileID);
					PreLoadListingResourceDelegate del = new PreLoadListingResourceDelegate(this.ProcessDownloadListingResource);
					this.ThreadPool.QueueWorkItem(del, fileID, path);

				}
			}
		}

		private void ProcessDownloadListingResource(string fileID, string path)
		{
			try
			{				
				// see if the file exists				
				if (!System.IO.File.Exists(path))
				{
					// try to down load the file
                    Inaugura.Log.AddLog(string.Format("Downloading Resource {0}", fileID));
					File file = this.RealLeadsData.ListingStore.GetFile(fileID);
					file.Save(path);
				}							
			}
			catch (Exception ex)
			{
				this.mResourceLocker.SetException(fileID, ex);
			}
			finally
			{
				this.mResourceLocker.UnlockResource(fileID);
			}
		}
		#endregion

		#region VoiceMail Resources
		private delegate void PreLoadVoiceMailResourceDelegate(string fileID, string path);
		/// <summary>
		/// Preloads the voice maill message recording
		/// </summary>
		/// <param name="voiceMailMessage">The voice mail message</param>
		public void PreLoadVoiceMail(VoiceMail voiceMailMessage)
		{
			if (voiceMailMessage.FileID != null && voiceMailMessage.FileID != string.Empty)
			{
				// determin the path to the file
				string path = RealLeadsService.GetVoiceMailPath(voiceMailMessage.AgentID,voiceMailMessage.FileID);

				// if the file does not exist load it
				if (!System.IO.File.Exists(path))
				{
					#region Create the directories
					string agentDirectory = RealLeadsService.GetAgentDirectory(voiceMailMessage.AgentID);
					if (!System.IO.Directory.Exists(agentDirectory))
						System.IO.Directory.CreateDirectory(agentDirectory);

					string voiceMail = RealLeadsService.GetVoiceMailDirectory(voiceMailMessage.AgentID);
					if (!System.IO.Directory.Exists(voiceMail))
						System.IO.Directory.CreateDirectory(voiceMail);
					#endregion

					this.mResourceLocker.LockResource(voiceMailMessage.FileID);
					PreLoadVoiceMailResourceDelegate del = new PreLoadVoiceMailResourceDelegate(this.ProcessDownloadVoiceMailResource);
					this.ThreadPool.QueueWorkItem(del, voiceMailMessage.FileID, path);

				}
			}			
		}

		/// <summary>
		/// Preloads voice maill message recordings
		/// </summary>
		/// <param name="voiceMailMessage">The list of voice mails to preload</param>
		public void PreLoadVoiceMail(VoiceMail[] voiceMails)
		{
			foreach(VoiceMail voiceMail in voiceMails)
			{
				this.PreLoadVoiceMail(voiceMail);
			}
		}
	
		private void ProcessDownloadVoiceMailResource(string fileID, string path)
		{
			try
			{
				// see if the file exists				
				if (!System.IO.File.Exists(path))
				{
					// try to down load the file
                    Inaugura.Log.AddLog(string.Format("Downloading Voice Mail Resource {0}", fileID));
					File file = this.RealLeadsData.VoiceMailStore.GetFile(fileID);
					file.Save(path);
				}
			}
			catch (Exception ex)
			{
				this.mResourceLocker.SetException(fileID, ex);
			}
			finally
			{
				this.mResourceLocker.UnlockResource(fileID);
			}
		}
		#endregion

		#endregion

		#region Call Logging
		private delegate void CallLogDelegate(CallLog details);
		public void LogCall(CallLog details)
		{
			CallLogDelegate del = new CallLogDelegate(this.ProcessLogCall);
			ThreadPool.QueueWorkItem(del, details);			
		}

		private void ProcessLogCall(CallLog details)
		{
			try
			{
				this.RealLeadsData.CallLogStore.Add(details);
			}
			catch (Exception ex)
			{
				RealLeadsService.LogException(ex);
			}
		}
		#endregion

		#region Static Members
		/// <summary>
		/// The prompts files directory ([Application Directory]\RealLeads\")
		/// </summary>
		/// <value></value>
		public static string RealLeadsDirectory
		{
			get
			{
				return Switch.RunDirectory + "RealLeads\\"; ;
			}
		}

		/// <summary>
		/// The prompts files directory ([Application Directory]\RealLeads\Prompts\")
		/// </summary>
		/// <value></value>
		public static string PromptDirectory
		{
			get
			{
				return RealLeadsDirectory + "Prompts\\";
			}
		}

		/// <summary>
		/// The temp files directory ([Application Directory]\RealLeads\Temp\")
		/// </summary>
		/// <value></value>
		public static string TempFilesDirectory
		{
			get
			{
				return RealLeadsDirectory + "Temp\\";
			}
		}

		/// <summary>
		/// The pending directory ([Application Directory]\RealLeads\Pending\")
		/// </summary>
		/// <value></value>
		public static string PendingDirectory
		{
			get
			{
				return RealLeadsDirectory + "Pending\\";
			}
		}

		/// <summary>
		/// The pending directory ([Application Directory]\RealLeads\Cache\")
		/// </summary>
		/// <value></value>
		public static string CacheDirectory
		{
			get
			{
				return RealLeadsDirectory + "Cache\\";
			}
		}

        
		/// <summary>
		/// Gets the path to an agent directory
		/// </summary>
		/// <param name="agentID">The agent ID</param>
		/// <returns>The path to the agents directory</returns>
		public static string GetAgentDirectory(string agentID)
		{
			return RealLeadsService.CacheDirectory + string.Format("Agents\\{0}\\",agentID);
		}

		/// <summary>
		/// Gets the path to a listing directory
		/// </summary>
		/// <param name="agentID">The agent ID</param>
		/// <param name="listingID">The listing ID</param>
		/// <returns>The path to the listing directory</returns>
		public static string GetListingDirectory(string agentID, string listingID)
		{
			return GetAgentDirectory(agentID) + string.Format("Listings\\{0}\\", listingID);
		}

		/// <summary>
		/// Gets the path to a voice mail directory
		/// </summary>
		/// <param name="agentID">The agent ID</param>
		/// <returns>The path to an agents voice mail directory</returns>
		public static string GetVoiceMailDirectory(string agentID)
		{
			return GetAgentDirectory(agentID) +"VoiceMail\\";
		}

		/// <summary>
		/// Gets the path to a voice mail file
		/// </summary>
		/// <param name="agentID">The agent ID</param>
		/// <param name="fileID">The voice mail file ID</param>
		/// <returns>The path to the voice mail recording</returns>
		public static string GetVoiceMailPath(string agentID, string fileID)
		{
			return GetVoiceMailDirectory(agentID) + fileID + ".wav";
		}         


		/// <summary>
		/// Gets the path to a listing prompt file
		/// </summary>
		/// <param name="agentID">The agent ID</param>
		/// <param name="listingID">The listing ID</param>
		/// <param name="fileID">The prompt file ID</param>
		/// <returns>The path to the listing prompt</returns>
		public static string GetListingPromptPath(string agentID, string listingID, string fileID)
		{
			return GetListingDirectory(agentID, listingID) + fileID + ".wav";
		}
		

		#region Error Logging
		public static void LogException(Exception ex)
		{
            Inaugura.Log.AddException(ex);
            Inaugura.Log.AddError(ex.ToString());
		}
		#endregion
		#endregion

		#endregion
	}
}
