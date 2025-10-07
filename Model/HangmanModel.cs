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

            if (CorrectGuesses.Contains(c)) return;

            if (WordToGuess.Contains(c))
            {
                CorrectGuesses.Add(c);
                CurrentLetterAttempts = 3;
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
