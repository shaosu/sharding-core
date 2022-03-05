using Microsoft.EntityFrameworkCore;
using ShardingCore.Core.VirtualRoutes.TableRoutes.RouteTails.Abstractions;
using ShardingCore.Sharding;
using ShardingCore.Sharding.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RazorWeb.Maps;
using RazorWeb.Models;

namespace RazorWeb.Data
{
    internal class MContext : DbContext
    {
        public DbSet<LaoHuaItem> LaoHuaItems { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=127.0.0.1;port=3306;Database=LaoHua;uid=Admin;pwd=xxx;Character Set=utf8;", new MariaDbServerVersion("10.2.9"));
        }

    }


    public class DefaultShardingDbContext : AbstractShardingDbContext, IShardingTableDbContext
    {

        public DbSet<LaoHuaItem> LaoHuaItems { get; set; }

        public DefaultShardingDbContext(DbContextOptions<DefaultShardingDbContext> options) : base(options)
        {
            //切记不要在构造函数中使用会让模型提前创建的方法
            //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            //Database.SetCommandTimeout(30000);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new UserBMap());
            modelBuilder.ApplyConfiguration(new LaoHuaItemMap());
            modelBuilder.ApplyConfiguration(new LaoHuaHistoryMap());
        }

        public IRouteTail RouteTail { get; set; }
    }
}
