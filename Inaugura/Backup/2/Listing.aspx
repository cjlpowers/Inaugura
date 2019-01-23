<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="Listing.aspx.cs"
    Inherits="Listing" ValidateRequest="false" %>

<%@ Register Src="Controls/Listing.ascx" TagName="Listing" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="Controls/Components/SearchScroller.ascx" TagName="SearchScroller"
    TagPrefix="uc2" %>
<%@ Register Src="Controls/Components/ListingViewPanel.ascx" TagName="ListingViewPanel"
    TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/Default.master" %>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="mPlaceHolderBody">
    <asp:UpdatePanel ID="mUpdateSearchResults" runat="server" ChildrenAsTriggers="false"
        UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:SearchScroller ID="mSearchScroller" runat="server"></uc2:SearchScroller>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="mUpdateListing" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="ListingContent">
                <uc3:Listing ID="Listing1" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
