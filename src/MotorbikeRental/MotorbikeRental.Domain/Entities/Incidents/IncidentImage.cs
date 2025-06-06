using System.ComponentModel.DataAnnotations;

namespace MotorbikeRental.Domain.Entities.Incidents
{
    public class IncidentImage
    {
        public int ImageId { get; set; }

        [Required(ErrorMessage = "Incident ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid incident ID")]
        public int IncidentId { get; set; } //Mã sự cố
        public virtual Incident Incident { get; set; }

        [Required(ErrorMessage = "Image URL is required")]
        [MaxLength(500, ErrorMessage = "Image URL cannot exceed 500 characters")]
        [Url(ErrorMessage = "Invalid URL format")]
        public string ImageUrl { get; set; }
    }
}