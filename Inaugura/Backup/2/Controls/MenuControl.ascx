<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MenuControl.ascx.cs" Inherits="MenuControl" %>
<%@ Register Src="MenuHeaderControl.ascx" TagName="MenuHeaderControl" TagPrefix="uc1" %>
<div class="panel">
    <uc1:MenuHeaderControl ID="MenuHeaderControl1" runat="server" />
    <div class="contentPanel">
        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
    </div>
</div>
