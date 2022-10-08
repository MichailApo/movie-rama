
using MovieRamaWeb.Domain.Enums;
namespace MovieRamaWeb.Application.Requests
{
    public class MovieListQueryParameters
    {
        public SortOrder SortOrder { get; set; } = SortOrder.Desc;
        public MovieSortType? SortType { get; set; }
    }
}
