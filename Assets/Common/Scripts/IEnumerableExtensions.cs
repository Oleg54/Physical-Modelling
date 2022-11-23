using System.Collections.Generic;
using System;
using System.Linq;

public static class IEnumerableExtensions
{
    public static bool Exist<TValue>(this IEnumerable<TValue> enumerable, Func<TValue, bool> comparer)
    {
        foreach (TValue value in enumerable)
            if (comparer(value))
                return true;

        return false;
    }

    public static int IndexOf<TElement>(this IEnumerable<TElement> collection, Predicate<TElement> comparison)
    {
        int index = 0;

        foreach (TElement e in collection)
        {
            if (comparison(e))
                return index;

            index++;
        }

        return -1;
    }

    public static int IndexOfMin<TElement>(this IEnumerable<TElement> collection, Comparison<TElement> comparison)
    {
        return collection.IndexOfMaxOrMin(comparison, comparisonResult => comparisonResult > 0);
    }

    public static int IndexOfMax<TElement>(this IEnumerable<TElement> collection, Comparison<TElement> comparison)
    {
        return collection.IndexOfMaxOrMin<TElement>(comparison, comparisonResult => comparisonResult < 0);
    }

    private static int IndexOfMaxOrMin<TElement>(this IEnumerable<TElement> collection,
        Comparison<TElement> comparison, Predicate<int> comparisonComparer)
    {
        if (collection.Count() is 0)
            return -1;

        int currentIndex = 0;
        int resultIndex = 0;
        TElement element = collection.First();

        foreach (TElement item in collection)
        {
            if (comparisonComparer(comparison(element, item)))
            {
                resultIndex = currentIndex;
                element = item;
            }

            currentIndex++;
        }

        return resultIndex;
    }

    public static TElement Find<TElement>(this IEnumerable<TElement> elements, Predicate<TElement> predicate)
    {
        foreach (TElement element in elements)
            if (predicate(element))
                return element;

        return default;
    }
}