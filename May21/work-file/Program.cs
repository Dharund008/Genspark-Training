// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeApplication.Interfaces;
using WholeApplication.Models;


//sample to note for !!!
namespace WholeApplication
{
    public class ManageEmployee
    {
        private readonly IEmployeeService _employeeService;

        public ManageEmployee(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        public void Start()
        {
            bool exit = false;
            while (!exit)
            {
                PrintMenu();
                int option = 0;
                while (!int.TryParse(Console.ReadLine(), out option) || (option < 1 && option > 2))
                {
                    Console.WriteLine("Invalid entry. Please enter a valid option");
                }
                switch (option)
                {
                    case 1:
                        AddEmployee();
                        break;
                    case 2:
                        SearchEmployee();
                        break;
                    default:
                        exit = true;
                        break;
                }
            }
        }
        public void PrintMenu()
        {
            Console.WriteLine("Choose what you wanted");
            Console.WriteLine("1. Add Employee");
            Console.WriteLine("2. Search Employee");
        }
        public void AddEmployee()
        {
            Employee employee = new Employee();
            employee.TakeEmployeeDetailsFromUser();
            int id = _employeeService.AddEmployee(employee);
            Console.WriteLine("The employee added. The Id is id");
        }
        public void SearchEmployee()
        {
            var searchMenu = PrintSearchMenu();
            var employees = _employeeService.SearchEmployee(searchMenu);
            Console.WriteLine("The search options you have selected");
            Console.WriteLine(searchMenu);
            if ((employees == null))
            {
                Console.WriteLine("No Employees for the search");
            }
            PrintEmployees(employees);

        }

        private void PrintEmployees(List<Employee>? employees)
        {
            foreach (var employee in employees)
            {
                Console.WriteLine(employee);
            }
        }

        private SearchModel PrintSearchMenu()
        {
            Console.WriteLine("Please select the search option");
            SearchModel searchModel = new SearchModel();
            Console.WriteLine("Search by ID? Type 1 for yes Type 2 no");
            int idOption = 0;
            while (!int.TryParse(Console.ReadLine(), out idOption) || (idOption != 1 && idOption != 2))
            {
                Console.WriteLine("Invalid entry. Please enter 1 for yes and 2 for no");
            }
            if (idOption == 1)
            {
                Console.WriteLine("Please enter the employee ID");
                int id;
                while (!int.TryParse(Console.ReadLine(), out id) || id <= 0)
                {
                    Console.WriteLine("Invalid entry for ID. Please enter a valid employee ID");
                }
                searchModel.Id = id;
                idOption = 0;
                return searchModel;
            }
            Console.WriteLine("Search by Name. ? Type 1 for yes Type 2 no");
            while (!int.TryParse(Console.ReadLine(), out idOption) || (idOption != 1 && idOption != 2))
            {
                Console.WriteLine("Invalid entry. Please enter 1 for yes and 2 for no");
            }
            if (idOption == 1)
            {
                Console.WriteLine("Please enter the employee Name");
                string name = Console.ReadLine() ?? "";
                searchModel.Name = name;
                idOption = 0;
            }
            Console.WriteLine("3. Search by Age. Please enter 1 for yes and 2 for no");
            while (!int.TryParse(Console.ReadLine(), out idOption) || (idOption != 1 && idOption != 2))
            {
                Console.WriteLine("Invalid entry. Please enter 1 for yes and 2 for no");
            }
            if (idOption == 1)
            {
                searchModel.Age = new Range<int>();
                int age;
                Console.WriteLine("Please enter the min employee Age");
                while (!int.TryParse(Console.ReadLine(), out age) || age <= 18)
                {
                    Console.WriteLine("Invalid entry for min age. Please enter a valid employee age");
                }
                searchModel.Age.MinVal = age;
                Console.WriteLine("Please enter the max employee Age");
                while (!int.TryParse(Console.ReadLine(), out age) || age <= 18)
                {
                    Console.WriteLine("Invalid entry for max age. Please enter a valid employee age");
                }
                searchModel.Age.MaxVal = age;
                idOption = 0;
            }

            Console.WriteLine("4. Search by Salary. Please enter 1 for yes and 2 for no");
            while (!int.TryParse(Console.ReadLine(), out idOption) || (idOption != 1 && idOption != 2))
            {
                Console.WriteLine("Invalid entry. Please enter 1 for yes and 2 for no");
            }
            if (idOption == 1)
            {
                searchModel.Salary = new Range<double>();
                double salary;
                Console.WriteLine("Please enter the min employee Salary");
                while (!double.TryParse(Console.ReadLine(), out salary) || salary <= 0)
                {
                    Console.WriteLine("Invalid entry for min salary. Please enter a valid employee salary");
                }
                searchModel.Salary.MinVal = salary;
                Console.WriteLine("Please enter the max employee Salary");
                while (!double.TryParse(Console.ReadLine(), out salary) || salary <= 0)
                {
                    Console.WriteLine("Invalid entry for max salary. Please enter a valid employee salary");
                }
                searchModel.Salary.MaxVal = salary;
                idOption = 0;
            }
            return searchModel;
        }
    }
}
----------------------------------------------------------
using WholeApplication.Interfaces;
using WholeApplication.Models;
using WholeApplication.Repositories;
using WholeApplication.Services;

namespace WholeApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            IRepositor<int, Employee> employeeRepository = new EmployeeRepository();
            IEmployeeService employeeService = new EmployeeService(employeeRepository);
            ManageEmployee manageEmployee = new ManageEmployee(employeeService);
            manageEmployee.Start();
        }
    }
}
--------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeApplication.Interfaces;
using WholeApplication.Models;

