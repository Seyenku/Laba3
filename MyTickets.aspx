<%@ Page Title="My Tickets" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyTickets.aspx.cs" Inherits="Laba3.MyTickets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>Мои заявки</h2>
        <hr />
        
        <div class="mb-3">
            <asp:LinkButton ID="NewTicketButton" runat="server" CssClass="btn btn-primary" OnClick="NewTicketButton_Click" Visible="false">
                <i class="fas fa-plus"></i> Создать новую заявку
            </asp:LinkButton>
        </div>
        
        <div class="table-responsive">
            <asp:GridView ID="TicketsGridView" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-striped table-bordered table-hover" 
                EmptyDataText="У вас нет заявок" OnRowCommand="TicketsGridView_RowCommand">
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="Название" />
                    <asp:BoundField DataField="CategoryName" HeaderText="Категория" />
                    <asp:BoundField DataField="StatusName" HeaderText="Статус" />
                    <asp:BoundField DataField="DateCreated" HeaderText="Дата создания" DataFormatString="{0:dd.MM.yyyy HH:mm}" ItemStyle-CssClass="d-none d-md-table-cell" HeaderStyle-CssClass="d-none d-md-table-cell" />
                    <asp:BoundField DataField="PersonalName" HeaderText="Исполнитель" ItemStyle-CssClass="d-none d-md-table-cell" HeaderStyle-CssClass="d-none d-md-table-cell" />
                    <asp:BoundField DataField="DateClosed" HeaderText="Дата закрытия" DataFormatString="{0:dd.MM.yyyy HH:mm}" ItemStyle-CssClass="d-none d-lg-table-cell" HeaderStyle-CssClass="d-none d-lg-table-cell" />
                    <asp:TemplateField HeaderText="Действия">
                        <ItemTemplate>
                            <div class="d-flex flex-wrap gap-1">
                                <asp:LinkButton ID="ViewButton" runat="server" CommandName="ViewTicket" CommandArgument='<%# Eval("Id") %>'
                                    CssClass="btn btn-info btn-sm">
                                    <i class="fas fa-eye"></i> <span class="d-none d-sm-inline">Просмотр</span>
                                </asp:LinkButton>
                                
                                <asp:LinkButton ID="CloseButton" runat="server" CommandName="CloseTicket" CommandArgument='<%# Eval("Id") %>'
                                    CssClass='<%# (Eval("DateClosed") != null && Eval("DateClosed").ToString() != "") ? "btn btn-secondary btn-sm" : "btn btn-success btn-sm" %>' 
                                    Enabled='<%# Eval("DateClosed") == null || Eval("DateClosed").ToString() == "" %>'
                                    Visible='<%# Session["UserRole"] != null && Session["UserRole"].ToString() == "CLIENT" %>'>
                                    <i class='<%# (Eval("DateClosed") != null && Eval("DateClosed").ToString() != "") ? "fas fa-check" : "fas fa-check-circle" %>'></i>
                                    <span class="d-none d-sm-inline"><%# (Eval("DateClosed") != null && Eval("DateClosed").ToString() != "") ? "Выполнено" : "Закрыть" %></span>
                                </asp:LinkButton>
                                
                                <asp:LinkButton ID="EditButton" runat="server" CommandName="EditTicket" CommandArgument='<%# Eval("Id") %>'
                                    CssClass="btn btn-primary btn-sm"
                                    Visible='<%# Session["UserRole"] != null && Session["UserRole"].ToString() == "CLIENT" && (Eval("DateClosed") == null || Eval("DateClosed").ToString() == "") %>'>
                                    <i class="fas fa-edit"></i> <span class="d-none d-sm-inline">Редактировать</span>
                                </asp:LinkButton>
                                
                                <asp:LinkButton ID="DeleteButton" runat="server" CommandName="DeleteTicket" CommandArgument='<%# Eval("Id") %>'
                                    CssClass="btn btn-danger btn-sm"
                                    Visible='<%# Session["UserRole"] != null && Session["UserRole"].ToString() == "CLIENT" %>'
                                    OnClientClick="return confirm('Вы уверены, что хотите удалить эту заявку?');">
                                    <i class="fas fa-trash"></i> <span class="d-none d-sm-inline">Удалить</span>
                                </asp:LinkButton>
                                
                                <asp:LinkButton ID="AssignButton" runat="server" CommandName="AssignTicket" CommandArgument='<%# Eval("Id") %>'
                                    CssClass="btn btn-warning btn-sm" Visible='<%# Session["UserRole"] != null && Session["UserRole"].ToString() == "IT_STAFF" && Eval("PersonalId") == null %>'>
                                    <i class="fas fa-user-plus"></i> <span class="d-none d-sm-inline">Взять в работу</span>
                                </asp:LinkButton>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content> 