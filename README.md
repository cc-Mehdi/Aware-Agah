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

## 🖼️ تصاویر رابط کاربری

#### دسکتاپ
<img width="477" height="556" alt="image" src="https://github.com/user-attachments/assets/da4437e9-0ba6-4740-916b-edad3cf5d80d" />

<img width="620" height="959" alt="image" src="https://github.com/user-attachments/assets/721431f6-cb29-49c3-94e2-5d2e2ed1f31d" />

<img width="754" height="560" alt="image" src="https://github.com/user-attachments/assets/20a215ff-5f35-436a-87e7-d3c985694d96" />

<img width="766" height="781" alt="image" src="https://github.com/user-attachments/assets/53d10332-efae-4183-83e1-a34515650f8c" />

<img width="1920" height="960" alt="image" src="https://github.com/user-attachments/assets/47cdd766-5cc7-40a4-82b5-a856eff79d73" />

#### موبایل
<img width="418" height="776" alt="image" src="https://github.com/user-attachments/assets/54af8ea5-f1cb-49e2-aee6-671cbe61e8ae" />

<img width="455" height="769" alt="image" src="https://github.com/user-attachments/assets/518d513e-a92b-40d0-b2f4-683848efcc5b" />

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
cd frontend
npm i
npm run dev
```

رابط کاربری روی ```http://localhost:5173``` اجرا خواهد شد.

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
