<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Domain.SSO.Server.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>login - Domain site2</title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>登录 - Domain site2</h1>
        <asp:Label runat="server" ID="lblUserName" Text="用户名:"></asp:Label>
        <asp:TextBox runat="server" ID="txtUserName" Text="admin"></asp:TextBox>
        <asp:Label runat="server" ID="lblPassword" Text="密码:"></asp:Label>
        <asp:TextBox runat="server" ID="txtPassword" TextMode="Password"></asp:TextBox>
        <asp:Button runat="server" ID="btnLogin" OnClick="btnLogin_Click" Text="登录" />
        <p>
            <asp:Label ID="lblMessage" Text="" runat="server" />
        </p>
    </form>
</body>
</html>
