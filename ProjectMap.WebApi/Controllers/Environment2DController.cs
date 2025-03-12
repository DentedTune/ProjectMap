using Microsoft.AspNetCore.Mvc;
using ProjectMap.WebApi.Models;
using ProjectMap.WebApi.Repositories;
using ProjectMap.WebApi.Services;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace ProjectMap.WebApi.Controllers;

[ApiController]
[Route("Environments2D")]
public class Environment2DController : ControllerBase
{
    private readonly IEnvironment2DRepository _environment2DRepository;
    private readonly ILogger<Environment2DController> _logger;
    private readonly IAuthenticationService _authenticationService;

    public Environment2DController(IEnvironment2DRepository environment2DRepository, ILogger<Environment2DController> logger, IAuthenticationService authenticationService)
    {
        _environment2DRepository = environment2DRepository;
        _logger = logger;
        _authenticationService = authenticationService;
    }

    [Authorize]
    [HttpGet(Name = "ReadEnvironments2D")]
    public async Task<ActionResult<IEnumerable<Environment2D>>> Get()
    {

        var environments2D = await _environment2DRepository.ReadAsync(_authenticationService.GetCurrentAuthenticatedUserId());
        foreach (Environment2D environment2D in environments2D)
        {
            environment2D.Name = environment2D.Name.Trim();
            environment2D.UserId = environment2D.UserId.Trim();
        }
        return Ok(environments2D);
    }

    [HttpGet("AllEnvironments", Name = "ReadAllEnvironments2D")]
    public async Task<ActionResult<IEnumerable<Environment2D>>> GetAllOfEm()
    {
        var environments2D = await _environment2DRepository.ReadAsync();
        foreach (Environment2D environment2D in environments2D)
        {
            environment2D.Name = environment2D.Name.Trim();
        }
        return Ok(environments2D);
    }

    [HttpGet("{environment2DId}", Name = "ReadEnvironment2D")]
    public async Task<ActionResult<Environment2D>> Get(Guid environment2DId)
    {
        var environment2D = await _environment2DRepository.ReadAsync(environment2DId);
        if (environment2D == null)
            return NotFound();

        environment2D.Name = environment2D.Name.Trim();
        return Ok(environment2D);
    }

    [HttpPost(Name = "CreateEnvironment2D")]
    public async Task<ActionResult> Add(Environment2D environment2D)
    {
        environment2D.Id = Guid.NewGuid();
        environment2D.UserId = _authenticationService.GetCurrentAuthenticatedUserId();

        var createdEnvironment2D = await _environment2DRepository.InsertAsync(environment2D);
        return Created();
    }
    [HttpPut("{environment2DId}", Name = "UpdateEnvironment2D")]
    public async Task<ActionResult> Update(Guid environment2DId, Environment2D newEnvironment2D)
    {
        var existingEnvironment2D = await _environment2DRepository.ReadAsync(environment2DId);

        if (existingEnvironment2D == null)
            return NotFound();

        await _environment2DRepository.UpdateAsync(newEnvironment2D);

        return Ok(newEnvironment2D);
    }

    [HttpDelete("{environment2DId}", Name = "DeleteEnvironment2DByGuid")]
    public async Task<IActionResult> Update(Guid environment2DId)
    {
        var existingEnvironment2D = await _environment2DRepository.ReadAsync(environment2DId);

        if (existingEnvironment2D == null)
            return NotFound();

        await _environment2DRepository.DeleteAsync(environment2DId);

        return Ok();
    }

}