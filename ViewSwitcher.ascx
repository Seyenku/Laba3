<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewSwitcher.ascx.cs" Inherits="Laba3.ViewSwitcher" %>
<div id="viewSwitcher" class="mt-3 mb-2 text-center">
    <span class="me-2 text-muted small"><i class="fas fa-display me-1"></i><%: CurrentView %> версия сайта</span>
    <a href="<%: SwitchUrl %>" class="btn btn-sm btn-outline-secondary" data-ajax="false">
        <i class="fas fa-sync-alt me-1"></i>Перейти на <%: AlternateView.ToLower() %> версию
    </a>
</div>