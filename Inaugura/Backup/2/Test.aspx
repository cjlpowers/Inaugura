<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Test.aspx.cs" Inherits="Test" %>

<%@ Register Assembly="Inaugura" Namespace="Inaugura.Web.Controls" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <cc1:ListRadioButton ID="mRadio" runat="server" GroupName="Test">
            </cc1:ListRadioButton>
           <cc1:ListRadioButton ID="ListRadioButton1" runat="server" GroupName="Test">
            </cc1:ListRadioButton>
           <cc1:ListRadioButton ID="ListRadioButton2" runat="server" GroupName="Test">
            </cc1:ListRadioButton>
           <cc1:ListRadioButton ID="ListRadioButton3" runat="server" GroupName="Test">
            </cc1:ListRadioButton>
           <cc1:ListRadioButton ID="ListRadioButton4" runat="server" GroupName="Test">
            </cc1:ListRadioButton>    
            <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click1" />
        </div>
    </form>
</body>
</html>
