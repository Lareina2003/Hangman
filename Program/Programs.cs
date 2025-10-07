using HangmanMVCPro.Model;
using HangmanMVCPro.Controller;

namespace HangmanMVCPro
{
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
