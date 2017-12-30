using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Generator.People {
    class PeopleGenerator {
        Random rand = new Random(Guid.NewGuid().GetHashCode());
        private string[] Names = {
            "John", "Paul", "Ben", "Ron", "Harry", "Victoria", "Emma", "Luke", "Rey", "Miriam",
            "Harrison", "James", "Rusty", "Mark", "Michael", "Lucy", "Morgan", "Edward", "Leonardo",
            "Steven", "Alan", "Geff", "Alison", "Adonis", "Sylwester", "Thomas", "Rudolph", "Jim",
            "Sophia", "Joe", "Bill", "Steve", "Raul", "Xavier", "Eva", "Oscar", "Dennis"
        };

        private string[] Surnames = {
            "Black", "Oliver", "Solo", "Wesley", "Potter", "Beck", "Robberts", "Stone", "Skywalker", "Hudson",
            "Stevenson", "Ford", "Black", "Stanford", "Bay", "Stark", "Freeman", "Norton", "DiCaprio", "Soon",
            "Rickmann", "Bright", "Stalone", "Lemmar", "Monday", "Harris", "Cash", "Masters"
        };

        public PersonNameSurname[] GenerateNamesSurnames(int ammount) {
            PersonNameSurname[] res = new PersonNameSurname[ammount];
            PersonNameSurname[] tmp;
            var cartesianQuery = from x in Names
                                 from y in Surnames
                                 select new PersonNameSurname(x, y);
            tmp = cartesianQuery.OrderBy(x => rand.Next()).ToArray();
            int generatorCapacity = Names.Length * Surnames.Length;
            if (ammount <= generatorCapacity) {
                res = tmp.Take(ammount).ToArray();
            } else {
                for (int i = 0; i < ammount; i++) {
                    res[i] = tmp[i % generatorCapacity];
                }
            }
            return res;
        }

        public PersonNameSurname GenerateSingleNameSurname() {
            return new PersonNameSurname(Names[rand.Next(0, Names.Length)], Surnames[rand.Next(0, Surnames.Length)]);
        }
    }
}
