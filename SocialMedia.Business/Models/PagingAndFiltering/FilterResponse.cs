namespace SocialMedia.Business.Models.PagingAndFiltering;

public class FilterResponse<T>
{
    public int NumberFound { get; set; }
    public int NumberRetrieved { get; set; }
    public List<T> Results { get; set; }
}
