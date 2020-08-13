// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FileContextCore.Infrastructure.Internal;
using FileContextCore.Utilities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using Remotion.Linq;

namespace FileContextCore.Storage.Internal
{
    /// <summary>
    ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    class FileContextDatabase : Database, IFileContextDatabase
    {
        private readonly IFileContextStore _store;
        private readonly IDiagnosticsLogger<DbLoggerCategory.Update> _updateLogger;

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public FileContextDatabase(
            [NotNull] DatabaseDependencies dependencies,
            [NotNull] IFileContextStoreCache storeCache,
            [NotNull] IDbContextOptions options,
            [NotNull] IDiagnosticsLogger<DbLoggerCategory.Update> updateLogger)
            : base(dependencies)
        {
            Check.NotNull(storeCache, nameof(storeCache));
            Check.NotNull(options, nameof(options));
            Check.NotNull(updateLogger, nameof(updateLogger));

            _store = storeCache.GetStore(options.Extensions.OfType<FileContextOptionsExtension>().First());
            _updateLogger = updateLogger;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual IFileContextStore Store => _store;

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public override int SaveChanges(IReadOnlyList<IUpdateEntry> entries)
            => _store.ExecuteTransaction(Check.NotNull(entries, nameof(entries)), _updateLogger);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public override Task<int> SaveChangesAsync(
            IReadOnlyList<IUpdateEntry> entries,
            CancellationToken cancellationToken = default(CancellationToken))
            => Task.FromResult(_store.ExecuteTransaction(Check.NotNull(entries, nameof(entries)), _updateLogger));

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual bool EnsureDatabaseCreated(IModel model)
            => _store.EnsureCreated(Check.NotNull(model, nameof(model)));

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public override Func<QueryContext, IAsyncEnumerable<TResult>> CompileAsyncQuery<TResult>(QueryModel queryModel)
        {
            Check.NotNull(queryModel, nameof(queryModel));

            Func<QueryContext, IEnumerable<TResult>> syncQueryExecutor = CompileQuery<TResult>(queryModel);

            return qc => syncQueryExecutor(qc).ToAsyncEnumerable();
        }
    }
}
