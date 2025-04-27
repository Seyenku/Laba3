using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;

namespace Laba3.Models
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            // Add statuses
            var statuses = new List<Status>
            {
                new Status { StatusName = "Новая" },
                new Status { StatusName = "В работе" },
                new Status { StatusName = "Ожидает ответа клиента" },
                new Status { StatusName = "Завершена" }
            };
            statuses.ForEach(s => context.Statuses.Add(s));

            // Add categories
            var categories = new List<Category>
            {
                new Category { CategoryName = "Техническая проблема" },
                new Category { CategoryName = "Установка ПО" },
                new Category { CategoryName = "Настройка оборудования" },
                new Category { CategoryName = "Сетевая проблема" },
                new Category { CategoryName = "Другое" }
            };
            categories.ForEach(c => context.Categories.Add(c));

            // Add sample IT staff member
            var adminPassword = ComputeSha256Hash("admin123");
            var itStaff = new Personal
            {
                Fio = "Иванов Иван Иванович",
                Staff = "Системный администратор",
                Email = "admin@example.com",
                PasswordHash = adminPassword,
                Role = "IT_STAFF"
            };
            context.Personnel.Add(itStaff);

            context.SaveChanges();
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}