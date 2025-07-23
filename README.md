# آگاه - پلتفرم هوشمند مدیریت تغییرات قیمت

![GitHub repo size](https://img.shields.io/github/repo-size/cc-mehdi/Aware-Agah)
![GitHub stars](https://img.shields.io/github/stars/cc-mehdi/Aware-Agah?style=social)
![GitHub forks](https://img.shields.io/github/forks/cc-mehdi/Aware-Agah?style=social)
![GitHub watchers](https://img.shields.io/github/watchers/cc-mehdi/Aware-Agah?style=social)
![GitHub last commit](https://img.shields.io/github/last-commit/cc-mehdi/Aware-Agah)
![GitHub issues](https://img.shields.io/github/issues/cc-mehdi/Aware-Agah)

---

## ✨ معرفی پروژه

**آگاه** یک پلتفرم هوشمند برای رصد لحظه‌ای و مدیریت تغییرات قیمت در بازارهای مالی و کالایی است. این سامانه به کاربران کمک می‌کند با صرف کمترین زمان از نوسانات قیمتی مطلع شده و تصمیمات مالی آگاهانه‌تری بگیرند.

---

## ⚙️ ویژگی‌های کلیدی

- **هشدار چندکاناله:** اعلان از طریق ایمیل، پیامک، تماس تلفنی و اعلان درون‌برنامه‌ای  
- **رصد لحظه‌ای بازار:** مشاهده نوسانات به‌صورت لحظه‌ای  
- **تنظیم بازه قیمتی:** تعیین حداقل و حداکثر قیمت برای هر دارایی  
- **پشتیبانی از چندین دارایی مالی:** طلا، ارز، و به‌زودی سایر رمزارزها  
- **رابط کاربری ساده:** مناسب برای تمامی کاربران با هر سطح دانش  

---

## ✅ قابلیت‌های پیاده‌سازی‌شده

- هشدار از طریق **ایمیل** و **اعلان درون‌برنامه‌ای**  
- رصد قیمت **طلا** و **ارز**  
- اتصال کامل به **پایگاه‌داده SQL**  
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

---

## 🧪 راه‌اندازی دستی پروژه
برای اجرای دستی پروژه، مراحل زیر را دنبال کنید:

1. اجرای SQL Server
   
ابتدا از نصب و اجرای SQL Server روی سیستم خود اطمینان حاصل کنید. سپس یک دیتابیس جدید برای پروژه ایجاد نمایید. (در صورت نیاز میتوان Sql Server را با داکر نیز راه اندازی کرد)

2. پیکربندی Backend

- وارد آدرس `Backend>Agah>Agah>` شوید.
-  فایل تنظیمات (`appsettings.json`) را باز کرده و آدرس اتصال به SQL Server را مطابق دیتابیس خود وارد کنید:
```json
  "ConnectionStrings": {
    "DefaultConnection":     "Server=address,port;Database=Agah_Db;UserId=sa;Password=password;MultipleActiveResultSets=True;TrustServerCertificate=True"
}
```

3. اجرای Backend

پس از اجرا، API روی مرورگر در دسترس خواهد بود.

---
## 🤝 مشارکت

برای مشارکت در توسعه این پروژه:

1. این مخزن را فورک کنید
2. تغییرات خود را در یک Branch جدید اعمال کنید
3. در نهایت Pull Request بفرستید

---

## 📬 تماس

برای هرگونه سؤال یا همکاری:

📧 Email: [mailto:cc.mehdigholami@gmail.com](cc.mehdigholami@gmail.com)
🌐 Github: [https://github.com/cc-mehdi](cc-Mehdi)
