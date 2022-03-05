using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShardingCore;
using RazorWeb.Data;
using RazorWeb.Routes;
using ShardingCore.TableExists;
using ShardingCore.Bootstrapers;
using RazorWeb.Models;
using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释

namespace RazorWeb
{
    public class Startup
    {
        public static readonly ILoggerFactory efLogger = LoggerFactory.Create(builder =>

        {
            builder.AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information).AddConsole();
        });
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddDbContext<MContext>();

            services.AddShardingDbContext<DefaultShardingDbContext>()
                 .AddEntityConfig(o =>
                 {
                     o.CreateShardingTableOnStart = true;
                     o.EnsureCreatedWithOutShardingTable = true;
                     o.IgnoreCreateTableError = false;
                     o.AddShardingTableRoute<UserRoute>();
                     o.AddShardingTableRoute<LaoHuaHistoryRoute>();
                     o.AddShardingTableRoute<LaoHuaItemRoute>();
                     o.AddShardingTableRoute<UserBRoute>();
                     
                     o.UseShardingQuery((conStr, builder) =>
                     {
                         //builder.UseSqlite(conStr).UseLoggerFactory(efLogger);
                         builder.UseMySql(conStr, new MariaDbServerVersion("10.2.9")).UseLoggerFactory(efLogger);
                     });
                     o.UseShardingTransaction((connection, builder) =>
                     {
                         //builder.UseSqlite(connection).UseLoggerFactory(efLogger);
                         builder.UseMySql(connection, new MariaDbServerVersion("10.2.9")).UseLoggerFactory(efLogger);
                     });
                 })
                 .AddConfig(op =>
                 {
                     op.ConfigId = "c1";
                     op.AddDefaultDataSource("ds0", "Server=127.0.0.1;port=3306;Database=LaoHua;uid=Admin;pwd=xxx;Character Set=utf8;");
                     //op.AddDefaultDataSource("ds0", "DataSource =:memory:");
                     op.ReplaceTableEnsureManager(sp => new MySqlTableEnsureManager<DefaultShardingDbContext>());
                 }).EnsureConfig();
            // EmptyTableEnsureManager

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            var nnn = typeof(User).GetCustomAttributes(typeof(TableAttribute), false) as TableAttribute[];
            if (nnn.Length > 0)
            {
                Console.WriteLine($"{nnn[0].Name} {nnn[0].Schema} {nnn[0].TypeId}");
            }


            app.ApplicationServices.GetRequiredService<IShardingBootstrapper>().Start();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseShardingCore();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World!");
                //});
            });
            app.DbSeed();
        }
    }
}
