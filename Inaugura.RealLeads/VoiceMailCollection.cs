using System;
using System.Collections.Generic;
using System.Text;

namespace Inaugura.RealLeads
{
	public class VoiceMailCollection : System.Collections.CollectionBase
	{
		#region Accessors
		/// <summary>
		/// Gets a voice mail at a specific index
		/// </summary>
		/// <param name="index">The zero based index into the collection</param>
		/// <returns>The voice mail at the specified index</returns>
		public VoiceMail this[int index]
		{
			get
			{
				return this.List[index] as VoiceMail;
			}
		}

		/// <summary>
		/// Gets a voice mail with a specific ID
		/// </summary>
		/// <param name="voiceMailID">The ID of the voice mail</param>
		/// <returns>The voice mail matching the ID</returns>
		public VoiceMail this[string voiceMailID]
		{
			get
			{
				foreach (VoiceMail voiceMail in this)
				{
					if (voiceMail.ID == voiceMailID)
						return voiceMail;
				}
				return null;
			}
		}
		#endregion	

		#region Methods
		/// <summary>
		/// Constructor
		/// </summary>
		public VoiceMailCollection()
		{
		}

		/// <summary>
		/// Adds a voice mail to the collection
		/// </summary>
		/// <param name="voiceMail">The voice mail to add</param>
		public void Add(VoiceMail voiceMail)
		{
			if (!this.List.Contains(voiceMail))
				this.List.Add(voiceMail);
		}

		/// <summary>
		/// Adds a range of voice mails to the collection
		/// </summary>
		/// <param name="collection">The collection of voice mails to add to the collection</param>
		public void AddRange(System.Collections.ICollection collection)
		{
			foreach (VoiceMail voiceMail in collection)
			{
				this.Add(voiceMail);
			}
		}

		/// <summary>
		/// Removes a voice mail from the collection
		/// </summary>
		/// <param name="voiceMail">The voice mail to remove</param>
		public void Remove(VoiceMail voiceMail)
		{
			if (this.List.Contains(voiceMail))
				this.List.Remove(voiceMail);
		}

		/// <summary>
		/// Gets a list of voice mails in the collection that have a specific status
		/// </summary>
		/// <param name="status">The status of the voice mail</param>
		/// <returns>The list of voice mails with the specified status</returns>
		public VoiceMail[] GetVoiceMailOfStatus(VoiceMail.VoiceMailStatus status)
		{
			List<VoiceMail> list = new List<VoiceMail>();
			foreach (VoiceMail voiceMail in this)
			{
				if (voiceMail.Status == status)
					list.Add(voiceMail);
			}
			return list.ToArray();			
		}

		/// <summary>
		/// Converts the collection to an array of voice mails
		/// </summary>
		/// <returns>A voice mail array</returns>
		public VoiceMail[] ToArray()
		{
			List<VoiceMail> list = new List<VoiceMail>();
			foreach (VoiceMail voiceMail in this)
				list.Add(voiceMail);
			return list.ToArray();			
		}

		
		#endregion

	}
}
