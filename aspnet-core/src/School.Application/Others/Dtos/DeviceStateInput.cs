using System;
using System.Collections.Generic;
using System.Text;

namespace School.Others.Dtos
{
   public class DeviceStateInput
    {
        public string MachineCode { get; set; }
        public bool State { get; set; }
    }
    /// <summary>
    /// vmc状态报告参数
    /// </summary>
    public class StateReportInput
    {
        /// <summary>
        /// 机器码
        /// </summary>
        public string MachineCode { get; set; }
        /// <summary>
        /// 货道号
        /// </summary>
        public int BillStatus { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public int Cointype0Lack { get; set; }
    }
    /// <summary>
    /// 售货机货道余量
    /// </summary>
    public class ChannelStockReportInput
    {
        /// <summary>
        /// 机器码
        /// </summary>
        public string MachineCode { get; set; }
        /// <summary>
        /// 货道号
        /// </summary>
        public int BillStatus { get; set; }
        /// <summary>
        /// 余量
        /// </summary>
        public int Quantity { get; set; }
    }

    public class CheckPickCodeInput
    {
        /// <summary>
        /// 机器码
        /// </summary>
        public string MachineCode { get; set; }
        /// <summary>
        /// 货道号
        /// </summary>
        public string PickCode { get; set; }
    }

}
