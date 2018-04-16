using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;

namespace School.Authorization.Roles.Dto
{/// <summary>
/// 
/// </summary>
    public class RoleListDto : EntityDto, IHasCreationTime
    {/// <summary>
    /// 
    /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsStatic { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDefault { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}