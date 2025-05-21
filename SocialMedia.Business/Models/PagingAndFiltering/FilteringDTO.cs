namespace SocialMedia.Business.Models.PagingAndFiltering;

public class FilteringDTO
{
    public string FieldToFilterBy { get; set; }
    public string Value { get; set; }
    public string Operation { get; set; }
}