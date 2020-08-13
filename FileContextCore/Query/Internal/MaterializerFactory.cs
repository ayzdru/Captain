// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Linq.Expressions;
using FileContextCore.Utilities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace FileContextCore.Query.Internal
{
    /// <summary>
    ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    class MaterializerFactory : IMaterializerFactory
    {
        private readonly IEntityMaterializerSource _entityMaterializerSource;

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public MaterializerFactory([NotNull] IEntityMaterializerSource entityMaterializerSource)
        {
            Check.NotNull(entityMaterializerSource, nameof(entityMaterializerSource));

            _entityMaterializerSource = entityMaterializerSource;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual Expression<Func<IEntityType, ValueBuffer, object>> CreateMaterializer(IEntityType entityType)
        {
            Check.NotNull(entityType, nameof(entityType));

            ParameterExpression entityTypeParameter
                = Expression.Parameter(typeof(IEntityType), "entityType");

            ParameterExpression valueBufferParameter
                = Expression.Parameter(typeof(ValueBuffer), "valueBuffer");

            System.Collections.Generic.List<IEntityType> concreteEntityTypes
                = entityType.GetConcreteTypesInHierarchy().ToList();

            if (concreteEntityTypes.Count == 1)
            {
                return Expression.Lambda<Func<IEntityType, ValueBuffer, object>>(
                    _entityMaterializerSource
                        .CreateMaterializeExpression(
                            concreteEntityTypes[0], valueBufferParameter),
                    entityTypeParameter,
                    valueBufferParameter);
            }

            LabelTarget returnLabelTarget = Expression.Label(typeof(object));

            Expression[] blockExpressions
                = new Expression[]
                {
                    Expression.IfThen(
                        Expression.Equal(
                            entityTypeParameter,
                            Expression.Constant(concreteEntityTypes[0])),
                        Expression.Return(
                            returnLabelTarget,
                            _entityMaterializerSource
                                .CreateMaterializeExpression(
                                    concreteEntityTypes[0], valueBufferParameter))),
                    Expression.Label(
                        returnLabelTarget,
                        Expression.Default(returnLabelTarget.Type))
                };

            foreach (IEntityType concreteEntityType in concreteEntityTypes.Skip(1))
            {
                blockExpressions[0]
                    = Expression.IfThenElse(
                        Expression.Equal(
                            entityTypeParameter,
                            Expression.Constant(concreteEntityType)),
                        Expression.Return(
                            returnLabelTarget,
                            _entityMaterializerSource
                                .CreateMaterializeExpression(concreteEntityType, valueBufferParameter)),
                        blockExpressions[0]);
            }

            return Expression.Lambda<Func<IEntityType, ValueBuffer, object>>(
                Expression.Block(blockExpressions),
                entityTypeParameter,
                valueBufferParameter);
        }
    }
}
