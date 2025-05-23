// See https://aka.ms/new-console-template for more information
using System;
using Delegates.file;
using Delegates.Models;
using ExtensionFunctions.Misc;

namespace WholeApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            // DelegateExample example = new DelegateExample(); //instance


            //ExtensionFunction exp = new ExtensionFunction(); //dont need to create instance, 
            //like linq attributes,defaultly doesnt takes any object creation here!
            string input = "Studen";
            bool result = input.StringValidationCheck();
            Console.WriteLine(result);
        }
    }
}