namespace WholeApplication.Services
{
    public class EmployeeService : IEmployeeService
    {
        IRepositor<int, Employee> _employeeRepository;
        public EmployeeService(IRepositor<int, Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public int AddEmployee(Employee employee)
        {
            try
            {
                var result = _employeeRepository.Add(employee);
                if (result != null)
                {
                    return result.Id;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return -1;
        }

        public List<Employee>? SearchEmployee(SearchModel searchModel)
        {
            try
            {
                var employees = _employeeRepository.GetAll();
                employees = SearchById(employees, searchModel.Id);
                employees = SearchByName(employees, searchModel.Name);
                employees = SeachByAge(employees, searchModel.Age);
                employees = SearchBySalary(employees, searchModel.Salary);
                if(employees != null && employees.Count > 0)
                    return employees.ToList(); ;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        private ICollection<Employee> SearchBySalary(ICollection<Employee> employees, Range<double>? salary)
        {
            if(salary == null || employees.Count ==0 || employees==null)
            {
                return employees;
            }
            else
            {
                return employees.Where(e => e.Salary >= salary.MinVal && e.Salary <= salary.MaxVal).ToList();
            }
        }

        private ICollection<Employee> SeachByAge(ICollection<Employee> employees, Range<int>? age)
        {
            if(age == null || employees.Count == 0 || employees == null)
            {
                return employees;
            }
            else
            {
                return employees.Where(e => e.Age >= age.MinVal && e.Age <= age.MaxVal).ToList();
            }
        }

        private ICollection<Employee> SearchByName(ICollection<Employee> employees, string? name)
        {
            if(name == null ||  employees == null || employees.Count == 0 )
            {
                return employees;
            }
            else
            {
                return employees.Where(e => e.Name.ToLower().Contains(name.ToLower())).ToList();
            }
        }

        private ICollection<Employee> SearchById(ICollection<Employee> employees, int? id)
        {
            if (id == null || employees == null || employees.Count == 0)
            {
                return employees;
            }
            else
            {
                return employees.Where(e => e.Id == id).ToList();
            }
        }
    }
}
-----------------------------------------------------
using WholeApplication.Interfaces;
using WholeApplication.Models;
using WholeApplication.Repositories;
using WholeApplication.Services;

namespace WholeApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            IRepositor<int, Employee> employeeRepository = new EmployeeRepository();
            IEmployeeService employeeService = new EmployeeService(employeeRepository);
            ManageEmployee manageEmployee = new ManageEmployee(employeeService);
            manageEmployee.Start();
        }
    }
}
----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeApplication.Interfaces;
using WholeApplication.Models;

namespace WholeApplication.Services
{
    public class EmployeeService : IEmployeeService
    {
        IRepositor<int, Employee> _employeeRepository;
        public EmployeeService(IRepositor<int, Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public int AddEmployee(Employee employee)
        {
            try
            {
                var result = _employeeRepository.Add(employee);
                if (result != null)
                {
                    return result.Id;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return -1;
        }

        public List<Employee>? SearchEmployee(SearchModel searchModel)
        {
            try
            {
                var employees = _employeeRepository.GetAll();
                employees = SearchById(employees, searchModel.Id);
                employees = SearchByName(employees, searchModel.Name);
                employees = SeachByAge(employees, searchModel.Age);
                employees = SearchBySalary(employees, searchModel.Salary);
                if(employees != null && employees.Count > 0)
                    return employees.ToList(); ;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        private ICollection<Employee> SearchBySalary(ICollection<Employee> employees, Range<double>? salary)
        {
            if(salary == null || employees.Count ==0 || employees==null)
            {
                return employees;
            }
            else
            {
                return employees.Where(e => e.Salary >= salary.MinVal && e.Salary <= salary.MaxVal).ToList();
            }
        }

        private ICollection<Employee> SeachByAge(ICollection<Employee> employees, Range<int>? age)
        {
            if(age == null || employees.Count == 0 || employees == null)
            {
                return employees;
            }
            else
            {
                return employees.Where(e => e.Age >= age.MinVal && e.Age <= age.MaxVal).ToList();
            }
        }

        private ICollection<Employee> SearchByName(ICollection<Employee> employees, string? name)
        {
            if(name == null || employees.Count == 0 || employees == null)
            {
                return employees;
            }
            else
            {
                return employees.Where(e => e.Name.ToLower().Contains(name.ToLower())).ToList();
            }
        }

        private ICollection<Employee> SearchById(ICollection<Employee> employees, int? id)
        {
            if (id == null || employees.Count == 0 || employees == null)
            {
                return employees;
            }
            else
            {
                return employees.Where(e => e.Id == id).ToL	ist();
            }
        }
    }
}

---------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholeApplication.Models
{
    public class SearchModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public Range<int>? Age { get; set; }
        public Range<double>? Salary { get; set; }
    }
    public class Range<T>
    {
        public T? MinVal { get; set; }
        public T? MaxVal { get; set; }
    }
}
---------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeApplication.Models;

namespace WholeApplication.Interfaces
{
    public interface IEmployeeService
    {
        int AddEmployee(Employee employee);
        List<Employee>? SearchEmployee(SearchModel searchModel);
    }
}
-----------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholeApplication.Exceptions
{
    public class CollectionEmptyException :Exception
    {
        private string _message = "Collection is empty";
        public CollectionEmptyException(string msg)
        {
            _message = msg;
        }
        public override string Message => _message;
    }
}

-------------------------------------------------------------


 



using System.Threading.Tasks;

namespace WholeApplication.Interfaces
{	
    public interface IRepositor<K,T> where T : class
    {

        T Add(T item);
        T Update(T item);
        T Delete(K id);
        T GetById(K id);
        ICollection<T> GetAll();
    }
}
-------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholeApplication.Exceptions
{
    public class DuplicateEntityException : Exception
    {
        private string _message = "Duplicate entity found";
        public DuplicateEntityException(string msg)
        {
            _message = msg;
        }
        public override string Message => _message;
    }
}

--------------------------------------------------------------------
using WholeApplication.Exceptions;
using WholeApplication.Interfaces;

namespace WholeApplication.Repositories
{
    public abstract class Repository<K,T> :IRepositor<K, T> where T : class
    {
        protected List<T> _items = new List<T>();
        protected abstract K GenerateID();
        public abstract ICollection<T> GetAll();
        public abstract T GetById(K id);

        public T Add(T item)
        {
           var id = GenerateID();
            var property = typeof(T).GetProperty("Id");
            if (property != null)
            {
                property.SetValue(item, id);
            }
            // Check if the item already exists in the list
            if (_items.Contains(item))
            {
                throw new DuplicateEntityException("Employee already exists");
            }
            _items.Add(item);
            return item;
        }

        public T Delete(K id)
        {
            var item = GetById(id);
            if (item == null)
            {
                throw new KeyNotFoundException("Item not found");
            }
            _items.Remove(item);
            return item;
        }

        public T Update(T item)
        {
            var myItem = GetById((K)item.GetType().GetProperty("Id").GetValue(item));
            if (myItem == null)
            {
                throw new KeyNotFoundException("Item not found");
            }
            var index = _items.IndexOf(myItem);
            _items[index] = item;
            return item;
        }
    }

}






-----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholeApplication.Models
{
    public class Employee : IComparable<Employee>, IEquatable<Employee>
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }
        public double Salary { get; set; }

        public Employee()
        {
            Name = string.Empty;
        }

        public Employee(int id, int age, string name, double salary) 
        {
            Id = id;
            Age = age;
            Name = name;
            Salary = salary;
        }

        public void TakeEmployeeDetailsFromUser()
        {
            Console.WriteLine("Please enter the employee ID");
            int id;
            while(!int.TryParse(Console.ReadLine(), out id) || id <= 0)
            {
                Console.WriteLine("Invalid entry for ID. Please enter a valid employee ID");
            }
            Id = id;
            Console.WriteLine("Please enter the employee name");
            Name = Console.ReadLine()??"";
            Console.WriteLine("Please enter the employee age");
            int age;
            while (!int.TryParse(Console.ReadLine(), out age) || age <= 18)
            {
                Console.WriteLine("Invalid entry for age. Please enter a valid employee age");
            }
            Age = age;
            Console.WriteLine("Please enter the employee salary");
            float salary ;
            while (!float.TryParse(Console.ReadLine(), out salary) || salary <= 0)
            {
                Console.WriteLine("Invalid entry for salary. Please enter a valid employee salary");
            }
            Salary = salary;
        }

        public override string ToString()
        {
            return "Employee ID : " + Id + "\nName : " + Name + "\nAge : " + Age + "\nSalary : " + Salary;
        }

        public int CompareTo(Employee? other)
        {
            return this.Id.CompareTo(other?.Id);
        }

        public bool Equals(Employee? other)
        {
           return this.Id == other?.Id;
        }
    }


}

----------------------------------------
using WholeApplication.Models;

namespace WholeApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
           Employee employee = new Employee(101,22,"Ramu",123456); ;
            Console.WriteLine(employee);
        }
    }
}