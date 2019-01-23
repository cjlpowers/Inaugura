using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for CachedSearch
/// </summary>
public class CachedSearch
{
    #region Internal Constructs
    private struct Block
    {
        public int StartIndex;
        public int EndIndex;
        public const int Unset = -1;

        public Block(int start, int end)
        {
            StartIndex = start;
            EndIndex = end;
        }
    }


    protected class Cache<T>
    {
        #region Variables
        private T[] mCache;
        private int mEndIndex;
        #endregion

        #region Methods
        public Cache()
        {
            this.mCache = new T[CachedSearch.PageSize];
            this.mEndIndex = -1;
        }

        public void Insert(int startIndex, System.Collections.Generic.IList<T> list)
        {
            if (this.mCache.Length < startIndex + list.Count)
                Grow(startIndex + list.Count);

            for (int i = 0; i < list.Count; i++)
                mCache[i+startIndex] = list[i];

            if (this.mEndIndex < list.Count + startIndex)
                this.mEndIndex = list.Count + startIndex;
        }

        public T[] Retreive(int startIndex, int endIndex)
        {
            System.Collections.Generic.List<T> list = new System.Collections.Generic.List<T>(endIndex-startIndex);
            for (int i = startIndex; i <= endIndex; i++)
            {
                if (i >= mEndIndex)
                    break;
                list.Add(mCache[i]);                
            }
            return list.ToArray();
        }

        private void Grow(int size)
        {
            int newSize = CachedSearch.PageSize;
            while (newSize < size)
                newSize = newSize * 2;

            T[] newCache = new T[newSize];
            for (int i = 0; i < mCache.Length; i++)
                newCache[i] = mCache[i];
            mCache = newCache;
        }

        public int FindMissingStartIndex(int startIndex, int endIndex)
        {
            if (startIndex > mEndIndex)
                return startIndex;
            for (int i = startIndex; i < mCache.Length && i < endIndex; i++)
                if (mCache[i] == null)
                    return i;
            return -1;
        }

        public int FindMissingEndIndex(int startIndex, int endIndex)
        {
            if (endIndex > mEndIndex)
                return endIndex;
            for (int i = endIndex; i > 0 && i > startIndex; i--)
                if (mCache[i] == null)
                    return i;
            return -1;
        }

        public T[] ToArray()
        {
            System.Collections.Generic.List<T> list = new System.Collections.Generic.List<T>(mCache);
            return list.ToArray();
        }
        #endregion
    }
    #endregion

    #region Variables
    private string mKey;
    private Inaugura.RealLeads.ListingSearch mSearch;
    private int mCacheStartIndex;
    private Cache<Inaugura.RealLeads.Listing> mCache;
    private const int PageSize = 40;    
    #endregion

    #region Properties
    /// <summary>
    /// The key
    /// </summary>
    public string Key
    {
        get
        {
            return this.mKey;
        }
        private set
        {
            this.mKey = value;
        }
    }

    /// <summary>
    /// The search object
    /// </summary>
    public Inaugura.RealLeads.ListingSearch Search
    {
        get
        {
            return this.mSearch;
        }
        private set
        {
            this.mSearch = value;
        }
    }    
    
    /// <summary>
    /// The cached results
    /// </summary>
    public Inaugura.RealLeads.Listing[] CachedResults
    {
        get
        {
            return mCache.ToArray();
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="search">The search object</param>
    public CachedSearch(Inaugura.RealLeads.ListingSearch search)
    {
        this.Search = search;
        this.Key = Guid.NewGuid().ToString();
        this.mCache = new Cache<Inaugura.RealLeads.Listing>();
    }

    /// <summary>
    /// Gets the results of the search
    /// </summary>
    /// <param name="startIndex">The start index (1 based index)</param>
    /// <param name="endIndex">The end index</param>
    /// <returns>The list of results</returns>
    public Inaugura.RealLeads.Listing[] GetResults(int startIndex, int endIndex)
    {       
        int start = GetPagedIndex(startIndex-1, true);
        int end = GetPagedIndex(endIndex-1, false);

        int cacheStart = mCache.FindMissingStartIndex(start, end);
        int cacheEnd = mCache.FindMissingEndIndex(start, end);
        if (cacheStart != -1)
        {
            // get a new block
            this.mSearch.StartIndex = cacheStart + 1;
            this.mSearch.EndIndex = cacheEnd + 1;
            Inaugura.RealLeads.Listing[] items = Helper.API.ListingManager.SearchListings(this.mSearch);
            this.mCache.Insert(cacheStart, items);
        }
        return mCache.Retreive(startIndex-1, endIndex-1);
    }

    private int GetPagedIndex(int index, bool lower)
    {
        if (lower)
            return index - index % CachedSearch.PageSize;
        else
            return index + CachedSearch.PageSize - index % CachedSearch.PageSize;
    }
    #endregion
}
