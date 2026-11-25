using Application.Interfaces.Database;
using Domain.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Infrastructure.Services.Database
{
    public class StoredProcedureExecutor : IStoredProcedureExecutor
    {
        private readonly string _connectionString;

        public StoredProcedureExecutor(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default")
                               ?? throw new InvalidOperationException("Connection string 'Default' not found.");
        }

        public async Task<DataTable> ExecuteAsync(string procedureName, Dictionary<string, object> parameters)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand(procedureName, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                foreach (var parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value ?? DBNull.Value);
                }

                await connection.OpenAsync();

                using var reader = await command.ExecuteReaderAsync();
                var table = new DataTable();
                table.Load(reader);
                return table;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50000)
                    throw new AppException(ex.Message, 400);

                // log real SQL error if needed
                throw new AppException("A database error occurred.", 500);
            }
        }
    }
}
