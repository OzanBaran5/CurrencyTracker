# 💰 CurrencyTracker

**CurrencyTracker**, Frankfurter API kullanarak anlık döviz kurlarını çeken, hafızada tutan ve **LINQ** sorguları ile veri analizi yapmanızı sağlayan, C# ile geliştirilmiş bir konsol uygulamasıdır.

## 🚀 Proje Hakkında

Bu proje, bir finans firmasının Türk Lirası (TRY) bazlı döviz kurlarını takip etme ihtiyacını karşılamak üzere geliştirilmiştir. Uygulama verileri canlı olarak çeker ve kullanıcıya filtreleme, sıralama ve istatistiksel analiz imkanı sunar.

## 🛠️ Kullanılan Teknolojiler ve Yöntemler

Bu projede aşağıdaki teknik yapılar kullanılmıştır:

* **C# / .NET**
* **HttpClient & JSON Parsing:** `System.Text.Json` ile API entegrasyonu.
* **Asynchronous Programming:** `async` / `await` ile asenkron veri çekme.
* **LINQ (Language Integrated Query):**
    * `Select`: Veri dönüştürme.
    * `Where`: Filtreleme ve arama.
    * `OrderBy` / `OrderByDescending`: Sıralama algoritmaları.
    * `Count`, `Max`, `Min`, `Average`: İstatistiksel hesaplamalar.
* **Clean Code:** Temiz, okunabilir ve modüler kod yapısı.

## ⚙️ Özellikler

Uygulama konsol arayüzü üzerinden şu işlemleri gerçekleştirir:

1.  **Tüm Dövizleri Listele:** Güncel kurları (1 TRY karşılığı) listeler.
2.  **Döviz Ara:** Kod (örn: USD, EUR) girerek spesifik kur bilgisine ulaşılır.
3.  **Filtreleme:** Belirli bir değerin üzerindeki kurları listeler.
4.  **Sıralama:** Kurları küçükten büyüğe veya büyükten küçüğe sıralar.
5.  **İstatistikler:**
    * Toplam döviz sayısı
    * En yüksek kur
    * En düşük kur
    * Ortalama kur değeri

## 🔌 API Kaynağı

Proje, verileri **Frankfurter API** üzerinden almaktadır.
* **Endpoint:** `https://api.frankfurter.app/latest?from=TRY`

## 💻 Kurulum ve Çalıştırma

Projeyi bilgisayarınıza klonlayın:

```bash
git clone [https://github.com/KULLANICIADIN/CurrencyTracker.git](https://github.com/KULLANICIADIN/CurrencyTracker.git)