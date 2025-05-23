// See https://aka.ms/new-console-template for more information
using System;
using SolidPrinciples;
using OCP.solid;
using LSP.solid;
using ISP.Interfaces;
using ISP.Employee;
using DIP.solid;

/*
A class should have only one responsibilities/function/method.
Or else it violates SRP
*/

namespace MyApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n--- Solid Principles Menu ---");
                Console.WriteLine("1. Single Responsiblity Principal");
                Console.WriteLine("2. Open/Closed Principle");
                Console.WriteLine("3. Liskov Substitution Principle");
                Console.WriteLine("4. Integrated Segration Principle");
                Console.WriteLine("5. Dependency Inversion Principle");
                Console.WriteLine("6. Exit");
                Console.Write("Enter your choice (1-6): ");

                string choice = Console.ReadLine().Trim();

                switch (choice)
                {
                    case "1":
                        EmployeeInput handler = new EmployeeInput();
                        Employee emp = handler.GetEmployeeFromUser();

                        Console.WriteLine("\nEmployee Details:");
                        Console.WriteLine(emp);
                        break;

                    case "2":
                        Console.WriteLine("Enter Loan Type(Home/Personal):");
                        string loanType = Console.ReadLine().Trim().ToLower();

                        Console.WriteLine("Enter Principal Amount:");
                        double principal = Convert.ToDouble(Console.ReadLine());

                        IInterestCalculator cal = null;
                        if (loanType == "home")
                        {
                            cal = new HomeLoan();
                        }
                        else if (loanType == "personal")
                        {
                            cal = new PersonalLoan();
                        }
                        else
                        {
                            Console.WriteLine("Unsupported loan type.");
                            return;
                        }

                        double interest = cal.CalculateInterest(principal);
                        Console.WriteLine($"Interest for {loanType} loan on {principal} is: {interest}");
                        break;

                    case "3":
                        Console.WriteLine("Employee's WOrking Hours");
                        Console.WriteLine("please give the employee type(parttime/fulltime/contract?)");
                        string input = Console.ReadLine().Trim().ToLower();
                        IWorkingHours part = null;
                        if (input == "fulltime")
                        {
                            part = new FulltimeEmployee();
                        }
                        else if (input == "contract")
                        {
                            part = new ContractEmployee();
                        }
                        else
                        {
                            Console.WriteLine("Unsupported employee type.");
                            return;
                        }
                        // IWorkingHours part = new ParttimeEmployee();
                        int hr = part.CalculateWorkingHours();
                        Console.WriteLine($"{input} empoloyee's working hours:- {hr}");
                        break;
                    
                    case "4":
                        Console.WriteLine("Choose employee type: developer / support / manager");
                        string type = Console.ReadLine().Trim().ToLower();
                        if (type == "developer")
                        {
                            IDeveloper dev = new Developer();
                            dev.WriteCode();
                        }
                        else if (type == "support")
                        {
                            ISupport sup = new Support();
                            sup.HandleSupport();
                        }
                        else if (type == "manager")
                        {
                            IManager manager = new Manager();
                            manager.Manages();
                        }
                        else
                        {
                            Console.WriteLine("Invalid employee type!..");
                            return;
                        }
                        break;
                    
                    case "5":

                        IEmployee dip = new DIPEmployee() { Name = "Dharun", Department = "IT" };

                        IReportGenerator reportGen = new ReportGenerator();

                        EmployeeReport empReport = new EmployeeReport(reportGen);
                        empReport.Generate(dip);
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
    }
}
