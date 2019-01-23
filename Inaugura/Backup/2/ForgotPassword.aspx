<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="ForgotPassword.aspx.cs"
    Inherits="ForgotPassword" ValidateRequest="false" %>

<%@ MasterType VirtualPath="~/Default.master" %>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="mPlaceHolderBody">
    <div style="text-align: center">
        <asp:PasswordRecovery ID="PasswordRecovery1" runat="server" UserNameInstructionText="Enter your email address to receive a new password." UserNameLabelText="Email:" UserNameTitleText="">
        </asp:PasswordRecovery>
    </div>
</asp:Content>
