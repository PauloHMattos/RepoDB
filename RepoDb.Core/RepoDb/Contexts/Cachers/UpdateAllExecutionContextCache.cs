﻿using RepoDb.Contexts.Execution;
using System.Collections.Concurrent;

namespace RepoDb.Contexts.Cachers
{
    /// <summary>
    /// A class that is being used to cache the execution context of the UpdateAll operation.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public static class UpdateAllExecutionContextCache<TEntity>
    {
        private static ConcurrentDictionary<string, UpdateAllExecutionContext<TEntity>> cache = new();

        /// <summary>
        /// Flushes all the cached execution context.
        /// </summary>
        public static void Flush() =>
            cache.Clear();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="context"></param>
        internal static void Add(string key,
            UpdateAllExecutionContext<TEntity> context) =>
            cache.TryAdd(key, context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static UpdateAllExecutionContext<TEntity> Get(string key)
        {
            if (cache.TryGetValue(key, out var result))
            {
                return result;
            }
            return null;
        }
    }
}
