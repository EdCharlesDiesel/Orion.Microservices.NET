using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.Admin.Tools
{
    public static class EventDiExtensions
    {
        public static IServiceCollection AddEventHandler<T, H>
            (this IServiceCollection services)
            where T : IEventNotification
            where H : class, IEventHandler<T>
        {
            services.TryAddScoped(typeof(EventTrigger<>));
            services.AddScoped<IEventHandler<T>, H>();

            return services;
        }
        public static IServiceCollection AddEventMediator(this IServiceCollection service)
        {
            service.AddTransient<IEventMediator, EventMediator>();
            return service;
        }
        public static IServiceCollection AddAllEventHandlers
            (this IServiceCollection service, Assembly assembly)
        {
            var method=typeof(EventDiExtensions).GetMethod("AddEventHandler",
                BindingFlags.Static | BindingFlags.Public);

            var handlers=assembly.GetTypes()
                .Where(x => !x.IsAbstract && x.IsClass 
                && typeof(IEventHandler).IsAssignableFrom(x));
            foreach(var handler in handlers)
            {
                var handlerInterface = handler.GetInterfaces()
                    .Where(i => i.IsGenericType && typeof(IEventHandler).IsAssignableFrom(i))
                    .SingleOrDefault();
                if(handlerInterface != null)
                {
                    var eventType = handlerInterface.GetGenericArguments()[0];
                    method.MakeGenericMethod(eventType, handler)
                        .Invoke(null, new object[] { service });
                }
            }
            service.AddEventMediator();
            return service;
        }
        public static IServiceCollection AddAllCommandHandlers
            (this IServiceCollection service, Assembly assembly)
        {
            var handlers = assembly.GetTypes()
                .Where(x => !x.IsAbstract && x.IsClass
                && typeof(ICommandHandler).IsAssignableFrom(x));
            foreach (var handler in handlers)
            {
                var handlerInterface = handler.GetInterfaces()
                    .Where(i => i.IsGenericType && typeof(ICommandHandler).IsAssignableFrom(i))
                    .SingleOrDefault();
                if (handlerInterface != null)
                {
                    service.AddScoped(handlerInterface, handler);
                }
            }
            return service;
        }
        public static IServiceCollection AddAllQueries
            (this IServiceCollection service, Assembly assembly)
        {
            var queries = assembly.GetTypes()
                .Where(x => !x.IsAbstract && x.IsClass
                && typeof(IQuery).IsAssignableFrom(x));
            foreach (var query in queries)
            {
                var queryInterface = query.GetInterfaces()
                    .Where(i => !i.IsGenericType && typeof(IQuery) != i &&
                    typeof(IQuery).IsAssignableFrom(i))
                    .SingleOrDefault();
                if (queryInterface != null)
                {
                    service.AddTransient(queryInterface, query);
                }
            }
            return service;
        }
    }
}
