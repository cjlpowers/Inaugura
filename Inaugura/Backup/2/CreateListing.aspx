<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateListing.aspx.cs" Inherits="Signup"
    ValidateRequest="false" MasterPageFile="~/Default.master" %>

<%@ Register Src="Controls/Components/DaysControl.ascx" TagName="DaysControl" TagPrefix="uc5" %>
<%@ Register Src="Controls/Components/TimeControl.ascx" TagName="TimeControl" TagPrefix="uc2" %>
<%@ Register Src="Controls/Components/ContactScheduleEditPanel.ascx" TagName="ContactScheduleEditPanel"
    TagPrefix="uc8" %>
<%@ MasterType VirtualPath="~/Default.master" %>
<%@ Register Src="~/Controls/ContentControl.ascx" TagName="ContentControl" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Editor.ascx" TagName="Editor" TagPrefix="uc4" %>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="mPlaceHolderBody">
    <asp:Wizard ActiveStepIndex="0" ID="Wizard1" runat="server" Width="100%" Height="250px"
        OnNextButtonClick="Wizard1_NextButtonClick" FinishDestinationPageUrl="~/Default.aspx"
        OnFinishButtonClick="Wizard1_FinishButtonClick" DisplaySideBar="False">
        <WizardSteps>
            <asp:WizardStep ID="Welcome" runat="server" Title="Welcome" StepType="Start">
                <uc1:ContentControl ID="mContentWelcome" runat="server" ContentPath="Signup\SignupWelcomeHeader.html" />
            </asp:WizardStep>
            <asp:WizardStep runat="server" Title="Email Address" ID="Email">
                <uc1:ContentControl ID="mContentEmail" runat="server" ContentPath="Signup\SignupEmailHeader.html" />
                <table class="Grid" cellpadding="0" cellspacing="0" style="width: 90%">
                    <tr>
                        <td class="dark">
                            Email Address</td>
                        <td>
                            <asp:TextBox ID="mTxtEmail" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr id="mRowEmailPassword" runat="server" visible="False">
                        <td class="dark" runat="server">
                            Password
                        </td>
                        <td runat="server">
                            <asp:TextBox ID="mTxtEmailPassword" runat="server" CssClass="LoginTextField" TextMode="Password"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <asp:Label ID="mLbEmailErrorMsg" runat="server" CssClass="ErrorText"></asp:Label>
            </asp:WizardStep>
            <asp:WizardStep ID="Name" runat="server" Title="Name">
                <uc1:ContentControl ID="mContentName" runat="server" ContentPath="Signup\SignupNameHeader.html" />
                <table class="Grid" cellpadding="0" cellspacing="0" style="width: 90%">
                    <tr>
                        <td class="dark">
                            First Name</td>
                        <td>
                            <asp:TextBox ID="mTxtFirstName" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="dark">
                            Last Name
                        </td>
                        <td>
                            <asp:TextBox ID="mTxtLastName" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <asp:Label ID="mLbNameErrorMsg" runat="server" CssClass="ErrorText"></asp:Label>
            </asp:WizardStep>
            <asp:WizardStep ID="PhoneNumber" runat="server" Title="Phone Number">
                <uc1:ContentControl ID="ContentControl1" runat="server" ContentPath="Signup\SignupPhoneNumberHeader.html" />
                <table class="Grid" cellpadding="0" cellspacing="0" style="width: 90%">
                    <tr>
                        <td class="dark" rowspan="2">
                            Phone Number</td>
                        <td>
                            <asp:TextBox ID="mTxtPhoneNumber" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:CheckBox ID="mChkHidePhoneNumber" runat="server" Text="Prevent other users from seeing my phone number." />
                        </td>
                    </tr>
                </table>
                <asp:Label ID="mLbPhoneNumberErrorMsg" runat="server" CssClass="ErrorText"></asp:Label>
            </asp:WizardStep>
            <asp:WizardStep ID="StepPassword" runat="server" Title="Password">
                <uc1:ContentControl ID="mContentLogin" runat="server" ContentPath="Signup\SignupLoginHeader.html" />
                <table class="Grid" cellpadding="0" cellspacing="0" style="width: 90%">
                    <tr>
                        <td class="dark">
                            Password</td>
                        <td>
                            <asp:TextBox ID="mTxtPassword" runat="server" TextMode="Password"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="dark">
                            Retype Password
                        </td>
                        <td>
                            <asp:TextBox ID="mTxtPassword2" runat="server" TextMode="Password"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <asp:Label ID="mLbLoginErrorMsg" runat="server" CssClass="ErrorText"></asp:Label>
            </asp:WizardStep>
            <asp:WizardStep ID="Pincode" runat="server" Title="Pincode">
                <uc1:ContentControl ID="mContentPincode" runat="server" ContentKey="RealLeadsSignup_PincodeHeader"
                    ContentPath="RealLeads\SignupPincodeHeader.html" />
                <table>
                    <tr>
                        <td align="right" style="height: 24px; width: 200px;">
                            <asp:Label ID="Label3" runat="server" CssClass="ImportantText" Text="Pincode:"></asp:Label>
                        </td>
                        <td align="left" style="height: 24px">
                            <asp:TextBox ID="mTxtPincode" runat="server" CssClass="SmTextField" Width="27px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Label ID="mLbPincodeErrorMsg" runat="server" CssClass="ErrorText"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:WizardStep>
            <asp:WizardStep ID="ContactSettings" runat="server" Title="Contact Settings">
                <uc1:ContentControl ID="mContentWeekdaySchedule" runat="server" ContentKey="RealLeadsSignup_ContactSettingsHeader"
                    ContentPath="RealLeads\SignupContactSettingsHeader.html" />
                <table class="Grid" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="dark">
                            Phone Number:</td>
                        <td colspan="3">
                            <asp:TextBox ID="mTxtContactNumber" runat="server" CssClass="Control"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="dark">
                            Start Time
                        </td>
                        <td>
                            <uc2:TimeControl ID="mContactTimeStart" runat="server" />
                        </td>
                        <td class="dark">
                            Stop Time</td>
                        <td>
                            <uc2:TimeControl ID="mContactTimeEnd" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="dark">
                            Days</td>
                        <td colspan="3">
                            <uc5:DaysControl ID="mContactDays" runat="server" />
                        </td>
                    </tr>
                </table>
            </asp:WizardStep>
            <asp:WizardStep runat="server" Title="Postal Code" ID="PostalCode">
                <uc1:ContentControl ID="mContentListingDetails" runat="server" ContentPath="Signup\SignupListingDetailsHeader.html" />
                <table class="Grid" cellpadding="0" cellspacing="0" style="width: 90%">
                    <tr>
                        <td class="dark">
                            Postal Code</td>
                        <td>
                            <asp:TextBox ID="mTxtPostal" runat="server" Width="50px"></asp:TextBox></td>
                    </tr>
                </table>
                <asp:Label ID="mLbPostalErrorMsg" runat="server" CssClass="ErrorText"></asp:Label>
            </asp:WizardStep>
            <asp:WizardStep ID="StreetAddress" runat="server" Title="Street Address">
                <uc1:ContentControl ID="ContentControl2" runat="server" ContentPath="Signup\SignupListingDetailsHeader.html" />
                <table class="Grid" cellpadding="0" cellspacing="0" style="width: 90%">
                    <tr>
                        <td class="dark">
                            Street Address</td>
                        <td>
                            <asp:TextBox ID="mTxtStreetAddress" runat="server" Width="400px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="dark">
                            City</td>
                        <td>
                            <asp:Label ID="mLbCity" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="dark">
                            Province</td>
                        <td>
                            <asp:Label ID="mLbProvince" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="dark">
                            Country</td>
                        <td>
                            <asp:Label ID="mLbCountry" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="dark" style="height: 19px">
                            Postal</td>
                        <td style="height: 19px">
                            <asp:Label ID="mLbPostal" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:Label ID="mLbStreetAddressErrorMsg" runat="server" CssClass="ErrorText"></asp:Label>
            </asp:WizardStep>
            <asp:WizardStep ID="ListingCode" runat="server" Title="Listing Code">
                <uc1:ContentControl ID="mContentListingCode" runat="server" ContentPath="Signup\SignupListingCodeHeader.html" />
                <table class="Grid" cellpadding="0" cellspacing="0" style="width: 90%">
                    <tr>
                        <td class="dark">
                            Available Numbers</td>
                        <td>
                            <asp:DropDownList ID="mDlListingNumbers" runat="server">
                            </asp:DropDownList></td>
                    </tr>
                </table>
                <asp:Label ID="mLbListingCodeErrorMsg" runat="server" CssClass="ErrorText"></asp:Label>
            </asp:WizardStep>
           <%--<asp:WizardStep ID="Promotions" runat="server" Title="Promotions">
                <uc1:ContentControl ID="ContentControl3" runat="server" ContentPath="Signup\SignupPromotionsHeader.html" />
                <table class="Grid" cellpadding="0" cellspacing="0" style="width: 90%">
                    <tr>
                        <td class="dark">
                            Promotion Code</td>
                        <td>
                            <asp:TextBox ID="mTxtPromotion" runat="server"></asp:TextBox>
                            </td>
                    </tr>
                </table>
                <asp:Label ID="mLbPromotionError" runat="server" CssClass="ErrorText"></asp:Label>
            </asp:WizardStep> --%>
            <asp:WizardStep ID="Finish" runat="server" AllowReturn="False" StepType="Finish"
                Title="Finished">
                <uc1:ContentControl ID="mContentFinished" runat="server" ContentPath="Signup\SignupFinishHeader.html" />
            </asp:WizardStep>
        </WizardSteps>
        <SideBarButtonStyle ForeColor="#6BC020" Font-Size="11pt" Font-Underline="True" />
        <StepStyle BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" />
        <SideBarStyle VerticalAlign="Top" Width="200px" HorizontalAlign="Center" Font-Bold="False" />
    </asp:Wizard>
</asp:Content>
