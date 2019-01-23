using System;
using System.Xml;
using System.Collections;
using System.Text;

namespace Inaugura.Telephony
{
	/// <summary>
	/// The language list
	/// </summary>
	public class LanguageList: CollectionBase
	{	
		#region Variables
		private string mDocName;
		private string mNodeName;
		#endregion

		#region Properties
		private string DocName
		{
			get
			{
				return this.mDocName;
			}
			set
			{
				this.mDocName = value;
			}
		}

		private string NodeName
		{
			get
			{
				return this.mNodeName;
			}
			set
			{
				this.mNodeName = value;
			}
		}
		#endregion		

		#region Accessors

		/// <summary>
		/// Accessor
		/// </summary>
		/// <value></value>
		public Language this[int index]
		{
			get
			{
				return (Language)this.List[index];
			}
		}		
		#endregion

		#region Methods
		/// <summary>
		/// Constructor
		/// </summary>
		public LanguageList()
		{
		}

		/// <summary>
		/// Adds a language to the list
		/// </summary>
		/// <param name="number">The language to add</param>
		public void Add(Language language)
		{
			if (!this.Contains(language))
				this.List.Add(language);
		}

		/// <summary>
		/// Removes a language from the list
		/// </summary>
		/// <param name="number">The language to remove</param>
		public void Remove(Language language)
		{
			this.List.Remove(language);
		}

		/// <summary>
		/// Checks if the list contains an item
		/// </summary>
		/// <param name="number">The language item</param>
		/// <returns>True if the list contains the item, false otherwise</returns>
		public bool Contains(Language language)
		{
			return this.List.Contains(language);
		}		
		#endregion
	}
}
