using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialGains
{
    public class Transaction
    {
        [Column("PK")]
        public long Id { get; set; }
        [Column("TIME_STAMP")]
        [DisplayName("Time Stamp")]
        public DateTime TimeStamp { get; set; }
        [Column("AMOUNT")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Amount { get; set; }
        [Column("DESCRIPTION")]
        public string Description { get; set; }
        [Column("CATEGORY_FK")]
        public long CategoryFk { get; set; }
        [NotMapped]
        public Category Category { get; set; }

        public Transaction()
        {
        }

        public Transaction(int id, DateTime timeStamp, decimal amount, string description, int categoryFk)
        {
            if (timeStamp == null || timeStamp == DateTime.MinValue || timeStamp == DateTime.MaxValue) { throw new ArgumentException("timeStamp must be a valid DateTIme.", "timeStamp"); }
            if (amount <= 0.0m) { throw new ArgumentException("amount must be a valid dollar amount.", "amount"); }
            if (description == null) { description = string.Empty; }

            Id = id;
            TimeStamp = timeStamp;
            Amount = amount;
            Description = description;
            CategoryFk = categoryFk;
        }

        public Transaction(int id, DateTime timeStamp, decimal amount, string description, int categoryFk, Category category)
        {
            if (timeStamp == null || timeStamp == DateTime.MinValue || timeStamp == DateTime.MaxValue) { throw new ArgumentException("timeStamp must be a valid DateTIme.", "timeStamp"); }
            if (amount <= 0.0m) { throw new ArgumentException("amount must be a valid dollar amount.", "amount"); }
            if (description == null) { description = string.Empty; }
            if (category == null || string.IsNullOrWhiteSpace(category.Name)) { throw new ArgumentException("category must be a valid category.", "category"); }

            Id = id;
            TimeStamp = timeStamp;
            Amount = amount;
            Description = description;
            CategoryFk = categoryFk;
            Category = category;
        }
    }
}
