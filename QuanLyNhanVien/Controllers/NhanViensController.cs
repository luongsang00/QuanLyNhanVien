using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyNhanVien.Models;

namespace QuanLyNhanVien.Controllers
{
    public class NhanViensController : Controller
    {
        private readonly quanlynhanvienContext _context;

        public NhanViensController(quanlynhanvienContext context)

        {
            _context = context;
        }

        public class NhanVienn
        {
            public int Id { get; set; }
            public string Name { get; set; }
            //public string Loai { get; set; }
            public string SoDT { get; set; }
            public string Skill { get; set; }
            public string ChucVu { get; set; }
            public string DiaChi { get; set; }
        }

        // GET: NhanViens1
        public async Task<IActionResult> Index()
        {

                
                return View(await _context.NhanViens.ToListAsync());
        }

        // GET: NhanViens1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NhanViens == null)
            {
                return NotFound();
            }
            //NhanVienn nhanVienns = (from p in _context.NhanViens
            //                        join c in _context.LoaiNhanViens on p.IdLoaiNv equals c.IdLoaiNv
            //                        select new NhanVienn
            //                        {
            //                            Id = p.Id,
            //                            Name = p.Ten,
            //                            Loai = c.TenLoaiNv
            //                        }).ToList();



            
            //var nhanvien = await _context.NhanViens;
            NhanVienn nhanVien = await (from p in _context.NhanViens 
                                  join c in _context.LoaiNhanViens on p.IdLoaiNv equals c.IdLoaiNv 
                                  join d in _context.KyNangs on p.IdKyNang equals d.IdKyNang
                                  select new NhanVienn { 
                                  Id = p.Id,
                                  Name=p.Ten,
                                  ChucVu = c.TenLoaiNv,
                                  SoDT=p.SoDienThoai,
                                  Skill=d.TenLoaiKn,
                                  DiaChi=p.DiaChi})
                .FirstOrDefaultAsync(m => m.Id == id);
                
            
            if (nhanVien == null)
            {
                return NotFound();
            }

            return View(nhanVien);
        }

        // GET: NhanViens1/Create
        public IActionResult Create()
        {   //Tạo select chức vụ nhân viên 
            List<LoaiNhanVien> LoaiNV = _context.LoaiNhanViens.ToList();
            SelectList ListChucVu = new SelectList(LoaiNV, "IdLoaiNv", "TenLoaiNv");
            ViewBag.LoaiNvlist = ListChucVu;
            //Tạo select skill
            List<KyNang> KyNang = _context.KyNangs.ToList(); 
            SelectList ListKyNang = new SelectList(KyNang, "IdKyNang", "TenLoaiKn");
            ViewBag.KyNanglist = ListKyNang;

            return View();
        }

        // POST: NhanViens1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ten,DiaChi,SoDienThoai,IdLoaiNv,IdKyNang")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nhanVien);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(nhanVien);
        }

        // GET: NhanViens1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //Tạo select chức vụ 

            List<LoaiNhanVien> dep =_context.LoaiNhanViens.ToList();
            SelectList ListChucVu = new SelectList(dep, "IdLoaiNv", "TenLoaiNv");
            ViewBag.LoaiNvlist = ListChucVu;

            //Tạo select skill
            List<KyNang> KyNang = _context.KyNangs.ToList();
            SelectList ListKyNang = new SelectList(KyNang, "IdKyNang", "TenLoaiKn");
            ViewBag.KyNanglist = ListKyNang;



            if (id == null || _context.NhanViens == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            return View(nhanVien);
        }

        // POST: NhanViens1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ten,DiaChi,SoDienThoai,IdLoaiNv,IdKyNang")] NhanVien nhanVien)
        {
            if (id != nhanVien.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhanVien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhanVienExists(nhanVien.Id))
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
            return View(nhanVien);
        }

        // GET: NhanViens1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NhanViens == null)
            {
                return NotFound();
            }
            NhanVienn nhanVien = await (from p in _context.NhanViens
                                        join c in _context.LoaiNhanViens on p.IdLoaiNv equals c.IdLoaiNv
                                        join d in _context.KyNangs on p.IdKyNang equals d.IdKyNang
                                        select new NhanVienn
                                        {
                                            Id = p.Id,
                                            Name = p.Ten,
                                            ChucVu = c.TenLoaiNv,
                                            SoDT = p.SoDienThoai,
                                            Skill = d.TenLoaiKn,
                                            DiaChi = p.DiaChi
                                        })           
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            return View(nhanVien);
        }

        // POST: NhanViens1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NhanViens == null)
            {
                return Problem("Entity set 'quanlynhanvienContext.NhanViens'  is null.");
            }
            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien != null)
            {
                _context.NhanViens.Remove(nhanVien);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhanVienExists(int id)
        {
          return _context.NhanViens.Any(e => e.Id == id);
        }
    }
}
