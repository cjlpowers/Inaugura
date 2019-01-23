<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="Promotions.aspx.cs" Inherits="Secure_Admin_Promotions" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/Default.master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="mPlaceHolderBody" Runat="Server">
<fieldset>
<legend>Add A Promtotion</legend>
    <asp:Label ID="Label4" runat="server" Text="Count" AssociatedControlID="mTxtCode"></asp:Label>
    <asp:TextBox ID="mTxtCode" runat="server" Text="1"></asp:TextBox>
    <br />
    <asp:Label ID="Label1" runat="server" Text="Description" AssociatedControlID="mTxtDescription"></asp:Label>
    <asp:TextBox ID="mTxtDescription" runat="server" CssClass="huge"></asp:TextBox> 
    <br />
    <asp:Label ID="Label3" runat="server" Text="Count" AssociatedControlID="mTxtCount"></asp:Label>
    <asp:TextBox ID="mTxtCount" runat="server" Text="1"></asp:TextBox>
    <br />
    <asp:Label ID="Label2" runat="server" Text="Actions" AssociatedControlID="mTxtActions"></asp:Label>
    <asp:TextBox ID="mTxtActions" runat="server" CssClass="huge"></asp:TextBox> 
    <br />    
    <asp:Button ID="mBtn" runat="server" Text="Add" OnClick="mBtn_Click" />
</fieldset>
<fieldset>
<legend>Current Promotions</legend>
    <asp:GridView ID="mGridPromotions" runat="server" AutoGenerateColumns="False" Width="100%" CellPadding="4" GridLines="None">
        <Columns>
            <asp:BoundField DataField="Code" HeaderText="Code" />
            <asp:BoundField DataField="Count" HeaderText="Count" />
            <asp:BoundField DataField="Description" HeaderText="Description" />
        </Columns>
        <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="Gainsboro" />
        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
        <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
        <HeaderStyle BackColor="DimGray" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="White" />
    </asp:GridView>
</fieldset>
</asp:Content>

