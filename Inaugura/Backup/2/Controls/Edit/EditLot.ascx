<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditLot.ascx.cs" Inherits="Controls_Edit_EditLot" %>
<%@ Register Src="../../Controls/Components/MeasurementControl.ascx" TagName="MeasurementControl"
    TagPrefix="realleads" %>
<fieldset>
    <asp:Label ID="Label2" runat="server" Text="Lot Size" AssociatedControlID="mLotSize"></asp:Label>
    <realleads:MeasurementControl ID="mLotSize" runat="server" Mode="ExteriorArea" />
    <br />
    <asp:Label ID="Label1" runat="server" Text="Description" AssociatedControlID="mTxtLotDescription"></asp:Label>
    <asp:TextBox ID="mTxtLotDescription" runat="server" Rows="4" TextMode="MultiLine"
        CssClass="huge" ToolTip="A description of the lot on which this listing is located"></asp:TextBox>
    <br />
    <asp:Label ID="Label6" runat="server" Text="Features" AssociatedControlID="mTxtFeatures"></asp:Label>
    <asp:TextBox ID="mTxtFeatures" runat="server" TextMode="MultiLine" ToolTip="The features of this lot"></asp:TextBox>
</fieldset>
