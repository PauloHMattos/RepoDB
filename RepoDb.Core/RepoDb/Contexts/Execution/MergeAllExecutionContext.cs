using System;
using System.Collections.Generic;
using System.Data.Common;

namespace RepoDb.Contexts.Execution
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    internal class MergeAllExecutionContext<TEntity>
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
        public int BatchSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Action<DbCommand, TEntity> SingleDataEntityParametersSetterFunc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Action<DbCommand, IList<TEntity>> MultipleDataEntitiesParametersSetterFunc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Action<TEntity, object> KeyPropertySetterFunc { get; set; }
    }
}
