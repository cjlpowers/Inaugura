<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="Search.aspx.cs"
    Inherits="Search" ValidateRequest="false" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Default.master" %>
<%@ Register Src="Controls/Components/ListingSearchResultPanel.ascx" TagName="ListingSearchResultPanel"
    TagPrefix="uc1" %>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="mPlaceHolderBody">
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
    <uc1:ListingSearchResultPanel ID="mSearchResultsPanel" runat="server"></uc1:ListingSearchResultPanel>
    <div runat="server" id="mNoResults" visible="false">
        <p id="noresults">
            No results were found</p>
    </div>
</asp:Content>
