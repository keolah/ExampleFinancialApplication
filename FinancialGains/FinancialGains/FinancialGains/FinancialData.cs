using System;
using System.Collections.Generic;
using System.Linq;

namespace FinancialGains
{
    public class FinancialData : IFinancialData
    {
        private readonly FinancialDataContext _context;

        public FinancialData()
        {
            _context = new FinancialDataContext();
        }

        public FinancialData(FinancialDataContext context)
        {
            _context = context;
        }

        #region Transaction
        public List<Transaction> GetTransactions(DateTime? dateTime)
        {
            if (dateTime != null) { return GetTransactionsByMonth(dateTime?.Month, dateTime?.Year); }

            return _context.Transaction.OrderByDescending(t => t.TimeStamp).ToList();
        }

        public Transaction GetTransactionById(long? id)
        {
            return _context.Transaction.FirstOrDefault(t => t.Id == id);
        }

        public Transaction FindTransactionById(long? id)
        {
            return _context.Transaction.Find(id);
        }

        public List<Transaction> GetTransactionsByMonth(int? month, int? year)
        {
            return _context.Transaction.Where(t => t.TimeStamp.Month == month && t.TimeStamp.Year == year).ToList();
        }

        public void AddTransaction(Transaction transaction)
        {
            _context.Add(transaction);
            _context.SaveChanges();
        }

        public void EditTransaction(Transaction transaction)
        {
            _context.Update(transaction);
            _context.SaveChanges();
        }

        public void RemoveTransation(Transaction transaction)
        {
            _context.Transaction.Remove(transaction);
            _context.SaveChanges();
        }

        public bool TransactionExists(long id)
        {
            return _context.Transaction.Any(e => e.Id == id);
        }
        #endregion

        #region Category
        public List<Category> GetCategories()
        {
            // Create a new object Category for every member in the list to avoid referencing issues
            return _context.Category.OrderBy(c => c.Name).Select(c => new Category { Id = c.Id, Name = c.Name, IsSelected = false }).ToList();
        }

        public Category GetCategoryById(long id)
        {
            return _context.Category.FirstOrDefault(c => c.Id == id);
        }
        #endregion

        #region Budget
        public List<Budget> GetBudgets()
        {
            return _context.Budget.OrderBy(b => b.Name).Select(b => new Budget { Id = b.Id, Name = b.Name, Amount = b.Amount }).ToList();
        }

        public Budget GetBudgetById(long? id)
        {
            return _context.Budget.FirstOrDefault(b => b.Id == id);
        }

        public Budget FindBudgetById(long? id)
        {
            return _context.Budget.Find(id);
        }

        public Budget FindBudgetByNameAndAmount(string name, decimal amount)
        {
            return _context.Budget.FirstOrDefault(b => b.Name == name && b.Amount == amount);
        }

        public void AddBudget(Budget budget)
        {
            _context.Add(budget);
            _context.SaveChanges();
        }

        public void EditBudget(Budget budget)
        {
            _context.Update(budget);
            _context.SaveChanges();
        }

        public void RemoveBudget(Budget budget)
        {
            _context.Budget.Remove(budget);
            _context.SaveChanges();
        }

        public bool BudgetExists(long id)
        {
            return _context.Budget.Any(e => e.Id == id);
        }
        #endregion

        #region BudgetCategory
        public List<BudgetCategory> GetAllBudgetCategories()
        {
            List<BudgetCategory> budgetCategories = _context.BudgetCategory.ToList();
            foreach (var bc in budgetCategories)
            {
                bc.Category = GetCategoryById(bc.CategoryFk);
            }
            return budgetCategories;
        }

        public List<BudgetCategory> GetBudgetCategoriesByBudgetId(long id)
        {
            List<BudgetCategory> budgetCategories = _context.BudgetCategory.Where(bc => bc.BudgetFk == id).ToList();
            foreach (var bc in budgetCategories)
            {
                bc.Category = GetCategoryById(bc.CategoryFk);
            }
            return budgetCategories;
        }

        public BudgetCategory FindBudgetCategory(long budgetFk, long categoryFk)
        {
            return _context.BudgetCategory.FirstOrDefault(bc => bc.BudgetFk == budgetFk && bc.CategoryFk == categoryFk);
        }

        public  void AddBudgetCategory(BudgetCategory budgetCategory)
        {
            _context.Add(budgetCategory);
            _context.SaveChanges();
        }

        public  void RemoveBudgetCategory(BudgetCategory budgetCategory)
        {
            _context.BudgetCategory.Remove(budgetCategory);
            _context.SaveChanges();
        }

        public void RemoveAllBudgetCategoriesByBudgetId(long id)
        {
            List<BudgetCategory> budgetCategories = GetBudgetCategoriesByBudgetId(id);
            foreach (var bc in budgetCategories)
            {
                RemoveBudgetCategory(bc);
            }
        }

        public bool BudgetCategoryExists(long budgetFk, long categoryFk)
        {
            return _context.BudgetCategory.Any(bc => bc.BudgetFk == budgetFk && bc.CategoryFk == categoryFk);
        }
        #endregion
    }
}
