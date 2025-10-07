using System;
using System.IO;

namespace HangmanMVCPro
{
    public class AdminController
    {
        private const string FilePath = "words.csv";

        public void Run()
        {
            Console.Clear();
            Console.WriteLine("=== ADMIN MODE ===");
            Console.Write("Enter new word: ");
            string word = Console.ReadLine()?.Trim().ToUpper();

            Console.Write("Enter clue for the word: ");
            string clue = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(word) || string.IsNullOrWhiteSpace(clue))
            {
                Console.WriteLine("❌ Word or clue cannot be empty!");
                return;
            }

            try
            {
                // Append to CSV file
                File.AppendAllText(FilePath, $"\n{word},{clue}");
                Console.WriteLine("\n✅ Word successfully added to words.csv!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error adding word: {ex.Message}");
            }
        }
    }
}
