using System;
using System.Collections.Generic;
using System.Data.Common;

namespace RepoDb.Contexts.Execution
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    internal class InsertExecutionContext<TEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        public string CommandText { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<DbField> InputFields { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Action<DbCommand, TEntity> ParametersSetterFunc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Action<TEntity, object> KeyPropertySetterFunc { get; set; }
    }
}
