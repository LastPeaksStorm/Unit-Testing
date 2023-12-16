using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNinja.Mocking
{
    public interface IEmployeeStorage
    {
        void RemoveEmployee(int id);
    }
    public class EmployeeStorage : IEmployeeStorage
    {
        private EmployeeContext _db;
        public EmployeeStorage(EmployeeContext db)
        {
            _db = db;
        }

        public void RemoveEmployee(int id)
        {
            var employee = _db.Employees.Find(id);
            _db.Employees.Remove(employee);
            _db.SaveChanges();
        }
    }
}
