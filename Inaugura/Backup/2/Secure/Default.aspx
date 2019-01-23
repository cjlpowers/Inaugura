<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="Default_aspx" Title="Untitled Page" ValidateRequest="false" %>
<%@ Register Assembly="EeekSoft.Web.PopupWin" Namespace="EeekSoft.Web" TagPrefix="cc1" %>
<%@ MasterType VirtualPath="~/Default.master" %>
<asp:Content ID="Content3" ContentPlaceHolderID="mPlaceHolderBody" runat="server">
    <table width="100%" cellpadding="0" cellspacing="0">
        <tr class="FadeDown">
            <td colspan="2">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="PicCell">
                            <asp:Image ID="Image5" runat="server" ImageUrl="~/Content/Images/Home.gif" />
                        </td>
                        <td>
                            <p class="Heading">
                                Account Details</p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table>
                    <tr>
                        <td class="ImportantText">
                            Listing Number:</td>
                        <td>
                            <asp:Label CssClass="Text" ID="mLbListingNumber" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="ImportantText">
                            Expiration Date:</td>
                        <td>
                            <asp:Label CssClass="Text" ID="mLbExpirationDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr class="FadeDown">
            <td colspan="2">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="PicCell">
                            <a href="VoiceMail.aspx">
                                <asp:Image ID="mImgVoiceMail" runat="server" ImageUrl="~/Content/Images/VoiceMail_Empty.gif" /></a>
                        </td>
                        <td>
                            <p class="Heading">
                                Voice Mail</p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table>
                    <tr>
                        <td class="ImportantText">
                            New Messages:</td>
                        <td>
                            <asp:Label CssClass="Text" ID="mLbNewMessages" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="ImportantText">
                            Old Messages:</td>
                        <td>
                            <asp:Label CssClass="Text" ID="mLbOldMessages" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="ImportantText">
                            Latest Message:</td>
                        <td>
                            <asp:Label CssClass="Text" ID="mLbLatestMessage" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr class="FadeDown">
            <td colspan="2">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="PicCell">
                            <a href="ContactSchedule.aspx">
                                <asp:Image ID="Image3" runat="server" ImageUrl="~/Content/Images/Clock.gif" />
                            </a>
                        </td>
                        <td>
                            <p class="Heading">
                                Contact Settings</p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table>
                    <tr>
                        <td class="ImportantText">
                            Current Status:</td>
                        <td>
                            <asp:Label CssClass="Text" ID="mLbContactStatus" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr class="FadeDown">
            <td colspan="2">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="PicCell">
                            <a href="activity.aspx">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Content/Images/Chart.gif" />
                            </a>
                        </td>
                        <td>
                            <p class="Heading">
                                Call Activity</p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>      
        <tr>
            <td colspan="2" align="center">
                <asp:Image ID="mImgActivity" runat="server" Width="574" Height="300" /></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:GridView AutoGenerateColumns="False" BorderColor="#6BC020" BorderStyle="Solid"
                    BorderWidth="1px" Font-Size="8pt" GridLines="Horizontal" ID="mGridActivity" runat="server"
                    Width="100%">
                    <AlternatingRowStyle BackColor="WhiteSmoke" />
                    <HeaderStyle BorderStyle="None" ForeColor="#6BC020" VerticalAlign="Top" />
                    <Columns>
                        <asp:BoundField DataField="CallTime" HeaderText="Call Time">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CallerID" HeaderText="Caller ID">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CallDuration" HeaderText="Duration">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:CheckBoxField DataField="AgentAccepted" HeaderText="Call Accepted">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:CheckBoxField>
                        <asp:CheckBoxField DataField="SentToVoiceMail" HeaderText="Sent to Voice Mail">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:CheckBoxField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="height: 18px">
                <asp:Label CssClass="Text" ID="mLbNoCallsToday" runat="server" Text="You have not had any calls today."
                    Visible="False"></asp:Label>
            </td>
        </tr>           
    </table>
    <cc1:popupwin id="PopupWin1" runat="server" actiontype="RaiseEvents" colorstyle="Custom"
        darkshadow="128, 128, 128" gradientdark="224, 224, 224" lightshadow="192, 192, 192"
        message="" onlinkclicked="PopupWin1_LinkClicked" shadow="128, 128, 128" textcolor="64, 64, 64"
        title="New Messages" visible="False" HideAfter="8000"></cc1:popupwin>
</asp:Content>
