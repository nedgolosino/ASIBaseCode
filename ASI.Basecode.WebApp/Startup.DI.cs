﻿using ASI.Basecode.Data;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
using ASI.Basecode.WebApp.Authentication;
using ASI.Basecode.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ASI.Basecode.WebApp
{
    // Other services configuration
    internal partial class StartupConfigurer
    {
        /// <summary>
        /// Configures the other services.
        /// </summary>
        private void ConfigureOtherServices()
        {
            // Framework services
            this._services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            this._services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            // Common services
            this._services.AddScoped<TokenProvider>();
            this._services.TryAddSingleton<TokenProviderOptionsFactory>();
            this._services.TryAddSingleton<TokenValidationParametersFactory>();
            this._services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Services
            this._services.TryAddSingleton<TokenValidationParametersFactory>();
            this._services.AddScoped<IAssignmentService, AssignmentService>();


            // Repositories
            this._services.AddScoped<IAssignmentRepository, AssignmentRepository>();

            this._services.AddScoped<IUserService, UserService>();
            this._services.AddScoped<IAnnouncementService, AnnouncementService>();

            // Register ExpenseService and ExpenseRepository
            this._services.AddScoped<IExpenseService, ExpenseService>();
            this._services.AddScoped<IExpenseRepository, ExpenseRepository>();

            this._services.AddScoped<ICategoryService, CategoryService>();
            this._services.AddScoped<ICategoryRespository, CategoryRepository>();

            // Repositories
            this._services.AddScoped<IUserRepository, UserRepository>();
            this._services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();

            // Manager Class
            this._services.AddScoped<SignInManager>();

            // Register HTTP client
            this._services.AddHttpClient();
        }
    }
}
