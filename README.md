# آگاه - پلتفرم هوشمند مدیریت تغییرات قیمت
- استفاده از **Docker و Docker Compose** برای اجرا و مدیریت سرویس‌ها  

---

## 🚧 قابلیت‌های پیش‌رو

- پشتیبانی از **SMS و تماس تلفنی خودکار**  
- افزودن دارایی‌های بیشتر از جمله **ارزهای دیجیتال**  
- امکانات پیشرفته شخصی‌سازی هشدار  
- تحلیل خودکار و هوشمند روند بازار  

---

## 🌱 چشم‌انداز

هدف ما این است که **آگاه** تبدیل به مرجعی هوشمند برای پایش بازار، هشداردهی دقیق و ارائه تحلیل‌های مالی شود.  
در آینده نزدیک قصد داریم:

- افزودن تحلیل‌گرهای خودکار مبتنی بر هوش مصنوعی  
- پشتیبانی از دارایی‌های متنوع مثل سهام، خودرو، ملک  
- طراحی نسخه ویژه تریدرها با امکانات تحلیل تکنیکال و نمودارهای پیشرفته  
- ارائه اپلیکیشن موبایل و نسخه تحت‌وب عمومی  

---

## 🧩 معماری سیستم

آگاه با استفاده از معماری ماژولار و تکنولوژی‌های روز پیاده‌سازی شده است:

- Backend: C# / .NET Core 8 | WEB API | EFCore 6
- Database: SQL Server
- Docker & Docker Compose
- Frontend: React / Tailwind | Redux

---


## 🐳 اجرای پروژه با Docker Compose

1. ابتدا مخزن را کلون کنید:
```bash
git clone https://github.com/yourusername/agah.git
cd agah
```

2. سپس با اجرای دستور زیر، کل سیستم را بالا بیاورید:
```bash
docker-compose up --d
```

3. پس از اجرا، پلتفرم روی http://localhost:5173 در دسترس خواهد بود (در صورت تنظیم این پورت).

<img width="1000" height="211" alt="image" src="https://github.com/user-attachments/assets/530aa294-9247-4074-9f95-f93c64ced156" />


### *توجه!!!*

در صورتی که کانتینرهای داکر به درستی اجرا نشدند ، آن ها را به ترتیب و به صورت دستی روشن کنید. 
1. SQL Server (به دلیل وابستگی سرویس backend برای seed کردن داده های پیشفرض)
2. Backend
3. Frontend

---

## 🧪 راه‌اندازی دستی پروژه
برای اجرای دستی پروژه، مراحل زیر را دنبال کنید:

1. اجرای SQL Server
   
ابتدا از نصب و اجرای SQL Server روی سیستم خود اطمینان حاصل کنید. سپس یک دیتابیس جدید برای پروژه ایجاد نمایید. (در صورت نیاز میتوان Sql Server را با داکر نیز راه اندازی کرد)

2. پیکربندی Backend

- وارد آدرس `Backend>Agah>Agah` شوید.
-  فایل تنظیمات (`appsettings.json`) را باز کرده و آدرس اتصال به SQL Server را مطابق دیتابیس خود وارد کنید:
```json
  "ConnectionStrings": {
    "DefaultConnection":     "Server=address,port;Database=Agah_Db;UserId=sa;Password=password;MultipleActiveResultSets=True;TrustServerCertificate=True"
}
```

3. اجرای Backend

پس از اجرا، API روی ```http://localhost:8080/api``` در دسترس خواهد بود.

4. پیکربندی Frontend

- وارد آدرس `Frontend>Agah` شوید.
- در فایل تنظیمات (`.env`) مقدار VITE_API_BASE_URL را روی آدرس لوکال سرویس بک‌اند قرار دهید:
```bash
VITE_API_BASE_URL=http://localhost:8080/api
```

5. اجرای Frontend
```bash
