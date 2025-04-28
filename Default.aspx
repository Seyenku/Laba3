<%@ Page Title="Главная" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Laba3._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <section class="row mt-4" aria-labelledby="helpDeskTitle">
            <div class="col-md-12 text-center">
                <h1 id="helpDeskTitle">Система технической поддержки</h1>
                <p class="lead">Добро пожаловать в Help Desk - сервис для регистрации, учета и исполнения заявок технической поддержки.</p>
                
            </div>
        </section>
        <div class="row mt-5">
            <section class="col-md-4" aria-labelledby="clientsTitle">
                <div class="card h-100">
                    <div class="card-body">
                        <h2 id="clientsTitle" class="card-title"><i class="fas fa-user me-2"></i>Для клиентов</h2>
                        <p class="card-text">
                            Используйте систему Help Desk для создания заявок в службу технической поддержки, отслеживания статуса выполнения и получения уведомлений о ходе работы.
                        </p>
                        <ul>
                            <li>Простая и удобная форма подачи заявки</li>
                            <li>Отслеживание статуса заявки в реальном времени</li>
                            <li>Уведомления по электронной почте</li>
                        </ul>
                    </div>
                </div>
            </section>
            <section class="col-md-4" aria-labelledby="staffTitle">
                <div class="card h-100">
                    <div class="card-body">
                        <h2 id="staffTitle" class="card-title"><i class="fas fa-headset me-2"></i>Для IT-специалистов</h2>
                        <p class="card-text">
                            Управляйте заявками клиентов, отслеживайте их выполнение и обновляйте статус работы в удобном интерфейсе.
                        </p>
                        <ul>
                            <li>Единая база заявок от клиентов</li>
                            <li>Удобная система назначения задач</li>
                            <li>Обновление статуса выполнения работ</li>
                        </ul>
                    </div>
                </div>
            </section>
            <section class="col-md-4" aria-labelledby="featuresTitle">
                <div class="card h-100">
                    <div class="card-body">
                        <h2 id="featuresTitle" class="card-title"><i class="fas fa-cogs me-2"></i>Возможности системы</h2>
                        <p class="card-text">
                            Наша система Help Desk предлагает полный набор функций для эффективного управления заявками на обслуживание.
                        </p>
                        <ul>
                            <li>Автоматические уведомления</li>
                            <li>Различные категории для заявок</li>
                            <li>Отслеживание времени обработки заявок</li>
                            <li>Адаптивный дизайн для всех устройств</li>
                        </ul>
                    </div>
                </div>
            </section>
        </div>
    </main>

</asp:Content>
