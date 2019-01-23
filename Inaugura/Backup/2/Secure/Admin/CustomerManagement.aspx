<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true"
    CodeFile="CustomerManagement.aspx.cs" Inherits="CustomerManagement" %>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="mPlaceHolderBody">
    <asp:Panel ID="mPanelAddCustomer" runat="server" Height="50px" Width="100%">
    <a href="javascript:OpenWindow('PopupControls/AddCustomer.aspx','600','450','AddCustomer')">Add Customer</a>        
    </asp:Panel>
</asp:Content>
