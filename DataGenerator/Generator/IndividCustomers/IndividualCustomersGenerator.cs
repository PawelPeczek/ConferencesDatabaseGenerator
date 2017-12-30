using DataGenerator.Generator.Dates;
using DataGenerator.Generator.Numbers;
using DataGenerator.Generator.Participants;
using DataGenerator.Generator.People;
using DataGenerator.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Generator.IndividCustomers {
    class IndividualCustomersGenerator {
        private const int numberOfStudCardPerStud = 8;
        private const int termLength = 6;
        private DateTime lowerBound = new DateTime(2015, 1, 1);
        private DateTime upperBound = new DateTime(2019, 1, 1);
        private void AttachParticipantWithStudCards(ORM.Participants p, ConferencesModelContext ctx) {
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
        public void GenerateIndividualCustomers(int ammount) {
            PersonNameSurname[] people = new PeopleGenerator().GenerateNamesSurnames(ammount);
            ParticipantsStudCardsGenerator pScGen = new ParticipantsStudCardsGenerator();
            using (ConferencesModelContext ctx = new ConferencesModelContext()) {
                ctx.Configuration.AutoDetectChangesEnabled = false;
                for (int i = 0; i < ammount; i++) {
                    Clients c = new Clients {
                        Login = "IndivClient" + i,
                        Email = GeneratorCore.GetFollowingEmail(),
                        PhoneNumber = GeneratorCore.GetFollowingPhone(),
                        Password = SHA512Managed.Create().ComputeHash(Encoding.Unicode.GetBytes("zaq1@WSX"))
                    };
                    ORM.Participants p = new ORM.Participants {
                        PESEL = GeneratorCore.GetFollowingPESEL(),
                        Name = people[i].Name,
                        Surname = people[i].Surname
                    };
                    c.Participants.Add(p);
                    c.ParticipantsReg.Add(p);
                    p.ClientsReg.Add(c);
                    p.Clients.Add(c);
                    ctx.Clients.Add(c);
                    ctx.Participants.Add(p);
                    if (i % 4 == 0) {
                        pScGen.AttachParticipantWithStudCards(p, ctx);
                    }
                }
                Console.WriteLine("Computing finished!");
                ctx.SaveChanges();
            }
        }
    }
}
