using System;

namespace cardGamesServer {
    public class Type
    {
        public static readonly Type CARRE_VALETS = new Type("Carre of Jack", 200);
        public static readonly Type CARRE_NEUF = new Type("Carre of Nine", 150);
        public static readonly Type CARRE = new Type("Carre", 100);
        public static readonly Type CENT = new Type("Cent", 100);
        public static readonly Type CINQUANTE = new Type("Cinquante", 50);
        public static readonly Type TIERCE = new Type("Tierce", 20);
        public static readonly Type BELOTE = new Type("Belote", 20);
        public static readonly Type NONE = new Type("", 0);

        public int value { get; }
        public String name { get; }

        Type(String name, int value) {
            this.name = name;
            this.value = value;
        }
    }
    
    public class Annonce {
        public Type type { get; set; } = Type.NONE;

        public String getName() {
            return type.name;
        }
        
        public int getValue() {
            return type.value;
        }

    }
}