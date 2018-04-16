using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;

namespace School.Models.TokenAuth
{
    public class AuthenticateModel
    {
        [Required]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string UserNameOrEmailAddress { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        public string Password { get; set; }
        /// <summary>
        /// 是否超管登陆
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}
