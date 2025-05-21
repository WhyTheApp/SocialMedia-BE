using FluentValidation;
using SocialMedia.Business.Constants;

namespace SocialMedia.API.Requests.PagingAndFiltering;

public class FilteringRequest
{
    public string FieldToFilterBy { get; set; }
    public string Value { get; set; }
    public string Operation { get; set; }
}

public class FilteringRequestValidator : AbstractValidator<FilteringRequest>
{
    public FilteringRequestValidator()
    {
        RuleFor(filtering => filtering.Operation)
            .Must(field => FilteringConstants.MapAcronymsToOperations.ContainsKey(field.ToLower()));
    }
}