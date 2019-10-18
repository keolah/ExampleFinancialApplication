using System;
using FinancialGains;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancialGainsWeb.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly IFinancialBusiness _business;

        public TransactionsController(IFinancialBusiness business)
        {
            _business = business;
        }

        public IActionResult Index()
        {
            return View(_business.GetTransactions(null));
        }

        public IActionResult Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = _business.GetTransactionById(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }
        
        public IActionResult Create()
        {
            ViewData["Categories"] = _business.GetCategories();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(DateTime dateTime)
        {
            ViewData["Month"] = dateTime.Month;
            ViewData["MonthName"] = dateTime.ToString("MMMM");
            ViewData["Year"] = dateTime.Year;
            return View(_business.GetTransactions(dateTime));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,TimeStamp,Amount,Description,CategoryFk")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _business.AddTransaction(transaction);
                return RedirectToAction(nameof(Index));
            }
            return View(transaction);
        }
        
        public IActionResult Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = _business.FindTransactionById(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return View(transaction);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,TimeStamp,Amount,Description,CategoryFk")] Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _business.EditTransaction(transaction);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.Id))
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
            return View(transaction);
        }
        
        public IActionResult Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = _business.GetTransactionById(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(long id)
        {
            var transaction = _business.FindTransactionById(id);
            _business.RemoveTransaction(transaction);
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(long id)
        {
            return _business.TransactionExists(id);
        }
    }
}
