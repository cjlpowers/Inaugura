<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ListingSearchPagePanel.ascx.cs"
    Inherits="ListingSearchPagePanel" %>
<table width="95%" cellspacing="0" cellpadding="0">
    <tr>
        <td>
            <span id="mSpan" runat="server"></span>
        </td>
        <td align="right">
            <a id="mLnkPageStart" runat="server">
                <asp:Image ID="mImgPageStart" runat="server" ImageUrl="~/Content/Images/PageStart.gif" AlternateText="First Page" /></a>
            <a id="mLnkPagePrev" runat="server">
                <asp:Image ID="mImgPagePrev" runat="server" ImageUrl="~/Content/Images/PagePrev.gif" AlternateText="Previous Page" /></a>
            <span id="mRow" runat="server"></span><a id="mLnkPageNext" runat="server"><asp:Image ID="mImgPageNext" runat="server" ImageUrl="~/Content/Images/PageNext.gif" AlternateText="Next Page"/></a><a id="mLnkPageEnd" runat="server"><asp:Image ID="mImgPageEnd" runat="server" ImageUrl="~/Content/Images/PageEnd.gif" AlternateText="Last Page" /></a>
        </td>
    </tr>
</table>