using BandLabAssesment.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace BandLabAssesment.Repository;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task UpsertAsync(T item, CancellationToken token);
}