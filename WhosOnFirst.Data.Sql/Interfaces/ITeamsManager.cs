using System.Collections.Generic;
using WhosOnFirst.Data.Sql.Models;

namespace WhosOnFirst.Data.Sql.Interfaces
{
    public interface ITeamsManager
    {
        void Add(string TeamName);
        void Remove(int Id);
        IEnumerable<Teams> GetAll();
    }
}
