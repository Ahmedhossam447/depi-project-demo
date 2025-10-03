using test.Models;

namespace test.ModelViews
{
    public class Animalviewmodel
    {
        public IQueryable<Animal> animals { get; set; }
        public string filter { get; set; } = "";
    }
}
