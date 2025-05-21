using System.Linq.Dynamic.Core;
using SocialMedia.Business.Constants;
using SocialMedia.Business.Models.PagingAndFiltering;

namespace SocialMedia.Business.Services.Filtering;

public class FilterService<T>: IFilterService<T>
{
    public async Task<FilterResponse<T>> Filter(FilterObjectDTO request, IQueryable<T> data)
    {
        var result = data;
        
        if (request.Filtering.Count != 0)
        {
            foreach (var filter in request.Filtering)
            {
                var field = filter.FieldToFilterBy;
                var value = filter.Value;
                var operation = filter.Operation;
                var expression = String.Empty;
                
                switch (operation)
                {
                    case "in":
                        expression = $"{field}.Contains(@0)";
                        break;
                    case "notin":
                        expression = $"!{field}.Contains(@0)";
                        break;
                    default:
                        var mappedOp = FilteringConstants.MapAcronymsToOperations[operation];
                        expression = $"{field} {mappedOp} @0";
                        break;
                }

                result = result.Where(expression, value);
            }
        }

        if (!string.IsNullOrEmpty(request.Sorting.FieldToSortBy))
        {
            result = result.OrderBy($"{request.Sorting.FieldToSortBy} {request.Sorting.Order}");
        }

        var numberFound = result.Count();
        if (request.OnlyCount)
        {
            return new FilterResponse<T>
            {
                NumberFound = numberFound,
                NumberRetrieved = 0,
                Results = []
            };
        }
        
        var actualResult = result
            .Skip(request.Paging.PageSize * (request.Paging.PageNumber - 1))
            .Take(request.Paging.PageSize)
            .ToList();
        
        
        return new FilterResponse<T>
        {
            NumberFound = numberFound,
            NumberRetrieved = Math.Min(request.Paging.PageSize, numberFound),
            Results = actualResult,
        };
    }
}