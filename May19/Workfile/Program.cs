// 1) create a program that will take name from user and greet the user:

using System;
using System.Collections.Generic;

public class HelloWorld
{
    public static void Main(string[] args)
    {
        Console.Write("Enter the name:");
        String name = Console.ReadLine().Trim();

        Console.WriteLine($"Hello {name}!"); //interpolation
        //isNullorWhiteSpace//trim
    }
}


//2)Take two numbers from the user and print the largest 


public class Program
{
    public static void Main(string[] args)
    {
        Console.Write("Enter first number: ");
        string input1 = Console.ReadLine();
        Console.Write("Enter second number: ");
        string input2 = Console.ReadLine();

        if (int.TryParse(input1, out int number1) && int.TryParse(input2, out int number2))
        {
            if (number1 > number2)
            {
                Console.WriteLine($"{number1} is the largest");
            }
            else if (number2 > number1)
            {
                Console.WriteLine($"{number2} is the largest");
            }
            else
            {
                Console.WriteLine("Both are equal!");
            }
        }
        else
        {
            Console.WriteLine("Enter an valid input.");
        }
    }

}

//3) Take 2 numbers from user, check the operation user wants to perform
// (+, -, *,/). Do the operation and print the result


class Program
{
    public static void Main(string[] args)
    {
        int num1 = getnumbers();
        int num2 = getnumbers();

        while (true)
        {
            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1.Add the numbers");
            Console.WriteLine("2.Subtract the numbers");
            Console.WriteLine("3.Multiply the numbers");
            Console.WriteLine("4.Divide the numbers");
            Console.WriteLine("5.Exit");

            Console.Write("Enter your Choice:");
            string choice = Console.ReadLine().Trim();

            if (choice == "1")
            {
                int result = num1 + num2;
                Console.WriteLine($"After adding, the result is {result}");
            }
            else if (choice == "2")
            {
                int result = num1 - num2;
                Console.WriteLine($"After subtracting, the result is {result}");
            }
            else if (choice == "3")
            {
                int result = num1 * num2;
                Console.WriteLine($"After multiplying, the result is {result}");
            }
            else if (choice == "4")
            {
                if (num2 != 0)
                {
                    int result = num1 / num2;
                    Console.WriteLine($"After dividing, the result is {result}");
                }
                else
                {
                    Console.WriteLine("Cannot divide by zero!");
                }
            }
            else if (choice == "5")
            {
                Console.WriteLine("Exiting......");
                break;
            }
            else
            {
                Console.WriteLine("Enter valid choice!");
            }
        }
    }
    public static int getnumbers()
    {
        while (true)
        {
            Console.Write("Enter a valid number: ");
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



/*
4) Take username and password from user. Check if user name is "Admin" and password is "pass" 
if yes then print success message.
Give 3 attempts to user. In the end of eh 3rd attempt if user still is unable to provide 
valid creds then exit the application after print the message 
"Invalid attempts for 3 times. Exiting...."
*/

class Program
{
    public static void Main(string[] args)
    {
        int i = 3;
        while (i > 0)
        {
            Console.WriteLine($"\nYou have totally {i} attemps left ");   

            Console.Write("Give the valid username: ");
            string username = Console.ReadLine();
            Console.Write("Give the valid password: ");
            string password = Console.ReadLine();
            if ((username.ToLower() == "admin" && password.ToLower() == "pass"))
            {
                Console.WriteLine("Success");
                return;
            }
            else
            {
                Console.WriteLine("Invalid credentials!");
                i--;
            }
        }
        Console.WriteLine("\n Invalid attempts for 3 times. Exiting....");
    }
}



//5) Take 10 numbers from user and print the number of numbers that are divisible by 7:


class Divisbleby7
{
    public static void Main(string[] args)
    {
        int n = 1;
        int total = 0;
        Console.WriteLine("Numbers divisble by 7:");
        while (n <= 10)
        {
            int num = getnumbers();
            if (num % 7 == 0)
            {
                Console.WriteLine($"{n}.) {num} is divisible!");
                total++;
            }
            n++;
        }
        Console.WriteLine($"Total numbers divisible by 7: {total}");
    }
    public static int getnumbers()
    {
        while (true)
        {
            Console.Write("Enter a valid number: ");
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



/*
6) Count the Frequency of Each Element
Given an array, count the frequency of each element and print the result.
Input: {1, 2, 2, 3, 4, 4, 4}
*/


class Program
{
    public static void Main(String[] args)
    {
        Console.WriteLine("Enter the Array_Size ");
        int n = getnumbers();
        int[] numbers = new int[n];
        for (int i = 0; i < n; i++)
        {
            int input = getnumbers();
            numbers[i] = input;
        }

        Dictionary<int, int> frequency = new Dictionary<int, int>();

        foreach (int num in numbers)
        {
            if (frequency.ContainsKey(num))
            {
                frequency[num]++;
            }
            else
            {
                frequency[num] = 1;
            }
        }

        Console.WriteLine("\nFrequency of each element: ");
        foreach (var num in frequency)
        {
            Console.WriteLine($"{num.Key} occurs {num.Value} times");
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
                Console.WriteLine("Invalid input. Please enter a valid integer again.");
            }
        }
    }
}

/*
7) create a program to rotate the array to the left by one position.
Input: {10, 20, 30, 40, 50}
Output: {20, 30, 40, 50, 10}
*/


class program
{
    public static void Main(String[] args)
    {
        Console.WriteLine("Enter the Array_Size ");
        int n = getnumbers();
        int[] numbers = new int[n];
        Console.WriteLine($"Enter {n} numbers:");
        for (int i = 0; i < n; i++)
        {
            int input = getnumbers();
            numbers[i] = input;
        }
        Console.WriteLine("please give the number of positions to rotate array!");
        int pos = getnumbers();
        rotate(numbers, n, pos);
        Console.WriteLine("Array after left rotation:");
        foreach (int num in numbers)
        {
            Console.Write(num + " ");
        }
    }
    public static void rotate(int[] numbers, int n, int pos)
    {
        pos = pos % n;
        for (int i = 0; i < pos; i++)
        {
            int temp = numbers[0];
            for (int j = 0; j < n - 1; j++)
            {
                numbers[j] = numbers[j + 1];
            }
            numbers[n - 1] = temp;
        }
    }
    public static int getnumbers()
    {
        while (true)
        {
            string input = Console.ReadLine();
            if (int.TryParse(input, out int number) && number > 0)
            {
                return number;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid integer again.");
            }
        }
    }
}


/*
8) Given two integer arrays, merge them into a single array.
Input: {1, 3, 5} and {2, 4, 6}
Output: {1, 3, 5, 2, 4, 6}
*/


class Program
{
    public static void Main(string[] args)
    {
        Console.Write("please, Enter first array size!: ");
        int m = getnumbers();
        int[] arr1 = new int[m];

        Console.WriteLine("Enter elements for the first array:");
        for (int i = 0; i < m; i++)
        {
            arr1[i] = getnumbers();
        }

        Console.Write("please, Enter second array size!: ");
        int n = getnumbers();
        int[] arr2 = new int[n];

        Console.WriteLine("Enter elements for the second array:");
        for (int i = 0; i < n; i++)
        {
            arr2[i] = getnumbers();
        }
        if (arr1 == null || arr2 == null)
        {
            Console.WriteLine("One of the arrays is null. Cannot merge.");
            return;
        }

        int[] merged = MergeArray(arr1, arr2, m, n);

        Console.WriteLine("Merged Array:");
        foreach (int num in merged)
        {
            Console.Write(num + " ");
        }
        Console.WriteLine();
    }

    public static int[] MergeArray(int[] arr1, int[] arr2, int m, int n)
    {
        int[] result = new int[m + n];
        for (int i = 0; i < m; i++)
        {
            result[i] = arr1[i];
        }
        for (int i = 0; i < n; i++)
        {
            result[m + i] = arr2[i];
        }
        return result;
    }
    public static int getnumbers()
    {
        while (true)
        {
            string input = Console.ReadLine();
            if (int.TryParse(input, out int number) && number > 0)
            {
                return number;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid integer again.");
            }
        }
    }
}


/*
9) Write a program that:

Has a predefined secret word (e.g., "GAME").

Accepts user input as a 4-letter word guess.

Compares the guess to the secret word and outputs:

X Bulls: number of letters in the correct position.

Y Cows: number of correct letters in the wrong position.

Continues until the user gets 4 Bulls (i.e., correct guess).

Displays the number of attempts.

Bull = Correct letter in correct position.

Cow = Correct letter in wrong position.

Secret Word	User Guess	Output	Explanation
GAME	GAME	4 Bulls, 0 Cows	Exact match
GAME	MAGE	1 Bull, 3 Cows	A in correct position, MGE misplaced
GAME	GUYS	1 Bull, 0 Cows	G in correct place, rest wrong
GAME	AMGE	2 Bulls, 2 Cows	A, E right; M, G misplaced
NOTE	TONE	2 Bulls, 2 Cows	O, E right; T, N misplaced

*/


class Program
{
    public static void Main(string[] args)
    {
        string secret = "GAME";
        SecretWordGuess(secret);
    }
    public static void SecretWordGuess(string secret)
    {
        int attempts = 0;
        while (true)
        {
            string input = GetUserInput();
            attempts++;
            string upper_secret = secret.ToUpper();
            string upper_guess = input.ToUpper();
            if (upper_secret == upper_guess)
            {
                Console.WriteLine("Yos guessed it!, 4-Bulls, 0-Cows!");
                Console.WriteLine($"You have taken {attempts} attempts to guess it");
                break;
            }
            int bull = 0;
            int cow = 0;

            bool[] secret_letter = new bool[input.Length];
            bool[] guess_letter = new bool[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                if (upper_secret[i] == upper_guess[i])
                {
                    bull++;
                    secret_letter[i] = true;
                    guess_letter[i] = true;
                }
            }

            for (int i = 0; i < input.Length; i++)
            {
                if (guess_letter[i]) continue; //if its bull

                for (int j = 0; j < input.Length; j++)
                {
                    if (!secret_letter[j] && upper_guess[i] == upper_secret[j])
                    {
                        cow++;
                        secret_letter[j] = true;
                        break;
                    }
                }
            }
            Console.WriteLine($"Bulls:- {bull}, Cows:- {cow}");

        }


    }
    public static string GetUserInput()
    {
        while (true)
        {
            Console.WriteLine("please, Enter the valid input!");
            string input = Console.ReadLine().Trim();
            if (!string.IsNullOrWhiteSpace(input) && input.Length == 4)
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



/*
10) write a program that accepts a 9-element array representing a Sudoku row.

Validates if the row:

Has all numbers from 1 to 9.

Has no duplicates.

Displays if the row is valid or invalid.
*/

class Program
{
    public static void Main(string[] args)
    {
        int[] arr = new int[9];
        Console.WriteLine("Enter elements for the array:");
        for (int i = 0; i < 9; i++)
        {
            arr[i] = getnumbers();
        }
        bool status = validate(arr);
        if (status)
        {
            Console.WriteLine("It's a valid sudoku row!");
        }
        else
        {
            Console.WriteLine("It's a Invalid sudoku row...");
        }
    }

    public static bool validate(int[] arr)
    {
        Dictionary<int, int> freq = new Dictionary<int, int>();
        foreach (int num in arr)
        {
            if (num < 1 || num > 9)
            {
                return false;
            }
            //var duplicate = new HashSet<int>(arr);
            //return duplicate.Count == 9;

            if (freq.ContainsKey(num))
            {
                freq[num]++;
                return false;
            }
            else
            {
                freq[num] = 1;
            }
        }
        return true;
    }

    public static int getnumbers()
    {
        while (true)
        {
            string input = Console.ReadLine();
            if (int.TryParse(input, out int number) && number > 0)
            {
                return number;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid integer again.");
            }
        }
    }

}


/*
11)  In the question ten extend it to validate a sudoku game. 
Validate all 9 rows (use int[,] board = new int[9,9])

*/

class Program
{
    public static void Main(string[] args)
    {
        int[,] sudoku = new int[9, 9];
        Console.WriteLine("Enter elements for the sudoku (9,9):");
        for (int r = 0; r < 9; r++)
        {
            Console.WriteLine($"please,Enter elements for row {r + 1}:");
            for (int c = 0; c < 9; c++)
            {
                sudoku[r, c] = getnumbers();
            }
        }
        bool status = SudokuGame(sudoku);
        if (status)
        {
            Console.WriteLine("This sudoku game is valid!");
        }
        else
        {
            Console.WriteLine("This sudoku game is invalid!");
        }
    }

    public static bool SudokuGame(int[,] sudoku)
    {
        int m = 0;
        for (int r = 0; r < 9; r++)
        {
            int[] row_arr = new int[9];
            for (int c = 0; c < 9; c++)
            {
                row_arr[c] = sudoku[r, c];
            }

            if (!validate(row_arr))
            {
                Console.WriteLine($"row {r + 1} is invalid ");
                return false;
            }
        }

        for (int c = 0; c < 9; c++)
        {
            int[] col_arr = new int[9];
            for (int r = 0; r < 9; r++)
            {
                col_arr[r] = sudoku[r, c];
            }

            if (!validate(col_arr))
            {
                Console.WriteLine($"column {c + 1} is invalid ");
                return false;
            }
        }

        for (int r = 0; r < 9; r += 3)
        {
            for (int c = 0; c < 9; c += 3)
            {
                int[] box_arr = new int[9];
                for (int i = r; i < r+3; i++)
                {
                    for (int j = c; j < c+3; j++)
                    {
                        box_arr[m++] = sudoku[i, j];
                    }
                }
                if (!validate(box_arr))
                {
                    Console.WriteLine($"subgrid from ({r + 1},{c + 1} is invalid! ");
                    return false;
                }
            }

        }
        return true;
    }
    public static bool validate(int[] arr)
    {
        Dictionary<int, int> freq = new Dictionary<int, int>();
        foreach (int num in arr)
        {
            if (num < 1 || num > 9)
            {
                return false;
            }
            //var duplicate = new HashSet<int>(arr);
            //return duplicate.Count == 9;

            if (freq.ContainsKey(num))
            {
                freq[num]++;
                return false;
            }
            else
            {
                freq[num] = 1;
            }
        }
        return true;
    }

    public static int getnumbers()
    {
        while (true)
        {
            string input = Console.ReadLine();
            if (int.TryParse(input, out int number) && number > 0)
            {
                return number;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid integer again.");
            }
        }
    }
}


/*
12) Write a program that:

Takes a message string as input (only lowercase letters, no spaces or symbols).

Encrypts it by shifting each character forward by 3 places in the alphabet.

Decrypts it back to the original message by shifting backward by 3.

Handles wrap-around, e.g., 'z' becomes 'c'.

Examples
Input:     hello
Encrypted: khoor
Decrypted: hello
-------------
Input:     xyz
Encrypted: abc
Test cases
| Input | Shift | Encrypted | Decrypted |
| ----- | ----- | --------- | --------- |
| hello | 3     | khoor     | hello     |
| world | 3     | zruog     | world     |
| xyz   | 3     | abc       | xyz       |
| apple | 1     | bqqmf     | apple     |

*/

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("please, enter the word to encrypt");
        string input = getstring();
        int shift = getnumbers();
        string encrypted = encryption(input, shift);
        string decrypted = decryption(encrypted, shift);
        Console.WriteLine($"Encrypted value:- {encrypted}, Decrypted value :- {decrypted}");
    }
    public static string encryption(string input, int shift)
    {
        char[] result = new char[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];
            if (char.IsLetter(c))
            {
                char encrypt = (char)(((c - 'a' + shift) % 26) + 'a');
                result[i] = encrypt;
            }
            else
            {
                result[i] = c;
            }
        }
        return new string(result);
    }

    public static string decryption(string encryption, int shift)
    {
        char[] result = new char[encryption.Length];
        for (int i = 0; i < encryption.Length; i++)
        {
            char c = encryption[i];
            if (char.IsLetter(c))
            {
                char decrypt = (char)(((c - 'a' - shift) % 26) + 'a');
                result[i] = decrypt;
            }
            else
            {
                result[i] = c;
            }
        }
        return new string(result);
    }
    public static string getstring()
    {
        while (true)
        {
            string? input = Console.ReadLine().Trim().ToLower();
            if (!string.IsNullOrEmpty(input))
            {
                return input;
            }
            else
            {
                Console.WriteLine("Invalid input. please enter valid input!");
            }
        }
    }

    public static int getnumbers()
    {
        while (true)
        {
            Console.WriteLine("please, enter the number of position to shift");
            string input = Console.ReadLine();
            if (int.TryParse(input, out int number) && number > 0)
            {
                return number;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid integer again.");
            }
        }
    }
}