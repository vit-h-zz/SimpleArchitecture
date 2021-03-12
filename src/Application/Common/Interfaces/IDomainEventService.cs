using SimpleArchitecture.Domain.Common;
using System.Threading.Tasks;

namespace SimpleArchitecture.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
