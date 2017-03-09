using WhosOnFirst.Data.Sql.Models;

namespace WhosOnFirst.Data.Sql.Interfaces
{
    public interface ISecurity
    {
        UserValidation Authenticate(string username, string password);
    }
}
