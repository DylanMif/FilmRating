using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmRating.Models.EntityFramework;
using TP3Console.Models.EntityFramework;
using FilmRating.Models.DataManager;

namespace FilmRating.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UtilisateursController : ControllerBase
    {
        private readonly UtilisateurManager utilisateurManager;
        //private readonly FilmDBContext _context;

        public UtilisateursController(UtilisateurManager userManager)
        {
            //_context = context;
            utilisateurManager = userManager;
        }

        // GET: api/Utilisateurs
        [HttpGet]
        [ActionName("GetUtilisateurs")]
        public async Task<ActionResult<IEnumerable<Utilisateur>>> GetUtilisateurs()
        {
            return utilisateurManager.GetAll();
        }

        // GET: api/Utilisateurs/5
        [HttpGet("{id}")]
        [ActionName("GetUtilisateurById")]
        public async Task<ActionResult<Utilisateur>> GetUtilisateurById(int id)
        {

            var utilisateur = utilisateurManager.GetById(id);
            //var utilisateur = await _context.Utilisateurs.FindAsync(id);
            if (utilisateur == null)
            {
                return NotFound();
            }
            return utilisateur;
        }

        // GET : api/Utilisateurs/dydy@gmail.com
        [HttpGet("{email}")]
        [ActionName("GetUtilisateurByEmail")]
        public async Task<ActionResult<Utilisateur>> GetUtilisateurByEmail(string email)
        {
            var utilisateur = utilisateurManager.GetByString(email);
            //var utilisateur = await _context.Utilisateurs.FindAsync(id);
            if (utilisateur == null)
            {
                return NotFound();
            }
            return utilisateur;
        }

        // PUT: api/Utilisateurs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ActionName("PutUtilisateur")]
        public async Task<IActionResult> PutUtilisateur(int id, Utilisateur utilisateur)
        {
            if (id != utilisateur.UtilisateurId)
            {
                return BadRequest();
            }
            var userToUpdate = utilisateurManager.GetById(id);
            if (userToUpdate == null)
            {
                return NotFound();
            }
            else
            {
                utilisateurManager.Update(userToUpdate.Value, utilisateur);
                return NoContent();
            }
        }

        // POST: api/Utilisateurs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ActionName("PostUtilisateur")]
        public async Task<ActionResult<Utilisateur>> PostUtilisateur(Utilisateur utilisateur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            utilisateurManager.Add(utilisateur);
            return CreatedAtAction("GetUtilisateurById", new { id = utilisateur.UtilisateurId }, utilisateur); // GetById : nom de l’action
        }

        // DELETE: api/Utilisateurs/5
        [HttpDelete("{id}")]
        [ActionName("DeleteUtilisateur")]
        public async Task<IActionResult> DeleteUtilisateur(int id)
        {
            var utilisateur = utilisateurManager.GetById(id);
            if (utilisateur == null)
            {
                return NotFound();
            }
            utilisateurManager.Delete(utilisateur.Value);
            return NoContent();
        }
    }
}
