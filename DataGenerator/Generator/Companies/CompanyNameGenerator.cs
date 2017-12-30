using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Generator.Companies {
    class CompanyNameGenerator {
        private String[] Prefixes =  {
            "High Impact", "NextDoor", "Reliable", "Horizon", "Olympia",
            "CarpeDiem", "Elite", "Tom's", "Shire", "SilkRoad", ""
        };

        private String[] Infixes = {
            "Groceery", "Medicus", "Discount", "Store", "Jewelery", "Logistics", "Developer",
            "Architect", "Programmers", "SQL Experts", "Security Masters"
        };

        private String[] Postfixes = {
            "Ltd.", "inc.", ""
        };

        public string[] GenerateCompNames(int ammount) {
            string[] res = new string[ammount];
            string[] tmp;
            var cartesianQuery = from x in Prefixes
                                 from y in Infixes
                                 from z in Postfixes
                                 select (x + " " + y + " " + z).Trim();
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            tmp = cartesianQuery.OrderBy(x => rand.Next()).ToArray();
            int generatorCapacity = Prefixes.Length * Infixes.Length * Postfixes.Length;
            if (ammount <= generatorCapacity) {
                res = tmp.Take(ammount).ToArray();
            } else {
                for (int i = 0; i < ammount; i++) {
                    res[i] = tmp[i % generatorCapacity];
                }
            }
            return res;
        }
    }
}
