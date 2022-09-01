#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RpsAPI.ApiModels;
using RpsAPI.Models;

namespace RpsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly RpsDbContext _context;

        public GameController(RpsDbContext context)
        {
            _context = context;
        }

        // GET: api/Gameg
        [HttpGet ("Get All Games")]
        public async Task<ActionResult<IEnumerable<GameItem>>> GetGameItem()
        {      
            return await _context.GameItem.ToListAsync();
            
        }

        // GET: api/Game/5
        [HttpGet("Get GameID")]
        public async Task<ActionResult<GameItem>> GetGameItem(long id)
        {
            var gameItem = await _context.GameItem.FindAsync(id);

            if (gameItem == null)
            {
                return NotFound();
            }

            return gameItem;
        }

        [HttpPost("Create a game")]
        public IActionResult createGame([FromBody] CreateGame request)
        {
            GameItem game = new GameItem();
            var gamessid = _context.GameItem.ToList();
            var Latestgame = gamessid.Last();
            var newId = Latestgame.GameID + 1;
          
            game.GameID = newId;
            game.InviteFromID = request.InviteFromID;
            game.InviteToID = request.InviteToID;
            game.InviteFromIDMove = request.InviteFromIDMove;
            game.InviteToIDMove = request.InviteToIDMove;
            game.Gamestatus = request.Gamestatus;
            _context.GameItem.Add(game);
            _context.SaveChanges();
            var games = _context.UserItem.ToList();
            return Ok(games);
        }

 

        // PUT: api/Game/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPatch("{id}")]
        public async Task<ActionResult<GameStatus_>> GetUserItem(long id, string gamestatus)
        {
            var gameItem = await _context.GameItem.FindAsync(id);
            if (id != gameItem.GameID)
            {
                return BadRequest();
            }

           gameItem.Gamestatus = gamestatus;
           _context.Entry(gameItem).State = EntityState.Modified;       
               await _context.SaveChangesAsync();    
                if (!GameItemExists(id))
                {
                    return NotFound();
                }            
            return NoContent();
        }

     

        // DELETE: api/Game/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGameItem(long id)
        {
            var gameItem = await _context.GameItem.FindAsync(id);
            if (gameItem == null)
            {
                return NotFound();
            }

            _context.GameItem.Remove(gameItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GameItemExists(long id)
        {
            return _context.GameItem.Any(e => e.GameID == id);
        }
    }
}
