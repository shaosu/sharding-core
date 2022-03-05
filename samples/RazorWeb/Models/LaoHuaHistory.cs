using ShardingCore.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace RazorWeb.Models
{
    /// <summary>
    /// 测试的过程中的数据
    /// </summary>
    [Table(TableNamePre)]
    public class LaoHuaHistory
    {
        /// <summary>
        /// 表格名称前缀
        /// </summary>
        public const string TableNamePre = "process_history";

        public LaoHuaHistory()
        {
            //var b = new EntityTypeBuilder();
        }

        [Key]
        public int Index { get; set; }

        /// <summary>
        /// 唯一编码-每次测试产生一个,表示某一次测试
        /// </summary>
        [NotNull]
        public string? GUID { get; set; }

        /// <summary>
        /// 老化板子的编码
        /// </summary>
        [NotNull]
        public string? SN { get; set; }
        [ShardingTableKey(TableSeparator = "")]
        public DateTime Time { get; set; }

        /// <summary>
        /// 速度
        /// </summary>
        public int Speed { get; set; }

        /// <summary>
        /// 电流零点漂移
        /// </summary>
        public int ZeroMoveI { get; set; }

        [MaxLength(255)]
        public string? JsonData { get; set; }
    }
}
