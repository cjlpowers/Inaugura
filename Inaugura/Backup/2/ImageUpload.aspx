<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImageUpload.aspx.cs" Inherits="ImageUpload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="background-color: White">
    <form id="form1" runat="server">
        <fieldset>
            <legend>Add New Image</legend>
            <asp:Label ID="Label3" runat="server" Text="Image" AssociatedControlID="mFileUpload"></asp:Label>
            <asp:FileUpload ID="mFileUpload" runat="server" Font-Bold="False" Font-Overline="True"
                CssClass="large" />
            <br />
            <asp:Label ID="mLbError" runat="server" Text="" CssClass="Error"></asp:Label>
            <div class="listingEditButtonRow">
                <asp:Button ID="mBtnUpload" runat="server" Text="Add Image" OnClick="mBtnUpload_Click" />
            </div>
        </fieldset>
        <asp:Panel ID="mPanelScript" runat="server" Visible="false">
            <script type="text/javascript">
                  window.parent.FireActionEvent('refreshimages')
            </script>
        </asp:Panel>
    </form>
</body>
</html>
