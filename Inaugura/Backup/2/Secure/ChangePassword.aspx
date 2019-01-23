<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs"
    Inherits="ChangePassword" ValidateRequest="false" %>
<%@ MasterType VirtualPath="~/Default.master" %>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="mPlaceHolderBody">
    <div style="text-align: center">
        <asp:ChangePassword ID="ChangePassword1" runat="server" CancelDestinationPageUrl="~/Secure/Profile.aspx"
            ContinueDestinationPageUrl="~/Secure/Profile.aspx" ChangePasswordTitleText="">
        </asp:ChangePassword>
    </div>
</asp:Content>
