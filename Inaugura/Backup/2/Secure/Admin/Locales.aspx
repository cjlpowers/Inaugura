<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="Locales.aspx.cs"
    Inherits="Secure_Admin_Locales" %>

<%@ MasterType VirtualPath="~/Default.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content4" runat="server" ContentPlaceHolderID="mPlaceHolderBody">
    <table width="100%">
        <tr>
            <td>
                <asp:ListBox ID="mLstLocales" runat="server" Width="400px"></asp:ListBox></td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div>
                <a>Name</a>  
                    <asp:TextBox ID="mTxtName" runat="server" Width="230px"></asp:TextBox>
                   <br />
                  <a>Type</a>  
                    <asp:DropDownList ID="mDlType" runat="server">
                    </asp:DropDownList>
                   <br />
                   <a>Street</a>
                    <asp:TextBox ID="mTxtStreet" runat="server" Width="284px"></asp:TextBox>
                   <br /> 
                   <a>Postal</a> 
                    <asp:TextBox ID="mTxtPostal" runat="server" Width="100px"></asp:TextBox> 
                   <br />
                  <a>Radius</a>  
                    <asp:TextBox ID="mTxtRadius" runat="server"></asp:TextBox>
                </div>
                <div>
                    <span>Entering a latitude and longitude will override the one determined by the address</span>
                    <asp:TextBox ID="mTxtLatitude" runat="server"></asp:TextBox>
                    <asp:TextBox ID="mTxtLongitude" runat="server"></asp:TextBox>
                </div>
                <asp:Button ID="mBtnAdd" runat="server" Text="Button" OnClick="mBtnAdd_Click" />
                <div>
                    <asp:Label ID="mLbGeocodeError" runat="server" Text=""></asp:Label>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
