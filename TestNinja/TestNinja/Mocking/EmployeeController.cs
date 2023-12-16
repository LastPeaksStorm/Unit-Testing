using System.Data.Entity;

namespace TestNinja.Mocking
{
    public class EmployeeController
    {
        IEmployeeStorage _employeeStorage;

        public EmployeeController(IEmployeeStorage employeeStorage)
        {
            _employeeStorage = employeeStorage;
        }

        public ActionResult DeleteEmployee(int id)
        {
            _employeeStorage.RemoveEmployee(id);

            return RedirectToAction("Employees");
        }

        private ActionResult RedirectToAction(string employees)
        {
            return new RedirectResult();
        }
    }

    public class ActionResult { }
 
    public class RedirectResult : ActionResult { }
    
    public class EmployeeContext
    {
        public DbSet<Employee> Employees { get; set; }

        public void SaveChanges()
        {
        }
    }

    public class Employee
    {
    }
}