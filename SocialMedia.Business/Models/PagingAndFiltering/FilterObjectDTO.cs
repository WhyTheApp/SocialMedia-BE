namespace SocialMedia.Business.Models.PagingAndFiltering;

public class FilterObjectDTO
{
    public bool OnlyCount { get; set; } 
    public PagingDTO Paging { get; set; } 
    public List<FilteringDTO> Filtering { get; set; } 
    public SortingDTO Sorting { get; set; }
    
}