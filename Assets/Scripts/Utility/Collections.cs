using System;
using System.Collections.Generic;
using System.Linq;

namespace TrenchWarfare.Utility {
    public static class Collections {
        public static int IndexOf<T>(
            this IEnumerable<T> collection,
            Func<T, bool> check
        ) {
            return collection.TakeWhile(i => !check(i)).Count();
        }

       public static bool IsNotEmpty<T>(this IEnumerable<T> collection) {
            return collection.Any();
       }

       public static T GetByIndex<T>(this IEnumerable<T> collection, int index) {
            int idx = 0;
            foreach (T i in collection) {
                if (idx == index)
                    return i;
                idx++;
            }

            throw new ArgumentOutOfRangeException("at", "expected value less then " + idx);
       }
    }
}
