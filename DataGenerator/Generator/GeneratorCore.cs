using DataGenerator.Generator.Companies;
using DataGenerator.Generator.CompaniesEmployees;
using DataGenerator.Generator.Conferences;
using DataGenerator.Generator.Dates;
using DataGenerator.Generator.Emails;
using DataGenerator.Generator.IndividCustomers;
using DataGenerator.Generator.Numbers;
using DataGenerator.Generator.Orders;
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
        public const int NUMBER_OF_PARTICIP = 11000;
        public const int NUMBER_OF_COMPANIES = 300;
        public const int NUMBER_OF_INDIV_CLIENTS = 1000;
        public const int NUMBER_OF_STUD_PER_COPM = 10;
        private static string[] Emails;
        private static string[] PESELs;
        private static string[] Phones;
        private static int EmailsPointer = 0;
        private static int PESELsPointer = 0;
        private static int PhonesPointer = 0;
        private Random rnd = new Random(Guid.NewGuid().GetHashCode());
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
        public void GenerateData() {
            CompaniesGenerator CompGen = new CompaniesGenerator();
            IndividualCustomersGenerator IcGen = new IndividualCustomersGenerator();
            CompaniesEmployeesGenerator CompEmpGen = new CompaniesEmployeesGenerator();
            ConferenceGenerator ConfGen = new ConferenceGenerator();
            OrdersGenerator OrdGen = new OrdersGenerator();
                try {
                    Console.WriteLine("\n[STEP 1/5] Generating conferences...");
                    ConfGen.GenerateWholeBatchOfConf();
                    Console.WriteLine("\n[STEP 2/5] Generating Companies Clients...");
                    CompGen.GenerateCompaniesClients(NUMBER_OF_COMPANIES);
                    Console.WriteLine("\n[STEP 3/5] Generating Individuals Clients...");
                    IcGen.GenerateIndividualClients(NUMBER_OF_INDIV_CLIENTS);
                    int PartPerComp = (NUMBER_OF_PARTICIP - NUMBER_OF_INDIV_CLIENTS) / NUMBER_OF_COMPANIES;
                    Console.WriteLine("\n[STEP 4/5] Generating Employees for Companies...");
                    CompEmpGen.GenerateEmployeesForCompanies(PartPerComp, NUMBER_OF_STUD_PER_COPM);
                    Console.WriteLine("\n[STEP 5/5] Generating Orders...");
                    OrdGen.GenerateBatchOfOrders();
                } catch (Exception ex) {
                    Console.WriteLine("\n\n[DEBUG MODE]");
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine("\n[ERROR MESSAGE]");
                    Console.WriteLine(ex.Message);
                    if(ex.InnerException != null && ex.InnerException.InnerException != null) {
                        Console.WriteLine("\n[SUPRESSED EXCEPTION MESSAGE]");
                        Console.WriteLine(ex.InnerException.InnerException.Message);
                    }   
                }
        }
    }
}
