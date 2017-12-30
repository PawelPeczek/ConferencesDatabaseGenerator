using DataGenerator.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Generator {
    static class DataContextExtension {
        public static ConferencesModelContext BulkInsert<T>
            (this ConferencesModelContext ctx, T entity, int count, int batchSize) where T : class {
            ctx.Set<T>().Add(entity);
            if(count % batchSize == 0) {
                ctx.SaveChanges();
                ctx.Dispose();
                ctx = new ConferencesModelContext();
            }
            return ctx;
        }

        public static ConferencesModelContext RenewContext(this ConferencesModelContext ctx) {
            ctx.Database.Log = Console.Write;
            ctx.SaveChanges();
            ctx.Dispose();
            ctx = new ConferencesModelContext();
            return ctx;
        }
    }
}
