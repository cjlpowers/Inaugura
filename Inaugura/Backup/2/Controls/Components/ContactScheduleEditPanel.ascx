<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ContactScheduleEditPanel.ascx.cs"
    Inherits="ContactScheduleEditPanel" %>
<%@ Register Src="~/Controls/Components/TimeControl.ascx" TagName="TimeControl" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Components/DaysControl.ascx" TagName="DaysControl" TagPrefix="uc2" %>
<div class="dContainer">
    <div class="dRow">
        <div class="dDark" style="float: left;">
            Schedule Name
        </div>
        <div class="dLight">
            <asp:TextBox ID="mTxtName" runat="server" CssClass="Control"></asp:TextBox>
        </div>
    </div>
    <div class="dRow">
        <div class="dDark" style="float: left;">
            Time
        </div>
        <div class="dLight">
            <uc1:TimeControl ID="mTimeStart" runat="server" AutoPostBack="True"></uc1:TimeControl>
            to
            <uc1:TimeControl ID="mTimeStop" runat="server"></uc1:TimeControl>
        </div>
    </div>
    <div class="dRow" id="mDateRow" runat="server">
        <div class="dDark" style="float: left;">
            Date
        </div>
        <div class="dLight" style="float: left">
            <asp:Calendar ID="mCalStartDate" runat="server" BackColor="White" BorderColor="#999999"
                DayNameFormat="Shortest" Font-Names="Arial" Font-Size="8pt" ForeColor="Black"
                Height="160px" Width="200px" CssClass="Empty">
                <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                <SelectorStyle BackColor="#CCCCCC" />
                <WeekendDayStyle BackColor="WhiteSmoke" />
                <OtherMonthDayStyle ForeColor="Gray" />
                <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                <NextPrevStyle VerticalAlign="Bottom" />
                <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
            </asp:Calendar>
        </div>
        <div class="dLight">
            <asp:Calendar ID="mCalStopDate" runat="server" BackColor="White" BorderColor="#999999"
                DayNameFormat="Shortest" Font-Names="Arial" Font-Size="8pt" ForeColor="Black"
                Height="160px" Width="200px" CssClass="Empty">
                <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                <SelectorStyle BackColor="#CCCCCC" />
                <WeekendDayStyle BackColor="WhiteSmoke" />
                <OtherMonthDayStyle ForeColor="Gray" />
                <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                <NextPrevStyle VerticalAlign="Bottom" />
                <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
            </asp:Calendar>
        </div>
    </div>
    <div class="dRow">
        <div class="dDark" style="float: left;">
            Contact Number
        </div>
        <div class="dLight">
            <asp:TextBox ID="mTxtContactNumber" CssClass="Control" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="dRow">
        <div class="dDark" style="float: left;">
            Voice Mail Rings:
        </div>
        <div class="dLight">
            <asp:DropDownList ID="mDlVoiceMailRings" CssClass="Control" runat="server">
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    <div class="dRow">
        <div class="dDark" style="float: left;">
            Days
        </div>
        <div class="dLight">
            <uc2:DaysControl ID="mDays" runat="server" />
        </div>
    </div>
</div>
<asp:HiddenField ID="mAllowDateChange" runat="server" />
