<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SignUpContactSchedule.ascx.cs"
    Inherits="SignUpContactSchedule" %>
<%@ Register Src="TimeControl.ascx" TagName="TimeControl" TagPrefix="uc1" %>
<table>
    <tr>
        <td>
            Start Time</td>
        <td>
            <uc1:TimeControl id="mStartTime" runat="server">
            </uc1:TimeControl>
        </td>
    </tr>
    <tr>
        <td>
            Stop Time</td>
        <td>
            <uc1:TimeControl id="mStopTime" runat="server">
            </uc1:TimeControl>
        </td>
    </tr>
</table>
