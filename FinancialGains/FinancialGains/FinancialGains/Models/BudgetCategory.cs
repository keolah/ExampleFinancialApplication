using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialGains
{
    public class BudgetCategory
    {
        [Column("PK")]
        public long Id { get; set; }
        [Column("BUDGET_FK")]
        [ForeignKey("PK")]
        public long BudgetFk { get; set; }
        [Column("CATEGORY_FK")]
        [ForeignKey("PK")]
        public long CategoryFk { get; set; }
        [NotMapped]
        public Category Category { get; set; }
    }
}
