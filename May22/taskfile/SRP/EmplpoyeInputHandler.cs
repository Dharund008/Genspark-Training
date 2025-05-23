using System;
using System.Collections.Generic;
using System.Linq;

namespace SolidPrinciples
{
    public class EmployeeInput
    {
        public Employee GetEmployeeFromUser()
        {
            Employee emp = new Employee();

            Console.WriteLine("Please enter the employee ID:");
            emp.Id = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Please enter the employee name:");
            emp.Name = Console.ReadLine().Trim();

            Console.WriteLine("Please enter the employee age:");
            emp.Age = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Please enter the employee salary:");
            emp.Salary = Convert.ToDouble(Console.ReadLine());

            return emp;
        }
    }
}