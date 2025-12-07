using System.ComponentModel.DataAnnotations;

namespace test.ViewModels
{
    public class CreateAnimalViewModel
    {
        public string Name { get; set; }
        [Range(1,30, ErrorMessage = "Age must be between 1 and 30")]
        public byte Age { get; set; }
        public string Type { get; set; }
        public string? Breed { get; set; }
        public string? CustomBreed { get; set; }
        public string? About { get; set; }
        public IFormFile Photo { get; set; }
    }
}
