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
    public class UseriController : ControllerBase
    {
        private readonly RpsDbContext _context;

        public UseriController(RpsDbContext context)
        {
            _context = context;
        }

        [HttpPost("CreateUser")]

        public IActionResult create([FromBody] CreateNewUser request)
        {
            UserItem user = new UserItem();

            var users = _context.UserItem.ToList();
            var latestUser = users.Last();
            var newId = latestUser.UserID + 1;
            user.UserID = newId;
            user.Username = request.Username;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            var hashpass = AuthUtil.ToSha256Hash(request.Password).Replace("-", "");
            user.PasswordHash = hashpass;

            _context.UserItem.Add(user);
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet("GetUsers")]

        public IActionResult get()
        {

            var user = _context.UserItem.ToList();
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }


        [HttpPost("Log in")]
        public async Task<ActionResult<Token>> LogIn(string username, string password)
        {
            var hashpass = AuthUtil.ToSha256Hash(password).Replace("-", "");

            var token = new Token();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Empty values can't be entered");
            }
            var user = await _context.UserItem.FirstOrDefaultAsync(row => row.Username.Equals(username));

            if (user == null)
            {
                return NotFound(" user was not found ");
            }

            if (user.Username == username && user.PasswordHash == hashpass)
            {
                user.TokenIssued = DateTime.Now;
                user.Token = AuthUtil.SecureRandomString(30);
                token.Value = user.Token;
                _context.SaveChanges();
                return Ok(token);

            }
            return Ok("Password was incorrect");
        }





        // GET: api/Useri/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OpponentRespond>> GetUserItem(long id, string token)
        {
            //  Koll om TOKEN FINNS I DATABASEN
            var antal = _context.UserItem.Count(item => item.Token.Equals(token));
            if (antal == 0)
            {
                return Unauthorized();
            }

            var userItem = await _context.UserItem.FindAsync(id);

            if (userItem == null)
            {
                return NotFound();
            }

            var opponent = new OpponentRespond();
            opponent.Username = userItem.Username;
            opponent.FirstName = userItem.FirstName;
            opponent.LastName = userItem.LastName;
            opponent.Email = userItem.Email;

            return Ok(opponent);
        }

        [HttpPost("Reset Pass")]
        public async Task<ActionResult<ResetPass>> Resetpass(string email)
        {
            var antal = _context.UserItem.Count(item => item.Email.Equals(email));
            if (antal == 0)
            { return NotFound("this email is not registered");
            }
            var user = await _context.UserItem.FirstOrDefaultAsync(row => row.Email.Equals(email));
            var resettoken = AuthUtil.SecureRandomString(5);
            user.ResetToken = resettoken;
            _context.SaveChanges();
            Mail.Email(resettoken, email);
            return Ok();


        }


        [HttpPatch]
        public async Task<ActionResult<Token>> ChangePass(string token , string Newpassword)
        {
            var antal = _context.UserItem.Count(item => item.ResetToken.Equals(token));
            if (antal == 0)
            {
                return NotFound("this is not a valid code");
            }

            var user = await _context.UserItem.FirstOrDefaultAsync(row => row.ResetToken.Equals(token));
            var hashpass = AuthUtil.ToSha256Hash(Newpassword).Replace("-", "");
            user.PasswordHash = hashpass;
            _context.SaveChanges();

            return Ok();
        }


        // PUT: api/Useri/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754


        // DELETE: api/Useri/5


    }
}
