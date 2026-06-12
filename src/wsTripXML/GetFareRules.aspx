<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GetFareRules.aspx.vb" Inherits="wsTripXML.GetFareRules" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>View Rules</title>
    <script type="text/javascript">
    
      function pageLoad() {
      }
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdatePanel ID="upPNR" runat="server">
            <ContentTemplate>
            <table>
            <tr>
            <td>
                PNR # :
            </td>
            <td></td>
            <td>
                <asp:TextBox ID="txtPNR" runat="server"></asp:TextBox>
            </td>
            <td></td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="Get Rules" />
            </td>
            </tr>
            </table>
            <br />
            <br />
                <asp:Repeater ID="RPTHeader" runat="server" OnItemDataBound="RPTHeader_ItemDataBound">
                <ItemTemplate>
                    <asp:Label ID="lblTitle" runat="server" Font-Bold="True" Font-Size="Medium" Font-Underline="True"></asp:Label>
                    <br />
                    <asp:Table ID="tb" runat="server"></asp:Table>
                    <br />
                    <br />
                </ItemTemplate>
                </asp:Repeater>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upPNR">
            <ProgressTemplate>
                Loading....
            </ProgressTemplate>
        </asp:UpdateProgress>
        </div>
    </form>
</body>
</html>
