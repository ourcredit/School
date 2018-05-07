using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using School.Models;
using School.Others.Dtos;

namespace School.Others
{
    /// <summary>
    /// Point应用层服务的接口方法
    /// </summary>
    public interface IOtherAppService : IApplicationService
    {
        /// <summary>
        /// 获取Point的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<dsc_Goods>> GetPagedGoods(GetGoodsInput input);

        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        Task<PagedResultDto<OrderListDto>> GetOrdersAsync(GetOrderInput input);
        /// <summary>
        /// 获取机构下机器内商品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<ProductListDto>> Products(GetProductsInput input);

        /// <summary>
        /// 更新售货机工控编号接口
        /// </summary>
        /// <returns></returns>
        Task UpdateDeviceControlCode(MachineCodeInput input);
        /// <summary>
        /// 获取货道列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ChannelListDto>> GetPagedChannels(GetDeviceGoodsInput input);
        /// <summary>
        /// 获取展示位列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ShowListDto>> GetPagedBoxs(GetDeviceGoodsInput input);
        /// <summary>
        /// 心跳程序 检查设备状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        Task Beat(DeviceStateInput input);

        /// <summary>
        /// 线上出货上报
        /// </summary>
        /// <returns></returns>
        Task OnlineOrder(DealOrderInput input);

        /// <summary>
        /// 现金出货上报
        /// </summary>
        /// <returns></returns>
        Task CachOrder(CashOrderInput input);
        /// <summary>
        /// Vmc系统状态报告
        /// </summary>
        /// <returns></returns>
        Task StatusReport(StateReportInput input);

        /// <summary>
        /// 余量调整
        /// </summary>
        /// <returns></returns>
        Task ChannelStockReport(ChannelStockReportInput input);

        /// <summary>
        /// 状态调整
        /// </summary>
        /// <returns></returns>
        Task ChannelStatusReport(ChannelStatusReportInput input);

        /// <summary>
        /// 取货码验证
        /// </summary>
        /// <returns></returns>
        Task<CheckPickCodeResult> CheckPickCode(CheckPickCodeInput input);

        /// <summary>
        /// 更新货道
        /// </summary>
        /// <returns></returns>
        Task UpGxvmChannel(CreateOrUpdateChannelInput input);

        /// <summary>
        /// 更新货柜机展示
        /// </summary>
        /// <returns></returns>
        Task UpGxvmShow(CreateOrUpdateShowInput input);

        /// <summary>
        /// 更新货柜机展示
        /// </summary>
        /// <returns></returns>
        Task UpGxvmShow2Channel(CreateOrUpdateShowChannelInput input);

    }
}
