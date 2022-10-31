using Microsoft.AspNetCore.Mvc;
using DemoAPI.Data;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace DemoAPI;

[ApiController]
[Route("[controller]")]
public class ClientController : Controller
{
    private readonly ClientDb _db;

    public ClientController(ClientDb db)
    {
        _db = db;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get all clients", Description = "Get all clients")]
    [SwaggerResponse(200, "Returns all clients", typeof(IEnumerable<ClientInfo>))]
    public async Task<IActionResult> Get()
    {
        return Ok(await _db.ClientInfos.ToListAsync());
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get client by id", Description = "Get client by id")]
    [SwaggerResponse(200, "Client found", typeof(ClientInfo))]
    public async Task<IActionResult> Get(int id)
    {
        return Ok(await _db.ClientInfos.FindAsync(id));
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Add a new client", Description = "Add a new client")]
    [SwaggerResponse(201, "Client added")]
    public async Task<IActionResult> Post(ClientInfo clientInfo)
    {
        _db.ClientInfos.Add(clientInfo);
        await _db.SaveChangesAsync();
        return Ok(clientInfo);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update a client", Description = "Update a client")]
    [SwaggerResponse(200, "Client updated")]
    [SwaggerResponse(404, "Client not found")]
    public async Task<IActionResult> Put(int id, ClientInfo clientInfo)
    {
        var currentInfo = await _db.ClientInfos.FindAsync(id);
        if (currentInfo is null) return NotFound();
        currentInfo.FirstName = clientInfo.FirstName;
        currentInfo.LastName = clientInfo.LastName;
        currentInfo.Email = clientInfo.Email;
        currentInfo.LastUpdated = DateTime.Now;
        await _db.SaveChangesAsync();
        return Ok(currentInfo);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete a client", Description = "Delete a client")]
    [SwaggerResponse(200, "Client deleted")]
    [SwaggerResponse(404, "Client not found")]
    public async Task<IActionResult> Delete(int id)
    {
        if (await _db.ClientInfos.FindAsync(id) is ClientInfo clientInfo)
        {
            _db.ClientInfos.Remove(clientInfo);
            await _db.SaveChangesAsync();
            return Ok(clientInfo);
        }
        return NotFound();
    }
}