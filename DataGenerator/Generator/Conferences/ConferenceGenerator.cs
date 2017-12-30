using System;
using DataGenerator.ORM;
using DataGenerator.Generator.People;

namespace DataGenerator.Generator.Conferences {
    class ConferenceGenerator {
        private Random rnd = new Random(Guid.NewGuid().GetHashCode());
        private string[] Titles = {
            "Medical", "IT", "Bussiness", "Marketing", "PR", "Programmers", "Sport fans",
            "Star Wars fans", "Cinematographers", "Football", "History", "Fantasy", "Gaming",
            "E-sport", "Biologist"
        };
        private string[] Names = {
            "Help everyone", "Unusual Behavoiurs", "Corporate image", "Common pratfalls", "Lambda Calculus",
            "Goals", "Secrete life", "The magic of colours", "The mystery of ages", "Best performance of the year"
        };
        private string[] Topics = {
            "Healthcare", "Sport", "Animals", "Games", "Sport", "Entertainment", "Discussions", null
        };
        private string[] Descriptions = {
            "Healthcare", "Sport", "Animals", "Games", "Sport", "Entertainment", "Discussions", null
        };
        private float[] Disounts = { 0.1F, 0.15F, 0.2F, 0.22F, 0.25F, 0.35F, 0.5F };
        private short[] WorkshopSpaceLimits = { 15, 20, 25, 30, 32, 35, 40, 50 };
        //private short[] WorkshopSpaceLimits = { 8, 10 };
        private short[] ConfDaySpaceLimits = { 180, 190, 200, 210, 220};
        //private short[] ConfDaySpaceLimits = { 30, 35, 40 };
        private string[] WorkNames = { "Practise", "Research", "Advanced", "Semi-pro", "Beginners" };

        public void GenerateWholeBatchOfConf() {
            Conferences.ConferenceGenerator cg = new Conferences.ConferenceGenerator();
            DateTime simulationDate = new DateTime(2015, 1, 1);
            DateTime sentinelDate = new DateTime(2018, 3, 1);
            using (ConferencesModelContext ctx = new ConferencesModelContext()) {
                ctx.Configuration.AutoDetectChangesEnabled = false;
                while (simulationDate < sentinelDate) {
                    Console.Write(".");
                    cg.CreateConferenceComplex(rnd.Next(3, 5), simulationDate, ctx);
                    simulationDate = simulationDate.AddDays(rnd.Next(10, 16));
                }
                Console.Write("[DB]");
                ctx.SaveChanges();
            }
        }

        private ORM.Conferences GenerateConference() {
            ORM.Conferences res = new ORM.Conferences {
                ConfName = Titles[rnd.Next(0, Titles.Length)] + " conference \"" + Names[rnd.Next(0, Names.Length)] + "\"",
                ConfTopic = Topics[rnd.Next(0, Topics.Length)],
                ConfDescription = "Conference mainly about " + Descriptions[rnd.Next(0, Descriptions.Length)],
                StudentDiscount = Disounts[rnd.Next(0, Disounts.Length)]
            };
            return res;
        }

        private void GeneratePriceThresholds(DaysOfConf doc, ConferencesModelContext ctx) {
            decimal basePrice = 50 + (decimal) rnd.NextDouble() * 200;
            if (basePrice <= 100) basePrice = 0;
            for (int i = 0; i < 3; i++) {
                PriceThresholds pt = new PriceThresholds();
                if (i == 0) pt.EndDate = doc.Date;
                else pt.EndDate = doc.Date.AddDays(-i * 10 + (rnd.Next(0, 4) - 2));
                pt.Value = basePrice - i * 25 > 0 ? basePrice - i * 25 : 0 ;
                pt.DaysOfConf = doc;
                ctx.PriceThresholds.Add(pt);
            }
        }

        private void GenerateWorkshops(DaysOfConf doc, ConferencesModelContext ctx) {
            int NumOfWOrkshops = rnd.Next(3, 7);
            TimeSpan baseTimeStart = new TimeSpan(9, 0, 0);
            TimeSpan baseTimeEnd = new TimeSpan(10, 30, 0);
            TimeSpan interval = new TimeSpan(1, 0, 0);
            for (int i = 0; i < NumOfWOrkshops; i++) {
                Person techer = new PeopleGenerator().GenerateSingleNameSurname();
                decimal val = rnd.Next(0, 200) + (decimal) rnd.NextDouble();
                if (val < 25) val = 0;
                Workshops w = new Workshops {
                    DaysOfConf = doc,
                    StartTime = baseTimeStart,
                    EndTime = baseTimeEnd,
                    SpaceLimit = WorkshopSpaceLimits[rnd.Next(0, WorkshopSpaceLimits.Length)],
                    Name = (doc.Conferences.ConfTopic ?? "Workshop with/of") + 
                            " " + WorkNames[rnd.Next(0, WorkNames.Length)],
                    TeacherName = techer.Name,
                    TeacherSurname = techer.Surname,
                    Value = val
                };
                baseTimeStart = baseTimeStart.Add(interval);
                baseTimeEnd = baseTimeEnd.Add(interval);
                ctx.Workshops.Add(w);
            }
        }

        private void GenerateDaysOfConf(int numbOfConfDays, DateTime confStart, ORM.Conferences conf, ConferencesModelContext ctx) {
            for(int i = 0; i < numbOfConfDays; i++) {
                DaysOfConf doc = new DaysOfConf {
                    Conferences = conf,
                    Date = confStart.AddDays(i),
                    SpaceLimit = ConfDaySpaceLimits[rnd.Next(0, ConfDaySpaceLimits.Length)]
                };
                ctx.DaysOfConf.Add(doc);
                GeneratePriceThresholds(doc, ctx);
                GenerateWorkshops(doc, ctx);
            }
        }

        private void CreateConferenceComplex(int numbOfConfDays, DateTime confStart, ConferencesModelContext ctx) {
            ORM.Conferences conf = GenerateConference();
            ctx.Conferences.Add(conf);
            GenerateDaysOfConf(numbOfConfDays, confStart, conf, ctx);
        }
    }
}
