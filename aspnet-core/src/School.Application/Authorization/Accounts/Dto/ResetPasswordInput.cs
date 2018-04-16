using System.ComponentModel.DataAnnotations;
using Abp.Auditing;

namespace School.Authorization.Accounts.Dto
{/// <summary>
/// 
/// </summary>
    public class ResetPasswordInput
    {/// <summary>
    /// 
    /// </summary>
        [Range(1, long.MaxValue)]
        public long UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string ResetCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [DisableAuditing]
        public string Password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ReturnUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SingleSignIn { get; set; }
    }
}