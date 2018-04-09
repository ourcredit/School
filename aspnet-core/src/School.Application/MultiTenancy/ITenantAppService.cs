using Abp.Application.Services;
using Abp.Application.Services.Dto;
using School.MultiTenancy.Dto;

namespace School.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}
