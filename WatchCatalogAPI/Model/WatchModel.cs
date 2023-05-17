using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace WatchCatalogAPI.Model
{
    public class WatchDetails:MainDetails
    {
        public string? Image { get; set; }
    }
    public class MainDetails
    {
        public  string? ItemName { get; set; }
        public  string? ShortDescription { get; set; }
        public  string ?FullDescription { get; set; }
        public  decimal Price { get; set; }
        public  string? Caliber { get; set; }
        public  string? Movement { get; set; }
        public  string? Chronograph { get; set; }
        public  decimal Weight { get; set; }
        public decimal Height { get; set; }
        public decimal Diameter { get; set; }
        public decimal Thickness { get; set; }
        public int Jewel { get; set; }
        public string? CaseMaterial { get; set; }
        public string? StrapMaterial { get; set; }
    }
    public class WatchDetails1: MainDetails
    {
        public int ItemNo { get; set; }
    }
    public class WatchImage
    {
        public string Image { get; set;}
        public int ItemNo { get; set;}
    }
    public class WatchId
    {
        public int itemNo { get; set; }
    }
}
