using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ShardingCore.Extensions.InternalExtensions
{
    internal static class InternalIEntityTypeExtension
    {
        /// <summary>
        /// 获取在db context内的数据库表名称对应叫什么
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static string GetEntityTypeTableName(this IEntityType entityType)
        {
#if !EFCORE2
            string tableName;
            var nnn = entityType.ClrType.GetCustomAttributes(typeof(TableAttribute), false) as TableAttribute[];
            if (nnn.Length > 0)
            {
                tableName = nnn[0].Name;
            }
            else
            {
                tableName = entityType.GetTableName();
            } 
#endif
#if EFCORE2
            var tableName = entityType.Relational().TableName;
#endif
            return tableName;
        }
    }
}
