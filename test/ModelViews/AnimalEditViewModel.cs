namespace test.ModelViews
{
    public class AnimalEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public byte? Age { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
