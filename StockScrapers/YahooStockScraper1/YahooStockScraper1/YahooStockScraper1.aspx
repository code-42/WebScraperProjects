<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="YahooStockScraper1.aspx.cs" Inherits="YahooStockScraper1.YahooStockScraper1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <p>Yahoo Stock Scraper gets data from https://finance.yahoo.com/quote/<span>SYMBOL</span>/history?p=<span>SYMBOL</span>

    </p>
    <form id="form1" runat="server">
        
        <div>
            <asp:Label ID="InputHereLabel" runat="server" Text="Input Stock Symbol Here" /> 
            <asp:TextBox ID="InputTextBox" value="hd" runat="server" />   
            <asp:Button ID="ClickMeButton" runat="server" Text="Submit" OnClick="ClickMeButton_Click" /> <br /> 
            <br />
            <asp:Label ID="DataBaseLabel" runat="server" />
            <asp:Label ID="DataBuider" runat="server" />
            <asp:Label ID="RowCount" runat="server" />
            <asp:Label ID="Tagslabel" runat="server" /> 
            <asp:Label ID="OutputLabel" runat="server" /> 
        </div>
    </form>
    <script src="https://code.jquery.com/jquery-3.2.1.min.js" integrity="sha256-hwg4gsxgFZhOsEEamdOYGBf13FyQuiTwlAQgxVSNgt4=" crossorigin="anonymous"></script>
    <script type="text/javascript">
        $(function () {
            $('#ClickMeButton').click();
        });
    </script>
</body>
</html>
