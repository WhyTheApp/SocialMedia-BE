using FluentValidation;
using SocialMedia.API.CONSTANTS;

namespace SocialMedia.API.Requests.PagingAndFiltering;

public class PagingRequest
{
    public int PageSize { get; set; } = Constants.DefaultPageSize;
    public int PageNumber { get; set; } = Constants.DefaultPageNumber;
}

public class PagingRequestValidator : AbstractValidator<PagingRequest>
{
    public PagingRequestValidator()
    {
        RuleFor(paging => paging.PageSize).GreaterThan(0);
        RuleFor(paging => paging.PageNumber).GreaterThan(0);
    }
}