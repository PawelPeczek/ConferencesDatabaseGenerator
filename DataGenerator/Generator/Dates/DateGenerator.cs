using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Generator.Dates {
    class DateGenerator {
        private Random rnd = new Random(Guid.NewGuid().GetHashCode());
        public DateTime GenerateDateBeforeGiven(DateTime endDate) {
            return endDate.AddDays(-1 * rnd.Next(0, 61));
        }
        /*
         * lower and upper bounds -> only months matters 
         */
        public DatePeriod[] GenerateNonOverlapingPeriods
            (DateTime lowerBoud, DateTime upperBound, int monthInterval, int numbOfPeriods) {
            int diff = MonthsDiff(lowerBoud, upperBound);
            if (monthInterval * numbOfPeriods > diff)
                throw new InvalidOperationException("Unable to generate so much periods from so shord date range");
            // choosing slots
            int[] slots = Enumerable
                .Range(0, diff / monthInterval)
                .OrderBy(i => rnd.Next()).Take(numbOfPeriods).ToArray();
            DatePeriod[] res = new DatePeriod[numbOfPeriods];
            for(int i = 0; i < numbOfPeriods; i++) {
                DateTime start, end;
                start = lowerBoud.AddMonths(slots[i] * monthInterval);
                end = lowerBoud.AddMonths((slots[i] + 1) * monthInterval).AddDays(-1);
                res[i] = new DatePeriod(start, end);
            }
            return res;
        }

        private int MonthsDiff(DateTime start, DateTime end) {
            return ((end.Year - start.Year) * 12) + end.Month - start.Month;
        }
    }
}
