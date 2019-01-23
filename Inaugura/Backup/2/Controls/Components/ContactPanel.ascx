<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ContactPanel.ascx.cs"
    Inherits="ContactPanel_ascx" %>
<%@ Register Src="../MenuHeaderControl.ascx" TagName="MenuHeaderControl" TagPrefix="uc1" %>
<table cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <uc1:MenuHeaderControl ID="MenuHeaderControl1" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="MenuPanel">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="right">
                        <asp:Label ID="mLbFirstName" runat="server" CssClass="ImportantText" Text="First Name:"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="mTxtFirstName" runat="server" CssClass="SmTextField"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="mLbLastName" runat="server" CssClass="ImportantText" Text="Last Name:"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="mTxtLastName" runat="server" CssClass="SmTextField"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="mLbPhone" runat="server" CssClass="ImportantText" Text="Phone:"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="mTxtPhone" runat="server" CssClass="SmTextField"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="mLbErrorMsg" runat="server" CssClass="ErrorText"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
