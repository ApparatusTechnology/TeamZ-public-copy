using System;
using System.Collections.Generic;

public static class LinqExtentions
{
    public static void ForEach<TValue>(this IEnumerable<TValue> values, Action<TValue> action)
    {
        foreach (var item in values)
        {
            action(item);
        }
    }

    public static TValue SelectRandom<TValue>(this IReadOnlyList<TValue> values)
    {
        var count = values.Count;
        var item = UnityEngine.Random.Range(0, count);

        return values[item];
    }
    
    public static int IndexOf<TValue>(this IReadOnlyList<TValue> values, Func<TValue, bool> preficate)
    {
        for (int i = 0; i < values.Count; i++)
        {
            if (preficate(values[i]))
            {
                return i;
            }
        }
        
        return -1;
    }

    public static IEnumerable<LinkedList<TValue>> Window<TValue>(this IEnumerable<TValue> values, int size)
    {
        var list = new LinkedList<TValue>();
        foreach (var value in values)
        {
            list.AddLast(value);

            if (list.Count < size)
            {
                continue;
            }

            yield return list;
            list.RemoveFirst();
        }
    }
    
    public static IEnumerable<TValue> With<TValue>(this TValue value, IEnumerable<TValue> values)
    {
        yield return value;
        foreach (var item in values)
        {
            yield return item;
        }
    }
}
