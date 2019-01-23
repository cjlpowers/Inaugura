<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RealEstateSearchPanel.ascx.cs"
    Inherits="RealEstateSearchPanel" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<table class="Grid" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <th colspan="4" id="mHeader" runat="server">
        </th>
    </tr>
    <%--  <tr id="mRowAdmin" runat="server">
        <td colspan="4" align="right" class="admin">
            <asp:CheckBox ID="mChkAdvancedSearch" runat="server" AutoPostBack="True" OnCheckedChanged="mChkAdvancedSearch_CheckedChanged"
                Text="Advanced Search" CssClass="Control" />
        </td>
    </tr>--%>
    <tr id="mRowLocation" runat="server">
        <td class="dark">
            Location</td>
        <td>
            <%--  <div>
                <asp:DropDownList ID="mDlCountries" runat="server" CssClass="Control" Width="200px">
                </asp:DropDownList>
            </div>--%>
            <div>
                <asp:DropDownList ID="mDlProvinces" runat="server" CssClass="Control" Width="200px">
                </asp:DropDownList>
            </div>
            <div>
                <asp:DropDownList ID="mDlCities" runat="server" CssClass="Control" Width="200px">
                </asp:DropDownList>
            </div>
        </td>
        <td>
            <div>
                <asp:DropDownList ID="mDlLocaleType" runat="server" CssClass="Control" Width="200px">
                </asp:DropDownList>
            </div>
            <div>
                <asp:DropDownList ID="mDlLocale" runat="server" CssClass="Control" Width="200px">
                </asp:DropDownList>
            </div>
        </td>
        <td>
            <asp:TextBox ID="mTxtAddress" runat="server" Width="250px"></asp:TextBox>
            <div>
                <asp:Label ID="mLbGeocodeError" runat="server"></asp:Label>
            </div>
        </td>
    </tr>
    <tr id="mRowPrice" runat="server">
        <td class="dark">
            Price
        </td>
        <td colspan="3">
            <asp:DropDownList ID="mDlPriceLow" runat="server" CssClass="Control">
            </asp:DropDownList>
            -
            <asp:DropDownList ID="mDlPriceHigh" runat="server" CssClass="Control">
            </asp:DropDownList>
        </td>
    </tr>
    <tr id="mRowPropertyType" runat="server">
        <td class="dark">
            Property Type
        </td>
        <td colspan="3">
            <asp:DropDownList ID="mDlPropertyType" runat="server" CssClass="Control">
            </asp:DropDownList>
        </td>
    </tr>
    <tr id="mRowBedBath" runat="server">
        <td class="dark">
            Bedrooms
        </td>
        <td>
            <asp:DropDownList ID="mDlBedrooms" runat="server" CssClass="Control">
            </asp:DropDownList>
        </td>
        <td class="dark">
            Bathrooms
        </td>
        <td>
            <asp:DropDownList ID="mDlBathrooms" runat="server" CssClass="Control">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="4" align="center">
            <asp:ImageButton ID="mBtnSearch" runat="server" OnClick="mBtnSearch_Click" />
        </td>
    </tr>
</table>
<cc1:CascadingDropDown ID="CascadingDropDown1" runat="server" TargetControlID="mDlProvinces"
    Category="province" PromptText="Please select a state/province" ServicePath="~/WebServices/AddressService.asmx"
    ServiceMethod="GetDropDownContents" ParentControlID="mDlCountries" />
<cc1:CascadingDropDown ID="CascadingDropDown2" runat="server" TargetControlID="mDlCities"
    Category="city" PromptText="Please select a city" ServicePath="~/WebServices/AddressService.asmx"
    ServiceMethod="GetDropDownContents" ParentControlID="mDlProvinces" />
<cc1:CascadingDropDown ID="CascadingDropDown3" runat="server" TargetControlID="mDlLocaleType"
    Category="localetype" PromptText="or Select Category" ServicePath="~/WebServices/AddressService.asmx"
    ServiceMethod="GetDropDownContents" />
<cc1:CascadingDropDown ID="CascadingDropDown4" runat="server" TargetControlID="mDlLocale"
    Category="locale" PromptText="Select Place" ServicePath="~/WebServices/AddressService.asmx"
    ServiceMethod="GetDropDownContents" ParentControlID="mDlLocaleType" />
<cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="mTxtAddress" WatermarkText="Enter Address" >
</cc1:TextBoxWatermarkExtender>
