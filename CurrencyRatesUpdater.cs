using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExampleApp
{
    public class CurrencyRatesUpdater
    {
        private readonly CbrApi _cbrApi = new();
        private readonly DbContext _dbContext = new();
        private CancellationTokenSource _cancellationTokenSource;
        public TimeSpan Interval { get; set; }

        private DateTime _debugDateInc = DateTime.Now.AddMonths(-6);


        public CurrencyRatesUpdater(TimeSpan interval)
        {
            Interval = interval;
        }

        public async void RunAsync()
        {
            if (_cancellationTokenSource != null)
            {
                Console.WriteLine("Alredy runing");
                return;
            }

            Console.WriteLine("Started");
            _cancellationTokenSource = new CancellationTokenSource();

            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                await UpdateAsync();
                _cancellationTokenSource.Token.WaitHandle.WaitOne(Interval);
            }

            _cancellationTokenSource = null;
            Console.WriteLine("Stoped");
        }

        public void Stop()
        {
            if (_cancellationTokenSource == null)
                Console.WriteLine("Not started yet");
            else
                _cancellationTokenSource.Cancel();
        }

        public async Task UpdateAsync()
        {
            try
            {
                _debugDateInc = _debugDateInc.AddDays(1);
                var date = _debugDateInc.Date;
                var сurrencyRates = await _cbrApi.GetCurrencyRates(date);

                //ЗАМЕЧАНИЕ: сервер может отвечать документом на предыдущую дату, поэтому в AddCurrencyRatesAsync передает
                //           дату из запроса и коллекцию значений котировок.
                await _dbContext.AddCurrencyRatesAsync(date, сurrencyRates.Valutes);
                Console.WriteLine("Updated for {0:d}, {1} currencies", date, сurrencyRates.Valutes.Count);
            }
            catch (Exception ex)
            {
                PrintError(ex);
            }
        }

        private static void PrintError(Exception ex)
        {
            Console.WriteLine("Error: {0}", ex.Message);
            var _ex = ex;
            while (_ex.InnerException != null)
            {
                _ex = _ex.InnerException;
                Console.WriteLine("\t{0}", _ex.Message);
            }
        }
    }
}
