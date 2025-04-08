using Domain.Entites;
using Domain.Entities;
using Infrastructure.DbHelper;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class GuestHourRepository(ApplicationDbContext dbContext) : GenericRepository<GuestHour>(dbContext), IGuestHourRepository
{
    
}