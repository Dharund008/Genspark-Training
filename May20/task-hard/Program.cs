// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Linq;

/*
1)Use the application created in Question 1 of medium.  
Display a menu to user which will enable to print all the employee details, add an employee, 
    modify the details of an employee (all except id), print an employee details given his id and 
    delete an employee from the collection .
Ensure the application does not break at any point. Handles all the cases with proper response 
Example – If user enters an employee id that does not exists the response should inform the user the same. 
*/
class Employee
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

    public void ModifyDetails()
    {
        Console.Write("please,enter new name: ");
        name = GetUserInput();

        Console.Write("please,enter new age: ");
        age = Convert.ToInt32(Console.ReadLine());

        Console.Write("please,enter new salary: ");
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
            Console.WriteLine("4. Modify Employee Details");
            Console.WriteLine("5. Delete Employee");
            Console.WriteLine("6. Exit");
            Console.Write("Enter your choice (1-6): ");

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
                    Console.WriteLine("please give the employeeID to modify!");
                    int n = getnumbers();
                    if (dict.TryGetValue(n, out Employee targetID))
                    {
                        Console.WriteLine("Now you can enter the modifying details (ID can't be changed)");
                        targetID.ModifyDetails();
                        Console.WriteLine("Employee details updated.");
                    }
                    else
                    {
                        Console.WriteLine("Employee not found!..");
                    }
                    break;

                case "5":
                    Console.WriteLine("please enter the employee ID to get removed!");
                    int m = getnumbers();
                    if (dict.Remove(m))
                    {
                        Console.WriteLine("Employee deleted successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Employee not found!..ID not found!");
                    }
                    break;

                case "6":
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
}
