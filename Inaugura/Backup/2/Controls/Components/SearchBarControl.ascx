<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchBarControl.ascx.cs"
    Inherits="SearchBarControl" %>
<div runat="server" id="mDiv">
    <div style="position: relative; left: 482px; top: 39px;">
        <asp:TextBox ID="mTxtCode" runat="server" CssClass="Control" MaxLength="4" Width="32px"></asp:TextBox>
    </div>
    <div style="position: relative; left: 520px; top: 18px;">
        <asp:ImageButton ID="mBtnGo" runat="server" OnClick="mBtnGo_Click" />
    </div>
    <div style="position: relative; left: 480px; top: 23px;">
        <asp:HyperLink ID="mLnkSearch" runat="server" Font-Size="110%" Font-Underline="True"
            ForeColor="White" Font-Bold="True">Search Listings</asp:HyperLink>
    </div>
</div>
<asp:Label ID="mLbError" runat="server" CssClass="ErrorText" Visible="false" EnableViewState="false"></asp:Label>
