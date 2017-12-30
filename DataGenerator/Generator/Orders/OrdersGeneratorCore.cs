using DataGenerator.Generator.Dates;
using DataGenerator.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Generator.Orders {
    class OrdersGeneratorCore {
        private Random rnd = new Random(Guid.NewGuid().GetHashCode());
        private DateGenerator DateGen;
        public OrdersGeneratorCore() {
            DateGen = new DateGenerator();
        }
        public void GenerateBatchOfOrders() {
            using(ConferencesModelContext ctx = new ConferencesModelContext()) {
                ctx.Configuration.AutoDetectChangesEnabled = false;
                ctx.Database.Log = Console.Write;
                DaysOfConf[] DaysOfConf = ctx.DaysOfConf.ToArray();
                Clients[] Clients = ctx.Clients.ToArray();
                foreach(DaysOfConf doc in DaysOfConf) {
                    CreateOrders(doc, Clients, ctx);
                    ctx.SaveChanges();
                }
            }
            
        }

        private void CreateOrders(DaysOfConf doc, Clients[] Clients, ConferencesModelContext ctx) {
            int FulfilledSpace = 0;
            int NumOfChoosenParticip = 0;
            while (FulfilledSpace != doc.SpaceLimit) {
                Clients ChoosenClient = Clients[rnd.Next(0, Clients.Length)];
                ORM.Orders o = new ORM.Orders {
                    Clients = ChoosenClient,
                    DateOfBook = DateGen.GenerateDateBeforeGiven(doc.Date),
                    Status = false
                };
                ctx.Orders.Add(o);
                ORM.Participants[] ParticipToAssign;
                if (NumOfChoosenParticip + ChoosenClient.Participants.Count <= doc.SpaceLimit) {
                    NumOfChoosenParticip += ChoosenClient.Participants.Count;
                    ParticipToAssign = ChoosenClient.Participants.ToArray();
                } else {
                    ParticipToAssign = ChoosenClient.Participants
                                        .Take(doc.SpaceLimit - NumOfChoosenParticip)
                                        .ToArray();
                    NumOfChoosenParticip = doc.SpaceLimit;
                }
                short NumOfStud = (short)
                                   (from p in ParticipToAssign
                                   where p.StudentCards.Count == 0
                                   select p.ParticipantID).Count();
                short NumbOfReg = (short) (doc.SpaceLimit - NumOfStud);
                OrdersOnConfDays oocd = GenerateOrdOnConfDay(o, doc, NumbOfReg, NumOfStud, ctx);
                GenerateAssignation(ParticipToAssign, oocd, ctx);
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
            return oocd;
        }

        private void GenerateWorkshopSubOrder(OrdersOnConfDays oocd, ConferencesModelContext ctx) {
            foreach(Workshops w in oocd.DaysOfConf.Workshops) {
                int SpaceAlredayTaken = (from wso in w.WorkshopsSubOrders
                                          select (int)wso.NumberOfSeats).Sum();
                if(SpaceAlredayTaken != w.SpaceLimit) {
                    // we can add another participants to workshop (max 5 - to not worry about overlapping workshops
                    // because there is a trigget checking that)
                    int NumOfParticipPossibleToWork = oocd.NumberOfRegularSeats + oocd.NumberOfRegularSeats;
                    NumOfParticipPossibleToWork = NumOfParticipPossibleToWork > 5 ? 5 : NumOfParticipPossibleToWork;
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
            foreach (ORM.Participants p in ParticipToAssig) {
                ParticipAtConfDay pacd = new ParticipAtConfDay {
                    OrdersOnConfDays = oocd,
                    Participants = p
                };
                oocd.ParticipAtConfDay.Add(pacd);
                p.ParticipAtConfDay.Add(pacd);
                ctx.ParticipAtConfDay.Add(pacd);
            }
        }
    }
}
