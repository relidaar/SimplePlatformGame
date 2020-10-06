using System;

namespace SimplePlatformGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new PlatformGame();
            game.Run();
        }
    }
}