using System.ComponentModel.DataAnnotations;
using Abp.MultiTenancy;

namespace School.Authorization.Accounts.Dto
{/// <summary>
/// 
/// </summary>
    public class IsTenantAvailableInput
    {/// <summary>
    /// 
    /// </summary>
        [Required]
        [MaxLength(AbpTenantBase.MaxTenancyNameLength)]
        public string TenancyName { get; set; }
    }
}