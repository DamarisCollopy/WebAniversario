using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAniversario.Models;

namespace WebAniversario.Repository
{
    public interface IPessoaRepository
    {
        Task<Pessoa> Details(int? id);
        IEnumerable<Pessoa> Orden();
        IEnumerable<Pessoa> OrdenBirthday();
        IEnumerable<Pessoa> Search(string Nome, string Sobrenome);
    }
}