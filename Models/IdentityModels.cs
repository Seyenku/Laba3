using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Laba3.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }

    public class Status
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string StatusName { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }

    public class Personal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Fio { get; set; }

        [Required]
        [StringLength(100)]
        public string Staff { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(20)]
        [DefaultValue("IT_STAFF")]
        public string Role { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }

    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(20)]
        public string Telephone { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(20)]
        [DefaultValue("CLIENT")]
        public string Role { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }

    public class Task
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public string Description { get; set; }

        public int? PersonalId { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public DateTime? DateClosed { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int StatusId { get; set; }

        [ForeignKey("PersonalId")]
        public virtual Personal Personal { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [ForeignKey("StatusId")]
        public virtual Status Status { get; set; }
    }

    // Можно добавить данные о пользователе, указав больше свойств для класса User. Подробности см. на странице https://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        public ClaimsIdentity GenerateUserIdentity(ApplicationUserManager manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = manager.CreateIdentity(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте настраиваемые утверждения пользователя
            return userIdentity;
        }

        public Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager)
        {
            return System.Threading.Tasks.Task.FromResult(GenerateUserIdentity(manager));
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Personal> Personnel { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Task>()
                .ToTable("task");

            modelBuilder.Entity<Category>()
                .ToTable("category");

            modelBuilder.Entity<Client>()
                .ToTable("client");

            modelBuilder.Entity<Status>()
                .ToTable("status");

            modelBuilder.Entity<Personal>()
                .ToTable("personal");

            modelBuilder.Entity<Category>()
                .Property(c => c.CategoryName)
                .HasColumnName("category");

            modelBuilder.Entity<Status>()
                .Property(s => s.StatusName)
                .HasColumnName("status");

            modelBuilder.Entity<Personal>()
                .Property(p => p.Fio)
                .HasColumnName("fio");

            modelBuilder.Entity<Personal>()
                .Property(p => p.Staff)
                .HasColumnName("staff");

            modelBuilder.Entity<Personal>()
                .Property(p => p.Email)
                .HasColumnName("email");

            modelBuilder.Entity<Personal>()
                .Property(p => p.PasswordHash)
                .HasColumnName("password_hash");

            modelBuilder.Entity<Personal>()
                .Property(p => p.Role)
                .HasColumnName("role");

            modelBuilder.Entity<Client>()
                .Property(c => c.Name)
                .HasColumnName("name");

            modelBuilder.Entity<Client>()
                .Property(c => c.Address)
                .HasColumnName("address");

            modelBuilder.Entity<Client>()
                .Property(c => c.Telephone)
                .HasColumnName("telephone");

            modelBuilder.Entity<Client>()
                .Property(c => c.Email)
                .HasColumnName("email");

            modelBuilder.Entity<Client>()
                .Property(c => c.PasswordHash)
                .HasColumnName("password_hash");

            modelBuilder.Entity<Client>()
                .Property(c => c.Role)
                .HasColumnName("role");

            modelBuilder.Entity<Task>()
                .Property(t => t.Name)
                .HasColumnName("name");

            modelBuilder.Entity<Task>()
                .Property(t => t.Description)
                .HasColumnName("description");

            modelBuilder.Entity<Task>()
                .Property(t => t.PersonalId)
                .HasColumnName("personal_id");

            modelBuilder.Entity<Task>()
                .Property(t => t.ClientId)
                .HasColumnName("client_id");

            modelBuilder.Entity<Task>()
                .Property(t => t.DateCreated)
                .HasColumnName("date_created");

            modelBuilder.Entity<Task>()
                .Property(t => t.DateClosed)
                .HasColumnName("date_closed");

            modelBuilder.Entity<Task>()
                .Property(t => t.CategoryId)
                .HasColumnName("category_id");

            modelBuilder.Entity<Task>()
                .Property(t => t.StatusId)
                .HasColumnName("status_id");
        }
    }
}

#region Вспомогательные приложения
namespace Laba3
{
    public static class IdentityHelper
    {
        // Используется для обработки XSRF при связывании внешних имен входа
        public const string XsrfKey = "XsrfId";

        public const string ProviderNameKey = "providerName";
        public static string GetProviderNameFromRequest(HttpRequest request)
        {
            return request.QueryString[ProviderNameKey];
        }

        public const string CodeKey = "code";
        public static string GetCodeFromRequest(HttpRequest request)
        {
            return request.QueryString[CodeKey];
        }

        public const string UserIdKey = "userId";
        public static string GetUserIdFromRequest(HttpRequest request)
        {
            return HttpUtility.UrlDecode(request.QueryString[UserIdKey]);
        }

        public static string GetResetPasswordRedirectUrl(string code, HttpRequest request)
        {
            var absoluteUri = "/Account/ResetPassword?" + CodeKey + "=" + HttpUtility.UrlEncode(code);
            return new Uri(request.Url, absoluteUri).AbsoluteUri.ToString();
        }

        public static string GetUserConfirmationRedirectUrl(string code, string userId, HttpRequest request)
        {
            var absoluteUri = "/Account/Confirm?" + CodeKey + "=" + HttpUtility.UrlEncode(code) + "&" + UserIdKey + "=" + HttpUtility.UrlEncode(userId);
            return new Uri(request.Url, absoluteUri).AbsoluteUri.ToString();
        }

        private static bool IsLocalUrl(string url)
        {
            return !string.IsNullOrEmpty(url) && ((url[0] == '/' && (url.Length == 1 || (url[1] != '/' && url[1] != '\\'))) || (url.Length > 1 && url[0] == '~' && url[1] == '/'));
        }

        public static void RedirectToReturnUrl(string returnUrl, HttpResponse response)
        {
            if (!String.IsNullOrEmpty(returnUrl) && IsLocalUrl(returnUrl))
            {
                response.Redirect(returnUrl);
            }
            else
            {
                response.Redirect("~/");
            }
        }
    }
}
#endregion
