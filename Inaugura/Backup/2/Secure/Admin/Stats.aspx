<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="Stats.aspx.cs"
    Inherits="Secure_Admin_Stats" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/Default.master" %>
<asp:Content ID="Content4" ContentPlaceHolderID="mPlaceHolderBody" runat="Server">
    <div style="text-align: center">
        <table class="Grid" cellpadding="0" cellspacing="0" style="width: 90%">
            <tr>
                <th colspan="2">
                    Listings</th>
            </tr>
            <tr>
                <td class="dark">
                    Total Listings
                </td>
                <td>
                    <asp:Label ID="mLbTotalListings" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td class="dark">
                    Total Active
                </td>
                <td>
                    <asp:Label ID="mLbActiveListings" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td class="dark">
                    Total Inactive
                </td>
                <td>
                    <asp:Label ID="mLbInactiveListings" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td class="dark">
                    Total Suspended
                </td>
                <td>
                    <asp:Label ID="mLbSuspendedListings" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td class="dark">
                    Total Sold
                </td>
                <td>
                    <asp:Label ID="mLbSoldListings" runat="server"></asp:Label></td>
            </tr>
        </table>
        <table class="Grid" cellpadding="0" cellspacing="0" style="width: 90%">
            <tr>
                <th colspan="2">
                    Cache</th>
            </tr>
            <tr>
                <td class="dark">
                    Kilobytes Avaliable
                </td>
                <td>
                    <asp:Label ID="mCacheKilobytesAvaliable" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td class="dark">
                    Count
                </td>
                <td>
                    <asp:Label ID="mCount" runat="server"></asp:Label></td>
            </tr>
        </table>
    </div>
</asp:Content>
