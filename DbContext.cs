using ExampleApp.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleApp
{
    public class DbContext
    {
        public string ConnectionString { get; set; } = "port=3306;server=localhost;database=example;user id=root;password=root";


        /// <summary>
        /// Возвращает значение курса указанной валюты на заданную дату.
        /// <para />
        /// Запрашивается результат явным SQL запросом.
        /// </summary>
        public async Task<decimal?> GetCurrencyRateAsync(DateTime date, int cursNumCode)
        {
            using var conn = new MySqlConnection(ConnectionString);
            decimal? result;

            try
            {
                var taskOpen = conn.OpenAsync();

                const string query =
                    "SELECT `currency_rates`.`value` " +
                    "FROM `currency_rates`, `currencies` " +
                    "WHERE `currencies`.`numCode` = @curNumCode " +
                        "AND `currencies`.`id` = `currency_rates`.`currencyId` " +
                        "AND `currency_rates`.`date` = @targetDate;";

                using var queryCmd = new MySqlCommand(query, conn);
                queryCmd.Parameters.AddWithValue("curNumCode", cursNumCode);
                queryCmd.Parameters.AddWithValue("targetDate", date.ToString("yyyyMMdd"));

                await taskOpen;
                if (conn.State != ConnectionState.Open)
                    throw new Exception();

                var ret = await queryCmd.ExecuteScalarAsync();
                result = ret == null || ret is DBNull ? null : (decimal)ret;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await conn.CloseAsync();
            }

            return result;
        }

        /// <summary>
        /// Возвращает значение курса указанной валюты на заданную дату.
        /// <para />
        /// Запрашивается результат хранимой функции БД.
        /// </summary>
        /// <param name="cursNumCode"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<decimal?> GetCurrencyRateFuncAsync(DateTime date, int cursNumCode)
        {
            using var conn = new MySqlConnection(ConnectionString);
            decimal? result;

            try
            {
                var taskOpen = conn.OpenAsync();

                const string query = "SELECT GET_CURRENCY_VALUE(@curNumCode, @targetDate);";

                using var queryCmd = new MySqlCommand(query, conn);
                queryCmd.Parameters.AddWithValue("curNumCode", cursNumCode);
                queryCmd.Parameters.AddWithValue("targetDate", date.ToString("yyyyMMdd"));

                await taskOpen;
                if (conn.State != ConnectionState.Open)
                    throw new Exception();

                var ret = await queryCmd.ExecuteScalarAsync();
                result = ret == null || ret is DBNull ? null : (decimal)ret;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await conn.CloseAsync();
            }

            return result;
        }

        /// <summary>
        /// Обновляет значения курсов валют.
        /// </summary>
        public async Task AddCurrencyRatesAsync(DateTime date, IEnumerable<Valute> valutes)
        {
            using var conn = new MySqlConnection(ConnectionString);

            try
            {
                var taskOpen = conn.OpenAsync();
                var query =
                    "INSERT INTO `currency_rates` (`date`, `currencyId`, `value`) VALUES";

                foreach (var valute in valutes)
                {
                    query += string.Format(
                        "\n('{0:yyyy-MM-dd}', (" +
                            "SELECT `example`.`currencies`.`id` " +
                            "FROM `example`.`currencies`" +
                            "WHERE `example`.`currencies`.`extId` = '{1}' " +
                        "), {2}){3}",
                        date,
                        valute.Id,
                        valute.Value.Value.ToString(CultureInfo.InvariantCulture),
                        valutes.Last() == valute ? ';' : ','
                    );
                }

                using var queryCmd = new MySqlCommand(query, conn);

                await taskOpen;
                if (conn.State != ConnectionState.Open)
                    throw new Exception();

                await queryCmd.ExecuteNonQueryAsync();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await conn.CloseAsync();
            }
        }

        /// <summary>
        /// Добавляет валюты.
        /// </summary>
        public async Task AddCurrenciesAsync(IEnumerable<Valute> valutes)
        {
            using var conn = new MySqlConnection(ConnectionString);

            try
            {
                var taskOpen = conn.OpenAsync();
                var query =
                    "INSERT INTO `currencies` (`extId`, `nominal`, `numCode`, `charCode`, `name`) VALUES";

                foreach (var valute in valutes)
                {
                    query += string.Format(
                        "\n('{0}', {1}, {2}, '{3}', '{4}'){5}",
                        valute.Id,
                        valute.Nominal,
                        valute.NumCode,
                        valute.CharCode,
                        valute.Name,
                        valutes.Last() == valute ? ';' : ','
                    );
                }

                using var queryCmd = new MySqlCommand(query, conn);

                await taskOpen;
                if (conn.State != ConnectionState.Open)
                    throw new Exception();

                await queryCmd.ExecuteNonQueryAsync();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
    }
}
