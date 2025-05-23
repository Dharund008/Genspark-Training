using System;
using System.Collections.Generic;

namespace DIP.solid
{
    public class DIPEmployee : IEmployee
    {
        public string Name { get; set; }
        public string Department { get; set; }

        public string GetEmployeeDetails()
        {
            return $"Employee: {Name}, Department: {Department}";
        }
    }
}