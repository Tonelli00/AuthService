using Domain.Entities;

namespace Application.Interfaces.Query
{
    public interface IRoleQuery
    {
        Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Role> GetByIdAsync(int paymentStatusId, CancellationToken cancellationToken = default);
    }
}
