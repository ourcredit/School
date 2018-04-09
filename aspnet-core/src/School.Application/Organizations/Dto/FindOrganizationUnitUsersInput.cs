using MyCompanyName.AbpZeroTemplate.Dto;
using School.Dto;

namespace MyCompanyName.AbpZeroTemplate.Organizations.Dto
{
    public class FindOrganizationUnitUsersInput : PagedAndFilteredInputDto
    {
        public long OrganizationUnitId { get; set; }
    }
}
