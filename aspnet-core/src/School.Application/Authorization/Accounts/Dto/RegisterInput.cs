using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Extensions;
using School.Validation;

namespace School.Authorization.Accounts.Dto
{/// <summary>
/// 
/// </summary>
    public class RegisterInput : IValidatableObject
    {/// <summary>
    /// 
    /// </summary>
        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

       
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

    
/// <summary>
        /// awd
        /// </summary>
        [Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        
        public string Password { get; set; }
 /// <summary>
        /// 
        /// </summary>
        [DisableAuditing]
       
        public string CaptchaResponse { get; set; }
 /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!UserName.IsNullOrEmpty())
            {
                if (ValidationHelper.IsEmail(UserName))
                {
                    yield return new ValidationResult("Username cannot be an email address unless it's same with your email address !");
                }
            }
        }
    }
}