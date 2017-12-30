using DataGenerator.Generator.Companies;
using DataGenerator.Generator.CompaniesEmployees;
using DataGenerator.Generator.Dates;
using DataGenerator.Generator.Emails;
using DataGenerator.Generator.IndividCustomers;
using DataGenerator.Generator.Numbers;
using DataGenerator.Generator.People;
using DataGenerator.ORM;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataGenerator.Generator.Numbers.NumberGenerator;

namespace DataGenerator.Generator {
    class GeneratorCore {
        public const int NUMBER_OF_PARTICIP = 3300;
        public const int NUMBER_OF_COMPANIES = 300;
        public const int NUMBER_OF_INDIV_CLIENTS = 300;
        public const int NUMBER_OF_STUD_PER_COPM = 3;
        private static string[] Emails;
        private static string[] PESELs;
        private static string[] Phones;
        private static int EmailsPointer = 0;
        private static int PESELsPointer = 0;
        private static int PhonesPointer = 0;

        static GeneratorCore() {
            Emails = new EmailGenerator().GenerateEmails(NUMBER_OF_PARTICIP + NUMBER_OF_COMPANIES);
            PESELs = new PESELOrPassportGenerator().Generate(NUMBER_OF_PARTICIP);
            Phones = new PhoneGenerator().Generate(NUMBER_OF_INDIV_CLIENTS + NUMBER_OF_COMPANIES);
        }

        public static string GetFollowingPESEL() {
            if (PESELsPointer < PESELs.Length) {
                PESELsPointer++;
                return PESELs[PESELsPointer - 1];
            } else throw new IndexOutOfRangeException();
        }

        public static string GetFollowingEmail() {
            if (EmailsPointer < Emails.Length) {
                EmailsPointer++;
                return Emails[EmailsPointer - 1];
            } else throw new IndexOutOfRangeException();
        }

        public static string GetFollowingPhone() {
            if (PhonesPointer < Phones.Length) {
                PhonesPointer++;
                return Phones[PhonesPointer - 1];
            } else throw new IndexOutOfRangeException();
        }

        private Random rnd = new Random(Guid.NewGuid().GetHashCode());
        public void GenerateData() {
            CompaniesGenerator compGen = new CompaniesGenerator();
            IndividualCustomersGenerator icGen = new IndividualCustomersGenerator();
            CompaniesEmployeesGenerator compEmpGen = new CompaniesEmployeesGenerator();
            
                try {
                    GenerateWholeBatchOfConf();
                    //compGen.GenerateCompaniesClients(NUMBER_OF_COMPANIES);
                    //Console.WriteLine("DONE!");
                    icGen.GenerateIndividualCustomers(NUMBER_OF_INDIV_CLIENTS);
                    Console.WriteLine("Computation individual clients done!");
                    Console.WriteLine("DONE!");
                    //int PartPerComp = (NUMBER_OF_PARTICIP - NUMBER_OF_INDIV_CLIENTS) / NUMBER_OF_COMPANIES;
                    //compEmpGen.GenerateEmployeesForCompanies(PartPerComp, NUMBER_OF_STUD_PER_COPM);
                    //Console.WriteLine("Computations done!");
                    //Console.WriteLine("DONE!");
                } catch (DbEntityValidationException e) {
                    foreach (var eve in e.EntityValidationErrors) {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors) {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                } catch (Exception ex) {
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine(ex.Message);
                   //Console.WriteLine(ex.InnerException.InnerException.Message);
                }
        }

        private void GenerateWholeBatchOfConf() {
            Conferences.ConferenceGenerator cg = new Conferences.ConferenceGenerator();
            DateTime simulationDate = new DateTime(2015, 1, 1);
            DateTime sentinelDate = new DateTime(2018, 3, 1);
            using(ConferencesModelContext ctx = new ConferencesModelContext()) {
                ctx.Configuration.AutoDetectChangesEnabled = false;
                while (simulationDate < sentinelDate) {
                    cg.CreateConferenceComplex(rnd.Next(3, 5), simulationDate, ctx);
                    simulationDate = simulationDate.AddDays(rnd.Next(10, 16));
                }
                Console.WriteLine("Computing done! Waiting for DB");
                ctx.SaveChanges();
            }
        }
    }
}
