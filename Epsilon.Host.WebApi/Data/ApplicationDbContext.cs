using Epsilon.Data;
using Microsoft.EntityFrameworkCore;

namespace Epsilon.Host.WebApi.Data;

public class ApplicationDbContext : LearningDomainDbContext
{
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }
}