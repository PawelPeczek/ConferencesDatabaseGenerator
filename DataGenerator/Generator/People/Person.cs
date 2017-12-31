namespace DataGenerator.Generator.People {
    class Person {
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public Person(string name, string surname) {
            Name = name;
            Surname = surname;
        }
        public new string ToString => Name + " " + Surname;
    }
}
