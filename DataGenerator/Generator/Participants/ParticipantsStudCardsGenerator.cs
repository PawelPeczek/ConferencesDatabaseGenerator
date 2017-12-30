using DataGenerator.Generator.Dates;
using DataGenerator.Generator.Numbers;
using DataGenerator.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Generator.Participants {
    class ParticipantsStudCardsGenerator {
        private const int numberOfStudCardPerStud = 8;
        private const int termLength = 6;
        private DateTime lowerBound = new DateTime(2015, 1, 1);
        private DateTime upperBound = new DateTime(2019, 1, 1);

        public void AttachParticipantWithStudCards(ORM.Participants p, ConferencesModelContext ctx) {
            // Individual customer as a student (to simplify -> whole period of time)
            string[] studentCards = new StudentCradNumberGenerator().Generate(1);
            DatePeriod[] dates = new DateGenerator()
                .GenerateNonOverlapingPeriods(lowerBound, upperBound, termLength, numberOfStudCardPerStud);
            for (int j = 0; j < numberOfStudCardPerStud; j++) {
                StudentCards sc = new StudentCards {
                    FromDate = dates[j].startDate,
                    ToDate = dates[j].endDate,
                    StudentCardNumber = studentCards[0],
                    Participants = p
                };
                p.StudentCards.Add(sc);
                ctx.StudentCards.Add(sc);
            }
        }
    }
}
