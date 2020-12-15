﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using GuardRail.Core.Helpers;
using GuardRail.LocalClient.Data.Interfaces;
using GuardRail.LocalClient.Interfaces;

namespace GuardRail.LocalClient.Data.Remote
{
    /// <summary>
    /// A remote stored IDataSink implementation.
    /// </summary>
    internal sealed class RemoteDataSink : IDataSink
    {
        private readonly IGuardRailFileProvider _guardRailFileProvider;
        private readonly IGuardRailApiClient _guardRailApiClient;
        private readonly ConcurrentQueue<RemoteSinkAction> _concurrentQueue;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly GuardRailBackgroundWorker _guardRailRemoteSyncBackgroundWorker;
        private readonly string _dataStoreFilePath;

        /// <summary>
        /// A remote stored IDataSink implementation.
        /// </summary>
        internal RemoteDataSink(
            IGuardRailFileProvider guardRailFileProvider,
            IGuardRailApiClient guardRailApiClient)
        {
            _guardRailFileProvider = guardRailFileProvider;
            _guardRailApiClient = guardRailApiClient;
            _concurrentQueue = LoadSavedQueue();
            _cancellationTokenSource = new CancellationTokenSource();
            _guardRailRemoteSyncBackgroundWorker = GuardRailBackgroundWorker.Create(
                "Remote Sink Try Push Changes",
                TimeSpan.FromSeconds(2),
                TryPushChanges,
                CancellationToken.None);
            _dataStoreFilePath = Path.Combine("Data", "Remote", "store.gr");
        }

        private ConcurrentQueue<RemoteSinkAction> LoadSavedQueue()
        {
            var store = _guardRailFileProvider.GetFileInfo(_dataStoreFilePath);
            if (!store.Exists)
            {
                return new ConcurrentQueue<RemoteSinkAction>();
            }

            var contents = store.CreateReadStream().FromJson<List<RemoteSinkAction>>();
            return new ConcurrentQueue<RemoteSinkAction>(contents);
        }

        private async Task TryPushChanges(CancellationToken ct)
        {
            var savedQueue = LoadSavedQueue();
            if (_concurrentQueue.Count <= 0 && savedQueue.Count <= 0)
            {
                return;
            }

            var changesToPush = new List<RemoteSinkAction>();
            if (savedQueue.Count > 0)
            {
                while (savedQueue.TryDequeue(out var item))
                {
                    changesToPush.Add(item);
                }
            }
            
            while (_concurrentQueue.TryDequeue(out var item))
            {
                changesToPush.Add(item);
            }
            
            try
            {
                await _guardRailApiClient.UploadChangesAsync(changesToPush, ct);
                _guardRailFileProvider.Delete(_dataStoreFilePath);
            }
            catch
            {
                await _guardRailFileProvider.AppendOrCreateAsync(_dataStoreFilePath, changesToPush.ToJson(), ct);
            }
        }

        /// <inheritdoc />
        public Task<T> SaveNew<T>(
            T item,
            CancellationToken cancellationToken) where T : class
        {
            _concurrentQueue.Enqueue(
                new RemoteSinkAction
                {
                    RemoteSinkActionType = RemoteSinkActionType.Add,
                    Type = typeof(T),
                    Item = item
                });
            return Task.FromResult(item);
        }

        /// <inheritdoc />
        public Task UpdateExisting<T>(
            T item,
            CancellationToken cancellationToken)
        {
            _concurrentQueue.Enqueue(
                new RemoteSinkAction
                {
                    RemoteSinkActionType = RemoteSinkActionType.Update,
                    Type = typeof(T),
                    Item = item
                });
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task DeleteExisting<T>(
            T item,
            CancellationToken cancellationToken)
        {
            _concurrentQueue.Enqueue(
                new RemoteSinkAction
                {
                    RemoteSinkActionType = RemoteSinkActionType.Delete,
                    Type = typeof(T),
                    Item = item
                });
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<IQueryable<T>> Get<T>(
            Expression<Func<IQueryable<T>>> getExpression,
            CancellationToken cancellationToken) =>
            _guardRailApiClient.GetDataFromQuery(getExpression, cancellationToken);

        /// <inheritdoc />
        public void Dispose()
        {
            _cancellationTokenSource.Dispose();
            _guardRailRemoteSyncBackgroundWorker.Dispose();
        }
    }
}