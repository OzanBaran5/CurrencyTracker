using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CurrencyTracker
{

    public class CurrencyResponse
    {
        [JsonPropertyName("base")]
        public string Base { get; set; }

        [JsonPropertyName("rates")]
        public Dictionary<string, decimal> Rates { get; set; }
    }

    public class Currency
    {
        public string Code { get; set; }
        public decimal Rate { get; set; }
    }

    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static List<Currency> currencies = new List<Currency>();

        static async Task Main(string[] args)
        {
            Console.Title = "CurrencyTracker - Döviz Takip";

            Console.WriteLine("Veriler sunucudan çekiliyor, lütfen bekleyiniz...");
            bool veriCekildi = await VerileriGetir();

            if (!veriCekildi)
            {
                Console.WriteLine("Veri çekilemedi. Program kapatılıyor.");
                return;
            }

            Console.WriteLine("\nVeriler yüklendi! Menüye geçiliyor...");
            await Task.Delay(1500);

            bool devam = true;
            while (devam)
            {

                Console.Clear();

                Console.WriteLine("===== CurrencyTracker =====");
                Console.WriteLine("1. Tüm dövizleri listele");
                Console.WriteLine("2. Koda göre döviz ara");
                Console.WriteLine("3. Belirli bir değerden büyük dövizleri listele");
                Console.WriteLine("4. Dövizleri değere göre sırala");
                Console.WriteLine("5. İstatistiksel özet göster");
                Console.WriteLine("0. Çıkış");
                Console.Write("Seçiminiz: ");

                string secim = Console.ReadLine();
                Console.WriteLine("-----------------------------");

                switch (secim)
                {
                    case "1":
                        TumDovizleriListele();
                        break;
                    case "2":
                        KodaGoreAra();
                        break;
                    case "3":
                        DegereGoreFiltrele();
                        break;
                    case "4":
                        SiraliListele();
                        break;
                    case "5":
                        IstatistikGoster();
                        break;
                    case "0":
                        devam = false;
                        Console.WriteLine("Çıkış yapılıyor...");
                        break;
                    default:
                        Console.WriteLine("Geçersiz seçim!");
                        break;
                }


                if (devam)
                {
                    Console.WriteLine("\nDevam etmek için bir tuşa basınız...");
                    Console.ReadKey();
                }
            }
        }

        static async Task<bool> VerileriGetir()
        {
            try
            {
                string url = "https://api.frankfurter.app/latest?from=TRY";
                string jsonString = await client.GetStringAsync(url);

                var response = JsonSerializer.Deserialize<CurrencyResponse>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (response != null && response.Rates != null)
                {
                    currencies = response.Rates.Select(k => new Currency
                    {
                        Code = k.Key,
                        Rate = k.Value
                    }).ToList();

                    Console.WriteLine($"Başarılı! Toplam {currencies.Count} adet döviz kuru yüklendi. (Baz: TRY)");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
            }
            return false;
        }

        static void TumDovizleriListele()
        {
            var liste = currencies.Select(c => $"{c.Code}: {c.Rate}").ToList();

            Console.WriteLine("--- Tüm Döviz Kurları (1 TRY Karşılığı) ---");
            foreach (var item in liste)
            {
                Console.WriteLine(item);
            }
        }

        static void KodaGoreAra()
        {
            Console.Write("Aramak istediğiniz döviz kodu (örn: USD, EUR): ");
            string input = Console.ReadLine();

            var sonuc = currencies
                        .Where(c => c.Code.Equals(input, StringComparison.OrdinalIgnoreCase))
                        .FirstOrDefault();

            if (sonuc != null)
            {
                Console.WriteLine($"Bulundu -> {sonuc.Code}: {sonuc.Rate}");
            }
            else
            {
                Console.WriteLine("Böyle bir döviz kodu bulunamadı.");
            }
        }

        static void DegereGoreFiltrele()
        {
            Console.Write("Hangi değerden büyük olanları listelemek istersiniz? (Örn: 0.05): ");
            if (decimal.TryParse(Console.ReadLine(), out decimal deger))
            {
                var sonuclar = currencies.Where(c => c.Rate > deger).ToList();

                if (sonuclar.Count > 0)
                {
                    Console.WriteLine($"\n{deger} değerinden büyük {sonuclar.Count} adet kur bulundu:");
                    foreach (var c in sonuclar)
                    {
                        Console.WriteLine($"{c.Code}: {c.Rate}");
                    }
                }
                else
                {
                    Console.WriteLine("Bu kriterde döviz bulunamadı.");
                }
            }
            else
            {
                Console.WriteLine("Geçersiz sayı girdiniz.");
            }
        }

        static void SiraliListele()
        {
            Console.WriteLine("1. Küçükten Büyüğe (Artan)");
            Console.WriteLine("2. Büyükten Küçüğe (Azalan)");
            Console.Write("Seçim: ");
            string secim = Console.ReadLine();

            List<Currency> siraliListe;

            if (secim == "1")
            {
                siraliListe = currencies.OrderBy(c => c.Rate).ToList();
            }
            else
            {
                siraliListe = currencies.OrderByDescending(c => c.Rate).ToList();
            }

            Console.WriteLine("\n--- Sıralı Liste ---");
            foreach (var c in siraliListe)
            {
                Console.WriteLine($"{c.Code}: {c.Rate}");
            }
        }

        static void IstatistikGoster()
        {
            if (currencies.Count == 0)
            {
                Console.WriteLine("Veri yok.");
                return;
            }

            int toplamSayi = currencies.Count();
            decimal enYuksek = currencies.Max(c => c.Rate);
            decimal enDusuk = currencies.Min(c => c.Rate);
            decimal ortalama = currencies.Average(c => c.Rate);

            var enYuksekKur = currencies.First(c => c.Rate == enYuksek);
            var enDusukKur = currencies.First(c => c.Rate == enDusuk);

            Console.WriteLine("=== İstatistikler ===");
            Console.WriteLine($"Toplam Döviz Sayısı : {toplamSayi}");
            Console.WriteLine($"En Yüksek Kur       : {enYuksek:F4} ({enYuksekKur.Code})");
            Console.WriteLine($"En Düşük Kur        : {enDusuk:F4} ({enDusukKur.Code})");
            Console.WriteLine($"Ortalama Kur        : {ortalama:F4}");
        }
    }
}