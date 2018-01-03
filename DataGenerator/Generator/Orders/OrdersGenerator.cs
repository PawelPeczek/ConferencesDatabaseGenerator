using DataGenerator.Generator.Dates;
using DataGenerator.Generator.Numbers;
using DataGenerator.Generator.People;
using DataGenerator.ORM;
using System;
using System.Linq;

namespace DataGenerator.Generator.Orders {
    class OrdersGenerator {
        private Random rnd = new Random(Guid.NewGuid().GetHashCode());
        private AccountNumberGenerator accGen = new AccountNumberGenerator();
        private PeopleGenerator pGen = new PeopleGenerator();
        private DateGenerator DateGen;
        public OrdersGenerator() {
            DateGen = new DateGenerator();
        }
        public void GenerateBatchOfOrders() {
            DaysOfConf[] DaysOfConf;
            Clients[] Clients;
            using (ConferencesModelContext ctx = new ConferencesModelContext()) {
                ctx.Configuration.AutoDetectChangesEnabled = false;
                DaysOfConf = ctx.DaysOfConf.ToArray();
                Clients = ctx.Clients.ToArray().OrderBy(i => rnd.Next()).ToArray();
            }
            foreach(DaysOfConf doc in DaysOfConf) {
                Console.WriteLine($"{doc.DayOfConfID}, {doc.Date}");
            }
            Console.ReadLine();
            foreach (DaysOfConf doc in DaysOfConf) {
                Console.Write(".");
                CreateOrders(doc, Clients);
                Console.Write("[DB]");
            }
            
        }

        private void CreateOrders(DaysOfConf doc, Clients[] Clients) {
            using (ConferencesModelContext ctx = new ConferencesModelContext()) {
                ctx.Configuration.AutoDetectChangesEnabled = true;
                doc = ctx.DaysOfConf.SqlQuery($"SELECT TOP 1 * FROM DaysOfConf WHERE DayOfConfID = {doc.DayOfConfID}").ToArray()[0];
                DateTime AssignBoundary = new DateTime(2018, 1, 1);
                int NumOfChoosenParticip = 0;
                int ClientPoiter = rnd.Next(0, Clients.Length);
                while (NumOfChoosenParticip != doc.SpaceLimit) {
                    Clients ChoosenClient = ctx.Clients.SqlQuery($"SELECT TOP 1 * FROM Clients WHERE ClientID = {Clients[ClientPoiter].ClientID}").ToArray()[0]; 
                    ClientPoiter = (ClientPoiter + 1) % Clients.Length;
                    
                    ORM.Orders o = new ORM.Orders {
                        Clients = ChoosenClient,
                        DateOfBook = DateGen.GenerateDateBeforeGiven(doc.Date),
                        Status = false
                    };
                    ctx.Orders.Add(o);
                    ORM.Participants[] ParticipToAssign = ctx.Participants.SqlQuery(
                            "SELECT * FROM Participants p JOIN ParticipRegByClients pr ON p.ParticipantID = pr.ParticipantID " +
                            $"AND pr.ClientID = {ChoosenClient.ClientID}"
                            ).ToArray();
                    if (NumOfChoosenParticip + ParticipToAssign.Length > doc.SpaceLimit) {
                        ParticipToAssign = ParticipToAssign.Take(doc.SpaceLimit - NumOfChoosenParticip).ToArray();
                    }
                    NumOfChoosenParticip += ParticipToAssign.Length;
                    short NumOfStud = (short)
                                       (from p in ParticipToAssign
                                        where p.StudentCards.Count != 0
                                        select p.ParticipantID).Count();
                    short NumbOfReg = (short)(ParticipToAssign.Length - NumOfStud);
                    OrdersOnConfDays oocd = GenerateOrdOnConfDay(o, doc, NumbOfReg, NumOfStud, ctx);
                    if (doc.Date < AssignBoundary) {
                        GeneratePayments(o, ctx);
                        GenerateAssignation(ParticipToAssign, oocd, ctx);
                    }
                }
                ctx.SaveChanges();
            }
        }

