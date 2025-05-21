using SocialMedia.Business.Models.PagingAndFiltering;

namespace SocialMedia.Business.Services.Filtering;

public interface IFilterService<T>
{
    Task<FilterResponse<T>> Filter(FilterObjectDTO request, IQueryable<T> data);
}
