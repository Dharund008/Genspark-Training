using System;

namespace SolidPrinciples
{
    public class Employee
    {
         public int Id { get; set; }
        public int Age { get; set; }
        public string? Name { get; set; }
        public double Salary { get; set; }


        public override string ToString()
        {
            return $"Employee ID : {Id}\nName : {Name}\nAge : {Age}\nSalary : {Salary}";
        }
    }
}