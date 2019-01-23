<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true"
    CodeFile="ContactSchedule.aspx.cs" Inherits="ContactSchedule" ValidateRequest="false" %>

<%@ Register Src="../Controls/Components/ContactSchedulePanel.ascx" TagName="ContactSchedulePanel"
    TagPrefix="uc2" %>
<%@ Register Src="../Controls/Components/ContactScheduleCollectionPanel.ascx" TagName="ContactScheduleCollectionPanel"
    TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/Default.master" %>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="mPlaceHolderBody">
    <div>
        &nbsp;
        <asp:Image ID="mBtnAddSchedule" runat="server" ImageUrl="~/Content/en/Images/Buttons/NewSchedule.gif" /></div>
    <uc1:ContactScheduleCollectionPanel ID="mContactSchedules" runat="server"></uc1:ContactScheduleCollectionPanel>
</asp:Content>
