using ShardingCore.Core.EntityMetadatas;
using ShardingCore.VirtualRoutes.Mods;
using RazorWeb.Models;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
namespace RazorWeb.Routes
{
    public class UserRoute : AbstractSimpleShardingModKeyStringVirtualTableRoute<User>
    {
        public UserRoute() : base(2, 3)
        {

        }
        public override void Configure(EntityMetadataTableBuilder<User> builder)
        {

        }
    }

    public class UserBRoute : AbstractSimpleShardingModKeyStringVirtualTableRoute<UserB>
    {
        public UserBRoute() : base(2, 3)
        {

        }
        public override void Configure(EntityMetadataTableBuilder<UserB> builder)
        {

        }
    }
}
