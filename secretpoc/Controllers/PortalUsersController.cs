using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using secretpoc.Data;
using secretpoc.Models;

namespace secretpoc.Controllers
{
    public class PortalUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PortalUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.PortalUser.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portalUser = await _context.PortalUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (portalUser == null)
            {
                return NotFound();
            }

            return View(portalUser);
        }

        public IActionResult Create()
        {
            return View();
        }
        [RequireSensitiveReauth("Reauth:CreateUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PortalUser portalUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(portalUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(portalUser);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portalUser = await _context.PortalUser.FindAsync(id);
            if (portalUser == null)
            {
                return NotFound();
            }
            return View(portalUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,PortalUser portalUser)
        {
            if (id != portalUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(portalUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PortalUserExists(portalUser.Id))
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
            return View(portalUser);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portalUser = await _context.PortalUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (portalUser == null)
            {
                return NotFound();
            }

            return View(portalUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var portalUser = await _context.PortalUser.FindAsync(id);
            if (portalUser != null)
            {
                _context.PortalUser.Remove(portalUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PortalUserExists(int id)
        {
            return _context.PortalUser.Any(e => e.Id == id);
        }
    }
}
