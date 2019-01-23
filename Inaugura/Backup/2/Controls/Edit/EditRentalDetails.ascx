<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditRentalDetails.ascx.cs"
    Inherits="Controls_Edit_EditRentalDetails" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="../../Controls/Components/MeasurementControl.ascx" TagName="MeasurementControl"
    TagPrefix="realleads" %>
<fieldset>
    <legend>Change Listing Details</legend>
    <label>
        Rent</label>
    <asp:TextBox ID="mTxtRent" runat="server" CssClass="small" ToolTip="The total monthly rent"></asp:TextBox>
    <br />
    <label>
        Available</label>
    <asp:TextBox ID="mTxtAvailabilityStart" runat="server"></asp:TextBox>
    <label>
        until</label>
    <asp:TextBox ID="mTxtAvailabilityEnd" runat="server"></asp:TextBox>
    <label>
        <font class="fine">(optional)</font></label>
    <ajax:CalendarExtender ID="mCalendarExtender1" TargetControlID="mTxtAvailabilityStart"
        runat="server" Format="MMMM d, yyyy">
    </ajax:CalendarExtender>
    <ajax:CalendarExtender ID="mCalendarExtender2" TargetControlID="mTxtAvailabilityEnd"
        runat="server" Format="MMMM d, yyyy">
    </ajax:CalendarExtender>
    <br />
    <label>
        Property Type</label>
    <asp:DropDownList ID="mDlPropertyType" runat="server" OnInit="mDlPropertyType_Init">
    </asp:DropDownList>
    <br />
    <label>
        Furnishings</label>
    <asp:DropDownList ID="mLstFurnishing" runat="server" CssClass="Control" OnInit="mLstFurnishing_Init">
    </asp:DropDownList>
    <br />
    <label>
        Size</label>
    <realleads:MeasurementControl ID="mSize" runat="server" Mode="InteriorArea" />
    <br />
    <label>
        Parking Spaces</label>
    <asp:DropDownList ID="mDlParking" runat="server" CssClass="Control">
        <asp:ListItem Value="0" Text="No Parking" />
        <asp:ListItem Value="1" Text="1 Space" />
        <asp:ListItem Value="2" Text="2 Spaces" />
        <asp:ListItem Value="3" Text="3 Spaces" />
        <asp:ListItem Value="4" Text="4 Spaces" />
    </asp:DropDownList>
    <asp:CheckBox ID="mChkParkingIncluded" runat="server" Text="Included with rent" />
    <br />
    <label>
        Appliances</label>
    <asp:TextBox ID="mTxtAppliances" runat="server" TextMode="MultiLine"
        ToolTip="List appliances one per line"></asp:TextBox>
    <label>
        <font class="fine">(List one per line)</font></label>
       <br />
       <label>
        Features</label>
       <asp:TextBox ID="mTxtFeatures" runat="server" TextMode="MultiLine" ToolTip="The listing features"></asp:TextBox> 
       <label>
        <font class="fine">(List one per line)</font></label>
       <br />
       <label>
        External Links</label>
       <asp:TextBox ID="mTxtLinks" runat="server" TextMode="MultiLine" ToolTip="External Links"></asp:TextBox> 
       <label>
        <font class="fine">(List one per line)</font></label> 
</fieldset>
<fieldset>
    <legend>Change Listing Features</legend>
    <asp:CheckBox ID="mChkPets" runat="server" Text="Pets Allowed" />
    <asp:CheckBox ID="mChkElectricity" runat="server" Text="Includes Electricity" />
    <asp:CheckBox ID="mChkHeating" runat="server" Text="Includes Heating" />
    <asp:CheckBox ID="mChkLaundryService" runat="server" Text="Laundry Service" />
    <asp:CheckBox ID="mChkInternet" runat="server" Text="Internet" />
    <asp:CheckBox ID="mChkTelevision" runat="server" Text="Television" />
    <asp:CheckBox ID="mChkPool" runat="server" Text="Pool" />
</fieldset>
