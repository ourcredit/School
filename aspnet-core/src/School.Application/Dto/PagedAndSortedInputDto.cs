using Abp.Application.Services.Dto;
using School;
using School.Dto;

namespace MyCompanyName.AbpZeroTemplate.Dto
{
    public class PagedAndSortedInputDto : PagedInputDto, ISortedResultRequest
    {
        public string Sorting { get; set; }

        public PagedAndSortedInputDto()
        {
            MaxResultCount = AppConsts.DefaultPageSize;
        }
    }
}