using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Generator.Emails {
    class EmailGenerator {
        private string[] Separators = { ".", "-", "_"};
        private string[] Prefixes = {
            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m",
            "n", "o", "p", "q", "r", "s", "t", "u", "w", "y", "z"
        };
        private string[] Infixes = {
            "mail", "email", "post", "contact", "corespondence", "importantmails",
            "bussiness", "anderson", "morgan", "edmilson", "neuman", "stone",
            "brown", "blue", "moon", "wolf", "ticker", "intern", "solo", "ben",
            "johnson", "store", "fisher", "alter", "star", "son", "ren", "straigth",
            "monday", "dark", "black", "original", "sun", "oracle", "linus", "charity",
            "corpo", "opt", "luck", "master", "mean", "stark", "julius", "lux", "temporary",
            "trainee", "sport", "light", "bright"
        };
        private string[] Postfixes = {
            "", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"
        };

        private string[] Domains = {
            "gmail.com", "inc.com", "ltd.com", "store.com", "yahoo.com", "hotmail.com", "aol.com",
            "protonmail.com"
        };

        public string[] GenerateEmails(int ammount) {
            int capacity = Separators.Length * Prefixes.Length * Infixes.Length * Postfixes.Length * Domains.Length;
            if (ammount > capacity) throw new IndexOutOfRangeException("Requested ammount of data to generate is to large!");
            string[] tmp;
            var cartesianQuery = from x in Prefixes
                                 from y in Separators
                                 from z in Infixes
                                 from k in Postfixes
                                 from d in Domains
                                 select x + y + z + k + "@" + d;
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            tmp = cartesianQuery.OrderBy(x => rand.Next()).ToArray();
            return tmp.Take(ammount).ToArray();
        }
    }
}
