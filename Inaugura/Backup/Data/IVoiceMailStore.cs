using System;

namespace Inaugura.RealLeads.Data
{
	/// <summary>
	/// Summary description for IUserStore.
	/// </summary>
	public interface IVoiceMailStore
	{
		#region Voice Mail
		/// <summary>
		/// Gets a Voice Mail
		/// </summary>
		/// <param name="id">The ID of the Voice Mail</param>
		/// <returns></returns>
		VoiceMail GetVoiceMail(string id);

		/// <summary>
		/// Gets a list of Voice Mail for a specific Agent
		/// </summary>
		/// <param name="agentID">The Agent ID</param>
		/// <returns>A list of Voice Mail</returns>
		VoiceMail[] GetVoiceMails(string agentID);

		/// <summary>
		/// Gets a list of Voice Mail with a given status for a specific Agent
		/// </summary>
		/// <param name="agentID">The Agent ID</param>
		/// <param name="status">The status</param>
		/// <returns>A list of Voice Mail</returns>
		VoiceMail[] GetVoiceMails(string agentID, VoiceMail.VoiceMailStatus status);

		/// <summary>
		/// Adds a voice Mail
		/// </summary>
		/// <param name="voiceMail">The Voice Mail to add</param>
		void Add(VoiceMail voiceMail);

		/// <summary>
		/// Removes a Voice Mail
		/// </summary>
		/// <param name="id">The ID of the Voice Mail to remove</param>
		bool Remove(string id);

		/// <summary>
		/// Updates a Voice Mail
		/// </summary>
		/// <param name="voiceMail">The updated Voice Mail</param>
		bool Update(VoiceMail voiceMail);
		#endregion

		#region Voice Mail Files
		/// <summary>
		/// Adds a file to a Voice Mail
		/// </summary>
		/// <param name="voiceMailID">The VoiceMail ID</param>
		/// <param name="file">The File to add</param>
        void AddFile(Guid voiceMailID, File file);

		/// <summary>
		/// Gets a File
		/// </summary>
		/// <param name="fileID">The ID of the File</param>
		/// <returns>The File with the specified ID</returns>
		File GetFile(Guid fileID);

		/// <summary>
		/// Removes a File
		/// </summary>
		/// <param name="fileId">The ID of the File</param>
		/// <returns>The File with the specified ID</returns>
        bool RemoveFile(Guid fileId);

		/// <summary>
		/// Updates a File
		/// </summary>
		/// <param name="file">The updated File</param>
		/// <returns></returns>
		bool UpdateFile(File file);
		#endregion
	}
}
