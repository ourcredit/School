using System;
using System.Collections.Generic;
using System.Text;

namespace School.Others.Dtos
{
   public class DeviceStateInput
    {
        public string MachineCode { get; set; }
        public int status_code { get; set; }
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
        /// 销售状态
        /// </summary>
        public int SaleStatus { get; set; }
        /// <summary>
        /// 工作模式
        /// </summary>
        public int WorkPattern { get; set; }
        /// <summary>
        /// 门状态
        /// </summary>
        public int DoorSw { get; set; }
        /// <summary>
        /// 应急链接状态
        /// </summary>
        public int CoinConnection { get; set; }
        /// <summary>
        /// 纸币连接状态
        /// </summary>
        public int BillConnection { get; set; }
        /// <summary>
        /// 硬币 5 角缺币状态。(0:不缺币;1:缺币)
        /// </summary>
        public int Cointype0Lack { get; set; }
        /// <summary>
        /// 硬币 1 元缺币状态。(0:不缺币;1:缺币)
        /// </summary>
        public int Cointype1Lack { get; set; }
        /// <summary>
        /// 纸币器停用状态。(0:未停用;1:停用)
        /// </summary>
        public int BillStatus { get; set; }
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
      
       public List<ChannelInputA> Status { get; set; }
        public class ChannelInputA
        {
            public int column { get; set; }
            public int quantity { get; set; }
        }
    }
    /// <summary>
    /// 售货机货道余量
    /// </summary>
    public class ChannelStatusReportInput
    {
        /// <summary>
        /// 机器码
        /// </summary>
        public string MachineCode { get; set; }

        public List<ChannelInputB> Status { get; set; }
        public class ChannelInputB
        {
            public int column { get; set; }
            public int state { get; set; }
        }
    }
    /// <summary>
    /// 取货吗input
    /// </summary>

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
