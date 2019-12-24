<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApplication8.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
    <style>
        @import url(https://fonts.googleapis.com/css?family=Roboto:300);
        .login-page {
          width: 360px;
          padding: 8% 0 0;
          margin: auto;
        }
        .form {
          position: relative;
          z-index: 1;
          background: #FFFFFF;
          max-width: 360px;
          margin: 0 auto 100px;
          padding: 45px;
          text-align: center;
          box-shadow: 0 0 20px 0 rgba(0, 0, 0, 0.2), 0 5px 5px 0 rgba(0, 0, 0, 0.24);
        }
        .input {
          border-style: none;
            border-color: inherit;
            border-width: 0;
            font-family: "Roboto", sans-serif;
            outline: 0;
            background: #f2f2f2;
            width: 100%;
            padding: 15px;
            box-sizing: border-box;
            font-size: 14px;
            margin-bottom: 5px;
        }
        .btn{
          font-family: "Roboto", sans-serif;
          text-transform: uppercase;
          outline: 0;
          background: #808681;
          width: 100%;
          border: 0;
          padding: 15px;
          color: #FFFFFF;
          font-size: 14px;
          -webkit-transition: all 0.3 ease;
          transition: all 0.3 ease;
          cursor: pointer;
        }
        .form button:hover,.form button:active,.form button:focus {
          background: #43A047;
        }
        body {
          background: #0078d4; /* fallback for old browsers */
          background: -webkit-linear-gradient(right, #0078d4, #b3d6f2);
          background: -moz-linear-gradient(right, #0078d4, #b3d6f2);
          background: -o-linear-gradient(right, #0078d4, #b3d6f2);
          background: linear-gradient(to left, #0078d4, #b3d6f2);
          font-family: "Roboto", sans-serif;
          -webkit-font-smoothing: antialiased;
          -moz-osx-font-smoothing: grayscale;      
        }
    </style>
</head>
<body>
    <script>
        function Message(title, type) {
            swal({
                title: title,
                text: '',
                icon: type,
                button: "Ok",
            })
        }
    </script>
    <form id="form1" runat="server">
        <div class="login-page">
          <div class="form">
              <asp:TextBox placeholder="Username" CssClass="input" ID="txtUser" runat="server"></asp:TextBox>
              <asp:TextBox placeholder="Password" CssClass="input" ID="txtPass" runat="server" TextMode="Password"></asp:TextBox>
              <asp:Button CssClass="btn" ID="Button1" runat="server" OnClick="Button1_Click" Text="Log In" />
          </div>
        </div>
    </form>
    
</body>
</html>
