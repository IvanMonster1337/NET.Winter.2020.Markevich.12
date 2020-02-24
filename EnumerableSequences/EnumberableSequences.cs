using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GenericsDemo;

namespace GenericsDemo
{
    public static class EnumberableSequences
    {
        /// <summary>Filters array by filter.</summary>
        /// <typeparam name="TSource">Type of source array.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="predicate">The filter.</param>
        /// <returns>Filtered array.</returns>
        public static IEnumerable<TSource> FilterBy<TSource>(this IEnumerable<TSource> source, IPredicate<TSource> predicate)
        {
            CheckParams(source, predicate);
            foreach (var item in source)
            {
                if (predicate.IsMatch(item))
                {
                    yield return item;
                }
            }
        }

        /// <summary>Transforms source array into array of the specified type following specified rule.</summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="transformer">The transformer.</param>
        /// <returns>Transformed array.</returns>
        /// <exception cref="ArgumentNullException">Source array is null or transforming rule is null.</exception>
        /// <exception cref="ArgumentException">Source array is empty.</exception>
        public static IEnumerable<TResult> Transform<TSource, TResult>(this IEnumerable<TSource> source, ITransformer<TSource, TResult> transformer)
        {
            CheckParams(source, transformer);
            foreach (var element in source)
            {
                yield return transformer.Transform(element);
            }
        }

        /// <summary>Orders the array according to some rule.</summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="comparer">The comparing rule.</param>
        /// <returns>Transformed array.</returns>
        /// <exception cref="ArgumentNullException">Source array is null or comparing rule is null.</exception>
        public static IEnumerable<TSource> OrderAccordingTo<TSource>(this IEnumerable<TSource> source, IComparer<TSource> comparer)
        {
            CheckParams(source, comparer);
            TSource element;

            var returnArray = new TSource[source.Length()];
            int i = 0;

            foreach (var sourceElement in source)
            {
                returnArray[i] = sourceElement;
                i++;
            }

            for (i = 0; i < returnArray.Length; i++)
            {
                for (int j = i; j < returnArray.Length; j++)
                {
                    if (comparer.Compare(returnArray[i], returnArray[j]) < 0)
                    {
                        element = returnArray[i];
                        returnArray[i] = returnArray[j];
                        returnArray[j] = element;
                    }
                }
            }

            foreach (var sourceElement in returnArray)
            {
                yield return sourceElement;
            }
        }

        /// <summary>  Returns source array's elements of the specified type.</summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>Returns the array of specified type.</returns>
        /// <exception cref="ArgumentNullException">Source is null.</exception>
        public static IEnumerable<TSource> TypeOf<TSource>(this IEnumerable source)
        {
            CheckParams(source);
            foreach (var element in source)
            {
                if (element is TSource)
                {
                    yield return (TSource)element;
                }
            }
        }

        /// <summary>Reverses the specified source.</summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>Reversed array.</returns>
        /// <exception cref="ArgumentNullException">Source array is null.</exception>
        public static IEnumerable<TSource> Reverse<TSource>(this IEnumerable<TSource> source)
        {
            CheckParams(source);

            TSource[] returnArray = new TSource[source.Length()];
            int i = source.Length() - 1;
            foreach (var element in source)
            {
                returnArray[i] = element;
                i--;
            }

            foreach (var element in returnArray)
            {
                yield return element;
            }
        }

        public static int Length<TSource>(this IEnumerable<TSource> enumerable)
        {
            int length = 0;
            foreach (var element in enumerable)
            {
                length++;
            }

            return length;
        }

        /// <summary>Checks the parameters.</summary>
        /// <param name="source">The source.</param>
        /// <exception cref="ArgumentNullException">source is null.</exception>
        private static void CheckParams(object source)
        {
            source = source ?? throw new ArgumentNullException($"{nameof(source)} cannot be null.");
        }

        /// <summary>Checks the parameters.</summary>
        /// <param name="source">The source.</param>
        /// <exception cref="ArgumentNullException">source is null or param is null.</exception>
        private static void CheckParams(object source, object param)
        {
            CheckParams(source);
            param = param ?? throw new ArgumentNullException($"{nameof(param)} cannot be null.");
        }
    }
}