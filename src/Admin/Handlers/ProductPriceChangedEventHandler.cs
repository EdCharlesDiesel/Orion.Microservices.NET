using Orion.Admin.Tools;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.Admin.Handlers
{
    public class PackagePriceChangedEventHandler(IPackageEventRepository repo) :
        IEventHandler<PackagePriceChangedEvent>
    {
        public Task HandleAsync(PackagePriceChangedEvent ev)
        {
            repo.New(PackageEventType.CostChanged, ev.PackageId, ev.OldVersion, ev.NewVersion, ev.NewPrice);
            return Task.CompletedTask;
        }
    }

    public class PackagePriceChangedEvent : IEventNotification
    {
        public object PackageId { get; set; }
        public object OldVersion { get; set; }
        public object NewVersion { get; set; }
        public object NewPrice { get; set; }
    }

    public class PackageEventType
    {
        public static object CostChanged { get; set; }
    }

    public interface IPackageEventRepository
    {
        void New(object costChanged, object packageId, object oldVersion, object newVersion, object newPrice);
    }
}
