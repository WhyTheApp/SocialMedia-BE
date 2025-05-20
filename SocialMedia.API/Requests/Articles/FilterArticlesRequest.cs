using FluentValidation;
using SocialMedia.API.Requests.PagingAndFiltering;
using SocialMedia.Business.Constants;

namespace SocialMedia.API.Requests.Articles;


public class FilterArticlesRequest
{
    public bool OnlyCount { get; set; } = false;
    public PagingRequest Paging { get; set; } = new PagingRequest();
    public List<FilteringRequest> Filtering { get; set; } = new List<FilteringRequest>();
    public SortingRequest Sorting { get; set; } = new SortingRequest();
    
}

public class FilterProblemsRequestValidator<T> : AbstractValidator<FilterArticlesRequest>
{
    public FilterProblemsRequestValidator()
    {
        RuleFor(filter => filter.Paging).SetValidator(new PagingRequestValidator());
        RuleFor(filter => filter.Sorting).SetValidator(new SortingRequestValidator());
        RuleForEach(filter => filter.Filtering).SetValidator(new FilteringRequestValidator());
        
        RuleForEach(filter => filter.Filtering).Must(filter => FilteringConstants.ValidArticlesFilteringFields.Contains(filter.FieldToFilterBy.ToLower()));
        RuleFor(filter => filter.Sorting.FieldToSortBy)
            .Must(field => FilteringConstants.ValidArticlesFilteringFields.Contains(field.ToLower()))
            .When(filter => filter.Sorting.FieldToSortBy != "");
    }
}