namespace MovieApi.DTOs
{
    public class PagedResult
    {
        public int Page {  get; set; }
        public int Limit { get; set; }
        public int TotalCount { get; set; } 
        public int PageCount { get; set; }
        public List<MovieApi.Models.Movie> Movies { get; set; } = [];
    }
}
