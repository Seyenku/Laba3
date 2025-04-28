using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Configuration;

namespace Laba3
{
    public class EmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _senderEmail;
        private readonly string _senderName;
        private readonly bool _enableSsl;

        public EmailService()
        {
            _smtpServer = ConfigurationManager.AppSettings["SmtpServer"] ?? "smtp.yandex.ru";
            _smtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"] ?? "587");
            _smtpUsername = ConfigurationManager.AppSettings["SmtpUsername"] ?? "Seyenku@yandex.ru";
            _smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"] ?? "objbtjmmkejqonsa";
            _senderEmail = ConfigurationManager.AppSettings["SenderEmail"] ?? _smtpUsername;
            _senderName = ConfigurationManager.AppSettings["SenderName"] ?? "Служба поддержки";
            _enableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["SmtpEnableSsl"] ?? "true");
        }

        public bool SendEmail(string recipientEmail, string subject, string body, bool isHtml = true)
        {
            try
            {
                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(_senderEmail, _senderName);
                    message.To.Add(new MailAddress(recipientEmail));
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = isHtml;
                    message.BodyEncoding = Encoding.UTF8;
                    message.SubjectEncoding = Encoding.UTF8;

                    using (var client = new SmtpClient(_smtpServer, _smtpPort))
                    {
                        client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                        client.EnableSsl = _enableSsl;
                        client.Send(message);
                    }
                }
                
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка отправки email: {ex.Message}");
                return false;
            }
        }

        public bool SendNewTicketNotification(string recipientEmail, string ticketNumber, string ticketTitle)
        {
            string subject = $"Новая заявка #{ticketNumber}";
            
            string body = $@"
            <html>
            <body style='font-family: Arial, sans-serif;'>
                <h2>Новая заявка создана</h2>
                <p>Заявка <strong>#{ticketNumber}</strong> - '{ticketTitle}' была зарегистрирована в системе.</p>
                <p>Наши специалисты рассмотрят её в ближайшее время.</p>
                <p>Вы можете отслеживать статус заявки в личном кабинете.</p>
                <hr>
                <p style='font-size: 12px; color: #666;'>Это автоматическое сообщение, пожалуйста, не отвечайте на него.</p>
            </body>
            </html>";

            return SendEmail(recipientEmail, subject, body);
        }

        public bool SendStatusChangeNotification(string recipientEmail, string ticketNumber, string ticketTitle, string newStatus)
        {
            string subject = $"Изменение статуса заявки #{ticketNumber}";
            
            string body = $@"
            <html>
            <body style='font-family: Arial, sans-serif;'>
                <h2>Статус заявки изменен</h2>
                <p>Статус заявки <strong>#{ticketNumber}</strong> - '{ticketTitle}' был изменен на <strong>{newStatus}</strong>.</p>
                <p>Для получения дополнительной информации перейдите в личный кабинет.</p>
                <hr>
                <p style='font-size: 12px; color: #666;'>Это автоматическое сообщение, пожалуйста, не отвечайте на него.</p>
            </body>
            </html>";

            return SendEmail(recipientEmail, subject, body);
        }

        public bool SendNewCommentNotification(string recipientEmail, string ticketNumber, string ticketTitle, string commentAuthor)
        {
            string subject = $"Новый комментарий к заявке #{ticketNumber}";
            
            string body = $@"
            <html>
            <body style='font-family: Arial, sans-serif;'>
                <h2>Новый комментарий к заявке</h2>
                <p>К заявке <strong>#{ticketNumber}</strong> - '{ticketTitle}' был добавлен новый комментарий от <strong>{commentAuthor}</strong>.</p>
                <p>Для просмотра комментария перейдите в личный кабинет.</p>
                <hr>
                <p style='font-size: 12px; color: #666;'>Это автоматическое сообщение, пожалуйста, не отвечайте на него.</p>
            </body>
            </html>";

            return SendEmail(recipientEmail, subject, body);
        }

        // Отправка уведомления о назначении специалиста
        public bool SendAssignmentNotification(string recipientEmail, string ticketNumber, string ticketTitle, string staffName)
        {
            string subject = $"Специалист назначен на заявку #{ticketNumber}";
            
            string body = $@"
            <html>
            <body style='font-family: Arial, sans-serif;'>
                <h2>Специалист назначен на вашу заявку</h2>
                <p>На заявку <strong>#{ticketNumber}</strong> - '{ticketTitle}' назначен специалист: <strong>{staffName}</strong>.</p>
                <p>Специалист свяжется с вами в ближайшее время.</p>
                <hr>
                <p style='font-size: 12px; color: #666;'>Это автоматическое сообщение, пожалуйста, не отвечайте на него.</p>
            </body>
            </html>";

            return SendEmail(recipientEmail, subject, body);
        }

        public bool TestConnection()
        {
            try
            {
                using (var client = new SmtpClient(_smtpServer, _smtpPort))
                {
                    client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                    client.EnableSsl = _enableSsl;
                    client.Timeout = 10000;
                    client.SendMailAsync(new MailMessage(
                        new MailAddress(_senderEmail, _senderName),
                        new MailAddress(_senderEmail)
                    )
                    {
                        Subject = "Тест подключения",
                        Body = "Это тестовое сообщение для проверки настроек SMTP.",
                        IsBodyHtml = true
                    }).Wait();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка подключения к SMTP: {ex.Message}");
                return false;
            }
        }
    }
} 