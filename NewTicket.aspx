<%@ Page Title="New Ticket" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewTicket.aspx.cs" Inherits="Laba3.NewTicket" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>Создание новой заявки</h2>
        <hr />
        
        <div class="alert alert-danger" runat="server" id="ErrorMessage" visible="false">
            <asp:Literal runat="server" ID="ErrorMessageText" />
        </div>
        
        <div class="row mb-3">
            <div class="col-md-3">
                <asp:Label runat="server" AssociatedControlID="TicketName" CssClass="form-label">Название заявки</asp:Label>
            </div>
            <div class="col-md-9">
                <asp:TextBox runat="server" ID="TicketName" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="TicketName"
                    CssClass="text-danger" ErrorMessage="Поле названия заявки обязательно для заполнения." />
            </div>
        </div>
        
        <div class="row mb-3">
            <div class="col-md-3">
                <asp:Label runat="server" AssociatedControlID="TicketDescription" CssClass="form-label">Описание проблемы</asp:Label>
            </div>
            <div class="col-md-9">
                <asp:TextBox runat="server" ID="TicketDescription" CssClass="form-control" TextMode="MultiLine" Rows="5" />
            </div>
        </div>
        
        <div class="row mb-3">
            <div class="col-md-3">
                <asp:Label runat="server" AssociatedControlID="TicketCategory" CssClass="form-label">Категория</asp:Label>
            </div>
            <div class="col-md-9">
                <asp:DropDownList runat="server" ID="TicketCategory" CssClass="form-control"></asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="TicketCategory"
                    CssClass="text-danger" ErrorMessage="Выберите категорию заявки." />
            </div>
        </div>
        
        <div class="row">
            <div class="col-md-12 text-center">
                <asp:Button runat="server" OnClick="SubmitTicket_Click" Text="Отправить заявку" CssClass="btn btn-primary" />
            </div>
        </div>
    </div>
</asp:Content> 