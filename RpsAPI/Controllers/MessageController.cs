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
    public class MessageController : ControllerBase
    {
        private readonly RpsDbContext _context;

        public MessageController(RpsDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<List<MessageItem>>> GetMessages(string token)
        {

            var antal = _context.UserItem.Count(item => item.Token.Equals(token));
            if (antal == 0)
            {
                return Unauthorized();
            }
            var userItem = await _context.UserItem.FirstOrDefaultAsync(row => row.Token.Equals(token));

            if (userItem == null)
            {
                return NotFound();
            }

            var id = userItem.UserID;
            var messageItem = await _context.MessageItem.Where(row => row.ToUserID == id).ToListAsync();


            return Ok(messageItem);
        }


        // GET: api/Message/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MessageItem>> GetMessageItem(long id)
        {
            var messageItem = await _context.MessageItem.FindAsync(id);

            if (messageItem == null)
            {
                return NotFound();
            }

            return messageItem;
        }
        [HttpPost("Create And save Message")]

        public IActionResult create([FromBody] CreateNewMsg request)
        {
            MessageItem message = new MessageItem();
           

            var Msg = _context.MessageItem.ToList();
            var latestsmsgID = Msg.Last();
            var newId = latestsmsgID.MessageID + 1;


            message.MessageID= newId;
            message.FromUserID = request.FromUserID;
            message.ToUserID = request.ToUserID;
            message.MessageValue = request.MessageValue;
           

            _context.MessageItem.Add(message);
            _context.SaveChanges();
            var messages = _context.MessageItem.ToList();
            return Ok(messages);


        }

        // PUT: api/Message/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessageItem(long id, MessageItem messageItem)
        {
            if (id != messageItem.MessageID)
            {
                return BadRequest();
            }

            _context.Entry(messageItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageItemExists(id))
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

        // POST: api/Message
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MessageItem>> PostMessageItem(MessageItem messageItem)
        {
            _context.MessageItem.Add(messageItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessageItem", new { id = messageItem.MessageID }, messageItem);
        }

        // DELETE: api/Message/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessageItem(long id)
        {
            var messageItem = await _context.MessageItem.FindAsync(id);
            if (messageItem == null)
            {
                return NotFound();
            }

            _context.MessageItem.Remove(messageItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MessageItemExists(long id)
        {
            return _context.MessageItem.Any(e => e.MessageID == id);
        }
    }
}
