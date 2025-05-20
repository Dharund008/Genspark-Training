// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;

/*
1) Design a C# console app that uses a jagged array to store data for Instagram posts by multiple users. 
Each user can have a different number of posts, 
and each post stores a caption and number of likes.

You have N users, and each user can have M posts (varies per user).

Each post has:

A caption (string)

A number of likes (int)

Store this in a jagged array, where each index represents one user's list of posts.

Display all posts grouped by user.

No file/database needed — console input/output only.

Example output
Enter number of users: 2

User 1: How many posts? 2
Enter caption for post 1: Sunset at beach
Enter likes: 150
Enter caption for post 2: Coffee time
Enter likes: 89

User 2: How many posts? 1
Enter caption for post 1: Hiking adventure
Enter likes: 230

--- Displaying Instagram Posts ---
User 1:
Post 1 - Caption: Sunset at beach | Likes: 150
Post 2 - Caption: Coffee time | Likes: 89

User 2:
Post 1 - Caption: Hiking adventure | Likes: 230


Test case
| User | Number of Posts | Post Captions        | Likes      |
| ---- | --------------- | -------------------- | ---------- |
| 1    | 2               | "Lunch", "Road Trip" | 40, 120    |
| 2    | 1               | "Workout"            | 75         |
| 3    | 3               | "Book", "Tea", "Cat" | 30, 15, 60 |

*/
class Instagram
{
    public string caption;
    public int likes;
}
class Program
{
    public static void Main(string[] args)
    {
        instagrampost();
        //jagged array;
        // int[][] jagged = JaggedArray();
        // display(jagged);

    }
    public static void instagrampost()
    {
        //jaggedarray
        Console.Write("Please,Enter how many users: ");
        int n = getnumbers();
        Instagram[][] user = new Instagram[n][]; //to store posts for different users(space- with null)

        for (int i = 0; i < n; i++)
        {
            Console.WriteLine($"Enter the total number of posts for user {i + 1}:");
            int cols = getnumbers();

            user[i] = new Instagram[cols];//creates with null refernce(just space allocated)

            for (int j = 0; j < cols; j++)
            {
                user[i][j] = new Instagram();//creates an objects for each user 

                Console.Write($"Enter the caption for the post{j + 1}: ");
                user[i][j].caption = Console.ReadLine();

                Console.Write($"Enter the likes of the post: ");
                user[i][j].likes = getnumbers();
            }
        }

        Console.WriteLine("------Display Instagram Posts-------");
        for (int i = 0; i < n; i++)
        {
            Console.WriteLine($"User {i + 1} :");
            for (int j = 0; j < user[i].Length; j++)
            {
                Console.WriteLine($"Post {j + 1} - Caption: {user[i][j].caption} | Likes: {user[i][j].likes}");
            }
            Console.WriteLine();
        }
    }
    public static int[][] JaggedArray()
    {
        Console.Write("please,give the number of rows: ");
        int rows = getnumbers();

        int[][] jagged = new int[rows][];

        for (int i = 0; i < rows; i++)
        {
            Console.WriteLine($"Enter the total elements of row {i + 1}:");
            int cols = getnumbers();

            jagged[i] = new int[cols];
            Console.WriteLine($"Enter the {cols} elements in row {i + 1}!");
            for (int j = 0; j < cols; j++)
            {
                Console.Write($"Elements [{i}][{j}]: ");
                jagged[i][j] = getnumbers();
            }
        }
        return jagged;
    }
    public static void display(int[][] jagged)
    {
        Console.WriteLine("\nJagged array contents:");
        for (int i = 0; i < jagged.Length; i++)
        {
            Console.Write($"Row {i + 1}: ");
            for (int j = 0; j < jagged[i].Length; j++)
            {
                Console.Write(jagged[i][j] + " ");
            }
            Console.WriteLine();
        }
    }
    public static int getnumbers()
    {
        while (true)
        {
            Console.Write("\nplease, enter the number: ");
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