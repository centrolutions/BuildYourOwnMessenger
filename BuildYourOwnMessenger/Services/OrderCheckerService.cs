using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace BuildYourOwnMessenger.Services
{
    public class OrderCheckerService : IOrderCheckerService, IDisposable
    {
        private CancellationTokenSource _CancellationTokenSource;

        public int OrderCount { get; private set; }

        public event EventHandler<EventArgs> OrderCountChanged;

        public OrderCheckerService()
        {
        }

        public async Task StartCheckingOrders()
        {
            try
            {
                _CancellationTokenSource = new CancellationTokenSource();
                await Task.Run(() =>
                {
                    for (; ; )
                    {
                        if (_CancellationTokenSource.IsCancellationRequested)
                            break;
                        Thread.Sleep(2000);
                        CalculateOrderCount();
                        OnOrderCountChanged();
                    }
                });
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public void StopCheckingOrders()
        {
            _CancellationTokenSource?.Cancel();
        }

        public void Dispose()
        {
            _CancellationTokenSource?.Dispose();
        }

        private void CalculateOrderCount()
        {
            var rnd = new Random();
            var orderCount = rnd.Next(1, 20);

            OrderCount += orderCount;
            if (OrderCount < 0)
                OrderCount = 0;
        }

        private void OnOrderCountChanged()
        {
            OrderCountChanged?.Invoke(this, new EventArgs());
        }
    }
}
