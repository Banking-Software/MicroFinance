using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.CompanyProfile
{
    public class UpdateCompanyProfileDto
    {
        [Required]
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string? CompanyNameNepali { get; set; }
        public string? CompanyAddress { get; set; }
        public string? CompanyAddressNepali { get; set; }
        public string? PANNo { get; set; }
        public string? EstablishedDate { get; set; }
        public string? PhoneNo { get; set; }
        public string? CompanyEmailAddress { get; set; }
        public DateTime CompanyValidityStartDate { get; set; }
        public DateTime CompanyValidityEndDate { get; set; }
        public IFormFile? CompanyLogo { get; set; }
        public bool IsLogoChanged { get; set; }=false;
        public decimal CurrentTax { get; set; }
        public string CurrentFiscalYear { get; set; }
    }
}