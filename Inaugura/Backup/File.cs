#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Inaugura.RealLeads
{
	/// <summary>
	/// Represents file data
	/// </summary>
	public class File
	{
		#region Variables
		private Guid mId;
		private string mFileName;
		private byte[] mData;
		#endregion

		#region Properties
		/// <summary>
		/// The ID of the File
		/// </summary>
		/// <value></value>
		public Guid ID
		{
			get
			{
				return this.mId;
			}
			private set
			{
				this.mId = value;
			}
		}

		/// <summary>
		/// The File name
		/// </summary>
		/// <value></value>
		public string FileName
		{
			get
			{
				return this.mFileName;
			}
			set
			{
				this.mFileName = value;
			}
		}

		/// <summary>
		/// The File data
		/// </summary>
		/// <value></value>
		public byte[] Data
		{
			get
			{                
				return this.mData;
			}
			set
			{
				this.mData = value;
			}
		}
		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		public File() : this(Guid.NewGuid(),String.Empty,new byte[0])
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="fileName">The File name</param>
		/// <param name="data">The File data</param>
		public File(Guid id, string fileName, byte[] data)
		{
			this.ID = id;
			this.FileName = fileName;
			this.Data = data;
		}

		/// <summary>
		/// Saves the file
		/// </summary>
		/// <param name="path">The file to be written</param>
		public void Save(string path)
		{
			System.IO.File.WriteAllBytes(path, this.mData);
		}

		/// <summary>
		/// Creates a File object and loads it with data
		/// </summary>
		/// <param name="path">The path to the file</param>
		/// <returns></returns>
		public static File Load(string path)
		{
			return new File(Guid.NewGuid(),System.IO.Path.GetFileName(path), System.IO.File.ReadAllBytes(path));
		}
	}
}
