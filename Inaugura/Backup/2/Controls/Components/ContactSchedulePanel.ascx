<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ContactSchedulePanel.ascx.cs"
    Inherits="ContactSchedulePanel" %>
<%@ Register Src="ToolButton.ascx" TagName="ToolButton" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/MenuHeaderControl.ascx" TagName="MenuHeaderControl"
    TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Components/DaysControl.ascx" TagName="DaysControl" TagPrefix="uc1" %>
<table class="matrix" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <th id="mHeader" runat="server" colspan="5">
        </th>
    </tr>
    <tr id="mAdminRow" runat="server">
        <td class="admin" style="text-align: right" colspan="5">
            <uc3:ToolButton ID="mBtnEdit" runat="server" ImageUrl="~/Content/Images/Icons/Edit.gif" />
            <a id="mLnkDelete" runat="server" onclick="return confirm('Are you sure you want to delete the schedule?');">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Content/Images/Icons/Delete.gif"
                    ToolTip="Delete" CssClass="ToolButton" BorderWidth="1px" EnableViewState="False" /></a></td>
    </tr>
    <tr>
        <td class="dark">
            Time</td>
        <td><%#this.Time %>
        </td>
        <td class="dark">Dates</td>
        <td><%#this.Date %></td>
        <td rowspan="2" class="blank" align="center">
            <asp:Image ID="mImgDays" runat="server" ToolTip="Days of the week for which this contact schedule is enabled" /></td>
    </tr>
    <tr>
        <td class="dark">
            Phone</td>
        <td >
            <%#this.ContactNumber %>
        </td>
        <td class="dark">
            Voice Mail</td>
        <td>
            <%#this.VoiceMail %>
        </td>
    </tr>
</table>
