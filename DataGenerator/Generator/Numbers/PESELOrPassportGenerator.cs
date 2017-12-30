using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Generator.Numbers {
    class PESELOrPassportGenerator : NumberSource, INumberGenerator {
        public string[] Generate(int ammount) {
            if (ammount > Math.Pow(Digits.Length, 11) + Math.Pow(Digits.Length, 9) * Math.Pow(Letters.Length, 2))
                throw new IndexOutOfRangeException("Cannot generate so many unique PESEL/Passport numbers");
            return GenerateGenericNumber(PESELStrategy, ammount);
        }

        private string PESELStrategy() {
            string tmp;
            
            if (rnd.Next(0, 20) == 17) tmp = GetSingleRandPassport();
            else tmp = GetSingleRandPESEL();
            return tmp;
        }

        private string GetSingleRandPassport() {
            StringBuilder res = new StringBuilder();
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < 2; i++) {
                res.Append(Letters[rnd.Next(0, Letters.Length)]);
            }
            for (int i = 0; i < 7; i++) {
                res.Append(Digits[rnd.Next(0, Digits.Length)]);
            }
            return res.ToString();
        }

        private string GetSingleRandPESEL() {
            StringBuilder res = new StringBuilder();
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < 11; i++) {
                res.Append(Digits[rnd.Next(0, 10)]);
            }
            return res.ToString();
        }
    }
}
