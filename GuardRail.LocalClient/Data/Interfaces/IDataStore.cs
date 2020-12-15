﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace GuardRail.LocalClient.Data.Interfaces
{
    /// <summary>
    /// A definition for a data store.
    /// </summary>
    internal interface IDataStore : IDisposable
    {
        /// <summary>
        /// Starts the syncing for all sinks.
        /// </summary>
        void StartSync();
        
        /// <summary>
        /// Save a new item of type T.
        /// </summary>
        /// <typeparam name="T">The type to save a new of.</typeparam>
        /// <param name="item">The item to save a new of.</param>
        /// <param name="cancellationToken">The CancellationToken that can cancel the request.</param>
        /// <returns>Task of T</returns>
        Task<T> SaveNew<T>(
            T item,
            CancellationToken cancellationToken)
            where T : class, IAddableItem;
        
        /// <summary>
        /// Update an existing item.
        /// </summary>
        /// <typeparam name="T">The type of item to update.</typeparam>
        /// <param name="item">The item to update.</param>
        /// <param name="cancellationToken">The CancellationToken that can cancel the request.</param>
        /// <returns>Task</returns>
        Task UpdateExisting<T>(
            T item,
            CancellationToken cancellationToken);

        /// <summary>
        /// Delete and existing item.
        /// </summary>
        /// <typeparam name="T">The type of item to delete.</typeparam>
        /// <param name="item">The item to update.</param>
        /// <param name="cancellationToken">The CancellationToken that can cancel the request.</param>
        /// <returns></returns>
        Task DeleteExisting<T>(
            T item,
            CancellationToken cancellationToken);

        /// <summary>
        /// Retrieve data from the data sick.
        /// </summary>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <param name="getExpression">The expression for how to get data.</param>
        /// <param name="cancellationToken">The CancellationToken that can cancel the request.</param>
        /// <returns>Task of IQueryable of T</returns>
        Task<IQueryable<T>> Get<T>(
            Expression<Func<IQueryable<T>>> getExpression,
            CancellationToken cancellationToken);
    }
}