using System.ComponentModel.DataAnnotations;

namespace RMChurchApp.Data.Models.Church
{
    public class ChurchMember
    {
        public int member_id { get; set; }

        [Required]
        [MaxLength(250)]
        public string member_name { get; set; }

        [Required]
        [MaxLength(100)]
        public string sur_name { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be 10 digits")]
        public string mobile_number { get; set; }

        [Range(0, 120, ErrorMessage = "Age must be between 0 and 120")]
        public int? age { get; set; }

        [MaxLength(1)]
        [RegularExpression(@"[MF]", ErrorMessage = "Gender must be 'M' or 'F'")]
        public string? gender { get; set; }

        [MaxLength(1)]
        [RegularExpression(@"[MUSW]", ErrorMessage = "Marital status must be M, U, S, or W")]
        public string? marital_status { get; set; }

        [MaxLength(255)]
        public string? city { get; set; }

        [MaxLength(255)]
        public string? area { get; set; }

        [MaxLength(255)]
        public string? land_mark { get; set; }

        [MaxLength(255)]
        public string? occupation { get; set; }

        [MaxLength(255)]
        public string? religion { get; set; }

        [MaxLength(45)]
        public string? reference_name { get; set; }

        [MaxLength(45)]
        public string? reference_type { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Reference mobile must be 10 digits")]
        public string? reference_mobile { get; set; }

        [MaxLength(200)]
        public string? reference_area { get; set; }

        [MaxLength(1)]
        [RegularExpression(@"[CBT]", ErrorMessage = "Membership must be C, B, or T")]
        public string? membership { get; set; } = "C";

        public string? remarks { get; set; }

        [Required]
        public int created_by { get; set; }

        [Required]
        public DateTime created_date { get; set; }

        [MaxLength(45)]
        public string? updated_by { get; set; }

        public DateTime? updated_date { get; set; }

        [MaxLength(10)]
        public string? followup_mode { get; set; }

        [MaxLength(1)]
        [RegularExpression(@"[YN]", ErrorMessage = "is_followup_active must be Y or N")]
        public string? is_followup_active { get; set; } = "Y";

        public DateTime? data_migrated_on { get; set; }
    }
}
