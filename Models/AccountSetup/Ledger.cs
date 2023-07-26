using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MicroFinance.Models.ClientSetup;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.Transactions;
using MicroFinance.Services;

namespace MicroFinance.Models.AccountSetup
{
    public class Ledger
    {
        public int Id { get; set; }
        public int? LedgerCode { get; set; }
        public virtual GroupType GroupType { get; set; }
        public int GroupTypeId { get; set; }
        [Required]
        public string Name { get; set; }
        public string? NepaliName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EntryDate { get; set; }
        [Column(TypeName ="decimal(5,2)")]
        public decimal? DepreciationRate { get; set; }
        public string? HisabNumber { get; set; }
        [Required]
        public bool IsSubLedgerActive { get; set; } // If True then allow to create Sub Ledger
        [Column(TypeName ="decimal(18,4)")]
        [ValidationForNegativeBalanceService]
        public decimal CurrentBalance { get; set; }=0;        
        [Required]
        public bool IsBank { get; set; }
        public virtual ICollection<DepositScheme> DepositSchemes { get; set; }
        public virtual BankSetup BankSetup{ get; set; }
        public virtual ICollection<SubLedger> SubLedger { get; set; } 
        public virtual ICollection<Client> Client {get; set;}
        public virtual ICollection<LedgerTransaction> LedgerTransactions { get; set; }
    }
}