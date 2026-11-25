using System.Data;

namespace Application.Interfaces.Database
{
    public interface IDapperConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
