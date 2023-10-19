﻿using System;
using System.Linq.Expressions;
using System.Reflection;
using RepoDb.Extensions;

namespace RepoDb.Reflection
{
    internal partial class Compiler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entityType"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        internal static Action<TEntity, object> CompileDataEntityPropertySetter<TEntity>(Type entityType,
            Field field)
        {
            // Get the entity property
            var property = entityType.GetMappedProperty(field.Name)?.PropertyInfo;

            // Return the function
            return CompileDataEntityPropertySetter<TEntity>(entityType,
                property,
                property?.PropertyType ?? field.Type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entityType"></param>
        /// <param name="property"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        internal static Action<TEntity, object> CompileDataEntityPropertySetter<TEntity>(Type entityType,
            PropertyInfo property,
            Type targetType)
        {
            // Check the property first
            if (property == null)
            {
                return null;
            }

            // Make sure we can write
            if (property.CanWrite == false)
            {
                return null;
            }

            // Variables for argument
            var valueParameter = Expression.Parameter(StaticType.Object, "value");

            // Get the converter
            var toTypeMethod = StaticType
                .Converter
                .GetMethod("ToType", new[] { StaticType.Object })
                .MakeGenericMethod(TypeCache.Get(targetType).GetUnderlyingType());

            // Conversion (if needed)
            var valueExpression = ConvertExpressionToTypeExpression(Expression.Call(toTypeMethod, valueParameter), targetType);

            // Property Handler
            if (TypeCache.Get(entityType).IsEntityType())
            {
                var classProperty = PropertyCache.Get(entityType, property, true);
                valueExpression = ConvertExpressionToPropertyHandlerSetExpression(valueExpression,
                    null, classProperty, targetType ?? classProperty.PropertyInfo.PropertyType);
            }

            // Assign the value into DataEntity.Property
            var entityParameter = Expression.Parameter(typeof(TEntity), "entity");
            var propertyAssignment = Expression.Call(Expression.Convert(entityParameter, entityType), property.SetMethod,
                valueExpression);

            // Return function
            return Expression.Lambda<Action<TEntity, object>>(propertyAssignment,
                entityParameter, valueParameter).Compile();
        }
    }
}
