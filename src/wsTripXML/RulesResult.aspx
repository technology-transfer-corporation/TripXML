<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RulesResult.aspx.vb" Inherits="wsTripXML.RulesResult" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Rules Results</title>
    <style type="text/css">
        .style1
        {
            font-size: large;
            text-decoration: underline;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <span class="style1">Rule Details
    </span>
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
      
                
    </div>
    </form>
</body>
</html>
