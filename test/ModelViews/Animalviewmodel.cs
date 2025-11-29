using test.Models;

namespace test.ModelViews
{
    public class Animalviewmodel
    {
        public IEnumerable<Animal> animals { get; set; }
        public string filter { get; set; } = "";
        public bool IsMine { get; set; }
    }
}
