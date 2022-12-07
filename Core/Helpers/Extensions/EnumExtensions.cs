using System;
using System.Linq;

namespace Helpers.Extensions;

public static class EnumExtensions
{
    public static TAttribute GetAttribute<TAttribute>(this Enum value)
        where TAttribute : Attribute
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);
        return type.GetField(name ?? string.Empty)?.GetCustomAttributes(false)
            .OfType<TAttribute>()
            .SingleOrDefault()!;
    }
}