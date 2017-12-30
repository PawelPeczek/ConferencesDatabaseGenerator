using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Generator.People {
    class PersonNameSurname {
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public PersonNameSurname(string name, string surname) {
            Name = name;
            Surname = surname;
        }
        public string ToString => Name + " " + Surname;
    }
}
