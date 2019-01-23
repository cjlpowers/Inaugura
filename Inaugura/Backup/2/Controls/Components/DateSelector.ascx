<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DateSelector.ascx.cs" Inherits="Controls_Components_DateSelector" %>
<div>
    <asp:DropDownList ID="mDlMonth" runat="server" OnSelectedIndexChanged="mDlMonth_SelectedIndexChanged" AutoPostBack="True">
    </asp:DropDownList>
     <asp:DropDownList ID="mDlYear" runat="server" OnSelectedIndexChanged="mDlYear_SelectedIndexChanged" AutoPostBack="True">
     </asp:DropDownList>
</div>
<div>
<asp:Calendar ID="mCal" runat="server" ShowGridLines="True" ShowTitle="False" CssClass="Calendar">
    <OtherMonthDayStyle ForeColor="Gray" />
</asp:Calendar>
</div>

