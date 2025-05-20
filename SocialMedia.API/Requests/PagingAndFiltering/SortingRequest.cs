using FluentValidation;
using SocialMedia.Business.Constants;

namespace SocialMedia.API.Requests.PagingAndFiltering;

public class SortingRequest
{
    public string FieldToSortBy { get; set; } = string.Empty;
    public string Order { get; set; } = string.Empty;
}

public class SortingRequestValidator : AbstractValidator<SortingRequest>
{
    public SortingRequestValidator()
    {
        RuleFor(sorting => sorting.Order)
            .Must(order => FilteringConstants.ValidSortingOrder.Contains(order.ToLower()));
    }
}