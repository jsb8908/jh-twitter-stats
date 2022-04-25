using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jh_twitter_stats_api_tests
{
    public static class Helpers
    {
        public static bool IsInDescendingOrder<T>(this IList<T> list, IComparer<T> comparer = null)
        {
            if (comparer == null)
            {
                comparer = Comparer<T>.Default;
            }

            if (list.Count > 1)
            {
                for (int i = 1; i < list.Count; i++)
                {
                    if (comparer.Compare(list[i - 1], list[i]) < 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
