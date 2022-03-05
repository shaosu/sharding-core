using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ShardingCore.Bootstrapers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RazorWeb.Data;
using RazorWeb.Models;
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
namespace RazorWeb
{
    public static class DIExtension
    {
        public static IApplicationBuilder UseShardingCore(this IApplicationBuilder app)
        {
            var shardingBootstrapper = app.ApplicationServices.GetRequiredService<IShardingBootstrapper>();
            shardingBootstrapper.Start();
            return app;
        }

        public static void DbSeed(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var virtualDbContext = scope.ServiceProvider.GetService<DefaultShardingDbContext>();
                if (!virtualDbContext.Set<User>().Any())
                {
                    var ids = Enumerable.Range(1, 1000);
                    var userMods = new List<User>();
                    foreach (var id in ids)
                    {
                        userMods.Add(new User()
                        {
                            Index = id,
                            Name = $"name_{id}",
                            Pwd = id.ToString(),
                        });
                    }
                    var userModMonths = new List<LaoHuaHistory>();
                    foreach (var id in ids)
                    {
                        userModMonths.Add(new LaoHuaHistory()
                        {
                            Index = id,
                            Time = DateTime.Now
                        });
                    }

                    virtualDbContext.AddRange(userMods);
                    virtualDbContext.AddRange(userModMonths);
                    virtualDbContext.SaveChanges();

                }

                if (!virtualDbContext.Set<UserB>().Any())
                {
                    var ids = Enumerable.Range(1, 1000);
                    var userMods = new List<UserB>();
                    foreach (var id in ids)
                    {
                        userMods.Add(new UserB()
                        {
                            Index = id,
                            Name = $"name_{id}",
                            Pwd = id.ToString(),
                        });
                    }
                 
                    virtualDbContext.AddRange(userMods);
         
                    virtualDbContext.SaveChanges();

                }

            }
        }
    }
}
