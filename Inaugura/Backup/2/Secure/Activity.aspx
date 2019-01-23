<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true"
    CodeFile="Activity.aspx.cs" Inherits="Activity_aspx" ValidateRequest="false" %>
<%@ Register Assembly="Inaugura" Namespace="Inaugura.Web.Controls" TagPrefix="cc1" %>
<%@ MasterType VirtualPath="~/Default.master" %>
<asp:Content ID="Content3" ContentPlaceHolderID="mPlaceHolderBody" runat="server">
    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:Image ID="Image2" runat="server" Height="300px" Width="574px" /></td>
        </tr>
        <tr>
            <td>
                <asp:Image ID="Image3" runat="server" Height="300px" Width="574px" /></td>
        </tr>
        <tr>
            <td>
                <p id="mLbLogin" class="Heading" runat="server">
                    Recent Calls</p>
            </td>
        </tr>
        <tr>
            <td>
                <p id="P2" class="Text" runat="server">
                    The following is a list of your most recent calls.</p>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
                <asp:GridView ID="GridView1" runat="server" Width="98%" BorderColor="#6BC020" BorderStyle="Solid"
                    GridLines="Horizontal" BorderWidth="1px" SkinID="GridViewSkin" AutoGenerateColumns="False" Font-Size="8pt">
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
                        <asp:HyperLinkField Text="Details" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="javascript: OpenWindow('PopupControls/CallLog.aspx?id={0}','600','250','CallDetails')" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Image ID="Image1" runat="server" Width="560px" ImageUrl="~/Content/Images/FadeBar.gif" />
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
