﻿using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.WebApp.Authentication;
using ASI.Basecode.WebApp.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager _signInManager;
        private readonly IUserService _userService;
        private readonly SessionManager _sessionManager;

        public AccountController(
            SignInManager signInManager,
            IHttpContextAccessor httpContextAccessor,
            ILogger<AccountController> logger,
            IConfiguration configuration,
            IMapper mapper,
            IUserService userService)
        {
            _signInManager = signInManager;
            _userService = userService;
            _sessionManager = new SessionManager(httpContextAccessor.HttpContext.Session);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            TempData["returnUrl"] = System.Net.WebUtility.UrlDecode(HttpContext.Request.Query["ReturnUrl"]);
            _sessionManager.Clear();
            HttpContext.Session.SetString("SessionId", Guid.NewGuid().ToString());
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid login data.";
                return View(model);
            }

            User user = null;
            var loginResult = _userService.AuthenticateUser(model.UserName, model.Password, ref user);
            if (loginResult == LoginResult.Success)
            {
                await _signInManager.SignInAsync(user);
                HttpContext.Session.SetString("UserName", user.UserName);
                return RedirectToAction("ExpenseTable", "Expense");
            }

            TempData["ErrorMessage"] = "Incorrect username or password.";
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid registration data.";
                return View(model);
            }

            try
            {
                _userService.AddUser(model);
                return RedirectToAction("Login", "Account");
            }
            catch (InvalidDataException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred. Please try again.";
            }
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> SignOutUser()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
