﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Generator.Numbers {
    class PhoneGenerator : NumberSource, INumberGenerator {
        public string[] Generate(int ammount) {
            if (ammount > Math.Pow(Digits.Length, 9)) throw new IndexOutOfRangeException("Cannot generate so many unique phone numbers");
            return GenerateGenericNumber(GenerateSingleRandPhone, ammount);
        }

        private string GenerateSingleRandPhone() {
            StringBuilder res = new StringBuilder("+48 ");
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 1; i <= 9; i++) {
                res.Append(Digits[rnd.Next(0, 10)]);
                if (i % 3 == 0 && i != 9) res.Append("-");
            }
            return res.ToString();
        }
    }
}