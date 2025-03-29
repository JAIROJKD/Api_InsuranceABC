using Api_InsuranceABC.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Api_InsuranceABC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Insured> Insureds { get; set; }
    }
}
