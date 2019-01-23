<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="ListingManagement.aspx.cs"
    Inherits="Secure_Admin_ListingManagement" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ MasterType VirtualPath="~/Default.master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="mPlaceHolderBody" runat="Server">
    <asp:TextBox ID="mTxtStartDate" runat="server"></asp:TextBox>
    <asp:Button ID="mBtnClean" runat="server" OnClick="mBtnClean_Click" Text="Clean" />
    <cc1:CalendarExtender ID="mCalendarExtender" runat="server" TargetControlID="mTxtStartDate"
        Format="MMMM dd, yyyy" />
    <cc1:TabContainer runat="server" ID="Tabs" Height="150px">
        <cc1:TabPanel runat="Server" ID="Panel2" HeaderText="First Page">
            <ContentTemplate>
                <div>
                    Controls authored by Toolkit User (read-only - demo purposes):</div>
                <ul>
                    <li>Calendar</li>
                    <li>MaskedEdit</li>
                    <li>Accordion</li>
                    <li>Calendar</li>
                    <li>Calendar</li>
                </ul>
                <br />
            </ContentTemplate>
        </cc1:TabPanel>
        <cc1:TabPanel runat="Server" ID="TabPanel1" HeaderText="Listing Xml">
            <ContentTemplate>
                <asp:TextBox ID="mTxtListingXmlID" runat="server" CssClass="large"></asp:TextBox>
                <asp:Button ID="mBtnGetListingXml" runat="server" Text="Button" OnClick="mBtnGetListingXml_Click" />
                <br />
                <asp:TextBox ID="mTxtListingXml" runat="server" CssClass="huge" TextMode="MultiLine"></asp:TextBox>
            </ContentTemplate>
        </cc1:TabPanel>
    </cc1:TabContainer>
</asp:Content>
