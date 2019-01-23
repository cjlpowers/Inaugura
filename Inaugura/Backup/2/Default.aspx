<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="Default.aspx.cs"
    Inherits="Default_aspx" Title="Untitled Page" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="Controls/Components/ListingViewPanel.ascx" TagName="ListingViewPanel"
    TagPrefix="uc3" %>
<%@ Register Src="Controls/Components/SearchBarControl.ascx" TagName="SearchBarControl"
    TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/Default.master" %>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="mPlaceHolderBody">
    <asp:UpdatePanel ID="mUpdateSearch" runat="server" UpdateMode="Conditional">
        <contenttemplate>
            <div id="mainPageSearch">
           <asp:TextBox ID="mTxtSearch" runat="server" Width="180px"></asp:TextBox> 
           <asp:Button ID="mBtnSearch" runat="server" Text="Search" OnClick="mBtnSearch_Click" />
           <br />
           <asp:Label ID="mLbSearchError" CssClass="MainPageError" runat="server" Text=""></asp:Label>                
                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="mTxtSearch" WatermarkCssClass="WatermarkText"
            WatermarkText="Enter postal code or listing number..."/> 
            </div>           
        </contenttemplate>
    </asp:UpdatePanel>    
    <asp:DropDownList ID="mDlSchool" CssClass="schoolSearch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="mDlSchool_SelectedIndexChanged">
    </asp:DropDownList>
    <div id="feature">
    <span id="featureTitle">Featured Listing</span>
        <uc3:ListingViewPanel ID="ListingViewPanel1" runat="server" />
    </div>
    <div id="featureBottom">
    </div>
</asp:Content>
