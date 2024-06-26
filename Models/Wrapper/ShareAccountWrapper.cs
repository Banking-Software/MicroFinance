using MicroFinance.Enums.Client;

namespace MicroFinance.Models.Wrapper
{
    public class ShareAccountWrapper
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public decimal CurrentShareBalance { get; set; }
        public bool IsActive { get; set; }
        public string ClientName { get; set; }
        public string ClientMemberId { get; set; }
        public ShareTypeEnum ShareType { get; set; }
    }
}