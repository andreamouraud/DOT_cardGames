namespace cardGamesServer {
    using System;

    public class DisplayManager {
        public static ConsoleColor BLACK = ConsoleColor.Black;
        public static ConsoleColor RED = ConsoleColor.Red;
        public static ConsoleColor GREEN = ConsoleColor.Green;
        public static ConsoleColor YELLOW = ConsoleColor.Yellow;
        public static ConsoleColor BLUE = ConsoleColor.Blue;
        public static ConsoleColor PURPLE = ConsoleColor.Magenta;
        public static ConsoleColor CYAN = ConsoleColor.Cyan;
        public static ConsoleColor WHITE = ConsoleColor.White;
        
        public static void print(string message, ConsoleColor color, bool newline) {
            Console.ForegroundColor = color;
            Console.Write(message + (newline ? "\n" : ""));
            Console.ResetColor();
        }
    }
}
