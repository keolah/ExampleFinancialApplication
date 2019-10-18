using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinancialGains;

namespace FinancialGainsWeb.Controllers
{
    public class BudgetsController : Controller
    {
        private readonly IFinancialBusiness _business;

        public BudgetsController(IFinancialBusiness business)
        {
            _business = business;
        }
        
        public IActionResult Index()
        {
            return View(_business.GetBudgets());
        }
        
        public IActionResult Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budget = _business.GetBudgetById(id);
            if (budget == null)
            {
                return NotFound();
            }

            budget.Categories = _business.GetSelectedCategories(budget);
            return View(budget);
        }
        
        public IActionResult Create()
        {
            List<Category> categories = _business.GetCategories();
            return View(new Budget { Categories = categories });
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Amount,Categories")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                List<Category> categories = budget.Categories;
                budget.Categories = null;
                _business.AddBudget(budget);
                Budget createdBudget = _business.FindBudgetByNameAndAmount(budget.Name, budget.Amount);
                foreach (var category in categories)
                {
                    if (category.IsSelected) { _business.AddBudgetCategory(createdBudget.Id, category.Id); }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(budget);
        }
        
        public IActionResult Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budget = _business.FindBudgetById(id);
            if (budget == null)
            {
                return NotFound();
            }
            budget.Categories = _business.GetSelectedCategories(budget);
            return View(budget);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id, [Bind("Id,Name,Amount,Categories")] Budget budget)
        {
            if (id != budget.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var category in budget.Categories)
                    {
                        if (category.IsSelected) { _business.AddBudgetCategory(budget.Id, category.Id); }
                        else { _business.RemoveBudgetCategory(budget.Id, category.Id); }
                    }
                    _business.EditBudget(budget);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BudgetExists(budget.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(budget);
        }
        
        public IActionResult Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budget = _business.GetBudgetById(id);
            if (budget == null)
            {
                return NotFound();
            }

            return View(budget);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(long id)
        {
            var budget = _business.FindBudgetById(id);
            _business.RemoveBudget(budget);
            return RedirectToAction(nameof(Index));
        }

        private bool BudgetExists(long id)
        {
            return _business.BudgetExists(id);
        }
        
        public IActionResult BudgetGoal(DateTime? dateTime = null)
        {
            if (dateTime == null || dateTime == DateTime.MinValue)
            {
                dateTime = DateTime.Now;
            }
            
            return View(_business.GetOverallBudgetGoalProgress(dateTime.Value));
        }
    }
}
