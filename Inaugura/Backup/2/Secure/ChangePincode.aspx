<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true"
    CodeFile="ChangePincode.aspx.cs" Inherits="ChangePincode" ValidateRequest="false" %>

<%@ MasterType VirtualPath="~/Default.master" %>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="mPlaceHolderBody">
    <table class="Grid" cellpadding="0" cellspacing="0" width="570">
        <tr>
            <th colspan="2">
                Pincode
            </th>
        </tr>
        <tr>
            <td id="mPincode" runat="server" colspan="2">
            </td>
        </tr>
        <tr>
            <td class="dark">
                New Pincode:</td>
            <td align="left">
                <asp:TextBox ID="mTxtPincode" runat="server" MaxLength="4" Width="40px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="dark" align="right" colspan="2">
                <asp:ImageButton ID="mBtnUpdate" runat="server" ImageUrl="~/Content/en/Images/Buttons/Update.gif" OnClick="mBtnUpdate_Click"/>
            </td>
        </tr>
    </table>
    <asp:Label ID="mLbError" runat="server"></asp:Label>
    <br />
</asp:Content>
