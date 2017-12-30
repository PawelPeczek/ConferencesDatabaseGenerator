using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Generator.Dates {
    class DatePeriod {
        public DateTime startDate { get; private set; }
        public DateTime endDate { get; private set; }
        public DatePeriod(DateTime start, DateTime end) {
            startDate = start;
            endDate = end;
        }

        public bool Overlaps(DatePeriod o) {
            bool result = false;
            if ((startDate >= o.startDate && startDate <= o.endDate) ||
                (o.startDate >= startDate && o.startDate <= endDate))
                result = true;
            return result;
        }
    }
}
