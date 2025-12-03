namespace test.ViewModels
{
    public class CreateAnimalViewModel
    {
        public string Name { get; set; }
        public byte Age { get; set; }
        public string Type { get; set; }
        public IFormFile Photo { get; set; }
    }
}
