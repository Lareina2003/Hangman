using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace HangmanMVCPro.Model
{
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

        static HangmanModel()//loading words from csv
        {
            WordList = File.ReadAllLines("words.csv")//Read all lines from words.csv
                .Skip(1)//Skips the first header line
                .Select(line => line.Split(','))//Splits each line into word and clue
                .Select(parts => (parts[0].ToUpper(), string.Join(",", parts.Skip(1))))//Converts to uppercase for easy matching
                .ToList();
        }

        public HangmanModel()
        {
            Random rnd = new Random();//creates random object called rnd
            var selected = WordList[rnd.Next(WordList.Count)];//WordList[rnd.Next(WordList.Count)]:Randomly picks one element [rnd.Next(WordList.Count) gives a random number from 0 to Count - 1.]
            WordToGuess = selected.Item1;//Stored the word to guess [Item1]
            Clue = selected.Item2;//Stores Clue [Item2]
            CorrectGuesses = new List<char>();
            WrongGuesses = new List<char>();
            CurrentLetterAttempts = 3;
            GameStatus = GameStatus.Ongoing;
        }

        public void Guess(char c)
        {
            c = char.ToUpper(c);//convert the guess to upper case

            if (CorrectGuesses.Contains(c)) return;//if guess word is already correct skip

            if (WordToGuess.Contains(c))
            {
                CorrectGuesses.Add(c);
                CurrentLetterAttempts = 3;//again attempts count turns 3
            }
            else
            {
                WrongGuesses.Add(c);
                CurrentLetterAttempts--;

                if (CurrentLetterAttempts <= 0)
                {
                    SetPlayerLost();
                }
            }

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
}
