using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Generator.Numbers {
    class StudentCradNumberGenerator : NumberSource, INumberGenerator {
        public string[] Generate(int ammount) {
            return GenerateGenericNumber(GetSingleRandStudCardNumb, ammount);
        }

        private string GetSingleRandStudCardNumb() {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            StringBuilder res = new StringBuilder();
            for(int i = 0; i < 2; i++) {
                res.Append(Letters[rnd.Next(0, Letters.Length)]);
            }
            for(int i = 0; i < 7; i++) {
                res.Append(Digits[rnd.Next(0, Digits.Length)]);
                if (i == 3) res.Append("-");
            }
            return res.ToString();
        }
    }
}
