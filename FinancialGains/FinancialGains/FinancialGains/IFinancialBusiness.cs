using System;
using System.Collections.Generic;

namespace FinancialGains
{
    public interface IFinancialBusiness
    {
        void AddBudget(Budget budget);
        void AddBudgetCategory(long budgetFk, long categoryFk);
        void AddTransaction(Transaction transaction);
        bool BudgetCategoryExists(long budgetFk, long categoryFk);
        bool BudgetExists(long id);
        void EditBudget(Budget budget);
        void EditTransaction(Transaction transaction);
        Budget FindBudgetById(long? id);
        Budget FindBudgetByNameAndAmount(string name, decimal amount);
        BudgetCategory FindBudgetCategory(long budgetFk, long categoryFk);
        Transaction FindTransactionById(long? id);
        List<Transaction> GetTransactionsByMonth(int month, int year);
        List<BudgetCategory> GetAllBudgetCategories();
        Budget GetBudgetById(long? id);
        List<BudgetCategory> GetBudgetCategoriesByBudgetId(long budgetFk);
        List<Budget> GetBudgets();
        List<Category> GetCategories();
        Category GetCategory(long id);
        List<Category> GetSelectedCategories(Budget budget);
        Transaction GetTransactionById(long? id);
        List<Transaction> GetTransactions(DateTime? dateTime);
        OverallBudgetGoal GetOverallBudgetGoalProgress(DateTime dateTime);
        void RemoveBudget(Budget budget);
        void RemoveBudgetCategory(long budgetFk, long categoryFk);
        void RemoveAllBudgetCategoriesByBudgetId(long id);
        void RemoveTransaction(Transaction transaction);
        bool TransactionExists(long id);
    }
}