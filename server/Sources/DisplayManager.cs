namespace cardGamesServer {
    using System;

    public class DisplayManager {
        private static string RESET = "\u001B[0m";
        public static string BLACK = "\u001B[30m";
        public static string RED = "\u001B[31m";
        public static string GREEN = "\u001B[32m";
        public static string YELLOW = "\u001B[33m";
        public static string BLUE = "\u001B[34m";
        public static string PURPLE = "\u001B[35m";
        public static string CYAN = "\u001B[36m";
        public static string WHITE = "\u001B[37m";

        public static void print(string message, string color, bool newline) {
            Console.Write(color + message + RESET + (newline ? "\n" : ""));
        }
    }
}
