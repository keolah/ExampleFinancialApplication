using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialGains
{
    public class Category
    {
        [Column("PK")]
        public long Id { get; set; }
        [Column("NAME")]
        [DisplayName("Category")]
        public string Name { get; set; }
        [NotMapped]
        public bool IsSelected { get; set; }
    }
}
