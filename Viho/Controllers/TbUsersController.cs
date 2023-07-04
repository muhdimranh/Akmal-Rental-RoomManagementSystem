using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Viho.web.DataDB;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;




namespace Viho.web.Controllers
{
    [Authorize]
    public class TbUsersController : Controller
    {

        
        private readonly DbRentalContext _context;

        public TbUsersController(DbRentalContext context)
        {
            _context = context;
        }

        // GET: TbUsers
        public async Task<IActionResult> Index()
        {
            var dbRentalContext = _context.TbUsers.Include(t => t.URole);
            return View(await dbRentalContext.ToListAsync());
        }

        // GET: TbUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TbUsers == null)
            {
                return NotFound();
            }

            var tbUser = await _context.TbUsers
                .Include(t => t.URole)
                .FirstOrDefaultAsync(m => m.UId == id);
            if (tbUser == null)
            {
                return NotFound();
            }

            return PartialView("_Details", tbUser);
        }

        // GET: TbUsers/Create
        public IActionResult Create()
        {
            ViewData["URoleid"] = new SelectList(_context.TbRoles, "RlId", "RlDesc");
            return View();
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    builder.Append(hashedBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        // POST: TbUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UId,UUsername,UPass,UPhone,UEmail,URoleid")] TbUser tbUser)
        {
            if (ModelState.IsValid)
            {
                tbUser.UPass = HashPassword(tbUser.UPass);
                _context.Add(tbUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            string errors = string.Join("; ", ModelState.Values
                .SelectMany(x=>x.Errors)
                .Select(x=>x.ErrorMessage));  
            ModelState.AddModelError("", errors);
            ViewData["URoleid"] = new SelectList(_context.TbRoles, "RlId", "RlDesc",  tbUser.URoleid);
            return View(tbUser);
        }

        // GET: TbUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TbUsers == null)
            {
                return NotFound();
            }

            var tbUser = await _context.TbUsers.FindAsync(id);
            if (tbUser == null)
            {
                return NotFound();
            }
            ViewData["URoleid"] = new SelectList(_context.TbRoles, "RlId", "RlDesc", tbUser.URoleid);
            return View(tbUser);
        }

        // POST: TbUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UId,UUsername,UPass,UPhone,UEmail,URoleid")] TbUser tbUser)
        {
            if (id != tbUser.UId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    tbUser.UPass = HashPassword(tbUser.UPass);
                    _context.Update(tbUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbUserExists(tbUser.UId))
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
            ViewData["URoleid"] = new SelectList(_context.TbRoles, "RlId", "RlDesc", tbUser.URoleid);
            return View(tbUser);
        }

        // GET: TbUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TbUsers == null)
            {
                return NotFound();
            }

            var tbUser = await _context.TbUsers
                .Include(t => t.URole)
                .FirstOrDefaultAsync(m => m.UId == id);
            if (tbUser == null)
            {
                return NotFound();
            }

            return View(tbUser);
        }

        // POST: TbUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TbUsers == null)
            {
                return Problem("Entity set 'DbRentalContext.TbUsers' is null.");
            }

            var tbUser = await _context.TbUsers.FindAsync(id);
            if (tbUser != null)
            {
                _context.TbUsers.Remove(tbUser);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool TbUserExists(int id)
        {
          return (_context.TbUsers?.Any(e => e.UId == id)).GetValueOrDefault();
        }
    }
}
