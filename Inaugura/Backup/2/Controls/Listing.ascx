<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Listing.ascx.cs" Inherits="Controls_Listing" %>
<%@ Register Assembly="Inaugura" Namespace="Inaugura.Web.Controls" TagPrefix="cc2" %>
<%@ Register Src="Edit/EditAddress.ascx" TagName="EditAddress" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Components/ListingViewPanel.ascx" TagName="ListingViewPanel"
    TagPrefix="uc1" %>

<script type="text/javascript">
listingMap = null;
searchPin = null;
function ActiveTabChanged(sender, e) 
{ 
    if(sender.get_activeTab().get_headerText() =="Map")
        if(listingMap==null)
            ShowListingMap($get("_listingID").value);
}

function changeTab( tabIndex )
{ 
    var tabBehavior = $get('<%=Tabs.ClientID%>').control; 
    tabBehavior.set_activeTabIndex(tabIndex); 
}
function FireEvent(args){__doPostBack('<%= this.ClientID.Replace('_','$') %>',args);}
function FireActionEvent(action, args) {args!=null?FireEvent('action,'+action+";"+args):FireEvent('action,'+action);}
</script>

<div class="listing">
    <uc1:ListingViewPanel ID="mListingViewHeader" runat="server" Mode="Header" />
    <cc1:TabContainer ID="Tabs" runat="server" CssClass="CustomTabStyle" ActiveTabIndex="0"
        OnClientActiveTabChanged="ActiveTabChanged">
        <cc1:TabPanel runat="server" ID="mTabInfo" HeaderText="Info">
            <ContentTemplate>
                <asp:UpdatePanel ID="mUpdateInfo" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:PlaceHolder ID="mPlaceHolderEdit" runat="server"></asp:PlaceHolder>
                        <uc1:ListingViewPanel ID="mListingViewInfo" runat="server" Mode="Details" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </cc1:TabPanel>
        <cc1:TabPanel runat="server" ID="mTabPhotos" HeaderText="Photos">
            <ContentTemplate>
                <asp:UpdatePanel ID="mUpdatePhoto" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="mPanelImageAdmin" runat="server" Visible="false">
                            <asp:Repeater ID="mRepeaterImages" runat="server">
                                <ItemTemplate>
                                    <div class="photos">
                                        <table>
                                            <tr>
                                                <td style="width: 190px; vertical-align: middle;">
                                                    <asp:HiddenField ID="mHiddenImageID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"ID")%>' />
                                                    <img src='<%# this.GetImageUrl(Container.DataItem as Inaugura.RealLeads.Image, Inaugura.Web.ImageHttpHandler.ImageMode.Size160) %>'/>
                                                </td>
                                                <td style="text-align: left;">
                                                    <label>
                                                        Caption</label>
                                                    <br />
                                                    <asp:TextBox ID="mTxtDescription" runat="server" TextMode="MultiLine" Text='<%# DataBinder.Eval(Container.DataItem, "Caption") %>'
                                                        Width="100%"></asp:TextBox>
                                                    <asp:CheckBox ID="mChkDelete" runat="server" Text="Remove" />
                                                   <input type="Radio" name="mRbDefaultImage" value='<%# DataBinder.Eval(Container.DataItem, "ID") %>' <%#MainImageCheck((Inaugura.RealLeads.Image)Container.DataItem) %> />
                                                    <label>
                                                        Main Image</label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:Panel ID="mPanelImageUpload" runat="server">
                                <iframe id="mFrameImageUpload" runat="server" class="PhotoUpload" frameborder="0"></iframe>
                            </asp:Panel>
                            <div class="listingEditButtonRow">
                                <asp:Button ID="mBtnPhotosDone" runat="server" Text="Update" OnClick="mBtnPhotosDone_Click" />
                            </div>
                        </asp:Panel>
                        <uc1:ListingViewPanel ID="mListingViewPhotos" runat="server" Mode="Photos" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </cc1:TabPanel>
        <cc1:TabPanel runat="server" ID="mTabMap" HeaderText="Map">
            <ContentTemplate>
                <uc1:ListingViewPanel ID="mListingViewMap" runat="server" Mode="Map" />
            </ContentTemplate>
        </cc1:TabPanel>
        <cc1:TabPanel runat="server" ID="mTabContact" HeaderText="Contact">
            <ContentTemplate>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <fieldset id="mFieldsetPhone" runat="server">
                            <legend>Contact By Phone</legend>
                            <asp:Label ID="Label2" runat="server" Text="Phone Number" AssociatedControlID="mLbPhoneNumber"></asp:Label>
                            <asp:Label ID="mLbPhoneNumber" runat="server" Text=""></asp:Label>
                        </fieldset>
                        <fieldset>
                            <legend>Contact By Email</legend>
                            <asp:Label ID="Label1" runat="server" Text="Your Email Address" AssociatedControlID="mTxtEmailAddress"></asp:Label>
                            <asp:TextBox ID="mTxtEmailAddress" runat="server" CssClass="large"></asp:TextBox>
                            <br />
                            <asp:Label ID="Label3" runat="server" Text="Message" AssociatedControlID="mTxtMessage"></asp:Label>
                            <asp:TextBox ID="mTxtMessage" runat="server" TextMode="MultiLine" CssClass="large"
                                Height="100px"></asp:TextBox>
                            <br />
                            <asp:Button ID="mBtnSend" runat="server" Text="Send" CssClass="btn" OnClick="mBtnSend_Click" />
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </cc1:TabPanel>
    </cc1:TabContainer>
</div>
<asp:TextBox ID="TextBox1" runat="server" Visible="false"></asp:TextBox>
<cc1:CalendarExtender ID="mCalendarExtender1" TargetControlID="TextBox1" runat="server">
</cc1:CalendarExtender>
