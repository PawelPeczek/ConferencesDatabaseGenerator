﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Generator.Numbers {
    class FaxGenerator : NumberSource, INumberGenerator {
        public string[] Generate(int ammount) {
            if (ammount > Math.Pow(Digits.Length, 11)) throw new IndexOutOfRangeException("Cannot generate so many unique FAX numbers");
            return GenerateGenericNumber(GenerateSingleRandFax, ammount);
        }
        private string GenerateSingleRandFax() {
            StringBuilder res = new StringBuilder("(");
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            for(int i = 0; i < 2; i++) {
                res.Append((Digits[rnd.Next(1, 10)]));
            }
            res.Append(") ");
            for (int i = 1; i <= 9; i++) {
                res.Append(Digits[rnd.Next(0, 10)]);
                if (i % 3 == 0 && i != 9) res.Append("-");
            }
            return res.ToString();
        }
    }
}
