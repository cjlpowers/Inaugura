using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class ListingSearchPagePanel : System.Web.UI.UserControl
{
    #region Variables
    private string mSearchKey;
    private int mPageCount;
    private int mPageIndex;
    private int mPageSize;
    private int mResultCount;    
    #endregion

    #region Properties
    /// <summary>
    /// The page count
    /// </summary>
    public int PageCount
    {
        get
        {
            return this.mPageCount;
        }
        set
        {
            this.mPageCount = value;
        }
    }

    /// <summary>
    /// The result count
    /// </summary>
    public int ResultCount
    {
        get
        {
            return this.mResultCount;
        }
        set
        {
            this.mResultCount = value;
        }
    }

    /// <summary>
    /// The page index
    /// </summary>
    public int PageIndex
    {
        get
        {
            return this.mPageIndex;
        }
        set
        {
            this.mPageIndex = value;
        }
    }

    /// <summary>
    /// The page size
    /// </summary>
    public int PageSize
    {
        get
        {
            return this.mPageSize;
        }
        set
        {
            this.mPageSize = value;
        }
    }

    /// <summary>
    /// The search key
    /// </summary>
    public string SearchKey
    {
        get
        {
            return this.mSearchKey;
        }
        set
        {
            this.mSearchKey = value;
        }
    }
    #endregion
    
    protected void Page_PreRender(object sender, EventArgs e)
    {        
        // calculate the number of pages
        int numberOfPages = this.PageCount;
        int pageIndex = this.PageIndex;

        int startPage = pageIndex - 5;
        int endPage = pageIndex + 5;

        if (startPage < 1)
            startPage = 1;
        if (endPage > numberOfPages)
            endPage = numberOfPages;

        this.DrawPages(startPage, endPage, pageIndex);
    }

    private void DrawPages(int startPage, int endPage, int currentPage)
    {
        string searchKey = this.SearchKey;
        int pageSize = this.PageSize;
        int startIndex = (currentPage - 1) * pageSize + 1;
        int endIndex = startIndex + pageSize - 1;
        if (endIndex > this.ResultCount)
            endIndex = this.ResultCount;
        this.mSpan.InnerText = string.Format("Currently displaying {0} to {1} of {2} matches.", startIndex, endIndex, this.ResultCount);

        if (startPage != endPage)
        {
            for (int page = startPage; page <= endPage; page++)
            {
                if (page == currentPage)
                {
                    Label label = new Label();
                    label.CssClass = "PageNumber";
                    label.Text = page.ToString();
                    this.mRow.Controls.Add(label);
                }
                else
                {
                    HyperLink link = new HyperLink();
                    link.CssClass = "PageNumber";
                    link.Text = page.ToString();
                    link.NavigateUrl = this.GetSearchUrl(searchKey, page);
                    this.mRow.Controls.Add(link);
                }
            }
        }       

        if (currentPage > 1)
        {
            this.mLnkPagePrev.Attributes["href"] = this.GetSearchUrl(searchKey, currentPage - 1);
            this.mLnkPageStart.Attributes["href"] = this.GetSearchUrl(searchKey, 1);
            this.mLnkPagePrev.Visible = true;
            this.mLnkPageStart.Visible = true;
        }
        else
        {
            this.mLnkPagePrev.Visible = false;
            this.mLnkPageStart.Visible = false;
        }

        if (currentPage < endPage)
        {
            this.mLnkPageNext.Attributes["href"] = this.GetSearchUrl(searchKey, currentPage + 1);
            this.mLnkPageEnd.Attributes["href"] = this.GetSearchUrl(searchKey, this.PageCount);
            this.mLnkPageNext.Visible = true;
            this.mLnkPageEnd.Visible = true;
        }
        else
        {
            this.mLnkPageNext.Visible = false;
            this.mLnkPageEnd.Visible = false;
        }        
    }

    private string GetSearchUrl(string searchKey, int pageIndex)
    {        
        return string.Format("~/Search.aspx?search={0}&pindex={1}", searchKey, pageIndex);
    }
}
