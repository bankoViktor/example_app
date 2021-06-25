using ExampleApp.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExampleApp
{
    class CbrApi
    {
        private readonly HttpClient _httpClient = new();
        const string ep_daily = "http://www.cbr.ru/scripts/XML_daily.asp";
        const string ep_val = "http://www.cbr.ru/scripts/XML_val.asp";

        public CbrApi()
        {
            // поддержка cp1251
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <summary>
        /// Получения котировок на заданный день.
        /// </summary>
        /// <exception cref="Exception" />
        public async Task<ValCurs> GetCurrencyRates(DateTime date)
        {
            var url = string.Format("{0}?date_req={1:dd.MM.yyyy}", ep_daily, date); // date format 21.08.2019
            return await LoadAsync<ValCurs>(url);
        }

        /// <summary>
        /// Получения справочника по кодам валют.
        /// </summary>
        /// <exception cref="Exception" />
        public async Task<Valuta> GetСurrenciesAsync()
        {
            return await LoadAsync<Valuta>(ep_val);
        }

        private async Task<T> LoadAsync<T>(string url) where T : class
        {
            try
            {
                var xmlResp = await _httpClient.GetStringAsync(url);

                if (string.IsNullOrWhiteSpace(xmlResp))
                    throw new Exception("Пустой ответ от сервера");

                using var reader = new StringReader(xmlResp);
                var serializer = new XmlSerializer(typeof(T));
                return serializer.Deserialize(reader) as T;
            }
            catch (Exception ex)
            {
                throw new Exception($"Не удалось загрузить данные. См. внутренее исключение.", ex);
            }
        }
    }
}
