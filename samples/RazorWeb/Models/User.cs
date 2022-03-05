using ShardingCore.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace RazorWeb.Models
{

    [Table(TableName)]
    public class User
    {
        /// <summary>
        /// 表格名称
        /// </summary>
        public const string TableName = "Users";
        [ShardingTableKey(TableSeparator ="")]
        [Key]
        public int Index { get; set; }

        [NotNull]
        public string? Name { get; set; }

        public string? Pwd { get; set; }

        public UserRole Role { get; set; }
    }

    [Table(TableName)]
    public class UserB
    {
        /// <summary>
        /// 表格名称
        /// </summary>
        public const string TableName = "User_B";
        [ShardingTableKey(TableSeparator = "")]
        [Key]
        public int Index { get; set; }

        [NotNull]
        public string? Name { get; set; }

        public string? Pwd { get; set; }

        public UserRole Role { get; set; }
    }
}
