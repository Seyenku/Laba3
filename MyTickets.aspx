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
        
        <asp:GridView ID="TicketsGridView" runat="server" AutoGenerateColumns="False" 
            CssClass="table table-striped table-bordered table-hover" 
            EmptyDataText="У вас нет заявок" OnRowCommand="TicketsGridView_RowCommand">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="№" />
                <asp:BoundField DataField="Name" HeaderText="Название" />
                <asp:BoundField DataField="CategoryName" HeaderText="Категория" />
                <asp:BoundField DataField="StatusName" HeaderText="Статус" />
                <asp:BoundField DataField="DateCreated" HeaderText="Дата создания" DataFormatString="{0:dd.MM.yyyy HH:mm}" />
                <asp:BoundField DataField="PersonalName" HeaderText="Исполнитель" />
                <asp:BoundField DataField="DateClosed" HeaderText="Дата закрытия" DataFormatString="{0:dd.MM.yyyy HH:mm}" />
                <asp:TemplateField HeaderText="Действия">
                    <ItemTemplate>
                        <asp:LinkButton ID="ViewButton" runat="server" CommandName="ViewTicket" CommandArgument='<%# Eval("Id") %>'
                            CssClass="btn btn-info btn-sm">
                            <i class="fas fa-eye"></i> Просмотр
                        </asp:LinkButton>
                        
                        <asp:LinkButton ID="CloseButton" runat="server" CommandName="CloseTicket" CommandArgument='<%# Eval("Id") %>'
                            CssClass='<%# (Eval("DateClosed") != null && Eval("DateClosed").ToString() != "") ? "btn btn-secondary btn-sm" : "btn btn-success btn-sm" %>' 
                            Enabled='<%# Eval("DateClosed") == null || Eval("DateClosed").ToString() == "" %>'
                            Visible='<%# Session["UserRole"] != null && Session["UserRole"].ToString() == "CLIENT" %>'>
                            <i class='<%# (Eval("DateClosed") != null && Eval("DateClosed").ToString() != "") ? "fas fa-check" : "fas fa-check-circle" %>'></i>
                            <%# (Eval("DateClosed") != null && Eval("DateClosed").ToString() != "") ? "Выполнено" : "Закрыть" %>
                        </asp:LinkButton>
                        
                        <asp:LinkButton ID="EditButton" runat="server" CommandName="EditTicket" CommandArgument='<%# Eval("Id") %>'
                            CssClass="btn btn-primary btn-sm"
                            Visible='<%# Session["UserRole"] != null && Session["UserRole"].ToString() == "CLIENT" && (Eval("DateClosed") == null || Eval("DateClosed").ToString() == "") %>'>
                            <i class="fas fa-edit"></i> Редактировать
                        </asp:LinkButton>
                        
                        <asp:LinkButton ID="DeleteButton" runat="server" CommandName="DeleteTicket" CommandArgument='<%# Eval("Id") %>'
                            CssClass="btn btn-danger btn-sm"
                            Visible='<%# Session["UserRole"] != null && Session["UserRole"].ToString() == "CLIENT" %>'
                            OnClientClick="return confirm('Вы уверены, что хотите удалить эту заявку?');">
                            <i class="fas fa-trash"></i> Удалить
                        </asp:LinkButton>
                        
                        <asp:LinkButton ID="AssignButton" runat="server" CommandName="AssignTicket" CommandArgument='<%# Eval("Id") %>'
                            CssClass="btn btn-warning btn-sm" Visible='<%# Session["UserRole"] != null && Session["UserRole"].ToString() == "IT_STAFF" && Eval("PersonalId") == null %>'>
                            <i class="fas fa-user-plus"></i> Взять в работу
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content> 