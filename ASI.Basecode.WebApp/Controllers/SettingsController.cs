using ASI.Basecode.Data;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ASI.Basecode.WebApp.Controllers
{
    public class SettingsController : Controller
    {
        private readonly AsiBasecodeDBContext _context;
        private readonly ILogger<SettingsController> _logger;
        private readonly string _uploadsFolder;

        public SettingsController(AsiBasecodeDBContext context, ILogger<SettingsController> logger)
        {
            _context = context;
            _logger = logger;
            _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profiles");
        }

        private string GetLoggedInUserName()
        {
            return User.Identity.Name;
        }

        public IActionResult AccountDetails()
        {
            try
            {
                string userName = GetLoggedInUserName();
                var user = _context.Users.FirstOrDefault(u => u.UserName == userName);

                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load account details.");
                return View("Error", new ErrorViewModel { ErrorMessage = "Failed to load account details." });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAccount(User user, IFormFile ProfilePicture)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _context.Users.FindAsync(user.Id);
                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    // Handle profile picture upload
                    if (ProfilePicture != null && ProfilePicture.Length > 0)
                    {
                        // Create uploads directory if it doesn't exist
                        if (!Directory.Exists(_uploadsFolder))
                        {
                            Directory.CreateDirectory(_uploadsFolder);
                        }

                        // Generate unique filename
                        string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(ProfilePicture.FileName)}";
                        string filePath = Path.Combine(_uploadsFolder, uniqueFileName);

                        // Delete old profile picture if it exists
                        if (!string.IsNullOrEmpty(existingUser.ProfilePicture))
                        {
                            string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",
                                existingUser.ProfilePicture.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        // Save new profile picture
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ProfilePicture.CopyToAsync(fileStream);
                        }

                        // Update database path
                        existingUser.ProfilePicture = $"/uploads/profiles/{uniqueFileName}";

                        // **Update the session with the new profile picture URL**
                        HttpContext.Session.SetString("ProfilePicture", existingUser.ProfilePicture);
                    }

                    // Update other user fields
                    existingUser.FirstName = user.FirstName;
                    existingUser.LastName = user.LastName;
                    existingUser.Email = user.Email;
                    existingUser.UpdatedBy = GetLoggedInUserName();
                    existingUser.UpdatedTime = DateTime.Now;

                    // Save changes to the database
                    await _context.SaveChangesAsync();

                    // **Update the session with the new user information**
                    HttpContext.Session.SetString("UserName", existingUser.FirstName + " " + existingUser.LastName);

                    HttpContext.Session.SetString("ProfilePicture", existingUser.ProfilePicture ?? "/images/default-avatar.png");

                    TempData["SuccessMessage"] = "Profile updated successfully!";
                    return RedirectToAction(nameof(AccountDetails));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to update account details.");
                    TempData["ErrorMessage"] = "Failed to update account details. Please try again.";
                }
            }

            return View("AccountDetails", user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string Password, string newPassword, string confirmPassword)
        {
            try
            {
                string userName = GetLoggedInUserName(); // Get the logged-in user's username
                var user = _context.Users.FirstOrDefault(u => u.UserName == userName);

                if (user == null)
                {
                    TempData["ErrorMessage"] = "User not found.";
                    return RedirectToAction(nameof(AccountDetails));
                }

                // Validate the current password
                try
                {
                    string decryptedPassword = PasswordManager.DecryptPassword(user.Password);

                    if (decryptedPassword != Password)
                    {
                        TempData["ErrorMessage"] = "Current password is incorrect.";
                        return RedirectToAction(nameof(AccountDetails));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error decrypting password. Possible data corruption.");
                    TempData["ErrorMessage"] = "Failed to verify the current password. Please contact support.";
                    return RedirectToAction(nameof(AccountDetails));
                }

                // Validate the new password and confirm password match
                if (newPassword != confirmPassword)
                {
                    TempData["ErrorMessage"] = "New password and confirm password do not match.";
                    return RedirectToAction(nameof(AccountDetails));
                }

                // Encrypt the new password and update the user
                user.Password = PasswordManager.EncryptPassword(newPassword);
                user.UpdatedBy = userName;
                user.UpdatedTime = DateTime.Now;

                // Save the updated password
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Password changed successfully! You can now log in with your new password.";
                return RedirectToAction(nameof(AccountDetails));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to change password.");
                TempData["ErrorMessage"] = "Failed to change password. Please try again.";
                return RedirectToAction(nameof(AccountDetails));
            }
        }


    }
}
