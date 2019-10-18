using System;
using System.Collections.Generic;
using System.Linq;

namespace FinancialGains
{
    public class FinancialBusiness : IFinancialBusiness
    {
        private readonly IFinancialData _data;

        public FinancialBusiness()
        {
            _data = new FinancialData(new FinancialDataContext());
        }

        #region Transaction
        public List<Transaction> GetTransactions(DateTime? dateTime)
        {
            List<Transaction> transactions = _data.GetTransactions(dateTime);
            foreach (var transaction in transactions)
            {
                transaction.Category = GetCategory(transaction.CategoryFk);
            }
            return transactions;
        }

        public Transaction GetTransactionById(long? id)
        {
            Transaction transaction = _data.GetTransactionById(id);
            if (transaction != null) { transaction.Category = GetCategory(transaction.CategoryFk); }
            return transaction;
        }

        public Transaction FindTransactionById(long? id)
        {
            Transaction transaction = _data.FindTransactionById(id);
            if (transaction != null) { transaction.Category = GetCategory(transaction.CategoryFk); }
            return transaction;
        }

        public List<Transaction> GetTransactionsByMonth(int month, int year)
        {
            return _data.GetTransactionsByMonth(month, year);
        }

        public void AddTransaction(Transaction transaction)
        {
            _data.AddTransaction(transaction);
        }

        public void EditTransaction(Transaction transaction)
        {
            _data.EditTransaction(transaction);
        }

        public void RemoveTransaction(Transaction transaction)
        {
            transaction.Category = null;
            _data.RemoveTransation(transaction);
        }

        public bool TransactionExists(long id)
        {
            return _data.TransactionExists(id);
        }
        #endregion

        #region Category
        public List<Category> GetCategories()
        {
            return _data.GetCategories();
        }

        public Category GetCategory(long id)
        {
            return _data.GetCategoryById(id);
        }

        public List<Category> GetSelectedCategories(Budget budget)
        {
            if (budget.Categories == null) { budget.Categories = GetCategories(); }
            List<BudgetCategory> budgetCategories = GetBudgetCategoriesByBudgetId(budget.Id);
            if (budgetCategories.Count == 0) { return budget.Categories; }
            foreach (var category in budget.Categories)
            {
                foreach (var bc in budgetCategories)
                {
                    if (bc.CategoryFk == category.Id) { category.IsSelected = true; }
                }
            }
            return budget.Categories;
        }
        #endregion

        #region Budget
        public List<Budget> GetBudgets()
        {
            List<Budget> budgets = _data.GetBudgets();
            foreach (var budget in budgets)
            {
                budget.Categories = GetSelectedCategories(budget);
            }
            return budgets;
        }

        public Budget GetBudgetById(long? id)
        {
            Budget budget = _data.GetBudgetById(id);
            budget.Categories = GetSelectedCategories(budget);
            return budget;
        }

        public Budget FindBudgetById(long? id)
        {
            Budget budget = _data.FindBudgetById(id);
            budget.Categories = GetSelectedCategories(budget);
            return budget;
        }

        public Budget FindBudgetByNameAndAmount(string name, decimal amount)
        {
            Budget budget = _data.FindBudgetByNameAndAmount(name, amount);
            budget.Categories = GetSelectedCategories(budget);
            return budget;
        }

        public void AddBudget(Budget budget)
        {
            _data.AddBudget(budget);
        }

        public void EditBudget(Budget budget)
        {
            _data.EditBudget(budget);
        }

        public void RemoveBudget(Budget budget)
        {
            RemoveAllBudgetCategoriesByBudgetId(budget.Id);
            _data.RemoveBudget(budget);
        }

        public bool BudgetExists(long id)
        {
            return _data.BudgetExists(id);
        }

        public OverallBudgetGoal GetOverallBudgetGoalProgress(DateTime dateTime)
        {
            OverallBudgetGoal goal = new OverallBudgetGoal { DateTime = dateTime };
            goal.BudgetRealities = new List<BudgetReality>();
            IEnumerable<Transaction> transactions = GetTransactionsByMonth(dateTime.Month, dateTime.Year);
            List<Budget> budgets = GetBudgets();

            foreach (var budget in budgets)
            {
                List<long> categoryIds = new List<long>();

                foreach (var category in budget.Categories)
                {
                    if (category.IsSelected) { categoryIds.Add(category.Id); }
                }

                List<Transaction> budgetTransactions = transactions.AsQueryable().Where(t => categoryIds.Contains(t.CategoryFk)).ToList();
                decimal amountSpent = 0;

                foreach (var transaction in budgetTransactions)
                {
                    amountSpent += transaction.Amount;
                }

                goal.BudgetRealities.Add(new BudgetReality
                {
                    Budget = budget,
                    AmountSpent = amountSpent,
                    BudgetMet = amountSpent <= budget.Amount,
                    Transactions = budgetTransactions
                });
            }

            return goal;
        }
        #endregion

        #region BudgetCategory
        public List<BudgetCategory> GetAllBudgetCategories()
        {
            return _data.GetAllBudgetCategories();
        }

        public List<BudgetCategory> GetBudgetCategoriesByBudgetId(long budgetFk)
        {
            return _data.GetBudgetCategoriesByBudgetId(budgetFk);
        }

        public BudgetCategory FindBudgetCategory(long budgetFk, long categoryFk)
        {
            return _data.FindBudgetCategory(budgetFk, categoryFk);
        }

        public void AddBudgetCategory(long budgetFk, long categoryFk)
        {
            if (!BudgetCategoryExists(budgetFk, categoryFk))
            {
                _data.AddBudgetCategory(new BudgetCategory { BudgetFk = budgetFk, CategoryFk = categoryFk });
            }
        }

        public void RemoveBudgetCategory(long budgetFk, long categoryFk)
        {
            if (BudgetCategoryExists(budgetFk, categoryFk))
            {
                _data.RemoveBudgetCategory(FindBudgetCategory(budgetFk, categoryFk));
            }
        }

        public void RemoveAllBudgetCategoriesByBudgetId(long id)
        {
            _data.RemoveAllBudgetCategoriesByBudgetId(id);
        }

        public bool BudgetCategoryExists(long budgetFk, long categoryFk)
        {
            return _data.BudgetCategoryExists(budgetFk, categoryFk);
        }
        #endregion
    }
}
