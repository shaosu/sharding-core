using ShardingCore.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace RazorWeb.Models
{
    /// <summary>
    /// TODO:DeepCopy,
    /// 测试记录
    /// </summary>
    [Table(TableName)]
    public class LaoHuaItem
    {
        /// <summary>
        /// 表格名称
        /// </summary>
        public const string TableName = "LaoHuaItemAAA";

        public LaoHuaItem()
        {
            PauseTime = DateTime.Now;
        }
        [ShardingTableKey(TableSeparator = "")]
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
        public string SN { get; set; }

        /// <summary>
        /// 老化节点的编码
        /// </summary>
        public string IPE { get; set; }

        /// <summary>
        /// 老化的工位
        /// </summary>
        public byte SlaveID { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 上一次暂停时间
        /// </summary>
        [NotMapped]
        public DateTime PauseTime { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime CompleteTime { get; set; }

        /// <summary>
        /// 结果 Enum.LaoHuaResult
        /// </summary>
        public int Result { get; set; }

        /// <summary>
        /// 状态 Enum.LaoHuaStatus
        /// </summary>
        public int LHStatus { get; set; } //TODO: 软件挂了,就不能要重新来,所以需要把持数据写入控制器

        /// <summary>
        /// 进度(0-100)
        /// </summary>
        public double Percent { get; set; }

        /// <summary>
        /// 时间长度(H)
        /// </summary>
        public double Times { get; set; }

        /// <summary>
        /// 电压等级
        /// </summary>
        public double Voltage { get; set; }

        /// <summary>
        /// 功率(KW)
        /// </summary>
        public double Power { get; set; }
    }

    public enum UserRole
    {
        /// <summary>
        /// 只读
        /// </summary>
        User,

        /// <summary>
        /// 可开机,停机等操作
        /// </summary>
        Engineer,

        /// <summary>
        /// 可更改编辑
        /// </summary>
        Admin
    }
}
