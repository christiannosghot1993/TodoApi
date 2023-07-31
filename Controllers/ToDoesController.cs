using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotLiquid;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelectPdf;
using TodoApi.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoesController : ControllerBase
    {
        private readonly MiDbContext _context;

        public ToDoesController(MiDbContext context)
        {
            _context = context;
        }

        // GET: api/ToDoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDo>>> GetToDo()
        {
          if (_context.ToDo == null)
          {
              return NotFound();
          }
            return await _context.ToDo.ToListAsync();
        }

        
        
        [HttpGet("ObtenerReporte")]
        public IActionResult obtenerReporte()
        {
            List<ToDo> tareasPendientes=_context.ToDo.Where(x=>x.Done==false).ToList();
            List<ToDo> tareasFinalizadas = _context.ToDo.Where(x => x.Done == true).ToList();
            string pathTemplate = "./assets/toDoReport.html";
            var html = "";
            using (StreamReader reader = new StreamReader(pathTemplate))
            {
                html = reader.ReadToEnd();
            }

            Template template = Template.Parse(html);
            var data = new
            {
                _tareasPendientes = tareasPendientes,
                _tareasFinalizadas = tareasFinalizadas
            };
            string htmlDinamico = template.Render(Hash.FromAnonymousObject(data));
            var htmlToPdf = new HtmlToPdf();
            var pdf = htmlToPdf.ConvertHtmlString(htmlDinamico);

            // Convertir el archivo PDF en un arreglo de bytes
            byte[] pdfBytes;
            using (MemoryStream pdfStream = new MemoryStream())
            {
                pdf.Save(pdfStream);
                pdfBytes = pdfStream.ToArray();
            }

            // Convertir el arreglo de bytes en una cadena Base64
            string base64String = Convert.ToBase64String(pdfBytes);

            // Devolver la cadena Base64 en una respuesta HTTP
            return Content(base64String, "application/pdf");
        }

        // GET: api/ToDoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDo>> GetToDo(int id)
        {
          if (_context.ToDo == null)
          {
              return NotFound();
          }
            var toDo = await _context.ToDo.FindAsync(id);

            if (toDo == null)
            {
                return NotFound();
            }

            return toDo;
        }

        // PUT: api/ToDoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDo(int id, ToDo toDo)
        {
            if (id != toDo.Id)
            {
                return BadRequest();
            }

            _context.Entry(toDo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ToDoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ToDo>> PostToDo(ToDo toDo)
        {
          if (_context.ToDo == null)
          {
              return Problem("Entity set 'MiDbContext.ToDo'  is null.");
          }
            //toDo.Id = new Random().Next(0, 999999999);
            _context.ToDo.Add(toDo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetToDo", new { id = toDo.Id }, toDo);
        }

        // DELETE: api/ToDoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDo(int id)
        {
            if (_context.ToDo == null)
            {
                return NotFound();
            }
            var toDo = await _context.ToDo.FindAsync(id);
            if (toDo == null)
            {
                return NotFound();
            }

            _context.ToDo.Remove(toDo);
            await _context.SaveChangesAsync();

            return Ok("Tarea eliminada correctamente");
        }

        private bool ToDoExists(int id)
        {
            return (_context.ToDo?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
