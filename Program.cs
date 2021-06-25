using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleApp
{
    class Program
    {
        private static readonly TimeSpan _updateInterval = TimeSpan.FromSeconds(5);
        private static readonly CurrencyRatesUpdater _currencyRatesUpdater = new(_updateInterval);

        static async Task Main(string[] args)
        {
            var date1 = DateTime.Now.AddMonths(-6);
            var cbrApi = new CbrApi();
            var сurrencyRates = await cbrApi.GetCurrencyRates(date1);
            var usd1 = сurrencyRates.Valutes.FirstOrDefault(v => v.NumCode == 840).Value;
            Console.WriteLine("Курс на {0:d}: 1 USD = {1} RUB с помощью запроса к ЦБР", date1, usd1 ?? -1);

            var dbContext = new DbContext();
            var usd2 = await dbContext.GetCurrencyRateAsync(date1, 840);
            Console.WriteLine("Курс на {0:d}: 1 USD = {1} RUB с помощью прямого SQL запроса", date1, usd2 ?? -1);

            var usd3 = await dbContext.GetCurrencyRateFuncAsync(date1, 840);
            Console.WriteLine("Курс на {0:d}: 1 USD = {1} RUB с помощью хранимой в БД функции", date1, usd3 ?? -1);

            // Заполнение таблицы Carrencies
            //var cbr = new CbrApi();
            //var currencyRates = await cbr.GetCurrencyRates(DateTime.Now);
            //await dbContext.AddCurrenciesAsync(currencyRates.Valutes);
            //Console.WriteLine("Добавлено {0} валют", currencyRates.Valutes.Count);

            Console.WriteLine("Legend: s - start / f - finish / u - update now / q - quit / p 2021.06.25 680");

            while (true)
            {
                var input = Console.ReadLine();

                if (input.ToLower() == "s")
                    _currencyRatesUpdater.RunAsync();

                if (input.ToLower() == "f")
                    _currencyRatesUpdater.Stop();

                if (input.ToLower() == "u")
                    await _currencyRatesUpdater.UpdateAsync();

                if (input.ToLower().StartsWith("p "))
                {
                    var parts = input.Split(' ');
                    if (parts.Length == 3 
                        && DateTime.TryParse(parts[1], out DateTime date)
                        && int.TryParse(parts[2], out int currencyNumCode))
                    {
                        try
                        {
                            var result = await dbContext.GetCurrencyRateAsync(date, currencyNumCode);
                            Console.WriteLine("Currency Rate on {0:d} for {1} quals {2} RUB", date, currencyNumCode, result ?? -1);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: ", ex.Message);
                        }
                    }
                }

                if (input.ToLower() == "q")
                    break;
            }

            Console.WriteLine("Press any key for exit...");
            Console.ReadKey();
        }
    }
}
