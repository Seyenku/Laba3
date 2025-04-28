using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Laba3.Models
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            // Seed Categories
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Id = 1, CategoryName = "Техническая проблема" },
                    new Category { Id = 2, CategoryName = "Установка ПО" },
                    new Category { Id = 3, CategoryName = "Настройка оборудования" },
                    new Category { Id = 4, CategoryName = "Сетевая проблема" },
                    new Category { Id = 5, CategoryName = "Другое" }
                };
                
                categories.ForEach(c => context.Categories.Add(c));
                context.SaveChanges();
            }

            // Seed Statuses
            if (!context.Statuses.Any())
            {
                var statuses = new List<Status>
                {
                    new Status { Id = 1, StatusName = "Новая" },
                    new Status { Id = 2, StatusName = "В работе" },
                    new Status { Id = 3, StatusName = "Ожидает ответа клиента" },
                    new Status { Id = 4, StatusName = "Завершена" }
                };
                
                statuses.ForEach(s => context.Statuses.Add(s));
                context.SaveChanges();
            }

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

            base.Seed(context);
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