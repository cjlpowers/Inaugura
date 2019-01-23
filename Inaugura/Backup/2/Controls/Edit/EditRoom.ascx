<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditRoom.ascx.cs" Inherits="Controls_Edit_EditRoom" %>
<%@ Register Src="../../Controls/Components/MeasurementControl.ascx" TagName="MeasurementControl"
    TagPrefix="realleads" %>
<fieldset>
<legend  id="legend" runat="server"></legend>
        <asp:Label ID="Label3" Text="Name" AssociatedControlID="mTxtName" runat="server"></asp:Label>
        <asp:TextBox ID="mTxtName" runat="server" CssClass="large"></asp:TextBox>
        <br />
        <asp:Label ID="Label1" Text="Description" AssociatedControlID="mTxtDescription" runat="server"></asp:Label>
        <asp:TextBox ID="mTxtDescription" runat="server" Rows="4" TextMode="MultiLine"></asp:TextBox>
        <br />
        <asp:Label ID="Label2" Text="Type" AssociatedControlID="mLstType" runat="server"></asp:Label>
        <asp:DropDownList ID="mLstType" runat="server" CssClass="Control" OnInit="mLstType_Init">
        </asp:DropDownList>
        <br />
        <asp:Label ID="Label4" Text="Width" AssociatedControlID="mWidth" runat="server"></asp:Label>
        <realleads:MeasurementControl ID="mWidth" runat="server" Mode="InteriorLength" />
        <br />
        <asp:Label ID="Label5" Text="Depth" AssociatedControlID="mDepth" runat="server"></asp:Label>
        <realleads:MeasurementControl ID="mDepth" runat="server" Mode="InteriorLength" />
        <br />
        <asp:Label ID="Label6" Text="Flooring" AssociatedControlID="mLstFlooring" runat="server"></asp:Label>
        <asp:DropDownList ID="mLstFlooring" runat="server" OnInit="mLstFlooring_Init">
        </asp:DropDownList>
        <br />
        <asp:Label ID="Label7" runat="server" Text="Features" AssociatedControlID="mTxtFeatures"></asp:Label>
        <asp:TextBox ID="mTxtFeatures" runat="server" TextMode="MultiLine" ToolTip="The features of this room"></asp:TextBox>
    </fieldset>