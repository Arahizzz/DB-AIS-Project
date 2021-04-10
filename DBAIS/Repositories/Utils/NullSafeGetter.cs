﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DBAIS.Repositories.Utils
{
    public static class NullSafeGetter
    {
        public static T GetValueOrDefault<T>(this IDataRecord row, string fieldName)
        {
            int ordinal = row.GetOrdinal(fieldName);
            return row.GetValueOrDefault<T>(ordinal);
        }

        public static T GetValueOrDefault<T>(this IDataRecord row, int ordinal)
        {
            return (T)(row.IsDBNull(ordinal) ? default(T) : row.GetValue(ordinal));
        }
    }
}
