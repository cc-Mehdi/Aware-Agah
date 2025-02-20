using System;
using System.Collections.Generic;

namespace Datalayer.Models
{
    public static class FakeDataGenerator
    {
        public static List<User> GenerateUsers(int count)
        {
            var users = new List<User>();
            for (int i = 1; i <= count; i++)
            {
                users.Add(new User
                {
                    Id = i,
                    Fullname = $"User {i}",
                    Email = $"user{i}@example.com",
                    IsEmailVerified = i % 2 == 0,
                    Phone = $"0912345678{i % 10}",
                    IsPhoneVerivied = i % 2 == 0,
                    HashedPassword = $"hashed_password_{i}",
                    CreatedAt = DateTime.Now.AddDays(-i),
                    DeletedAt = i % 5 == 0 ? DateTime.Now.AddDays(-i) : (DateTime?)null
                });
            }
            return users;
        }

        public static List<Reserve> GenerateReserves(int count, List<User> users, List<Product> products, List<Alarm> alarms)
        {
            var reserves = new List<Reserve>();
            var random = new Random();
            for (int i = 1; i <= count; i++)
            {
                reserves.Add(new Reserve
                {
                    Id = i,
                    User_Id = users[random.Next(users.Count)].Id,
                    User = users[random.Next(users.Count)],
                    Product_Id = products[random.Next(products.Count)].Id,
                    Product = products[random.Next(products.Count)],
                    Alarm_Id = alarms[random.Next(alarms.Count)].Id,
                    Alarm = alarms[random.Next(alarms.Count)],
                    MinPrice = random.Next(100, 1000),
                    MaxPrice = random.Next(1000, 10000),
                    IsSent = i % 2 == 0,
                    CreatedAt = DateTime.Now.AddDays(-i),
                    DeletedAt = i % 5 == 0 ? DateTime.Now.AddDays(-i) : (DateTime?)null
                });
            }
            return reserves;
        }

        public static List<ProductLog> GenerateProductLogs(int count, List<Product> products)
        {
            var productLogs = new List<ProductLog>();
            var random = new Random();
            for (int i = 1; i <= count; i++)
            {
                productLogs.Add(new ProductLog
                {
                    Id = i,
                    Product_Id = products[random.Next(products.Count)].Id,
                    Product = products[random.Next(products.Count)],
                    Price = random.Next(100, 10000),
                    CreatedAt = DateTime.Now.AddDays(-i)
                });
            }
            return productLogs;
        }

        public static List<Product> GenerateProducts(int count)
        {
            var products = new List<Product>();
            for (int i = 1; i <= count; i++)
            {
                products.Add(new Product
                {
                    Id = i,
                    EnglishName = $"Product {i}",
                    PersianName = $"محصول {i}",
                    IconName = "fa-solid fa-medal",
                    CreatedAt = DateTime.Now.AddDays(-i),
                    DeletedAt = i % 5 == 0 ? DateTime.Now.AddDays(-i) : (DateTime?)null
                });
            }
            return products;
        }

        public static List<Notification_User> GenerateNotificationUsers(int count, List<User> users)
        {
            var notifications = new List<Notification_User>();
            for (int i = 1; i <= count; i++)
            {
                notifications.Add(new Notification_User
                {
                    Id = i,
                    UserId = users[i % users.Count].Id,
                    User = users[i % users.Count],
                    MessageSubject = $"Notification {i}",
                    MessageContent = $"This is the content of notification {i}",
                    IsRead = i % 2 == 0,
                    CreatedAt = DateTime.Now.AddDays(-i),
                    DeletedAt = i % 5 == 0 ? DateTime.Now.AddDays(-i) : (DateTime?)null
                });
            }
            return notifications;
        }

        public static List<Alarm> GenerateAlarms(int count)
        {
            var alarms = new List<Alarm>();
            for (int i = 1; i <= count; i++)
            {
                alarms.Add(new Alarm
                {
                    Id = i,
                    EnglishName = $"Alarm {i}",
                    PersianName = $"هشدار {i}",
                    AlarmPrice = i * 100,
                    ShortDescription = $"Short description for alarm {i}",
                    IsActive = i % 2 == 0,
                    CreatedAt = DateTime.Now.AddDays(-i),
                    DeletedAt = i % 5 == 0 ? DateTime.Now.AddDays(-i) : (DateTime?)null
                });
            }
            return alarms;
        }
    }
}