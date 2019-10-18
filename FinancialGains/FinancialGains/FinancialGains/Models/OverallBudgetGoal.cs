using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FinancialGains
{
    public class OverallBudgetGoal
    {
        public DateTime DateTime { get; set; }
        public List<BudgetReality> BudgetRealities { get; set; }
    }

    public class BudgetReality
    {
        public Budget Budget { get; set; }
        [DisplayName("Amount Spent")]
        public decimal AmountSpent { get; set; }
        public bool BudgetMet { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}