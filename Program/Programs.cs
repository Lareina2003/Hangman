
using System;
using System.IO;
using HangmanMVCPro.Model;
using HangmanMVCPro.Controller;

namespace HangmanMVCPro
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("Select Option:");
                Console.WriteLine("1. Admin");
                Console.WriteLine("2. Player");
                Console.WriteLine("3. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AdminPanel();
                        break;
                    case "2":
                        StartGame();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid choice! Please try again.");
                        break;
                }

                Console.WriteLine();
            }
        }

        static void AdminPanel()
        {
            Console.WriteLine("--- Admin Panel ---");

            Console.Write("Enter the word: ");
            string word = Console.ReadLine();

            Console.Write("Enter the clue: ");
            string clue = Console.ReadLine();

            string filePath = "words.csv";

            // Append new word and clue to words.csv
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine($"{word},{clue}");
            }

            Console.WriteLine("Word and clue added successfully!");
        }

        static void StartGame()
        {
            var model = new HangmanModel();
            var controller = new HangmanController(model);
            controller.Run();
        }
    }
}
