using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Constants
{
    public static class SortKey
    {
        public const string by = nameof(by);
        public const string order = nameof(order);
    }

    public static class SortBy
    {
        public const string name = nameof(name);
    }

    public static class SortOrder
    {
        public const string asc = nameof(asc);
        public const string desc = nameof(desc);
    }
}
