<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="Feedback.aspx.cs" Inherits="Feedback_aspx" Title="Untitled Page" ValidateRequest="false"%>
<%@ MasterType VirtualPath="~/Default.master" %>
<asp:Content ID="Content3" ContentPlaceHolderID="mPlaceHolderBody" Runat="server">
    <table width="100%"><tr>
            <td  align="right">
                <asp:Label ID="Label2" Runat="server" CssClass="ImportantText" Text="From:"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="mTxtFrom" Runat="server" CssClass="MedTextField"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td  align="right">
                <asp:Label ID="Label3" Runat="server" CssClass="ImportantText" Text="Email:"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="mTxtEmail" Runat="server" CssClass="MedTextField"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label1" Runat="server" CssClass="ImportantText" Text="Message:"></asp:Label></td>
            <td align="left">
                <asp:TextBox ID="mTxtMessage" Runat="server" Width="95%" Height="80px" TextMode="MultiLine"
                    CssClass="TextBox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center" style="height: 26px">
                <asp:Button ID="mBtnSend" Runat="server" Text="Send" CssClass="Button" OnClick="mBtnSend_Click" />
                <asp:Label ID="mLbResult" Runat="server" Text="Your feedback has been submitted. Thank you."
                    CssClass="ImportantText" Visible="False"></asp:Label>
            </td>
        </tr></table>
    <br />
</asp:Content>
