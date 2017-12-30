using DataGenerator.Generator.Participants;
using DataGenerator.Generator.People;
using DataGenerator.ORM;
using System;
using System.Linq;


namespace DataGenerator.Generator.CompaniesEmployees {
    class CompaniesEmployeesGenerator {
        public void GenerateEmployeesForCompanies(int EmpPerComp, int StdPerCmp) {
                using (ConferencesModelContext ctx = new ConferencesModelContext()) {
                    ctx.Configuration.AutoDetectChangesEnabled = false;
                    var Companies = from c in ctx.Clients.OfType<CompDetails>()
                                    where c.Participants.Count == 0
                                    select c;
                    int saveCounter = 0;
                    foreach (CompDetails c in Companies.ToArray()) {
                        Console.Write(".");
                        CreateBatchOfEmplForComp(c, EmpPerComp, StdPerCmp, ctx);
                        saveCounter++;
                        if (saveCounter % 50 == 0) {
                            saveCounter = 0;
                            Console.Write("[DB]");
                            ctx.SaveChanges();
                        }
                    }
                    ctx.SaveChanges();
                }            
        }
        private void CreateBatchOfEmplForComp(CompDetails c, int EmpPerComp, int StdPerCmp, ConferencesModelContext ctx) {
            ParticipantsStudCardsGenerator pScGen = new ParticipantsStudCardsGenerator();
            for (int i = 1; i <= EmpPerComp; i++) {
                Person person = new PeopleGenerator().GenerateSingleNameSurname();
                ParticipantsDetails p = new ParticipantsDetails {
                    Email = GeneratorCore.GetFollowingEmail(),
                    Name = person.Name,
                    Surname = person.Surname,
                    PESEL = GeneratorCore.GetFollowingPESEL(),
                };
                c.ParticipantsReg.Add(p);
                p.ClientsReg.Add(c);
                ctx.Participants.Add(p);
                if(i >= EmpPerComp - StdPerCmp) {
                    pScGen.AttachParticipantWithStudCards(p, ctx);
                }
            }
        }
    }
}
