using System;
using System.Collections.Generic;
using System.Linq;
/*
1)hold data
2)getting input
3)return backs formatted data
*/



class EmpManaging
{
    int id, age;
    string name;
    double salary;

    public EmpManaging()
    {

    }
    public EmpManaging(int id, int age, string name, double salary)
    {
        this.id = id;
        this.age = age;
        this.name = name;
        this.salary = salary;
    }
    public void TakeEmployeeDetailsFromUser()
    {
        Console.WriteLine("Please enter the employee ID");
        id = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Please enter the employee name");
        name = GetUserInput();
        Console.WriteLine("Please enter the employee age");
        age = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Please enter the employee salary");
        salary = Convert.ToDouble(Console.ReadLine());
    }
    public string GetUserInput()
    {
        while (true)
        {
            Console.Write("Name: ");
            string input = Console.ReadLine().Trim();
            if (!string.IsNullOrWhiteSpace(input) && !input.Any(char.IsDigit))
            {
                return input;
            }
            else
            {
                Console.WriteLine("Invalid input. Please a enter a valid input again");
            }
        }
    }
    public override string ToString()
    {
        return "Employee ID : " + id + "\nName : " + name + "\nAge : " + age + "\nSalary : " + salary;
    }

    // public int CompareTo(Employee other)
    // {
    //     if (this.Salary < other.Salary)
    //     {
    //         return -1;
    //     }
    //     else if (this.Salary > other.Salary)
    //     {
    //         return 1;
    //     }
    //     else
    //     {
    //         return 0;
    //     }
    // }

    public int Id { get => id; set => id = value; }
    public int Age { get => age; set => age = value; }
    public string Name { get => name; set => name = value; }
    public double Salary { get => salary; set => salary = value; }
}


