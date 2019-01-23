using System;

namespace Inaugura
{
	/// <summary>
	/// Generic Key class (for use in hashtables or methods which need to group/sort items based on specific values)
	/// </summary>
	/// <remarks>This class computes a hash code which is a unique reflection of the selected 'key items'. The hash code is computed by XORing the hashes of each 'key item'. The 'key items' can be added/removed in any order without effecting overall the signature of the key.</remarks>
	public class Key : IComparable, ICloneable
	{		
		#region Variables
		private int mHash = 0;
		#endregion

		#region Properties	
		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		public Key()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="keyItems">The key items</param>
		public Key(params object[] keyItems)
		{
			if(keyItems == null)
				throw new ArgumentNullException("keyItems");

			foreach(object key in keyItems)
			{
				mHash ^= key.GetHashCode();
			}			
		}

		/// <summary>
		/// Construcotor
		/// </summary>
		/// <param name="hash">The initial hash</param>
		protected Key(int hash)
		{
			this.mHash = hash;
		}

		/// <summary>
		/// Adds key item
		/// </summary>
		/// <param name="keyItem">The item to be incorporated into the key</param>
		public void Add(object keyItem)
		{
			this.mHash ^= keyItem.GetHashCode();
		}

		/// <summary>
		/// Removes a key item
		/// </summary>
		/// <param name="keyItem">The previously associated item</param>
		public void Remove(object keyItem)
		{
			// adding an item two times has the same effect of removing that item
			this.Add(keyItem);
		}

		/// <summary>
		/// Returns the hash code which identifies this key
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return this.mHash;
		}

		/// <summary>
		/// Determins if this object equals another object
		/// </summary>
		/// <param name="obj">The other object</param>
		/// <returns>True if the objects are equal, otherwise false</returns>
		public override bool Equals(object obj)
		{
			if(obj.GetHashCode() == this.GetHashCode())
				return true;
			return false;
		}

        /// <summary>
        /// The string representation of this key
        /// </summary>
        /// <returns>The string representation of this key</returns>
        public override string ToString()
        {
            return string.Format("{0}", this.mHash.ToString());
        }

		#region IComparable
		/// <summary>
		/// Compares this object to another object
		/// </summary>
		/// <param name="obj">The object to compare</param>
		/// <returns>The difference in the two objects hash codes</returns>
		public int CompareTo(object obj)
		{
			return obj.GetHashCode() - this.GetHashCode();
		}
		#endregion

		#region ICloneable
		/// <summary>
		/// Creates a copy of the key
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			Key key = new Key(this.mHash);
			return key;
		}
		#endregion
	}
}
