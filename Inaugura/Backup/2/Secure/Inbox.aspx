<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="Inbox.aspx.cs" Inherits="Secure_Inbox" ValidateRequest="false"%>
<%@ MasterType VirtualPath="~/Default.master" %>
<asp:Content ID="Content3" ContentPlaceHolderID="mPlaceHolderBody" runat="server">
    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:GridView ID="GridView1" runat="server" Width="100%" AutoGenerateColumns="False"
                    BorderColor="#6BC020" BorderStyle="Solid" GridLines="Horizontal" BorderWidth="1px" SkinID="GridViewSkin"
                    Font-Size="8pt">
                    <Columns>
                        <asp:ImageField DataImageUrlField="ImageUrl">
                        </asp:ImageField>
                        <asp:BoundField DataField="Date" HeaderText="Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" />
                            <HeaderStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Time" HeaderText="Time">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Caller ID" DataField="CallerID">
                            <HeaderStyle Width="50%" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:TemplateField>
                            <HeaderStyle Width="5%" />
                            <ItemTemplate>
                                <a href='<%# Eval("PlayUrl")%>'>
                                    <img align="middle" alt="Play" src='<%# Eval("PlayImage")%>' /></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                          <asp:TemplateField>
                            <HeaderStyle Width="5%" />
                            <ItemTemplate>
                                <a href='<%# Eval("DeleteUrl")%>' onclick="return confirm('Are you sure you want to delete this voice mail');">
                                    <img align="middle" alt="Play" src='<%# Eval("DeleteImage")%>' /></a>
                            </ItemTemplate>
                        </asp:TemplateField>                       
                    </Columns>
                    <AlternatingRowStyle BackColor="WhiteSmoke" />
                    <HeaderStyle BorderStyle="None" ForeColor="#6BC020" VerticalAlign="Top" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="center" style="height: 18px">
                <asp:Label ID="mLbVoiceMailEmpty" runat="server" Text="Your voice mail box is empty."
                    Visible="False" CssClass="Text"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Image ID="Image1" runat="server" Width="574px" ImageUrl="~/Content/Images/FadeBar.gif" />
            </td>
        </tr>
    </table>
    <br />
</asp:Content>


