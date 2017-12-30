using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Generator.Numbers {
    class NumberGenerator {
        public enum NumberTypes {
            NIP,
            PESELOrPsssport,
            Phone,
            Fax,
            StudentCard
        }


        public string[] GenerateGenericNumber(NumberTypes type, int ammount) {
            INumberGenerator generator;
            switch (type) {
                case NumberTypes.NIP:
                    generator = new NIPGenerator();
                    break;
                case NumberTypes.PESELOrPsssport:
                    generator = new PESELOrPassportGenerator();
                    break;
                case NumberTypes.Phone:
                    generator = new PhoneGenerator();
                    break;
                case NumberTypes.Fax:
                    generator = new FaxGenerator();
                    break;
                case NumberTypes.StudentCard:
                    generator = new StudentCradNumberGenerator();
                    break;
                default:
                    generator = null;
                    break;
            }
            return generator != null ? generator.Generate(ammount) : new string[0];
        }
    }
}
