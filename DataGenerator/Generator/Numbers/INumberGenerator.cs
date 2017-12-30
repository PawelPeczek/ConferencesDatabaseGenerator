using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Generator.Numbers {
    interface INumberGenerator {
        string[] Generate(int ammount);
    }
}
