using ShardingCore.Core.EntityMetadatas;
using RazorWeb.Models;
using ShardingCore.VirtualRoutes.Mods;
using ShardingCore.Core.VirtualRoutes;
using ShardingCore.Core.VirtualRoutes.TableRoutes.Abstractions;
using ShardingCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ShardingCore.Core.PhysicTables;
using ShardingCore.Sharding.EntityQueryConfigurations;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
namespace RazorWeb.Routes
{
    public class LaoHuaItemRoute : ConstStringVirtualTableRoute<LaoHuaItem>
    {
        public LaoHuaItemRoute(string name):base(LaoHuaItem.TableName)
        {

        }

        public override void Configure(EntityMetadataTableBuilder<LaoHuaItem> builder)
        {
            builder.TableSeparator("");  
        }
    }



    /// <summary>
    /// 固定的表格名称
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class ConstStringVirtualTableRoute<TEntity> : AbstractShardingOperatorVirtualTableRoute<TEntity, string> where TEntity : class
    {
        public string TableName { get; set; }

        /// <summary>
        /// 固定表名称不变
        /// </summary>
        /// <param name="name">表格名称</param>>
        protected ConstStringVirtualTableRoute(string name)
        {
            TableName = name;
        }
        /// <summary>
        /// 如何将shardingkey转成对应的tail
        /// </summary>
        /// <param name="shardingKey"></param>
        /// <returns></returns>
        public override string ShardingKeyToTail(object shardingKey)
        {
            return "";
        }
        /// <summary>
        /// 获取对应类型在数据库中的所有后缀
        /// </summary>
        /// <returns></returns>
        public override List<string> GetAllTails()
        {
            return new List<string>() {""};
        }
        /// <summary>
        /// 路由表达式如何路由到正确的表
        /// </summary>
        /// <param name="shardingKey"></param>
        /// <param name="shardingOperator"></param>
        /// <returns></returns>
        public override Expression<Func<string, bool>> GetRouteToFilter(string shardingKey, ShardingOperatorEnum shardingOperator)
        {
            return tail => true;
        }

    }


}
