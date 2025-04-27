<%@ Page Title="Регистрация" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Laba3.Account.Register" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <main aria-labelledby="title">
        <h2 id="title"><%: Title %>.</h2>
        <p class="text-danger">
            <asp:Literal runat="server" ID="ErrorMessage" />
        </p>
        <h4>Создание новой учетной записи</h4>
        <hr />
        <asp:ValidationSummary runat="server" CssClass="text-danger" />
        
        <div class="row mb-3">
            <asp:Label runat="server" AssociatedControlID="UserType" CssClass="col-md-2 col-form-label">Тип пользователя</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList runat="server" ID="UserType" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="UserType_SelectedIndexChanged">
                    <asp:ListItem Text="Клиент" Value="CLIENT" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Сотрудник IT-отдела" Value="IT_STAFF"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        
        <asp:Panel ID="ClientPanel" runat="server">
            <div class="row mb-3">
                <asp:Label runat="server" AssociatedControlID="ClientName" CssClass="col-md-2 col-form-label">ФИО или название организации</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="ClientName" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ClientName"
                        CssClass="text-danger" ErrorMessage="Поле ФИО или название организации обязательно для заполнения." />
                </div>
            </div>
            
            <div class="row mb-3">
                <asp:Label runat="server" AssociatedControlID="Address" CssClass="col-md-2 col-form-label">Адрес</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="Address" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Address"
                        CssClass="text-danger" ErrorMessage="Поле адреса обязательно для заполнения." />
                </div>
            </div>
            
            <div class="row mb-3">
                <asp:Label runat="server" AssociatedControlID="Telephone" CssClass="col-md-2 col-form-label">Телефон</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="Telephone" CssClass="form-control" />
                </div>
            </div>
        </asp:Panel>
        
        <asp:Panel ID="StaffPanel" runat="server" Visible="false">
            <div class="row mb-3">
                <asp:Label runat="server" AssociatedControlID="StaffName" CssClass="col-md-2 col-form-label">ФИО</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="StaffName" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="StaffName"
                        CssClass="text-danger" ErrorMessage="Поле ФИО обязательно для заполнения." />
                </div>
            </div>
            
            <div class="row mb-3">
                <asp:Label runat="server" AssociatedControlID="Position" CssClass="col-md-2 col-form-label">Должность</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="Position" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Position"
                        CssClass="text-danger" ErrorMessage="Поле должности обязательно для заполнения." />
                </div>
            </div>
        </asp:Panel>
        
        <div class="row mb-3">
            <asp:Label runat="server" AssociatedControlID="Email" CssClass="col-md-2 col-form-label">Адрес электронной почты</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                    CssClass="text-danger" ErrorMessage="Поле адреса электронной почты заполнять обязательно." />
            </div>
        </div>
        
        <div class="row mb-3">
            <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-2 col-form-label">Пароль</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                    CssClass="text-danger" ErrorMessage="Поле пароля заполнять обязательно." />
            </div>
        </div>
        
        <div class="row mb-3">
            <asp:Label runat="server" AssociatedControlID="ConfirmPassword" CssClass="col-md-2 col-form-label">Подтверждение пароля</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPassword"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Поле подтверждения пароля заполнять обязательно." />
                <asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Пароль и его подтверждение не совпадают." />
            </div>
        </div>
        
        <div class="row">
            <div class="offset-md-2 col-md-10">
                <asp:Button runat="server" OnClick="CreateUser_Click" Text="Регистрация" CssClass="btn btn-outline-dark" />
            </div>
        </div>
    </main>
</asp:Content>
