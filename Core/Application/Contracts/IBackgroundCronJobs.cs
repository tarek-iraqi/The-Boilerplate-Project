using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IBackgroundCronJobs
    {
        Task HandleDomainEvents();
    }
}
