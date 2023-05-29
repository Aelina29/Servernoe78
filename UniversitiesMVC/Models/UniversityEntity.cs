using System.ComponentModel.DataAnnotations;

namespace UniversitiesMVC.Models
{
    public class UniversityEntity
    {
        public int Id { get; set; }
        public string? country { get; set; }
        public string? university { get; set; }
        public decimal ranking_system { get; set; }
        public string? message { get; set; }
    }
}
