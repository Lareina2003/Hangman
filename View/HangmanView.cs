using System;
using System.Collections.Generic;
using System.Linq;
using HangmanMVCPro.Model;

namespace HangmanMVCPro.View
{
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
}
