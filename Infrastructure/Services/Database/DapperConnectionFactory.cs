using Application.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Infrastructure.Services.Database
{
    public class DapperConnectionFactory : IDapperConnectionFactory
    {
        private readonly string _masterConnectionString;

        public DapperConnectionFactory(IConfiguration configuration)
        {
            _masterConnectionString = configuration.GetConnectionString("Default") ?? throw new InvalidOperationException("Default connection string is missing.");
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_masterConnectionString);
        }
    }
}
