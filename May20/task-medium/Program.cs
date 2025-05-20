// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Linq;

/*
1)Create an application that will take employee details (Use the employee class) and store it in a collection  
The collection should be able to give back the employee object if the employee id is provided. 

Hint – Use a collection that will store key-value pair. 
The ID of employee can never be null or have duplicate values. 


2)Use the application created for question 1. Store all the elements in the collection in a list. 
a)Sort the employees based on their salary.  
Hint – Implement the IComparable interface in the Employee class. 

b) Given an employee id find the employee and print the details. 
Hint – Use a LINQ with a where clause. 


3)Use the application created for question 2. 
Find all the employees with the given name (Name to be taken from user) 

4)Use the application created for question 3. 
Find all the employees who are elder than a given employee (Employee given by user) 
*/

class Employee : IComparable<Employee>
{
    int id, age;
    string name;
    double salary;

    public Employee()
    {

    }
    public Employee(int id, int age, string name, double salary)
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

    public int CompareTo(Employee other)
    {
        if (this.Salary < other.Salary)
        {
            return -1;
        }
        else if (this.Salary > other.Salary)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public int Id { get => id; set => id = value; }
    public int Age { get => age; set => age = value; }
    public string Name { get => name; set => name = value; }
    public double Salary { get => salary; set => salary = value; }
}




class Program
{
    public static void Main(string[] args)
    {
        Dictionary<int, Employee> dict = new Dictionary<int, Employee>();
        while (true)
        {
            Console.WriteLine("\n--- Employee Promotion Menu ---");
            Console.WriteLine("1. Add employee names");
            Console.WriteLine("2. Search Employee by ID");
            Console.WriteLine("3. Display Employee Details");
            Console.WriteLine("4. Sort Employee based on salary?(list)");
            Console.WriteLine("5. Search Employee using LINQ");
            Console.WriteLine("6. Search Employee using Name?");
            Console.WriteLine("7. Find employees older than a given employee?");
            Console.WriteLine("8. Exit");
            Console.Write("Enter your choice (1-8): ");

            string choice = Console.ReadLine().Trim();

            switch (choice)
            {
                case "1":
                    Employee emp = new Employee();
                    emp.TakeEmployeeDetailsFromUser();
                    if (!dict.ContainsKey(emp.Id))
                    {
                        dict.Add(emp.Id, emp);
                        Console.WriteLine("Employees Added Successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Error!..ID already exists!");
                    }
                    break;

                case "2":
                    Console.Write("please, enter the employee ID to search: ");
                    int s = getnumbers();
                    if (dict.ContainsKey(s))
                    {
                        Console.WriteLine("Credentials are valid!...");
                        Console.WriteLine(dict[s]);
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID.Employee Details not found!");
                    }
                    break;

                case "3":
                    Console.WriteLine("----Employee Details");
                    if (dict.Count == 0)
                    {
                        Console.WriteLine("No employess available! Try to add employee details");
                    }
                    else
                    {
                        foreach (var i in dict.Values)
                        {
                            Console.WriteLine(i);
                        }
                    }
                    break;

                case "4":
                    var emplist = dict.Values.ToList();
                    emplist.Sort();
                    foreach (var i in emplist)
                    {
                        Console.WriteLine(i);
                    }
                    break;

                case "5":
                    var searchList = dict.Values.ToList();
                    Console.WriteLine("Searching Employee using LINQ....");
                    int p = getnumbers();
                    var res = searchList.Where(i => i.Id == p).FirstOrDefault();
                    if (res != null)
                    {
                        Console.WriteLine(res);
                    }
                    else
                    {
                        Console.WriteLine("No ID found!");
                    }
                    break;

                case "6":
                    Console.WriteLine("----Searching with name------");
                    Console.WriteLine("\nPlease enter the name to be searched");
                    string input = GetUserInput();
                    var foundList = dict.Values.Where(i => i.Name.ToLower() == input.ToLower()).ToList();
                    if (foundList.Count > 0)
                    {
                        foreach (var e in foundList)
                        {
                            Console.WriteLine(e);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No employee found with that name.");
                    }
                    break;

                case "7":
                    Console.Write("please, enter the employee ID: ");
                    int n = getnumbers();
                    if (!dict.TryGetValue(n, out Employee targetID))
                    {
                        Console.WriteLine("Invalid ID!..please give proper ID");
                        break;
                    }
                    var obj = dict.Values.Where(i => i.Age > targetID.Age).ToList();
                    if (obj.Count > 0)
                    {
                        Console.WriteLine($"Employess older than {targetID.Name} , Age: {targetID.Age}");
                        foreach (var e in obj)
                        {
                            Console.WriteLine(e);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No employees are older!");
                    }
                    break;

                case "8":
                    Console.WriteLine("Exiting...... GoodBye!");
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please choose a valid option.");
                    break;
            }
        }
    }
    public static int getnumbers()
    {
        while (true)
        {
            string input = Console.ReadLine();
            if (int.TryParse(input, out int number))
            {
                return number;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid integer.");
            }
        }
    }
    public static string GetUserInput()
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
}
