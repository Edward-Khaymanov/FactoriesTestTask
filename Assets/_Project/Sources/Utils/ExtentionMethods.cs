using System;
using System.Collections.Generic;
using System.Linq;

public static class ExtentionMethods
{
    public static List<T> GetEnumValues<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T)).Cast<T>().ToList();
    }
}