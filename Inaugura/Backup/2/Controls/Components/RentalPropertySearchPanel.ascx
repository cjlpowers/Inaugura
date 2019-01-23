<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RentalPropertySearchPanel.ascx.cs"
    Inherits="RealEstateSearchPanel" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<table class="Grid" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <th colspan="4" id="mHeader" runat="server">
        </th>
    </tr>
    <tr id="mRowLocation" runat="server">
        <td class="dark">
            Location</td>
        <td colspan="3">
            <div style="border-bottom: solid 1px gray; margin-bottom: 10px;">
                <asp:LinkButton ID="mBtnByPostal" runat="server" CssClass="Tab" OnClick="mBtnByPostal_Click">By Postal</asp:LinkButton>
                <asp:LinkButton ID="mBtnByCity" runat="server" CssClass="Tab" OnClick="mBtnByCity_Click">By City</asp:LinkButton>
                <asp:LinkButton ID="mBtnByCategory" runat="server" CssClass="Tab" OnClick="mBtnByCategory_Click">By Category</asp:LinkButton></div>
            <asp:PlaceHolder ID="mPhByPostal" runat="server">Postal:
                <asp:TextBox ID="mTxtPostal" runat="server" Width="50px"></asp:TextBox>
                <asp:DropDownList ID="mPostalRadius" runat="server">
                    <asp:ListItem Text="1 km" Value="1" />
                    <asp:ListItem Text="2 km" Value="2" />
                    <asp:ListItem Text="3 km" Value="3" />
                    <asp:ListItem Text="5 km" Value="5" Selected="True" />
                    <asp:ListItem Text="10 km" Value="10" />
                    <asp:ListItem Text="25 km" Value="25" />
                    <asp:ListItem Text="50 km" Value="50" />
                </asp:DropDownList>
                <div>
                    <asp:Label ID="mLbAddressError" runat="server"></asp:Label>
                </div>
                <%--<cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server">
                    <cc1:TextBoxWatermarkProperties TargetControlID="mTxtAddress" WatermarkText="or Enter Address" />
                </cc1:TextBoxWatermarkExtender>--%>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="mPhByCity" runat="server" Visible="False">
                <div>
                    <asp:DropDownList ID="mDlProvinces" runat="server" CssClass="Control" Width="200px">
                    </asp:DropDownList>
                </div>
                <div>
                    <asp:DropDownList ID="mDlCities" runat="server" CssClass="Control" Width="200px">
                    </asp:DropDownList>
                </div>
                <cc1:CascadingDropDown ID="mCascadingCity" runat="server" TargetControlID="mDlProvinces"
                    Category="province" PromptText="Please select a state/province" ServicePath="~/WebServices/AddressService.asmx"
                    ServiceMethod="GetDropDownContents" />
                <cc1:CascadingDropDown ID="mCascadingProvince" runat="server" TargetControlID="mDlCities"
                    Category="city" PromptText="Please select a city" ServicePath="~/WebServices/AddressService.asmx"
                    ServiceMethod="GetDropDownContents" ParentControlID="mDlProvinces" />
                <div>
                    <asp:Label ID="mLbCityError" runat="server"></asp:Label>
                </div>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="mPhByCategory" runat="server" Visible="False">
                <div>
                    <asp:DropDownList ID="mDlLocaleType" runat="server" CssClass="Control" Width="200px">
                    </asp:DropDownList>
                </div>
                <div>
                    <asp:DropDownList ID="mDlLocale" runat="server" CssClass="Control" Width="200px">
                    </asp:DropDownList>
                </div>
                <cc1:CascadingDropDown ID="mCascadingLocaleType" runat="server" TargetControlID="mDlLocaleType"
                    Category="localetype" PromptText="Select Category" ServicePath="~/WebServices/AddressService.asmx"
                    ServiceMethod="GetDropDownContents" />
                <cc1:CascadingDropDown ID="mCascadingLocale" runat="server" TargetControlID="mDlLocale"
                    Category="locale" PromptText="Select Place" ServicePath="~/WebServices/AddressService.asmx"
                    ServiceMethod="GetDropDownContents" ParentControlID="mDlLocaleType" />
                <div>
                    <asp:Label ID="mLbCategoryError" runat="server"></asp:Label>
                </div>
            </asp:PlaceHolder>
        </td>
    </tr>
    <tr id="mRowPrice" runat="server">
        <td class="dark">
            Monthly Rent
        </td>
        <td>
            <asp:DropDownList ID="mDlPriceLow" runat="server" CssClass="Control">
            </asp:DropDownList>
            -
            <asp:DropDownList ID="mDlPriceHigh" runat="server" CssClass="Control">
            </asp:DropDownList>
        </td>
        <td class="dark">
            Property Type
        </td>
        <td colspan="3">
            <asp:DropDownList ID="mDlPropertyType" runat="server" CssClass="Control">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="dark">
            Availability
        </td>
        <td>
            <asp:DropDownList ID="mAvailabilityDate" runat="server">
            </asp:DropDownList>
        </td>
        <td class="dark">
            Duration
        </td>
        <td>
            <asp:DropDownList ID="mDlDuration" runat="server">
                <asp:ListItem Value="0">Unspecified</asp:ListItem>
                <asp:ListItem Value="4">4 Months</asp:ListItem>
                <asp:ListItem Value="6">6 Months</asp:ListItem>
                <asp:ListItem Value="8">8 Months</asp:ListItem>
                <asp:ListItem Value="12">1 Years</asp:ListItem>
                <asp:ListItem Value="24">2 Years</asp:ListItem>
            </asp:DropDownList></td>
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
            Parking Spaces
        </td>
        <td>
            <asp:DropDownList ID="mDlParkingSpaces" runat="server" CssClass="Control">
            </asp:DropDownList>
        </td>
    </tr>
    <tr runat="server">
        <td class="dark" style="height: 20px">
            Features
        </td>
        <td colspan="3" style="height: 20px">
            <asp:CheckBox ID="mChkPets" runat="server" Text="Pets Allowed" />
                <asp:CheckBox ID="mChkElectricity" runat="server" Text="Includes Electricity" />
                <asp:CheckBox ID="mChkHeating" runat="server" Text="Includes Heating" />
                <asp:CheckBox ID="mChkLaundryService" runat="server" Text="Laundry Service" />
                <asp:CheckBox ID="mChkInternet" runat="server" Text="Internet" />
                <asp:CheckBox ID="mChkTelevision" runat="server" Text="Television" />
                <asp:CheckBox ID="mChkPool" runat="server" Text="Pool" />
        </td>
    </tr>
    <tr>
        <td colspan="4" style="text-align: center">
            <asp:Button ID="mBtnSearch" runat="server" Text="Search" OnClick="mBtnSearch_Click" />
        </td>
    </tr>
</table>
