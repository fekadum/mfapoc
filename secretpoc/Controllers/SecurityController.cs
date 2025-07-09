using Microsoft.AspNetCore.Mvc;
using OtpNet;
using secretpoc.Data;
using secretpoc.Models;

public class SecurityController : Controller
{
    private readonly ApplicationDbContext _db;

    public SecurityController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult VerifyAuthenticator(string actionKey)
    {
        var model = new TOTPVerificationViewModel
        {
            ActionKey = actionKey
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult VerifyAuthenticator(TOTPVerificationViewModel model)
    {
        var user = GetCurrentUser();

        if (string.IsNullOrEmpty(user.AuthenticatorKey))
        {
            ModelState.AddModelError("", "Authenticator is not set up.");
            return View(model);
        }

        var totp = new Totp(Base32Encoding.ToBytes(user.AuthenticatorKey));
        bool isValid = totp.VerifyTotp(model.AuthenticatorCode, out _, new VerificationWindow(2, 2));

        if (!isValid)
        {
            ModelState.AddModelError("", "Invalid TOTP code.");
            return View(model);
        }

        // Store per-action verification
        HttpContext.Session.SetString(model.ActionKey, DateTime.UtcNow.ToString());

        // Optionally redirect to a known location, or use a returnUrl if stored
        return RedirectToAction("Index", "Home"); // or redirect based on ActionKey
    }

    private PortalUser GetCurrentUser()
    {
        var username = User.Identity?.Name;
        return _db.PortalUser.FirstOrDefault(u => u.Username == username);
    }
}
