using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using RepoDb.Interfaces;

namespace RepoDb.Reflection
{
    internal partial class Compiler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="inputFields"></param>
        /// <param name="outputFields"></param>
        /// <param name="dbSetting"></param>
        /// <param name="dbHelper"></param>
        /// <returns></returns>
        internal static Action<DbCommand, T> CompileDataEntityDbParameterSetter<T>(Type entityType,
            IEnumerable<DbField> inputFields,
            IEnumerable<DbField> outputFields,
            IDbSetting dbSetting,
            IDbHelper dbHelper)
        {
            var dbCommandExpression = Expression.Parameter(StaticType.DbCommand, "command");
            var entityParameterExpression = Expression.Parameter(typeof(T), "entityParameter");
            var dbParameterCollectionExpression = Expression.Property(dbCommandExpression,
                StaticType.DbCommand.GetProperty("Parameters"));
            var entityVariableExpression = Expression.Variable(entityType, "entityVariable");
            var entityExpressions = new List<Expression>();
            var entityVariableExpressions = new List<ParameterExpression>();
            var fieldDirections = new List<FieldDirection>();
            var bodyExpressions = new List<Expression>();

            // Class handler
            var handledEntityParameterExpression = ConvertExpressionToClassHandlerSetExpression(dbCommandExpression, entityType, entityParameterExpression);

            // Field directions
            fieldDirections.AddRange(GetInputFieldDirections(inputFields));
            fieldDirections.AddRange(GetOutputFieldDirections(outputFields));

            // Clear the parameter collection first
            bodyExpressions.Add(GetDbParameterCollectionClearMethodExpression(dbParameterCollectionExpression));

            // Entity instance
            entityVariableExpressions.Add(entityVariableExpression);
            entityExpressions.Add(Expression.Assign(entityVariableExpression, handledEntityParameterExpression));

            if (entityType.IsClass)
            {
                // Throw if null
                entityExpressions.Add(ThrowIfNullAfterClassHandlerExpression(entityType, entityVariableExpression));
            }

            // Iterate the input fields
            foreach (var fieldDirection in fieldDirections)
            {
                // Add the property block
                var propertyBlock = GetPropertyFieldExpression(dbCommandExpression,
                    ConvertExpressionToTypeExpression(entityVariableExpression, entityType),
                    fieldDirection,
                    0,
                    dbSetting,
                    dbHelper);

                // Add to instance expression
                entityExpressions.Add(propertyBlock);
            }

            // Add to the instance block
            var instanceBlock = Expression.Block(entityVariableExpressions, entityExpressions);

            // Add to the body
            bodyExpressions.Add(instanceBlock);

            // Set the function value
            return Expression
                .Lambda<Action<DbCommand, T>>(Expression.Block(bodyExpressions), dbCommandExpression, entityParameterExpression)
                .Compile();
        }
    }
}
