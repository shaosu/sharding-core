using ShardingCore.Core.EntityMetadatas;
using ShardingCore.VirtualRoutes.Months;
using ShardingCore.VirtualRoutes.Years;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RazorWeb.Models;

namespace RazorWeb.Routes
{
    public class LaoHuaHistoryRoute : AbstractSimpleShardingYearKeyDateTimeVirtualTableRoute<LaoHuaHistory>
    {
        public override DateTime GetBeginTime()
        {
            return new DateTime(2022, 1, 01);
        }
      
        public override bool AutoCreateTableByTime()
        {
            
            return true;
        }

      
        public override void Configure(EntityMetadataTableBuilder<LaoHuaHistory> builder)
        {

        }
    }
}
