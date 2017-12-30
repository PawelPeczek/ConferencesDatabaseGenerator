using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Generator.Numbers {
    class NIPGenerator : NumberSource, INumberGenerator {
        public string[] Generate(int ammount) {
            if (ammount > Math.Pow(Digits.Length, 13)) throw new IndexOutOfRangeException("Cannot generate so many unique NIP numbers");
            return GenerateGenericNumber(GetSingleRandNIP, ammount);
        }
        private string GetSingleRandNIP() {
            StringBuilder res = new StringBuilder();
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < 13; i++) {
                if (i != 3 && i != 7 && i != 10) res.Append(Digits[rnd.Next(0, Digits.Length)]);
                else res.Append("-");
            }
            return res.ToString();
        }
    }
}
