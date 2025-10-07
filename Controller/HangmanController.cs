using System;
using HangmanMVCPro.Model;
using HangmanMVCPro.View;

namespace HangmanMVCPro.Controller
{
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

                index++;
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
}
