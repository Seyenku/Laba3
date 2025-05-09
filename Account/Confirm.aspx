﻿<%@ Page Title="Подтверждение учетной записи" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Confirm.aspx.cs" Inherits="Laba3.Account.Confirm" Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <main aria-labelledby="title">
    <h2 id="title"><%: Title %>.</h2>
    <div>
        <asp:PlaceHolder runat="server" ID="successPanel" ViewStateMode="Disabled" Visible="true">
            <p>
                Благодарим за подтверждение вашей учетной записи. Щелкните <asp:HyperLink ID="login" runat="server" NavigateUrl="~/Account/Login">здесь</asp:HyperLink>  для входа             
            </p>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="errorPanel" ViewStateMode="Disabled" Visible="false">
            <p class="text-danger">
                Произошла ошибка.
            </p>
        </asp:PlaceHolder>
    </div>
    </main>
</asp:Content>
