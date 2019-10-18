using System;
using System.Collections.Generic;

namespace FinancialGains
{
    public interface IFinancialData
    {
        void AddBudget(Budget budget);
        void AddBudgetCategory(BudgetCategory budgetCategory);
        void AddTransaction(Transaction transaction);
        bool BudgetCategoryExists(long budgetFk, long categoryFk);
        bool BudgetExists(long id);
        void EditBudget(Budget budget);
        void EditTransaction(Transaction transaction);
        Budget FindBudgetById(long? id);
        Budget FindBudgetByNameAndAmount(string name, decimal amount);
        BudgetCategory FindBudgetCategory(long budgetFk, long categoryFk);
        Transaction FindTransactionById(long? id);
        List<Transaction> GetTransactionsByMonth(int? month, int? year);
        List<BudgetCategory> GetAllBudgetCategories();
        Budget GetBudgetById(long? id);
        List<BudgetCategory> GetBudgetCategoriesByBudgetId(long id);
        List<Budget> GetBudgets();
        List<Category> GetCategories();
        Category GetCategoryById(long id);
        Transaction GetTransactionById(long? id);
        List<Transaction> GetTransactions(DateTime? dateTime);
        void RemoveBudget(Budget budget);
        void RemoveBudgetCategory(BudgetCategory budgetCategory);
        void RemoveAllBudgetCategoriesByBudgetId(long id);
        void RemoveTransation(Transaction transaction);
        bool TransactionExists(long id);
    }
}