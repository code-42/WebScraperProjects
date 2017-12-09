<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockScraper1.aspx.cs" Inherits="StockScraper1.StockScraper1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Stock Scraper</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="InputHereLabel" runat="server" Text="Input Here" /> 
            <asp:TextBox ID="InputTextBox" runat="server" />   
            <asp:Button ID="ClickMeButton" runat="server" Text="Click Me" OnClick="ClickMeButton_Click" /> <br /> 
            <br /> <br /> 
            <asp:Label ID="OutputLabel" runat="server" /> 
        </div>
    </form>
</body>
</html>
