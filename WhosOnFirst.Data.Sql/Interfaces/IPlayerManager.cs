using System.Collections.Generic;
using WhosOnFirst.Data.Sql.Models;

namespace WhosOnFirst.Data.Sql.Interfaces
{
    public interface IPlayerManager
    {
        bool Exists(int Id);
        void Add(Person person, Players player);

        void Remove(int Id);
        IEnumerable<Players> GetAll();

    }
}
