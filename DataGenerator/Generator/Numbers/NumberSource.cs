using System;
using System.Collections.Generic;
using System.Linq;

namespace DataGenerator.Generator.Numbers {
    abstract class NumberSource {
        protected Random rnd = new Random(Guid.NewGuid().GetHashCode());
        protected string[] Digits = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        protected string[] Letters = {
            "A", "B", "C", "D", "E", "F", "G", "H", "I",
            "J", "K", "L", "M", "N", "O", "P", "Q", "R",
            "S", "T", "U", "V", "W", "X", "Y", "Z"
        };

        protected string[] GenerateGenericNumber(Func<string> f, int ammount) {
            HashSet<string> set = new HashSet<string>();
            int i = 0;
            while (i < ammount) {
                string tmp = f();
                if (!set.Contains(tmp)) {
                    set.Add(tmp);
                    i++;
                }
            }
            return set.ToArray();
        }

    }
}
