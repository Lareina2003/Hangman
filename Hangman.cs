using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace HangmanMVCPro
{
    // ===== MODEL =====
    public enum GameStatus { Ongoing, PlayerWon, PlayerLost }

    public class HangmanModel
    {
        public string WordToGuess { get; private set; }
        public string Clue { get; private set; }
        public List<char> CorrectGuesses { get; private set; }
        public List<char> WrongGuesses { get; private set; }
        public int CurrentLetterAttempts { get; set; }
        public GameStatus GameStatus { get; private set; }

        public event Action GameEnded;

        private static readonly List<(string Word, string Clue)> WordList;

        static HangmanModel()
        {
            WordList = File.ReadAllLines("words.csv")
                .Skip(1)
                .Select(line => line.Split(','))
                .Select(parts => (parts[0].ToUpper(), string.Join(",", parts.Skip(1))))
                .ToList();
        }

        public HangmanModel()
        {
            Random rnd = new Random();
            var selected = WordList[rnd.Next(WordList.Count)];
            WordToGuess = selected.Item1;
            Clue = selected.Item2;
            CorrectGuesses = new List<char>();
            WrongGuesses = new List<char>();
            CurrentLetterAttempts = 3;
            GameStatus = GameStatus.Ongoing;
        }

        public void Guess(char c)
        {
            c = char.ToUpper(c);

            // If already guessed correctly, ignore
            if (CorrectGuesses.Contains(c)) return;

            if (WordToGuess.Contains(c))
            {
                CorrectGuesses.Add(c);
                CurrentLetterAttempts = 3; // next letter attempts reset handled in controller
            }
            else
            {
                WrongGuesses.Add(c);
                CurrentLetterAttempts--;

                if (CurrentLetterAttempts <= 0)
                {
                    SetPlayerLost(); // lose immediately if 3 wrong for current letter
                }
            }

            // Check if all letters guessed
            if (WordToGuess.All(letter => CorrectGuesses.Contains(letter)))
            {
                GameStatus = GameStatus.PlayerWon;
                GameEnded?.Invoke();
            }
        }

        public void SetPlayerLost()
        {
            GameStatus = GameStatus.PlayerLost;
            GameEnded?.Invoke();
        }
    }

    // ===== VIEW =====
    public static class HangmanView
    {
        public static void Display(HangmanModel model)
        {
            Console.WriteLine($"Clue: {model.Clue}");
            Console.WriteLine($"Wrong guesses: {string.Join(", ", model.WrongGuesses)}");
            Console.WriteLine(DisplayWord(model.WordToGuess, model.CorrectGuesses));
            DisplayHangman(model.CurrentLetterAttempts);
        }

        public static string DisplayWord(string word, List<char> correctGuesses)
        {
            return string.Join(" ", word.Select(c => correctGuesses.Contains(c) ? c : '_'));
        }

        public static void DisplayHangman(int attemptsLeft)
        {
            Console.WriteLine("\nHangman State:");
            switch (attemptsLeft)
            {
                case 3:
                    Console.WriteLine("  |\n  |\n  |\n"); break;
                case 2:
                    Console.WriteLine("  |\n  |\n  O"); break;
                case 1:
                    Console.WriteLine("  |\n  |\n  O\n /|\\\n"); break;
            }
        }

        public static void ShowEndMessage(HangmanModel model)
        {
            //Console.Clear();
            Console.WriteLine("\n Correct Word : " + model.WordToGuess + "\n");
            if (model.GameStatus == GameStatus.PlayerWon)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(@"
##     ##  ########  ##     ##         
 ##   ##   ##     ## ##     ## 
  ## ##    ##     ## ##     ## 
   ###     ##     ## ##     ## 
    #      ##     ## ##     ## 
    #      ##     ## ##     ##      
    #       #######   #######  

##     ##   #########   ##      ##
##     ##   ##     ##  ## ##    ## 
##     ##   ##     ##  ##  ##   ## 
##  #  ##   ##     ##  ##   ##  ##
## # # ##   ##     ##  ##    ## ## 
## # # ##   ##     ##  ##     ####
 ##   ##    #########  ##      ###      
----------------------------------

  \o/   \o/   \o/  
   |     |     |   
  / \   / \   / \ 
Congratulations! You won!

--------------------------------
");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(@"

##     ##  ########  ##     ##         
 ##   ##   ##     ## ##     ## 
  ## ##    ##     ## ##     ## 
   ###     ##     ## ##     ## 
    #      ##     ## ##     ## 
    #      ##     ## ##     ##      
    #       #######   #######  

##         #########   #######  ###########
##         ##     ##   ##           ##
##         ##     ##   ##           ##
##         ##     ##   #######      ##
##         ##     ##        ##      ##
##         ##     ##        ##      ##
########   #########   #######      ##
------------
     |    
     O    
    /|\  
    / \  
Sorry, you lost! ");
            }
            Console.ResetColor();
        }
    }

    // ===== CONTROLLER =====
    public class HangmanController
    {
        private HangmanModel _model;

        public HangmanController(HangmanModel model)
        {
            _model = model;
            _model.GameEnded += OnGameEnded;
        }

        public void Run()
        {
            int index = 0;
            while (_model.GameStatus == GameStatus.Ongoing && index < _model.WordToGuess.Length)
            {
                char currentLetter = _model.WordToGuess[index];
                _model.CurrentLetterAttempts = 3;

                while (_model.CurrentLetterAttempts > 0 && !_model.CorrectGuesses.Contains(currentLetter))
                {
                    Console.Clear();
                    HangmanView.Display(_model);
           
                    Console.Write($"Guess letter {index + 1}/{_model.WordToGuess.Length}: ");
                 
                    char guess = Console.ReadKey().KeyChar;
                    _model.Guess(guess);
                }

                index++; // move to next letter
            }

            if (_model.GameStatus == GameStatus.Ongoing)
            {
                _model.SetPlayerLost();
            }
        }

        private void OnGameEnded()
        {
            HangmanView.ShowEndMessage(_model);
        }
    }

    // ===== PROGRAM =====
    class Program
    {
        static void Main()
        {
            var model = new HangmanModel();
            var controller = new HangmanController(model);
            controller.Run();
        }
    }
}
