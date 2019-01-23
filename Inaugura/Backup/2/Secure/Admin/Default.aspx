<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="Default.aspx.cs"
    Inherits="Secure_Admin_Default" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/Default.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="mPlaceHolderBody">
    <ajax:TabContainer runat="server" ID="Tabs" Height="250px">
        <ajax:TabPanel runat="Server" ID="Panel2" HeaderText="Tasks">
            <ContentTemplate>
                <asp:HyperLink runat="server" ID="mHlLocales" NavigateUrl="~/Secure/Admin/Locales.aspx"
                    Text="Locales"></asp:HyperLink>
                <br />
                <asp:HyperLink runat="server" ID="mHlStats" NavigateUrl="~/Secure/Admin/Stats.aspx"
                    Text="Stats"></asp:HyperLink>
                <br />
                <asp:HyperLink runat="server" ID="HyperLink1" NavigateUrl="~/Secure/Admin/Promotions.aspx"
                    Text="Promotions"></asp:HyperLink>
                   <br /> 
                <asp:HyperLink runat="server" ID="mHlListingManagement" NavigateUrl="~/Secure/Admin/ListingManagement.aspx"
                    Text="Listing Management"></asp:HyperLink>
                <fieldset>
                    <legend>Content Editing</legend>
                    <asp:CheckBox ID="mChkContentEditing" runat="server" AutoPostBack="True" OnCheckedChanged="mChkContentEditing_CheckedChanged"
                        Text="Turn Content Editing On/Off" />
                </fieldset>
            </ContentTemplate>
        </ajax:TabPanel>
        <ajax:TabPanel runat="Server" ID="TabPanel1" HeaderText="Email">
            <ContentTemplate>
                    <fieldset>
                    <asp:Label ID="Label1" runat="server" Text="Email Address" AssociatedControlID="mTxtEmailAddress"></asp:Label>
                    <asp:TextBox ID="mTxtEmailAddress" runat="server" CssClass="large"></asp:TextBox>
                    <br />
                    <asp:Label ID="Label3" runat="server" Text="Subject" AssociatedControlID="mTxtMessage"></asp:Label>
                    <asp:TextBox ID="mTxtSubject" runat="server" CssClass="huge"></asp:TextBox> 
                    <br /> 
                    <asp:Label ID="Label2" runat="server" Text="Message" AssociatedControlID="mTxtMessage"></asp:Label> 
                    <asp:TextBox ID="mTxtMessage" runat="server" TextMode="MultiLine" CssClass="huge"></asp:TextBox>
                    <br />
                    <asp:Button ID="mBtnSend" runat="server" Text="Send" CssClass="btn" OnClick="mBtnSend_Click" />
                </fieldset>
            </ContentTemplate>
        </ajax:TabPanel>
    </ajax:TabContainer>    
</asp:Content>
