// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FileContextCore.FileManager;
using FileContextCore.Infrastructure.Internal;
using FileContextCore.Internal;
using FileContextCore.Query.Internal;
using FileContextCore.Serializer;
using FileContextCore.ValueGeneration.Internal;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Update;

namespace FileContextCore.Storage.Internal
{
    /// <summary>
    ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    class FileContextTable<TKey> : IFileContextTable
    {
		private readonly IPrincipalKeyValueFactory<TKey> _keyValueFactory;
        private readonly Dictionary<TKey, object[]> _rows;
		private readonly FileContextIntegerValueGeneratorCache idCache;
		private readonly FileContextOptionsExtension options;

		private IEntityType entityType;
		
		private IFileManager fileManager;
        private ISerializer serializer;
        private string filetype;

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public FileContextTable([NotNull] IPrincipalKeyValueFactory<TKey> keyValueFactory, IEntityType _entityType, FileContextIntegerValueGeneratorCache _idCache, FileContextOptionsExtension _options)
        {
			idCache = _idCache;
            entityType = _entityType;
			options = _options;
            _keyValueFactory = keyValueFactory;

            _rows = Init();
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual IReadOnlyList<object[]> SnapshotRows()
            => _rows.Values.ToList();

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual void Create(IUpdateEntry entry)
        {
            _rows.Add(CreateKey(entry), CreateValueBuffer(entry));
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual void Delete(IUpdateEntry entry)
        {
            TKey key = CreateKey(entry);

            if (_rows.ContainsKey(key))
            {
                _rows.Remove(key);
            }
            else
            {
                throw new DbUpdateConcurrencyException(FileContextStrings.UpdateConcurrencyException, new[] { entry });
            }
        }

        public void Save()
        {
            UpdateMethod(_rows);
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual void Update(IUpdateEntry entry)
        {
            TKey key = CreateKey(entry);

            if (_rows.ContainsKey(key))
            {
                List<IProperty> properties = entry.EntityType.GetProperties().ToList();
                object[] valueBuffer = new object[properties.Count];

                for (int index = 0; index < valueBuffer.Length; index++)
                {
                    valueBuffer[index] = entry.IsModified(properties[index])
                        ? entry.GetCurrentValue(properties[index])
                        : _rows[key][index];
                }

                _rows[key] = valueBuffer;
            }
            else
            {
				throw new DbUpdateConcurrencyException(FileContextStrings.UpdateConcurrencyException, new[] { entry });
            }
        }

        private TKey CreateKey(IUpdateEntry entry)
            => _keyValueFactory.CreateFromCurrentValues((InternalEntityEntry)entry);

        private static object[] CreateValueBuffer(IUpdateEntry entry)
            => entry.EntityType.GetProperties().Select(entry.GetCurrentValue).ToArray();

        private Action<Dictionary<TKey, object[]>> UpdateMethod;

        private Dictionary<int, string> GetAutoGeneratedFields()
        {
            IProperty[] props = entityType.GetProperties().ToArray();
            Dictionary<int, string> AutoGeneratedFields = new Dictionary<int, string>();

            for (int i = 0; i < props.Length; i++)
            {
                if (props[i].ValueGenerated == ValueGenerated.OnAdd || props[i].ValueGenerated == ValueGenerated.OnAddOrUpdate || props[i].ValueGenerated == ValueGenerated.OnUpdate)
                {
					if (props[i].ClrType == typeof(long) || props[i].ClrType == typeof(int) || props[i].ClrType == typeof(short) || props[i].ClrType == typeof(byte) || props[i].ClrType == typeof(ulong) || props[i].ClrType == typeof(uint) || props[i].ClrType == typeof(ushort) || props[i].ClrType == typeof(sbyte))
					{
						AutoGeneratedFields.Add(i, props[i].Name);
					}
                }
            }

            return AutoGeneratedFields;
        }

        private void GenerateLastAutoPropertyValues(Dictionary<TKey, object[]> list)
        {
            Dictionary<int, string> fields = GetAutoGeneratedFields();

            if (fields.Any())
            {
                Dictionary<string, long> values = new Dictionary<string, long>();

                foreach (KeyValuePair<int, string> val in fields)
                {
                    object last = list.Select(p => p.Value[val.Key]).OrderByDescending(p => p).FirstOrDefault();

                    if (last != null)
                    {
                        values.Add(val.Value, (long)Convert.ChangeType(last, typeof(long), CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        values.Add(val.Value, 0);
                    }
                }

                if (idCache.LastIds.ContainsKey(entityType.Name))
                {
					idCache.LastIds[entityType.Name] = values;
                }
                else {
                    idCache.LastIds.Add(entityType.Name, values);
                }
            }
        }

        private Dictionary<TKey, object[]> Init()
        {
            filetype = options.Serializer;

            if (filetype.Length >= 5 && filetype.Substring(0, 5) == "excel")
            {
                string password = "";

                if (filetype.Length > 5)
                {
                    password = filetype.Substring(6);
                }

                EXCELSerializer excel = new EXCELSerializer(entityType, password, options.DatabaseName);

                UpdateMethod = new Action<Dictionary<TKey, object[]>>((list) =>
                {
                    excel.Serialize(list);
                });

                Dictionary<TKey, object[]> newlist = new Dictionary<TKey, object[]>(_keyValueFactory.EqualityComparer);
                Dictionary<TKey, object[]> excelresult = excel.Deserialize(newlist);
                GenerateLastAutoPropertyValues(excelresult);
                return excelresult;
            }

            if (options.Serializer == "xml")
            {
                serializer = new XMLSerializer(entityType);
            }
            else if (options.Serializer == "bson")
            {
                serializer = new BSONSerializer(entityType);
            }
            else if (options.Serializer == "csv")
            {
                serializer = new CSVSerializer(entityType);
            }
            else
            {
                serializer = new JSONSerializer(entityType);
            }

            string fmgr = options.FileManager;

            if (fmgr.Length >= 9 && fmgr.Substring(0, 9) == "encrypted")
            {
                string password = "";

                if (fmgr.Length > 9)
                {
                    password = fmgr.Substring(10);
                }

                fileManager = new EncryptedFileManager(entityType, filetype, password, options.DatabaseName);
            }
            else if (fmgr == "private")
            {
                fileManager = new PrivateFileManager(entityType, filetype, options.DatabaseName);
            }
            else
            {
                fileManager = new DefaultFileManager(entityType, filetype, options.DatabaseName);
            }

            UpdateMethod = new Action<Dictionary<TKey, object[]>>((list) =>
            {
                string cnt = serializer.Serialize(list);
                fileManager.SaveContent(cnt);
            });

            string content = fileManager.LoadContent();
            Dictionary<TKey, object[]> newList = new Dictionary<TKey, object[]>(_keyValueFactory.EqualityComparer);
            Dictionary<TKey, object[]> result = serializer.Deserialize(content, newList);
            GenerateLastAutoPropertyValues(result);
            return result;
        }
    }
}
