using System.Text;

namespace DataGenerator.Generator.Numbers {
    class StudentCradNumberGenerator : NumberSource, INumberGenerator {
        public string[] Generate(int ammount) {
            return GenerateGenericNumber(GetSingleRandStudCardNumb, ammount);
        }

        private string GetSingleRandStudCardNumb() {
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
