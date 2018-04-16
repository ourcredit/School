using Abp.Application.Services.Dto;

namespace School.Authorization.Accounts.Dto
{
    /// <summary>
    /// 
    /// </summary>
    //### This class is mapped in CustomDtoMapper ###
    public class CurrentTenantInfoDto : EntityDto
    {/// <summary>
    /// 
    /// </summary>
        public string TenancyName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
    }
}