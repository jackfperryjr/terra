using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using Terra.Data;
using Terra.Models;

namespace Terra.Controllers
{
    [Route("/api/v1/games")]
    public class ApiGameController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ApiGameController(ApplicationDbContext context)
        {
            _context = context;
        }

        //GET all api/games
        [AllowAnonymous]
        [HttpGet]
        public List<Game> GetAll()
        {
            var games = from g in _context.Games select g;
            games = games.OrderBy(g => g.Title);
            return games.ToList();
        }

        //GET api/games/id
        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetGame")]
        public IActionResult Get(Guid? id)
        {
            var game = _context.Games.Find(id);
            if (game == null)
            {
                return NotFound();
            }
            return Ok(game);
        }   
    }
}