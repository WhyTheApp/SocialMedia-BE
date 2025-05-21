namespace SocialMedia.Business.Models.PagingAndFiltering;

public class SortingDTO
{
    public string FieldToSortBy { get; set; } = string.Empty;
    public string Order { get; set; } = string.Empty;
}