        private OrdersOnConfDays GenerateOrdOnConfDay(ORM.Orders o, DaysOfConf doc, short NumOfRegSeats, short NumOfStudSeats,
            ConferencesModelContext ctx) {
            OrdersOnConfDays oocd = new OrdersOnConfDays {
                NumberOfRegularSeats = NumOfRegSeats,
                NumberOfStudentSeats = NumOfStudSeats,
                Orders = o,
                DaysOfConf = doc
            };
            doc.OrdersOnConfDays.Add(oocd);
            o.OrdersOnConfDays.Add(oocd);
            ctx.OrdersOnConfDays.Add(oocd);
            GenerateWorkshopSubOrder(oocd, ctx);
            return oocd;
        }

        private void GenerateWorkshopSubOrder(OrdersOnConfDays oocd, ConferencesModelContext ctx) {
            if (oocd.NumberOfRegularSeats + oocd.NumberOfStudentSeats == 1) return;
            foreach(Workshops w in oocd.DaysOfConf.Workshops) {
                int SpaceAlredayTaken = (from wso in w.WorkshopsSubOrders
                                          select (int)wso.NumberOfSeats).Sum();
                
                if(SpaceAlredayTaken != w.SpaceLimit) {
                    // we can add another participants to workshop (max half - to not worry about overlapping workshops
                    // because there is a trigger checking that)
                    int NumOfParticipPossibleToWork = (oocd.NumberOfRegularSeats + oocd.NumberOfStudentSeats) / 2;
                    if (w.SpaceLimit - SpaceAlredayTaken < NumOfParticipPossibleToWork)
                        NumOfParticipPossibleToWork = w.SpaceLimit - SpaceAlredayTaken;
                    WorkshopsSubOrders wso = new WorkshopsSubOrders {
                        NumberOfSeats = (short)NumOfParticipPossibleToWork,
                        OrdersOnConfDays = oocd,
                        Workshops = w
                    };
                    w.WorkshopsSubOrders.Add(wso);
                    oocd.WorkshopsSubOrders.Add(wso);
                    ctx.WorkshopsSubOrders.Add(wso);
                }
            }
        }

        private void GenerateAssignation(ORM.Participants[] ParticipToAssig, OrdersOnConfDays oocd,
            ConferencesModelContext ctx) {
            bool coin = true;
            foreach (ORM.Participants p in ParticipToAssig) {
                ParticipAtConfDay pacd = new ParticipAtConfDay {
                    OrdersOnConfDays = oocd,
                    Participants = p
                };
                oocd.ParticipAtConfDay.Add(pacd);
                p.ParticipAtConfDay.Add(pacd);
                ctx.ParticipAtConfDay.Add(pacd);
                AssignParticipantAtWorkshops(pacd, coin, ctx);
                coin = !coin;
            }
        }

        private void AssignParticipantAtWorkshops(ORM.ParticipAtConfDay pacd, bool coin, ConferencesModelContext ctx) {
            WorkshopsSubOrders[] WorkSubOrds = pacd.OrdersOnConfDays.WorkshopsSubOrders.ToArray();
            for(int i = coin == true ? 0 : 1; i < WorkSubOrds.Length; i += 2) {
                if(WorkSubOrds[i].NumberOfSeats > WorkSubOrds[i].ParticipAtConfDay.Count) {
                    WorkSubOrds[i].ParticipAtConfDay.Add(pacd);
                    pacd.WorkshopsSubOrders.Add(WorkSubOrds[i]);
                }
            }
        }

        private void GeneratePayments(ORM.Orders o, ConferencesModelContext ctx) {
            Person pers = pGen.GenerateSingleNameSurname();
            decimal val = o.CountValue();
            if(val > 0) {
                Payments p = new Payments {
                    Date = o.DateOfBook,
                    AccountNumber = accGen.GenerateSingleRandAccNumber(),
                    Orders = o,
                    TitleOfPayment = $"Payment for order #{o.GetHashCode()}",
                    TransferSender = $"{pers.Name} {pers.Surname}",
                    Value = val
                };
                o.Payments.Add(p);
                ctx.Payments.Add(p);
            }
        }
    }
}
