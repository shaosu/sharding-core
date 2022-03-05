using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RazorWeb.Data;
using RazorWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly DefaultShardingDbContext DbContext;
        public IndexModel(ILogger<IndexModel> logger, DefaultShardingDbContext Context)
        {
            _logger = logger;
            DbContext = Context;
        }

        private int _UserCount;
        public int UserCount
        {
            get { return _UserCount; }
            set
            {
                _UserCount = value;
            }
        }

        public int LaoHuaHistoryCount { get; set; }
        public int LaoHuaItemCount { get; set; }
        public void OnGet()
        {

            //LaoHuaItemCount = DbContext.Set<LaoHuaItem>().Count();  不分表直接使用原先EFcore的DbSet
            UserCount = DbContext.Set<User>().Count();
            LaoHuaHistoryCount = DbContext.Set<LaoHuaHistory>().Count();
            UserCount += DbContext.Set<UserB>().Count();

            using (MContext my = new MContext())
            {
                LaoHuaItemCount += my.LaoHuaItems.Count();
            }
            DbContext.Set<UserB>().Add(new UserB() { Name = $"AA{UserCount}" ,Role=0, Pwd="123" });
            DbContext.SaveChanges();
        }
    }
}
