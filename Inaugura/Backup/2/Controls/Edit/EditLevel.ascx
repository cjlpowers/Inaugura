<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditLevel.ascx.cs" Inherits="Controls_Edit_EditLevel" %>
<%@ Register Src="../../Controls/Components/MeasurementControl.ascx" TagName="MeasurementControl"
    TagPrefix="realleads" %>
<fieldset>
    <legend  id="legend" runat="server"></legend>
    <asp:Label ID="Label1" runat="server" Text="Name" AssociatedControlID="mTxtName"></asp:Label>
    <asp:TextBox ID="mTxtName" runat="server" CssClass="large" ToolTip="The name given to this level (ie 'First Foor', 'Main Floor'...)"></asp:TextBox>
    <br />
    <asp:Label ID="Label2" runat="server" Text="Above Grade" AssociatedControlID="mTxtName"></asp:Label>
    <asp:CheckBox ID="mChkAboveGrade" runat="server" />
    <br />
    <asp:Label ID="Label4" runat="server" Text="Level Size" AssociatedControlID="mSize"></asp:Label>
    <realleads:MeasurementControl ID="mSize" runat="server" Mode="InteriorArea" />
    <br />
    <asp:Label ID="Label3" runat="server" Text="Description" AssociatedControlID="mTxtDescription"></asp:Label>
    <asp:TextBox ID="mTxtDescription" runat="server" Rows="4" TextMode="MultiLine" ToolTip="The description of this level"></asp:TextBox>
    <br />
    <asp:Label ID="Label5" runat="server" Text="Features" AssociatedControlID="mTxtFeatures"></asp:Label>
    <asp:TextBox ID="mTxtFeatures" runat="server" TextMode="MultiLine" ToolTip="The features of this level, listed one per line"></asp:TextBox>
</fieldset>
