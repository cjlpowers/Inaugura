<%@ Page Language="C#" MasterPageFile="~/PopupControl.master" AutoEventWireup="true"
    CodeFile="ContactListingOwner.aspx.cs" Inherits="ContactListingOwner" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/PopupControl.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset id="mFieldsetPhone" runat="server">
        <legend>Contact By Phone</legend>
        <asp:Label ID="Label2" runat="server" Text="Phone Number" AssociatedControlID="mLbPhoneNumber"></asp:Label> 
        <asp:Label ID="mLbPhoneNumber" runat="server" Text=""></asp:Label>                
    </fieldset>
    <fieldset>
        <legend>Contact By Email</legend>
        <asp:Panel ID="mPanelEmailMessage" runat="server">
            <asp:Label ID="Label1" runat="server" Text="Your Email Address" AssociatedControlID="mTxtEmailAddress"></asp:Label>
            <asp:TextBox ID="mTxtEmailAddress" runat="server" CssClass="large"></asp:TextBox>
            <br />
            <asp:Label ID="Label3" runat="server" Text="Message" AssociatedControlID="mTxtMessage"></asp:Label> 
            <asp:TextBox ID="mTxtMessage" runat="server" TextMode="MultiLine" CssClass="huge"  Height="100px"></asp:TextBox>
           <br /> 
            <asp:Button ID="mBtnSend" runat="server" Text="Send" OnClick="mBtnSend_Click" CssClass="btn" />
        </asp:Panel>
        <asp:Panel ID="mPanelSent" runat="server" Visible="false">
            <p>Your message has been sent.</p>
        </asp:Panel>
    </fieldset>
</asp:Content>
