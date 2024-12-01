using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tutorial.Attributes;
using Tutorial.Exceptions;
using Tutorial.Models;
using Tutorial.Models.Database;
using Tutorial.Models.Requests;
using Tutorial.Models.Response;
using Tutorial.Services.AccountService;
using Tutorial.Services.ItemService;

namespace Tutorial.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        public ItemController(IItemService ItemService)
        {
            _itemService = ItemService;
        }

        [HttpGet]
        [Route("get-all-items")]
        public async Task<ActionResult<List<Item>>> GetAllItems()
        {
            try
            {
                return Ok(await _itemService.GetAllItems());
            }
            catch(Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet]
        [Route("get-users-items/{userId}")]
        public async Task<ActionResult<List<Item>>> GetAllUsersItems([FromRoute] int userId)
        {
            try
            {
                return Ok(await _itemService.GetAllUsersItems(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet]
        [Route("get-item/{itemId}")]
        public async Task<ActionResult<Item>> GetItem([FromRoute] int itemId)
        {
            try
            {
                return Ok(await _itemService.GetItem(itemId));
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [Auth]
        [HttpPost]
        [Route("publish-item")]
        public async Task<ActionResult<Item>> AddItem([FromBody] CreateItemRequest item)
        {
            try
            {
                return Ok(await _itemService.AddItem((HttpContext.Items["User"] as User)!, item));
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [Auth]
        [HttpPut]
        [Route("change-item")]
        public async Task<ActionResult<Item>> ChangeItem([FromBody] ChangeItemRequest item)
        {
            try
            {
                return Ok(await _itemService.ChangeItem((HttpContext.Items["User"] as User)!, item));
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [Auth]
        [HttpDelete]
        [Route("delete-item/{itemId}")]
        public async Task<IActionResult> RemoveItem([FromRoute] int itemId)
        {
            try
            {
                await _itemService.DeleteItem((HttpContext.Items["User"] as User)!, itemId);
                return Ok();
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [Auth(new string[] { nameof(UserType.Admin) })]
        [HttpDelete]
        [Route("delete-all-items")]
        public async Task<IActionResult> RemoveAllItems()
        {
            try
            {
                await _itemService.DeleteAllItems();
                return Ok();
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
