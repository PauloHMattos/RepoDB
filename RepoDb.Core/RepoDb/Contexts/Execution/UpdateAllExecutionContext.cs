using System;
using System.Collections.Generic;
using System.Data.Common;

namespace RepoDb.Contexts.Execution
{
    /// <summary>
    /// 
    /// </summary>
    internal class UpdateAllExecutionContext<TEntity>
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
        public Action<DbCommand, TEntity> SingleDataEntityParametersSetterFunc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Action<DbCommand, IList<TEntity>> MultipleDataEntitiesParametersSetterFunc { get; set; }
    }
}
