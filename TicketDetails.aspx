<%@ Page Title="Ticket Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TicketDetails.aspx.cs" Inherits="Laba3.TicketDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>Детали заявки #<asp:Literal ID="TicketIdLiteral" runat="server"></asp:Literal></h2>
        <hr />
        
        <div class="alert alert-danger" runat="server" id="ErrorMessage" visible="false">
            <asp:Literal runat="server" ID="ErrorMessageText" />
        </div>
        
        <div class="row">
            <div class="col-md-8">
                <div class="card">
                    <div class="card-header">
                        <h3 class="card-title"><asp:Literal ID="TicketNameLiteral" runat="server"></asp:Literal></h3>
                    </div>
                    <div class="card-body">
                        <div class="row mb-3">
                            <div class="col-md-3 font-weight-bold">Категория:</div>
                            <div class="col-md-9"><asp:Literal ID="CategoryLiteral" runat="server"></asp:Literal></div>
                        </div>
                        
                        <div class="row mb-3">
                            <div class="col-md-3 font-weight-bold">Статус:</div>
                            <div class="col-md-9">
                                <asp:Literal ID="StatusLiteral" runat="server"></asp:Literal>
                                <asp:DropDownList ID="StatusDropDown" runat="server" CssClass="form-control" Visible="false"></asp:DropDownList>
                                <asp:Button ID="UpdateStatusButton" runat="server" Text="Обновить статус" CssClass="btn btn-primary mt-2" 
                                    OnClick="UpdateStatusButton_Click" Visible="false" />
                            </div>
                        </div>
                        
                        <div class="row mb-3">
                            <div class="col-md-3 font-weight-bold">Клиент:</div>
                            <div class="col-md-9"><asp:Literal ID="ClientLiteral" runat="server"></asp:Literal></div>
                        </div>
                        
                        <div class="row mb-3">
                            <div class="col-md-3 font-weight-bold">Контакты клиента:</div>
                            <div class="col-md-9">
                                <asp:Literal ID="ClientContactsLiteral" runat="server"></asp:Literal>
                            </div>
                        </div>
                        
                        <div class="row mb-3">
                            <div class="col-md-3 font-weight-bold">Исполнитель:</div>
                            <div class="col-md-9"><asp:Literal ID="PersonalLiteral" runat="server"></asp:Literal></div>
                        </div>
                        
                        <div class="row mb-3">
                            <div class="col-md-3 font-weight-bold">Дата создания:</div>
                            <div class="col-md-9"><asp:Literal ID="DateCreatedLiteral" runat="server"></asp:Literal></div>
                        </div>
                        
                        <div class="row mb-3" runat="server" id="DateClosedDiv" visible="false">
                            <div class="col-md-3 font-weight-bold">Дата закрытия:</div>
                            <div class="col-md-9"><asp:Literal ID="DateClosedLiteral" runat="server"></asp:Literal></div>
                        </div>
                        
                        <div class="row mb-3">
                            <div class="col-md-3 font-weight-bold">Описание:</div>
                            <div class="col-md-9">
                                <div class="card">
                                    <div class="card-body">
                                        <asp:Literal ID="DescriptionLiteral" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        <div class="row mb-3">
                            <div class="col-md-12 text-center">
                                <asp:Button ID="BackButton" runat="server" Text="Назад к списку заявок" CssClass="btn btn-secondary" 
                                    OnClick="BackButton_Click" />
                                
                                <asp:Button ID="CloseTicketButton" runat="server" Text="Закрыть заявку" CssClass="btn btn-success" 
                                    OnClick="CloseTicketButton_Click" Visible="false" />
                                
                                <asp:Button ID="AssignTicketButton" runat="server" Text="Взять в работу" CssClass="btn btn-warning" 
                                    OnClick="AssignTicketButton_Click" Visible="false" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content> 