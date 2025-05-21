using SocialMedia.Business.Models.PagingAndFiltering;

namespace SocialMedia.API.Requests.PagingAndFiltering;

public static class PagingAndFilteringExtensions
{
    public static FilteringDTO toFilterDTO(this FilteringRequest filteringRequest) => new FilteringDTO
    {
        FieldToFilterBy = filteringRequest.FieldToFilterBy,
        Operation = filteringRequest.Operation,
        Value = filteringRequest.Value
    };

    public static List<FilteringDTO> toFilterDTOList(this List<FilteringRequest> filteringRequests)
    {
        return filteringRequests.Select(filter => filter.toFilterDTO()).ToList();
    }

    public static PagingDTO toPagingDTO(this PagingRequest pagingRequest) => new PagingDTO
    {
        PageNumber = pagingRequest.PageNumber,
        PageSize = pagingRequest.PageSize,
    };

    public static SortingDTO toSortingDTO(this SortingRequest sortingRequest) => new SortingDTO
    {
        FieldToSortBy = sortingRequest.FieldToSortBy,
        Order = sortingRequest.Order,
    };
}