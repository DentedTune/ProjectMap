using Microsoft.AspNetCore.Mvc;
using ProjectMap.WebApi.Models;
using ProjectMap.WebApi.Repositories;
using ProjectMap.WebApi.Services;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectMap.WebApi.Controllers;

[ApiController]
[Route("Objects2D")]
public class Object2DController : ControllerBase
{
    private readonly IObject2DRepository _object2DRepository;
    private readonly ILogger<Object2DController> _logger;
    private readonly IAuthenticationService _authenticationService;

    public Object2DController(IObject2DRepository object2DRepository, ILogger<Object2DController> logger)
    {
        _object2DRepository = object2DRepository;
        _logger = logger;
    }

    [HttpGet(Name = "ReadObjects2D")]
    public async Task<ActionResult<IEnumerable<Object2D>>> Get()
    {
        var objects2D = await _object2DRepository.ReadAsync();
        foreach(Object2D object2D in objects2D)
        {
            object2D.ObjectType = object2D.ObjectType.Trim();
        }
        return Ok(objects2D);
    }

    [HttpGet("{object2DId}", Name = "ReadObject2D")]
    public async Task<ActionResult<Object2D>> Get(Guid Object2DId)
    {
        var object2D = await _object2DRepository.ReadAsync(Object2DId);
        if (object2D == null)
            return NotFound();

        object2D.ObjectType = object2D.ObjectType.Trim();

        return Ok(object2D);
    }

    [HttpPost(Name = "CreateObject2D")]
    public async Task<ActionResult> Add(Object2D object2D)
    {
        object2D.Id = Guid.NewGuid();

        var createdObject2D = await _object2DRepository.InsertAsync(object2D);
        return Created();
    }
    [HttpPut("{object2DId}", Name = "UpdateObject2D")]
    public async Task<ActionResult> Update(Guid object2DId, Object2D newObject2D)
    {
        var existingObject2D = await _object2DRepository.ReadAsync(object2DId);

        if (existingObject2D == null)
            return NotFound();

        await _object2DRepository.UpdateAsync(newObject2D);

        return Ok(newObject2D);
    }

    [HttpDelete("{object2DId}", Name = "DeleteObject2DByGuid")]
    public async Task<IActionResult> Update(Guid object2DId)
    {
        var existingObject2D = await _object2DRepository.ReadAsync(object2DId);

        if (existingObject2D == null)
            return NotFound();

        await _object2DRepository.DeleteAsync(object2DId);

        return Ok();
    }

}