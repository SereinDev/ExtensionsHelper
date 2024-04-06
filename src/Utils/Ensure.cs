using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtensionsHelper.Utils;

public static class Ensure
{
    public static void NotEmpty<T>(IEnumerable<T> objects, string paramName)
    {
        if (!objects.Any())
            throw new NotSupportedException($"{paramName} 不得为空");
    }

    public static void NotNullOrEmpty(string text, string paramName)
    {
        if (string.IsNullOrEmpty(text))
            throw new NotSupportedException($"{paramName} 不得为null或空");
    }

    public static void True(bool condition, string msg)
    {
        if (!condition)
            throw new NotSupportedException(msg);
    }
}
