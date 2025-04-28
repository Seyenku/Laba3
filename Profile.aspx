<%@ Page Title="Личный кабинет" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="Laba3.Profile" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-5">
        <div class="row">
            <div class="col-lg-4">
                <div class="card mb-4">
                    <div class="card-header">Профиль пользователя</div>
                    <div class="card-body text-center">
                        <img src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava3.webp" alt="avatar"
                            class="rounded-circle img-fluid" style="width: 150px;">
                        <h5 class="my-3"><asp:Label ID="lblFullName" runat="server"></asp:Label></h5>
                        <p class="text-muted mb-1"><asp:Label ID="lblRole" runat="server"></asp:Label></p>
                        <p class="text-muted mb-4"><asp:Label ID="lblEmail" runat="server"></asp:Label></p>
                    </div>
                </div>
            </div>
            <div class="col-lg-8">
                <div class="card mb-4">
                    <div class="card-header">Личная информация</div>
                    <div class="card-body">
                        <div class="row mb-3">
                            <div class="col-sm-3">
                                <p class="mb-0">Имя</p>
                            </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row mb-3">
                            <div class="col-sm-3">
                                <p class="mb-0">Фамилия</p>
                            </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row mb-3">
                            <div class="col-sm-3">
                                <p class="mb-0">Email</p>
                            </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row mb-3">
                            <div class="col-sm-3">
                                <p class="mb-0">Телефон</p>
                            </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12 text-end">
                                <asp:Button ID="btnSaveProfile" runat="server" Text="Сохранить изменения" 
                                    CssClass="btn btn-primary" OnClick="btnSaveProfile_Click" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="card mb-4">
                    <div class="card-header">Изменение пароля</div>
                    <div class="card-body">
                        <div class="row mb-3">
                            <div class="col-sm-3">
                                <p class="mb-0">Текущий пароль</p>
                            </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtCurrentPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row mb-3">
                            <div class="col-sm-3">
                                <p class="mb-0">Новый пароль</p>
                            </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row mb-3">
                            <div class="col-sm-3">
                                <p class="mb-0">Подтвердите пароль</p>
                            </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" 
                                    ControlToValidate="txtConfirmPassword" ControlToCompare="txtNewPassword"
                                    ErrorMessage="Пароли не совпадают" CssClass="text-danger" Display="Dynamic"></asp:CompareValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12 text-end">
                                <asp:Button ID="btnChangePassword" runat="server" Text="Изменить пароль" 
                                    CssClass="btn btn-primary" OnClick="btnChangePassword_Click" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="card mb-4">
                    <div class="card-header">Статистика заявок</div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-4 text-center mb-3">
                                <div class="bg-light p-3 rounded">
                                    <h2><asp:Label ID="lblTotalTickets" runat="server">0</asp:Label></h2>
                                    <p class="mb-0">Всего заявок</p>
                                </div>
                            </div>
                            <div class="col-md-4 text-center mb-3">
                                <div class="bg-light p-3 rounded">
                                    <h2><asp:Label ID="lblOpenTickets" runat="server">0</asp:Label></h2>
                                    <p class="mb-0">Открытых заявок</p>
                                </div>
                            </div>
                            <div class="col-md-4 text-center mb-3">
                                <div class="bg-light p-3 rounded">
                                    <h2><asp:Label ID="lblClosedTickets" runat="server">0</asp:Label></h2>
                                    <p class="mb-0">Закрытых заявок</p>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <h5 class="mt-3">Последние заявки</h5>
                                <asp:GridView ID="gvRecentTickets" runat="server" AutoGenerateColumns="False" 
                                    CssClass="table table-striped table-bordered table-hover" EmptyDataText="Нет заявок">
                                    <Columns>
                                        <asp:BoundField DataField="Id" HeaderText="№" />
                                        <asp:BoundField DataField="Name" HeaderText="Тема" />
                                        <asp:BoundField DataField="Status.StatusName" HeaderText="Статус" />
                                        <asp:BoundField DataField="DateCreated" HeaderText="Дата создания" DataFormatString="{0:dd.MM.yyyy}" />
                                        <asp:TemplateField HeaderText="Действия">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="HyperLink1" runat="server" 
                                                    NavigateUrl='<%# "TicketDetails.aspx?id=" + Eval("Id") %>'
                                                    CssClass="btn btn-sm btn-info">Просмотр</asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content> 