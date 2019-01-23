<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ContactScheduleCollectionPanel.ascx.cs"
    Inherits="ContactScheduleCollectionPanel" %>
<%@ Register Src="ContactSchedulePanel.ascx" TagName="ContactSchedulePanel" TagPrefix="uc1" %>
<asp:Repeater ID="mRepeater" runat="server">
    <HeaderTemplate>
        <table style="width: 100%">          
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <uc1:ContactSchedulePanel ID="ContactSchedulePanel1" runat="server" ContactSchedule='<%#Container.DataItem %>' />
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
