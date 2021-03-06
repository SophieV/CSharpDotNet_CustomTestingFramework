﻿using System;
using System.Collections.Generic;

namespace TestMVC4App.Models
{
    /// <summary>
    /// Linq Extension that allows to Flatten a tree.
    /// </summary>
    public static class LinqTreeExtension
    {
        public static IEnumerable<T> SelectNestedChildren<T>
            (this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
        {
            foreach (T item in source)
            {
                yield return item;
                foreach (T subItem in SelectNestedChildren(selector(item), selector))
                {
                    yield return subItem;
                }
            }
        }
    }

}