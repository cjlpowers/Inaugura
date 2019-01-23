<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddressPanel.ascx.cs" Inherits="AddressPanel_ascx" %>
 <table width="100%" cellpadding="0" cellspacing="0">       
        <tr>
            <td colspan="2">
                <p id="mLbAddress" class="Heading" runat="server">
                    Address</p>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="mLbStreet" Runat="server" CssClass="ImportantText" Text="Street:"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="mTxtStreet" Runat="server" CssClass="LgTextField"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="mLbCity" Runat="server" CssClass="ImportantText" Text="City:"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="mTxtCity" Runat="server" CssClass="SmTextField"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="mLbStateProv" Runat="server" CssClass="ImportantText" Text="Province:"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="mTxtStateProv" Runat="server" CssClass="SmTextField"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="mLbCountry" Runat="server" CssClass="ImportantText" Text="Country:"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="mTxtCountry" Runat="server" CssClass="SmTextField"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label1" Runat="server" CssClass="ImportantText" Text="Postal Code:"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="mTxtZipPostal" Runat="server" CssClass="SmTextField"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="mLbErrorMsg" Runat="server" CssClass="ErrorText"></asp:Label>
            </td>
        </tr>
    </table>
