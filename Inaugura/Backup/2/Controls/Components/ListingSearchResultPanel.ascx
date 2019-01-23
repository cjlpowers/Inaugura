<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ListingSearchResultPanel.ascx.cs"
    Inherits="ListingSearchResultPanel" EnableViewState="false" %>
<%@ Register Src="ListingSearchPagePanel.ascx" TagName="ListingSearchPagePanel" TagPrefix="uc2" %>
<uc2:ListingSearchPagePanel ID="ListingSearchPagePanel2" runat="server" PageSize="5" />
<asp:Repeater ID="mRepeater" runat="server" EnableViewState="false">
    <HeaderTemplate>
        <table width="100%">
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td>
            <%#this.GetContent((Inaugura.RealLeads.Listing)Container.DataItem) %>
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
<uc2:ListingSearchPagePanel ID="ListingSearchPagePanel1" runat="server" PageSize="5" />

