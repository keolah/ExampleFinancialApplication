using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialGains
{
    public class Budget
    {
        [Column("PK")]
        public long Id { get; set; }
        [Column("NAME")]
        [DisplayName("Budget Name")]
        public string Name { get; set; }
        [Column("AMOUNT")]
        [DisplayName("Amount Budgeted")]
        public decimal Amount { get; set; }
        [NotMapped]
        [DisplayName("Categories")]
        public List<Category> Categories { get; set; }
    }
}

