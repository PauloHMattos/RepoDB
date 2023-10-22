using RepoDb.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Dynamic;

namespace RepoDb.Reflection
{
    /// <summary>
    /// A static factory class used to create a custom compiled function.
    /// </summary>
    internal static class FunctionFactory
    {
        #region CompileDataReaderToType

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="reader"></param>
        /// <param name="dbFields">The list of the <see cref="DbField"/> objects to be used.</param>
        /// <param name="dbSetting">The instance of <see cref="IDbSetting"/> object to be used.</param>
        /// <returns></returns>
        public static Func<DbDataReader, TResult> CompileDataReaderToType<TResult>(DbDataReader reader,
            DbFieldCollection dbFields,
            IDbSetting dbSetting) =>
            Compiler.CompileDataReaderToType<TResult>(reader, dbFields, dbSetting);

        #endregion

        #region CompileDataReaderToExpandoObject

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="dbFields"></param>
        /// <param name="dbSetting"></param>
        /// <returns></returns>
        public static Func<DbDataReader, ExpandoObject> CompileDataReaderToExpandoObject(DbDataReader reader,
            DbFieldCollection dbFields,
            IDbSetting dbSetting) =>
            Compiler.CompileDataReaderToExpandoObject(reader, dbFields, dbSetting);

        #endregion

        #region CompileDataEntityDbParameterSetter

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="inputFields"></param>
        /// <param name="outputFields"></param>
        /// <param name="dbSetting"></param>
        /// <param name="dbHelper"></param>
        /// <returns></returns>
        public static Action<DbCommand, T> CompileDataEntityDbParameterSetter<T>(Type entityType,
            IEnumerable<DbField> inputFields,
            IEnumerable<DbField> outputFields,
            IDbSetting dbSetting,
            IDbHelper dbHelper) =>
            Compiler.CompileDataEntityDbParameterSetter<T>(entityType, inputFields, outputFields, dbSetting, dbHelper);

        #endregion

        #region CompileDataEntityListDbParameterSetter

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="inputFields"></param>
        /// <param name="outputFields"></param>
        /// <param name="batchSize"></param>
        /// <param name="dbSetting"></param>
        /// <param name="dbHelper"></param>
        /// <returns></returns>
        public static Action<DbCommand, IList<T>> CompileDataEntityListDbParameterSetter<T>(Type entityType,
            IEnumerable<DbField> inputFields,
            IEnumerable<DbField> outputFields,
            int batchSize,
            IDbSetting dbSetting,
            IDbHelper dbHelper) =>
            Compiler.CompileDataEntityListDbParameterSetter<T>(entityType, inputFields, outputFields, batchSize, dbSetting, dbHelper);

        #endregion

        #region CompileDictionaryStringObjectDbParameterSetter

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="inputFields"></param>
        /// <param name="dbSetting"></param>
        /// <param name="dbHelper"></param>
        /// <returns></returns>
        public static Action<DbCommand, T> CompileDictionaryStringObjectDbParameterSetter<T>(Type entityType,
            IEnumerable<DbField> inputFields,
            IDbSetting dbSetting,
            IDbHelper dbHelper) =>
            Compiler.CompileDictionaryStringObjectDbParameterSetter<T>(entityType, inputFields, dbSetting, dbHelper);

        #endregion

        #region CompileDictionaryStringObjectListDbParameterSetter

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="inputFields"></param>
        /// <param name="batchSize"></param>
        /// <param name="dbSetting"></param>
        /// <param name="dbHelper"></param>
        /// <returns></returns>
        public static Action<DbCommand, IList<T>> CompileDictionaryStringObjectListDbParameterSetter<T>(Type entityType,
            IEnumerable<DbField> inputFields,
            int batchSize,
            IDbSetting dbSetting,
            IDbHelper dbHelper) =>
            Compiler.CompileDictionaryStringObjectListDbParameterSetter<T>(entityType, inputFields, batchSize, dbSetting, dbHelper);

        #endregion

        #region CompileDictionaryStringObjectItemSetter

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entityType"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static Action<TEntity, object> CompileDictionaryStringObjectItemSetter<TEntity>(Type entityType,
            Field field) =>
            Compiler.CompileDictionaryStringObjectItemSetter<TEntity>(entityType, field);

        #endregion

        #region CompileDbCommandToProperty

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="field"></param>
        /// <param name="parameterName"></param>
        /// <param name="index"></param>
        /// <param name="dbSetting"></param>
        /// <returns></returns>
        public static Action<TEntity, DbCommand> CompileDbCommandToProperty<TEntity>(Field field,
            string parameterName,
            int index,
            IDbSetting dbSetting) =>
            Compiler.CompileDbCommandToProperty<TEntity>(field, parameterName, index, dbSetting);

        #endregion

        #region CompileDataEntityPropertySetter

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entityType"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static Action<TEntity, object> CompileDataEntityPropertySetter<TEntity>(Type entityType,
            Field field) =>
            Compiler.CompileDataEntityPropertySetter<TEntity>(entityType, field);

        #endregion

        #region GetPlainTypeToDbParametersCompiledFunction

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramType"></param>
        /// <param name="entityType"></param>
        /// <param name="dbFields"></param>
        /// <returns></returns>
        public static Action<DbCommand, object> GetPlainTypeToDbParametersCompiledFunction(Type paramType,
            Type entityType,
            DbFieldCollection dbFields = null) =>
            Compiler.GetPlainTypeToDbParametersCompiledFunction(paramType, entityType, dbFields);

        #endregion
    }
}
