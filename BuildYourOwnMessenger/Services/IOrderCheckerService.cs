using System;
using System.Threading.Tasks;

namespace BuildYourOwnMessenger.Services
{
    public interface IOrderCheckerService
    {
        event EventHandler<EventArgs> OrderCountChanged;
        int OrderCount { get; }
        Task StartCheckingOrders();
        void StopCheckingOrders();
    }
}