using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAniversario.Data;
using WebAniversario.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace WebAniversario.Repository
{
    public class PessoaRepository : IPessoaRepository
    {
        private readonly WebAniversarioContext _context;

        public PessoaRepository(WebAniversarioContext context)
        {
            _context = context;
        }

        public static bool IsBeforeNow(DateTime now, DateTime dateTime)
        {
            return dateTime.Month < now.Month
                || (dateTime.Month == now.Month && dateTime.Day < now.Day);
        }
        public IEnumerable<Pessoa> Orden()
        {
            var ordenarAniversario = _context.Pessoa.ToList();

            var now = DateTime.Now;
            var ordenar = from dt in ordenarAniversario
                          orderby IsBeforeNow(now, dt.DataNascimento), dt.DataNascimento.Month, dt.DataNascimento.Day
                          select dt;
            return ordenar;
        }
        public IEnumerable<Pessoa> Search(string Nome, string Sobrenome)
        {
            var pesquisa = from m in _context.Pessoa.ToList()
                           select m;

            if (!String.IsNullOrEmpty(Nome))
            {
                pesquisa = pesquisa.Where(s => s.Nome.Contains(Nome) && s.Sobrenome.Contains(Sobrenome));
            }
            return pesquisa;
        }
        public IEnumerable<Pessoa> OrdenBirthday()
        {
            DateTime data = DateTime.Now;
            var pesquisa = from m in _context.Pessoa.ToList()
                           select m;

            pesquisa = pesquisa.Where(s => ((s.DataNascimento.Month == data.Month) && 
            (s.DataNascimento.Day == data.Day)));

            return pesquisa;
        }
        public async Task<Pessoa> Details(int? id)
        {
            var pessoa = await _context.Pessoa
                .FirstOrDefaultAsync(m => m.Id == id);
            return pessoa;
        }
    }
}
