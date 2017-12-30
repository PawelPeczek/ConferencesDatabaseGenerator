using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGenerator.ORM;
using DataGenerator.Generator;
using System.Security.Cryptography;

namespace DataGenerator {
    class Program {
        static void Main(string[] args) {
            GeneratorCore gen = new GeneratorCore();
            gen.GenerateData();
            Console.ReadLine();
        }
    }
}
