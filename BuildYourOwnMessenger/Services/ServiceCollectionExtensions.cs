using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace BuildYourOwnMessenger.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddViewModels<TViewModelBase>(this IServiceCollection services)
        {
            var vmType = typeof(TViewModelBase);

            var viewModels = vmType.Assembly.ExportedTypes.Where(x => x.IsAssignableTo(vmType) && !x.IsAbstract && !x.IsInterface);

            foreach (var viewModel in viewModels)
            {
                services.AddSingleton(viewModel);
            }

            return services;
        }
    }
}
