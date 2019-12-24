<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Work.aspx.cs" Inherits="WebApplication8.Work" %>

<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>POC Azure Logic Apps BPM</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0-beta.2/css/bootstrap.min.css"
          integrity="sha384-PsH8R72JQ3SOdhVi3uxftmaW6Vc51MKb0q5P2rRUpPvrszuE4W1povHYgTpBfshb" crossorigin="anonymous">
    <script src="https://unpkg.com/react@16.0.0/umd/react.production.min.js"></script>
    <script src="https://unpkg.com/react-dom@16.0.0/umd/react-dom.production.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/classnames/2.2.6/index.js"></script>
    <script src="https://unpkg.com/babel-standalone@6.15.0/babel.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/cytoscape/3.9.4/cytoscape.min.js"></script>
    <script src="cytoscape-node-html-label.min.js"></script>
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
    <script src="https://code.jquery.com/jquery-3.2.1.slim.min.js"
            integrity="sha384-KJ3o2DKtIkvYIK3UENzmM7KCkRr/rE9/Qpg6aAZGJwFDMVNA/GpGFF93hXpG5KkN"
            crossorigin="anonymous"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.3/umd/popper.min.js"
            integrity="sha384-vFJXuSJphROIrBnz7yo7oB41mKfc8JzQZiCq4NCceLEaO4IHwicKwpJf9c9IpFgh"
            crossorigin="anonymous"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0-beta.2/js/bootstrap.min.js"
            integrity="sha384-alpBpkh1PFOepccYVYDB4do5UnbKysX5WZXm3XxPqe5iKTfUKjNkCk9SaVuEZflJ"
            crossorigin="anonymous"></script>
    <style>
        #cy {
            font-size: 5px;
            width: 100%;
            height: 100%;
            display: block;
        }

        #cyId {
            font-size: 5px;
            width: 100%;
            height: 100%;
            display: block;
        }

        .startButton {
            width: 100%;
        }

        .input {
            border-style: none;
            border-color: inherit;
            border-width: 0;
            font-family: "Roboto", sans-serif;
            outline: 0;
            background: #f2f2f2;
            width: 100%;
            box-sizing: border-box;
        }

        .btn {
            margin-bottom: 10px;
        }

        @media (min-width: 576px) {
            .modal-dialog {
                max-width: 90% !important;
                height: 90% !important;
                margin: 30px auto;
            }
        }
        
        .modal-body{
            padding:0px;
        }

        .modal-content{
            height:100%;
        }

        .loader-container {
            height: 500px;
            display: flex;
            overflow: hidden;
        }

        .loader {
            margin: auto;
            border: 5px dotted #dadada;
            border-top: 5px solid #3498db;
            border-radius: 50%;
            width: 100px;
            height: 100px;
            -webkit-animation: spin 2s linear infinite;
            animation: spin 2s linear infinite;
        }

        @-webkit-keyframes spin {
            0% {
                -webkit-transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
            }
        }

        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }
    </style>
</head>

<body>
    <script>
        var token = "<%=acc_token%>";
    </script>
    <h1>POC Azure Logic Apps BPM1</h1>

    <div id="root"></div>
    <script type="text/babel" src="/Scripts/WorkFlowList.js"></script>
</body>

</html>
