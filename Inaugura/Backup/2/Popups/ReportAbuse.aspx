<%@ Page Language="C#" MasterPageFile="~/PopupControl.master" AutoEventWireup="true" CodeFile="ReportAbuse.aspx.cs" Inherits="Popups_ReportAbuse" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/PopupControl.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<fieldset>
<legend>Was the content of this listing offensive to you?</legend>
<p>If you have found the content of this listing to be inappropriate or offensive please click the button below.</p>   
 </fieldset>    
<asp:Button ID="mBtnReportAbuse" runat="server" Text="Report Abuse" OnClick="mBtnReportAbuse_Click" CssClass="btn" /> 
</asp:Content>

