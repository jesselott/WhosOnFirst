using System.Collections.Generic;
using WhosOnFirst.Data.Sql.Models;

namespace WhosOnFirst.Data.Sql.Interfaces
{
    public interface IPersonManager
    {
        void Add(Person person);
        void Remove(int Id);
        IEnumerable<Person> GetAll();
        bool Exists(string firstName, string eMail);
        Person Retrieve(string firstName, string eMail);
        Person Retrieve(int Id);
        void Update(Person person);

    }
}
