using Pixelz.OrderStore;

namespace Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddOrderStore(this IServiceCollection services, string connectionString)
    {
        services.AddEventStoreClient(connectionString);
        services.AddScoped<IEventStoreRepository, DefaultEventStoreRepository>();
        return services;
    }

    public static IServiceCollection AddCheckoutFlow(this IServiceCollection services)
    {
        services.AddPaymentGateway();
        services.AddScoped<ICheckoutSaga, DefaultCheckoutSaga>();
        return services;
    }

    public static IServiceCollection AddPaymentGateway(this IServiceCollection services)
    {
        services.AddSingleton<IPaymentServiceClient, DefaultPaymentServiceClient>();
        services.AddSingleton<IPaymentAdapter, DefaultPaymentAdapter>();

        return services;
    }
}