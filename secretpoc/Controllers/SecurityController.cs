using Microsoft.AspNetCore.Mvc;
using OtpNet;
using secretpoc.Data;
using secretpoc.Models;

namespace secretpoc.Controllers
{
    public class SecurityController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SecurityController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult SetupAuthenticator()
        {
            var user = GetCurrentUser();
            var secret = GenerateSecretKey();
            if (user != null && !string.IsNullOrWhiteSpace(secret))
            {

            var qrUrl = GenerateQrCodeUri(user.Username, secret);
            TempData["SecretKey"] = secret;

            return View(new SetupAuthenticatorViewModel
            {
                SecretKey = secret,
                QrCodeUrl = qrUrl
            });
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult SetupAuthenticator(SetupAuthenticatorViewModel model)
        {
            var user = GetCurrentUser();
            var secret = TempData["SecretKey"]?.ToString();

            var totp = new Totp(Base32Encoding.ToBytes(secret));
            if (totp.VerifyTotp(model.Code, out _, new VerificationWindow(2, 2)))
            {
                user.AuthenticatorKey = secret;
                _db.SaveChanges();
                return RedirectToAction("SetupSuccess"); // You can create this view later
            }

            ModelState.AddModelError("", "Invalid code.");
            model.QrCodeUrl = GenerateQrCodeUri(user.Username, secret);
            model.SecretKey = secret;
            TempData.Keep("SecretKey");

            return View(model);
        }

        [HttpGet]
        public IActionResult VerifyAuthenticator(string actionKey)
        {
            return View(new TOTPVerificationViewModel { ActionKey = actionKey });
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
            var isValid = totp.VerifyTotp(model.AuthenticatorCode, out _, new VerificationWindow(2, 2));

            if (!isValid)
            {
                ModelState.AddModelError("", "Invalid code.");
                return View(model);
            }

            HttpContext.Session.SetString(model.ActionKey, DateTime.UtcNow.ToString());
            return RedirectToAction("Index", "Home"); // Or redirect based on ActionKey
        }

        private PortalUser GetCurrentUser()
        {
            var username = User.Identity.Name;
            return _db.PortalUser.FirstOrDefault(u => u.Username == username);
        }

        private string GenerateSecretKey() => Base32Encoding.ToString(KeyGeneration.GenerateRandomKey(20));

        private string GenerateQrCodeUri(string username, string secret)
        {
            var appName = "SecretPOC";
            return $"otpauth://totp/{appName}:{username}?secret={secret}&issuer={appName}";
        }
    }
}
