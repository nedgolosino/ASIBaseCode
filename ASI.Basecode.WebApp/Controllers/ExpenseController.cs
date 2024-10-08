using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace ASI.Basecode.WebApp.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly IExpenseService _expenseService;
        private readonly ILogger<ExpenseController> _logger;

        public ExpenseController(IExpenseService expenseService, ILogger<ExpenseController> logger)
        {
            _expenseService = expenseService;
            _logger = logger;
        }

        private string GetLoggedInUserId()
        {
            return HttpContext.User.Identity.Name;
        }


        public IActionResult Index()
        {
            try
            {
                string userId = GetLoggedInUserId();

                var expenses = _expenseService.GetAllExpenses()
                                    .Where(e => e.UserName == userId) 
                                    .ToList();

                return View(expenses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load expenses.");
                return View("Error", new ErrorViewModel { ErrorMessage = "Failed to load expenses." });
            }
        }

        public IActionResult ExpenseTable()
        {
            try
            {
                string userId = GetLoggedInUserId();

      
                var expenses = _expenseService.GetAllExpenses()
                                    .Where(e => e.UserName == userId) // Filter by the current user's ID
                                    .ToList();

                return View(expenses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load expense table.");
                return View("Error", new ErrorViewModel { ErrorMessage = "Failed to load expense table." });
            }
        }

  
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Expense model)
        {
            if (ModelState.IsValid)
            {
                try
                {
              
                    model.UserName = GetLoggedInUserId();

                    _expenseService.AddExpense(model);
                    TempData["SuccessMessage"] = "Expense added successfully!";
                    return RedirectToAction(nameof(ExpenseTable)); 
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to add new expense.");
                    TempData["ErrorMessage"] = "Failed to add new expense. Please try again.";
                }
            }
            return View(model);
        }

    
        public IActionResult Edit(int id)
        {
            var expense = _expenseService.GetExpenseById(id);
            if (!HasAccessToExpense(expense))
            {
                TempData["ErrorMessage"] = "Expense not found or access denied.";
                return RedirectToAction(nameof(ExpenseTable));
            }
            return View(expense);
        }

  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Expense model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!HasAccessToExpense(model))
                    {
                        TempData["ErrorMessage"] = "Access denied.";
                        return RedirectToAction(nameof(ExpenseTable));
                    }

                    _expenseService.UpdateExpense(model);
                    TempData["SuccessMessage"] = "Expense updated successfully!";
                    return RedirectToAction(nameof(ExpenseTable));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to update expense.");
                    TempData["ErrorMessage"] = "Failed to update expense. Please try again.";
                }
            }
            return View(model);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var expense = _expenseService.GetExpenseById(id);
                if (!HasAccessToExpense(expense))
                {
                    TempData["ErrorMessage"] = "Expense not found or access denied.";
                    return RedirectToAction(nameof(ExpenseTable));
                }

                _expenseService.DeleteExpense(id);
                TempData["SuccessMessage"] = "Expense deleted successfully!";
                return RedirectToAction(nameof(ExpenseTable));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete expense.");
                TempData["ErrorMessage"] = "Failed to delete expense. Please try again.";
                return RedirectToAction(nameof(ExpenseTable));
            }
        }


        public IActionResult Details(int id)
        {
            var expense = _expenseService.GetExpenseById(id);
            if (!HasAccessToExpense(expense))
            {
                TempData["ErrorMessage"] = "Expense not found or access denied.";
                return RedirectToAction(nameof(ExpenseTable));
            }
            return View(expense);
        }

     
        private bool HasAccessToExpense(Expense expense)
        {
            return expense != null && expense.UserName == GetLoggedInUserId();
        }
    }
}
