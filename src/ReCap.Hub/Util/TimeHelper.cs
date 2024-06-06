using System;
using System.Collections.Generic;
using System.Linq;

namespace ReCap.Hub
{
    public static class TimeHelper
    {
        static bool TrySortByTimeFunc<T>(IEnumerable<T> values, Func<T, double> getTimeFunc, out IEnumerable<T> sorted)
        {
            if (values.Count() > 0)
            {
                sorted = values.OrderBy(v => getTimeFunc(v));
                return true;
            }
            else
            {
                sorted = default;
                return false;
            }
        }


        public static bool TryGetNewest<T>(IEnumerable<T> values, Func<T, double> getTimeFunc, out T mostRecent)
        {
            mostRecent = default;
            if (!TrySortByTimeFunc(values, getTimeFunc, out IEnumerable<T> sorted))
                return false;

            mostRecent = sorted.FirstOrDefault();
            return mostRecent != null;
        }

        
        public static bool TryGetOldest<T>(IEnumerable<T> values, Func<T, double> getTimeFunc, out T mostRecent)
        {
            mostRecent = default;
            if (!TrySortByTimeFunc(values, getTimeFunc, out IEnumerable<T> sorted))
                return false;

            mostRecent = sorted.LastOrDefault();
            return mostRecent != null;
        }
    }
}