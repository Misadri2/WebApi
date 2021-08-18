using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProdutosController(ApplicationDbContext context)
        {
            _context = context;
        }

        //AsNoTracking() é usado para melhorar a perfomance em métodos de consulta

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
        try        
        { 
             return _context.Produtos.AsNoTracking().ToList();
        }
        catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Erro ao tentar obter a lista de Produtos");
            }
         }          
        

        [HttpGet("{id}", Name = "Obter Produto")]
        public ActionResult<Produto> GetId(int id)
        {
            var produto = _context.Produtos.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id);
            if (produto == null)
            {
                return NotFound();
            }
            return produto;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Produto produto)
        {
            /* if(!ModelState.IsValid){  //!Não precisa fazer isso mais, o framework faz automático.
                return BadRequest(ModelState);
            } */
            _context.Produtos.Add(produto);
            _context.SaveChanges();
                
            return new CreatedAtRouteResult("Obter Produto",
            new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest();
            }
            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok("Produto Alterado");
        }

        [HttpDelete("{id}")]
        public ActionResult <Produto> Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            if (produto == null)
            {
                return NotFound();
            }
            _context.Produtos.Remove(produto);
            _context.SaveChanges();
            return Ok("Produto excluído");
            
        }


    }
}