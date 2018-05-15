using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllPodcast.Extensions
{
    public static class LinqExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
                action(item);
        }

        public static async Task<IEnumerable<T>> WhenAll<T>(this IEnumerable<Task<T>> tasks) =>
            await Task.WhenAll(tasks);

        private static async Task<IEnumerable<TResult>> SelectManyAsync<TSource, TResult>(
            this IEnumerable<TSource> enumeration, Func<TSource, Task<IEnumerable<TResult>>> predicate) =>
            (await Task.WhenAll(enumeration.Select(predicate))).SelectMany(s => s);

        public static async Task<IEnumerable<TResult>> SelectManyAsync<TSource, TResult>(
            this IEnumerable<TSource> enumeration, Func<TSource, Task<List<TResult>>> predicate) => 
            (await Task.WhenAll(enumeration.Select(predicate))).SelectMany(s => s);

        public static async Task<List<T>> ToListAsync<T>(this Task<IEnumerable<T>> enumerable) =>
            (await enumerable).ToList();
    }
}