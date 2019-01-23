<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="Login.aspx.cs"
    Inherits="Login" Title="Untitled Page" %>
    <%@ MasterType VirtualPath="~/Default.master" %>
<asp:Content ID="Content4" runat="server" ContentPlaceHolderID="mPlaceHolderBody">
    <div style="text-align: center">
        <asp:Login ID="Login1" runat="server"
            UserNameLabelText="Email:" Width="309px" OnLoggedIn="Login1_LoggedIn" PasswordRecoveryText="Forogt your login?" PasswordRecoveryUrl="~/ForgotPassword.aspx" TitleText="" DisplayRememberMe="False">
        </asp:Login>
    </div>
</asp:Content>
