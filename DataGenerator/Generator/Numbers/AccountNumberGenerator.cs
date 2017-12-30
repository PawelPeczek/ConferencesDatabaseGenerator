using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Generator.Numbers {
    class AccountNumberGenerator : NumberSource {
        public string GenerateSingleRandAccNumber() {
            StringBuilder res = new StringBuilder();
            for(int i = 0; i < 26; i++) {
                res.Append(Digits[rnd.Next(0, Digits.Length)]);
            }
            return res.ToString();
        }
    }
}
