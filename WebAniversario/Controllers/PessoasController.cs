using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAniversario.Data;
using WebAniversario.Models;

namespace WebAniversario.Controllers
{
    public class PessoasController : Controller
    {
        private readonly WebAniversarioContext _context;
        private static bool IsBeforeNow(DateTime now, DateTime dateTime)
        {
            return dateTime.Month < now.Month
                || (dateTime.Month == now.Month && dateTime.Day < now.Day);
        }
        public PessoasController(WebAniversarioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var ordenarAniversario = _context.Pessoa.ToList();

            var now = DateTime.Now;
            var ordenar = from dt in ordenarAniversario
                          orderby IsBeforeNow(now, dt.DataNascimento), dt.DataNascimento.Month, dt.DataNascimento.Day
                          select dt;

            return View(ordenar);
        }
        [HttpPost]
        public ActionResult Index(string Nome, string Sobrenome)
        {

            var pesquisa = from m in _context.Pessoa.ToList()
                           select m;

            if (!String.IsNullOrEmpty(Nome))
            {
                pesquisa = pesquisa.Where(s => s.Nome.Contains(Nome) && s.Sobrenome.Contains(Sobrenome));
            }

            return View(pesquisa);
        }
        [HttpGet]
        public ActionResult IndexAniversario()
        {
            DateTime data = DateTime.Now;
            var pesquisa = from m in _context.Pessoa.ToList()
                           select m;

            pesquisa = pesquisa.Where(s => ((s.DataNascimento.Month == data.Month) && (s.DataNascimento.Day == data.Day)));

            return View(pesquisa);
        }

        // GET: Pessoas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pessoa = await _context.Pessoa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pessoa == null)
            {
                return NotFound();
            }

            return View(pessoa);
        }

        // GET: Pessoas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pessoas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Sobrenome,DataNascimento")] Pessoa pessoa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pessoa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pessoa);
        }

        // GET: Pessoas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pessoa = await _context.Pessoa.FindAsync(id);
            if (pessoa == null)
            {
                return NotFound();
            }
            return View(pessoa);
        }

        // POST: Pessoas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Sobrenome,DataNascimento")] Pessoa pessoa)
        {
            if (id != pessoa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pessoa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PessoaExists(pessoa.Id))
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
            return View(pessoa);
        }

        // GET: Pessoas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pessoa = await _context.Pessoa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pessoa == null)
            {
                return NotFound();
            }

            return View(pessoa);
        }

        // POST: Pessoas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pessoa = await _context.Pessoa.FindAsync(id);
            _context.Pessoa.Remove(pessoa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PessoaExists(int id)
        {
            return _context.Pessoa.Any(e => e.Id == id);
        }
    }
}
