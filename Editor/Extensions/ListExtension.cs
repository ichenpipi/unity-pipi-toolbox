using System.Collections.Generic;
using System.Text;

namespace ChenPipi.PipiToolbox.Editor
{

    internal static class ListExtension
    {

        internal static string Join<T>(this IList<T> list, string separator = ",")
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < list.Count; ++i)
            {
                builder.Append(list[i]);
                if (i < list.Count - 1)
                {
                    builder.Append(separator);
                }
            }
            return builder.ToString();
        }

    }

}
