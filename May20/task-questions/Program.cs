// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Linq;
/* 
https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic?view=net-9.0

Easy:-
1)Create a C# console application which has a class with name “EmployeePromotion” that will take employee
names in the order in which they are eligible for promotion.  
a)Example Input:  

Please enter the employee names in the order of their eligibility for promotion(Please enter blank to stop) 
Ramu 
Bimu 
Somu 
Gomu 
Vimu 

b)Create a collection that will hold the employee names in the same order that they are inserted. 
c)Hint – choose the correct collection that will preserve the input order (List) 


2)Use the application created for question 1 and in the same class do the following 

Given an employee name find his position in the promotion list 
Example Input:  
Please enter the employee names in the order of their eligibility for promotion 

Ramu
Bimu 
Somu 
Gomu 
Vimu 

Please enter the name of the employee to check promotion position 
Somu 
“Somu” is the the position 3 for promotion. 
Hint – Choose the correct method that will give back the index (IndexOf) 


3)Use the application created for question 1 and in the same class do the following 

The application seems to be using some excess memory for storing the name, 
contain the space by using only the quantity of memory that is required. 

Example Input:  
Please enter the employee names in the order of their eligibility for promotion 
Ramu 
Bimu 
Somu 
Gomu 
Vimu 

The current size of the collection is 8 
The size after removing the extra space is 5 
Hint – List multiples the memory when we add elements, 
ensure you use only the size that is equal to the number of elements that are present. 


4)Use the application created for question 1 and in the same class do the following 
The need for the list is over as all the employees are promoted. 
Not print all the employee names in ascending order. 

Example Input: 	 
Please enter the employee names in the order of their eligibility for promotion 
Ramu 
Bimu 
Somu 
Gomu 
Vimu 

Promoted employee list: 
Bimu 
Gomu 
Ramu 
Somu 
Vimu 
*/

class EmployeePromotion
{
    public string name { get; set; }

    private List<string> ls = new List<string>();
    public EmployeePromotion()
    {

    }

    public EmployeePromotion(string name)
    {
        this.name = name;
    }

    public void TakeInput()
    {
        while (true)
        {
            Console.WriteLine("please, enter the employee names in the order of their eligibility for promotion(press enter to stop!): ");
            name = Console.ReadLine().Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                break;
            }
            else
            {
                ls.Add(name);
            }
        }
    }

    public void CheckMemory()
    {
        Console.WriteLine("--------Memory Usage------");
        Console.WriteLine($"The current size of the collection is {ls.Capacity}");//gets double of number of inputs
        if (ls.Capacity != ls.Count)
        {
            ls.TrimExcess();
        }
        Console.WriteLine($"The size after removing the extra space is {ls.Capacity}");
    }

    public void CheckPromotion()
    {
        if (ls == null || ls.Count == 0)
        {
            Console.WriteLine("\nThe promotion list is empty. No employees available for promotion.");
            return;
        }

        if (ls.Count == 1)
        {
            Console.WriteLine("\nThere is only one employee in promotion list!");
            Console.WriteLine($"{ls[0]} is the only employee for promotion!");
            return;
        }
        Console.WriteLine("\nPlease enter the name of the employee to check promotion position: ");
        string input = GetUserInput();

        if (ls.Contains(input))
        {
            int index = ls.IndexOf(input) + 1; //zero-based-index
            Console.WriteLine($"\n {input} is at position  {index} for promotion!");
        }
        else
        {
            Console.WriteLine($"\n{input} is not found in the promotion list.");
        }
    }

    public void PromotedList()
    {

        ls.Sort(); //ascending order
        Console.WriteLine("Promoted employee list: ");
        foreach (string emp in ls)
        {
            Console.WriteLine(emp);
        }

    }

    public string GetUserInput()
    {
        while (true)
        {
            Console.WriteLine("please, Enter the valid input!");
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

    public void display()
    {
        if (ls == null || ls.Count == 0)
        {
            Console.WriteLine("\nThe list is empty. No employees available!.");
            return; 
        }

        Console.WriteLine("\n----List of employees Eligible for promotion in order:");
        foreach (string emp in ls)
        {
            Console.WriteLine(emp);
        }
    }
}
class Program
{

    public static void Main(string[] args)
    {
        EmployeePromotion emp = new EmployeePromotion();
        while (true)
        {
            Console.WriteLine("\n--- Employee Promotion Menu ---");
            Console.WriteLine("1. Add employee names");
            Console.WriteLine("2. Display all eligible employees");
            Console.WriteLine("3. Check promotion position");
            Console.WriteLine("4. Optimize memory");
            Console.WriteLine("5. Promoted List");
            Console.WriteLine("6. Exit");
            Console.Write("Enter your choice (1-6): ");

            string choice = Console.ReadLine().Trim();

            switch (choice)
            {
                case "1":
                    emp.TakeInput();
                    Console.WriteLine("Employees Added Successfully!");
                    break;

                case "2":
                    emp.display();
                    break;

                case "3":
                    emp.CheckPromotion();
                    break;

                case "4":
                    emp.CheckMemory();
                    break;

                case "5":
                    emp.PromotedList();
                    break;

                case "6":
                    Console.WriteLine("Exiting...... GoodBye!");
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please choose a valid option.");
                    break;
            }
        }
        // emp.TakeInput();
        // emp.display();
        // emp.CheckPromotion();
        

    } 
}


