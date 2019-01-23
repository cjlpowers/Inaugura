<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="Promotion.aspx.cs"
    Inherits="Promotion" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/Default.master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="mPlaceHolderBody" runat="Server">
    <div style="text-align: center">
        <asp:TextBox ID="mTxtCode" runat="server"></asp:TextBox>
        <asp:Button ID="mBtnRedeem" runat="server" OnClick="mBtnRedeem_Click" Text="Redeem" />
    </div>
    <asp:PlaceHolder ID="mPhSuccess" Visible="false" runat="server">
        <p>
            The promotion has been applied to your user account.</p>
    </asp:PlaceHolder>
</asp:Content>
