using DataGenerator.Generator.Dates;
using DataGenerator.Generator.Numbers;
using DataGenerator.Generator.Participants;
using DataGenerator.Generator.People;
using DataGenerator.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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
                        Console.WriteLine($"Generating emps for company: {c.ClientID}");
                        CreateBatchOfEmplForComp(c, EmpPerComp, StdPerCmp, ctx);
                        saveCounter++;
                        if (saveCounter % 50 == 0) {
                            saveCounter = 0;
                            Console.WriteLine("Context saving...");
                            ctx.Database.Log = Console.Write;
                            ctx.SaveChanges();
                            Console.WriteLine("Done...");
                        }
                    }
                    ctx.SaveChanges();
                }            
        }
        private void CreateBatchOfEmplForComp(CompDetails c, int EmpPerComp, int StdPerCmp, ConferencesModelContext ctx) {
            ParticipantsStudCardsGenerator pScGen = new ParticipantsStudCardsGenerator();
            for (int i = 1; i <= EmpPerComp; i++) {
                PersonNameSurname person = new PeopleGenerator().GenerateSingleNameSurname();
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
