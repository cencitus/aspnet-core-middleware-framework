using Microsoft.AspNetCore.Mvc;
using frameworks_pr1.Models;
using frameworks_pr1.Storage;

namespace frameworks_pr1.Controllers;

[ApiController]
[Route("api/items")]
public class ItemsController : ControllerBase
{
    private ApiErrorResponse MakeError(int code, string message)
    {
        var requestId = HttpContext.Items["X-Request-Id"]?.ToString();

        return new ApiErrorResponse
        {
            RequestId = requestId,
            Error = new ApiError
            {
                Code = code,
                Message = message
            }
        };
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(ItemStorage.GetAll());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var item = ItemStorage.GetById(id);

        if (item == null)
            return NotFound(MakeError(404, "Item not found"));

        return Ok(item);
    }

    [HttpPost]
    public IActionResult Create(Item item)
    {
        if (string.IsNullOrWhiteSpace(item.Name))
            return BadRequest(MakeError(400, "Name is required"));
        if (item.Price < 0)
            return BadRequest(MakeError(400, "Price must be non-negative"));
        var created = ItemStorage.Add(item);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

[HttpGet("crash")]
public IActionResult Crash() => throw new Exception("boom");
}