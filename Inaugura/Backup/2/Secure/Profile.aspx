<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="Profile.aspx.cs"
    Inherits="Profile_aspx" Title="Untitled Page" ValidateRequest="false" %>

<%@ MasterType VirtualPath="~/Default.master" %>
<asp:Content ID="Content3" ContentPlaceHolderID="mPlaceHolderBody" runat="server">
    <div style="text-align: center">
        <table class="Grid" cellpadding="0" cellspacing="0" style="width: 90%;">
            <tr>
                <th colspan="2">
                    Profile
                </th>
            </tr>
            <tr>
                <td class="dark">
                    First Name:</td>
                <td align="left">
                    <asp:TextBox ID="mTxtFirstName" runat="server" CssClass="SmTextField"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="dark">
                    Last Name:</td>
                <td align="left">
                    <asp:TextBox ID="mTxtLastName" runat="server" CssClass="SmTextField"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="dark">
                    Phone:</td>
                <td align="left">
                    <asp:TextBox ID="mTxtPhone" runat="server" CssClass="SmTextField"></asp:TextBox>
                    <asp:CheckBox ID="mChkHidePhoneNumber" runat="server" Text="Prevent other users from seeing my phone number." />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <a href="ChangePassword.aspx">Change Password</a>
                   <br /> 
                   <a href="Promotion.aspx">Enter A Promotional Code</a> 
                </td>
            </tr>
            <tr>
                <td colspan="2" class="dark">
                    <asp:Button ID="mBtnUpdate" runat="server" Text="Update" OnClick="mBtnUpdate_Click" />
                </td>
            </tr>            
        </table>
        <asp:Label ID="mLbErrorMsg" runat="server" CssClass="ErrorText"></asp:Label>
    </div>
</asp:Content>
