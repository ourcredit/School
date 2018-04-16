using System.ComponentModel.DataAnnotations;

namespace School.Authorization.Accounts.Dto
{/// <summary>
/// 
/// </summary>
    public class ImpersonateInput
    {/// <summary>
    /// 
    /// </summary>
        public int? TenantId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Range(1, long.MaxValue)]
        public long UserId { get; set; }
    }
}