﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShardingCore.Sharding.Abstractions;

namespace ShardingCore.TableExists
{
    public class EmptyTableEnsureManager<TShardingDbContext> : ITableEnsureManager<TShardingDbContext> where TShardingDbContext : DbContext, IShardingDbContext
    {
        public ISet<string> GetExistTables(string dataSourceName)
        {
            return new HashSet<string>();
        }

        public ISet<string> GetExistTables(IShardingDbContext shardingDbContext, string dataSourceName)
        {
            return new HashSet<string>();
        }
    }
}
