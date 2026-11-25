using System.Data;

namespace Application.Interfaces.Database
{
    public interface IStoredProcedureExecutor
    {
        Task<DataTable> ExecuteAsync(string procedureName, Dictionary<string, object> parameters);
    }
}
