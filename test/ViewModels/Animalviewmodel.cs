using System.Collections.Generic;
using System.Linq;
using test.Models;

namespace test.ViewModels
{
    public class Animalviewmodel
    {
        public IEnumerable<Animal> animals { get; set; } = Enumerable.Empty<Animal>();
        public string? TypeFilter { get; set; } = "any";
        public string? LocationFilter { get; set; }
        public string? GenderFilter { get; set; } = "any";
        public IEnumerable<string> TypeOptions { get; set; } = Enumerable.Empty<string>();
        public IEnumerable<string> LocationOptions { get; set; } = Enumerable.Empty<string>();
        public IEnumerable<string> GenderOptions { get; set; } = new[] { "Male", "Female" };
        public bool IsMine { get; set; }
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 6;
    }
}
