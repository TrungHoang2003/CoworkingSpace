using Domain.Entites;
using Infrastructure.Common;

namespace Infrastructure.Interfaces;

public interface IWorkingSpaceRepository: IGenericRepository<Space>
{
   Task<Result<IEnumerable<Space>>> GetAllWorkingSpacesAsync();
}