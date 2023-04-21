namespace WatchCatalogAPI.Model
{
    public class WatchDetails
    {
        public  string? ItemName { get; set; }
        public  int ItemNo { get; set; }
        public  string? ShortDesc { get; set; }
        public  string ?FullDesc { get; set; }
        public  decimal Price { get; set; }
        public  string? Caliber { get; set; }
        public  string? Movement { get; set; }
        public  string? Chronograph { get; set; }
        public  string? Weight { get; set; }
        public string? Height { get; set; }
        public string? Diameter { get; set; }
        public string? Thickness { get; set; }
        public int Jewel { get; set; }
        public string? CaseMaterial { get; set; }
        public string? StrapMaterial { get; set; }
    }
}
