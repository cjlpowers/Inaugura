<%@ Control Language="C#" AutoEventWireup="true" CodeFile="StringCollectionPanel.ascx.cs"
    Inherits="StringCollectionPanel" %>
<asp:Repeater ID="mRepeater" runat="server">
    <HeaderTemplate>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li>
            <%#DataBinder.Eval(Container, "DataItem") %>
        </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>
<div id="mAdmin" runat="server">
    <div>
        <asp:TextBox ID="mTxtItems" runat="server" Rows="4" TextMode="MultiLine" Width="100%" CssClass="Control"></asp:TextBox>
    </div>  
</div>
