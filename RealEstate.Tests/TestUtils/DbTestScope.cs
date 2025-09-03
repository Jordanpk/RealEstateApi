using System;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Tests.TestUtils;

namespace RealEstate.Tests.TestUtils
{
    public class DbTestScope : IDisposable
    {
        public RealEstateDbContext Db { get; }
        private readonly TransactionScope _tx;

        public DbTestScope()
        {
            // Crear DbContext contra la MISMA BD real
            var cs = ConfigHelper.GetConnectionString();
            var options = new DbContextOptionsBuilder<RealEstateDbContext>()
                .UseSqlServer(cs)
                .EnableSensitiveDataLogging()
                .Options;

            Db = new RealEstateDbContext(options);

            // Inicia una transacción distribuida para rollback
            _tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        }

        public void Dispose()
        {
            // Rollback
            _tx.Dispose();
            Db.Dispose();
        }
    }
}
