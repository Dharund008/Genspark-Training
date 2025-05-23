using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delegates.Models;

namespace Delegates.file
{
    internal class DelegateExample
    {
        List<Employee> employees = new List<Employee>()
        {
            new Employee(101,30, "John Doe",  50000),
            new Employee(102, 25,"Jane Smith",  60000),
            new Employee(103,35, "Sam Brown",  70000)
        };
        public delegate void MyDelegate(int num1, int num2);
        //generic:-
        //public delegate void MyDelegate<T>(T num1,T num2);

        //public delegate void MyFDelegate(float num1,float num2); 
        public void Add(int n1, int n2)
        {
            int sum = n1 + n2;
            Console.WriteLine($"The sum of {n1} and {n2} is {sum}");
        }
        public void Product(int n1, int n2)
        {
            int prod = n1 * n2;
            Console.WriteLine($"The sum of {n1} and {n2} is {prod}");
        }
        public DelegateExample()
        {
            // MyDelegate del = new MyDelegate(Add); //uncomment this doe running delegates

            //if generic :- MyDelegate<int> del = new MyDelegate<int>(Add);

            Action<int, int> del = Add; //predefined delegate :- Action
            del += Product;
            //del += delegate (int n1, int n2) //lambda!!!
            //{
            //    Console.WriteLine($"The division result of {n1} and {n2} is {n1 / n2}");
            //};
            del += (int n1, int n2) => Console.WriteLine($"The division result of {n1} and {n2} is {n1 / n2}");
            del(100, 20);

            FindEmployee();
            SortEmployee();
        }    
        void FindEmployee()
        {
            int empId = 102;
            Predicate<Employee> predicate = e => e.Id == empId;
            Employee? emp = employees.Find(predicate);
            Console.WriteLine(emp.ToString()??"No such employee");
        }
        void SortEmployee()
        {
            var sortedEmployees = employees.OrderBy(e => e.Name);
            foreach (var emp in sortedEmployees)
            {
                Console.WriteLine(emp.ToString());
            }
        }
        
    }
}
