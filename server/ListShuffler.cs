using System;
using System.Collections.Generic;

namespace cardGamesServer {
    public static class ListShuffler {
        private static readonly Random rng = new Random();  

        public static void randomize<T>(this IList<T> list) {  
            var n = list.Count;  
            while (n > 1) {  
                n--;  
                var k = rng.Next(n + 1);  
                var value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }  
        }
    }
}