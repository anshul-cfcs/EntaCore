using System.Data;

namespace Application.Interfaces
{
    public interface IDapperConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
