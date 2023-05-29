namespace UniversitiesMVC.Data
{
    public class FormUniversity
    {
        public int Id { get; set; }

        public int? CountryId { get; set; }

        public string? UniversityName { get; set; }

        public virtual Country? Country { get; set; }
    }
}
