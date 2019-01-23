<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditAddress.ascx.cs" Inherits="Controls_Edit_EditAddress" %>
<fieldset>
<legend>Change Listing Address</legend>
<label>Street Address</label>
<asp:TextBox ID="mTxtStreet" runat="server" CssClass="large"></asp:TextBox>
<br />
<label>Postal Code</label>
<asp:TextBox ID="mTxtPostal" MaxLength="7" runat="server" CssClass="small"></asp:TextBox>
</fieldset>
