﻿<%@ Page Title="Авторизация" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Laba3.Login" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .custom-checkbox-wrapper {
            margin-bottom: 15px;
            position: relative;
        }
        
        .custom-checkbox-wrapper input[type="checkbox"] {
            position: absolute;
            opacity: 0;
            width: 0;
            height: 0;
        }
        
        .custom-checkbox-ui {
            display: inline-block;
            width: 20px;
            height: 20px;
            background: #fff;
            border: 2px solid #ccc;
            border-radius: 3px;
            position: relative;
            margin-right: 10px;
            vertical-align: middle;
            cursor: pointer;
            transition: all 0.2s ease;
        }
        
        .custom-checkbox-ui:hover {
            border-color: #999;
        }
        
        .custom-checkbox-label {
            cursor: pointer;
            user-select: none;
            display: inline-flex;
            align-items: center;
        }
        
        .custom-checkbox-wrapper input[type="checkbox"]:checked + .custom-checkbox-label .custom-checkbox-ui {
            background-color: #2196F3;
            border-color: #2196F3;
        }
        
        .custom-checkbox-wrapper input[type="checkbox"]:checked + .custom-checkbox-label .custom-checkbox-ui:after {
            content: '';
            position: absolute;
            top: 4px;
            left: 7px;
            width: 4px;
            height: 8px;
            border: solid white;
            border-width: 0 2px 2px 0;
            transform: rotate(45deg);
        }
        
        .login-title {
            margin-bottom: 0.5rem;
        }
        
        .login-subtitle {
            margin-bottom: 1.5rem;
        }
        
        hr {
            margin: 1.5rem 0;
        }
        
        @media (max-width: 768px) {
            .login-container {
                padding: 15px;
            }
        }
    </style>
    
    <main aria-labelledby="title">
        <div class="container login-container py-4">
            <h2 id="title" class="login-title"><%: Title %></h2>
            <asp:Panel ID="errorMessage" runat="server" CssClass="text-danger" Visible="false">
                <asp:Literal ID="ErrorText" runat="server"></asp:Literal>
            </asp:Panel>
            <h4 class="login-subtitle">Вход в систему</h4>
            <hr />
            
            <div class="row mb-3">
                <asp:Label runat="server" AssociatedControlID="Email" CssClass="col-12 col-md-3 col-form-label">Email</asp:Label>
                <div class="col-12 col-md-9">
                    <asp:TextBox ID="Email" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email" 
                        CssClass="text-danger" ErrorMessage="Поле адреса электронной почты заполнять обязательно." Display="Dynamic" />
                </div>
            </div>
            
            <div class="row mb-3">
                <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-12 col-md-3 col-form-label">Пароль</asp:Label>
                <div class="col-12 col-md-9">
                    <asp:TextBox ID="Password" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" 
                        CssClass="text-danger" ErrorMessage="Поле пароля заполнять обязательно." Display="Dynamic" />
                </div>
            </div>
            
            <div class="row mb-3">
                <div class="col-12 col-md-9 offset-md-3">
                    <div class="custom-checkbox-wrapper">
                        <asp:CheckBox ID="RememberMe" runat="server" ClientIDMode="Static" />
                        <label for="RememberMe" class="custom-checkbox-label">
                            <span class="custom-checkbox-ui"></span>
                            Запомнить меня
                        </label>
                    </div>
                </div>
            </div>
            
            <div class="row">
                <div class="col-12 col-md-9 offset-md-3">
                    <asp:Button ID="LoginButton" runat="server" Text="Войти" OnClick="LogIn_Click" 
                        CssClass="btn btn-primary w-100 w-md-auto" />
                </div>
            </div>
        </div>
    </main>
    
    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () {
            var aspCheckbox = document.getElementById('RememberMe');
            if (aspCheckbox) {
                aspCheckbox.style.position = 'absolute';
                aspCheckbox.style.opacity = '0';
                aspCheckbox.style.width = '0';
                aspCheckbox.style.height = '0';
            }

            var checkboxLabel = document.querySelector('.custom-checkbox-label');
            if (checkboxLabel) {
                checkboxLabel.addEventListener('click', function (e) {
                    e.preventDefault();

                    if (aspCheckbox) {
                        aspCheckbox.checked = !aspCheckbox.checked;
                    }
                });
            }
        });
    </script>
</asp:Content> 