using DataGenerator.Generator.Numbers;
using DataGenerator.Generator.People;
using DataGenerator.ORM;
using System.Security.Cryptography;
using System;
using System.Text;

namespace DataGenerator.Generator.Companies {
    class CompaniesGenerator {
        public void GenerateCompaniesClients(int NumOfComp) {
            CompanyNameGenerator cng = new CompanyNameGenerator();
            FaxGenerator fg = new FaxGenerator();
            NIPGenerator ng = new NIPGenerator();
            PeopleGenerator peopleg = new PeopleGenerator();
            string[] CompNames = cng.GenerateCompNames(NumOfComp);
            string[] NIPs = ng.Generate(NumOfComp);
            string[] Faxes = fg.Generate(NumOfComp);
            Person[] Contacts = peopleg.GenerateNamesSurnames(NumOfComp);
            using (ConferencesModelContext ctx = new ConferencesModelContext()) {
                for (int i = 0; i < CompNames.Length; i++) {
                    Console.Write(".");
                    CompDetails cd = new CompDetails {
                        Login = "compClient" + i,
                        Email = GeneratorCore.GetFollowingEmail(),
                        Password = SHA512Managed.Create().ComputeHash(Encoding.Unicode.GetBytes("zaq1@WSX")),
                        PhoneNumber = GeneratorCore.GetFollowingPhone(),
                        CompanyName = CompNames[i],
                        ContactPersonName = Contacts[i].Name,
                        ContactPersonSurname = Contacts[i].Surname,
                        Fax = Faxes[i],
                        NIP = NIPs[i]
                    };
                    ctx.Clients.Add(cd);
                }
                Console.Write("[DB]");
                ctx.SaveChanges();
            }
        }
    }
}
