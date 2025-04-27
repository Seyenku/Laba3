<%@ Page Title="Редактирование заявки" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditTicket.aspx.cs" Inherits="Laba3.EditTicket" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>Редактирование заявки</h2>
        <hr />
        
        <asp:Panel ID="ErrorMessage" runat="server" Visible="false" CssClass="alert alert-danger">
            <asp:Literal ID="ErrorMessageText" runat="server"></asp:Literal>
        </asp:Panel>
        
        <div class="card mb-4">
            <div class="card-header">
                <h5>Информация о заявке</h5>
            </div>
            <div class="card-body">
                <div class="form-group row mb-3">
                    <label for="<%= TicketName.ClientID %>" class="col-sm-3 col-form-label">Название:</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="TicketName" runat="server" CssClass="form-control" MaxLength="255" required></asp:TextBox>
                        <asp:RequiredFieldValidator ID="NameRequired" runat="server" ControlToValidate="TicketName"
                            CssClass="text-danger" ErrorMessage="Необходимо указать название заявки." />
                    </div>
                </div>
                
                <div class="form-group row mb-3">
                    <label for="<%= TicketCategory.ClientID %>" class="col-sm-3 col-form-label">Категория:</label>
                    <div class="col-sm-9">
                        <asp:DropDownList ID="TicketCategory" runat="server" CssClass="form-control"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="CategoryRequired" runat="server" ControlToValidate="TicketCategory"
                            CssClass="text-danger" ErrorMessage="Необходимо выбрать категорию." />
                    </div>
                </div>
                
                <div class="form-group row mb-3">
                    <label for="<%= TicketDescription.ClientID %>" class="col-sm-3 col-form-label">Описание:</label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="TicketDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="form-group row">
            <div class="col-sm-12">
                <asp:Button ID="UpdateTicket" runat="server" Text="Сохранить изменения" CssClass="btn btn-primary" OnClick="UpdateTicket_Click" />
                <asp:Button ID="CancelButton" runat="server" Text="Отмена" CssClass="btn btn-secondary ml-2" OnClick="CancelButton_Click" CausesValidation="false" />
            </div>
        </div>
    </div>
</asp:Content> 