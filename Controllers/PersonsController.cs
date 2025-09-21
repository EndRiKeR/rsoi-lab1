using DataBaseAPI;
using Microsoft.AspNetCore.Mvc;
using Test.DataModels;
using Test.Models;

namespace Test.Controllers;


[ApiController]
[Route("[controller]")]
public class PersonsController : ControllerBase
{
    private readonly IRepository<Person> _personRepo;
    
    public PersonsController(IRepository<Person> personRepo)
    {
        _personRepo = personRepo;
    }
    
    [HttpGet("fillDB")]
    public async Task<IActionResult> FillDatabase()
    {
        try
        {
            var data = GenerateTestData();
            await _personRepo.AddListAsync(data);
            return Ok(Converter.ConvertToJson(data));
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPersons()
    {
        try
        {
            var persons = await _personRepo.GetListAsync();
            return Ok(persons);
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }
    
    [HttpGet("{personId}")]
    public async Task<IActionResult> GetPerson(long personId)
    {
        try
        {
            var person = await _personRepo.GetAsync(personId);
            return Ok(person);
        }
        catch (Exception ex)
        {
            return NotFound();
        }
    }
    
    [HttpPatch("{personId}")]
    public async Task<IActionResult> UpdatePerson(long personId)
    {
        try
        {
            var oldPerson = await _personRepo.GetAsync(personId);
            
            if (Request.Headers.TryGetValue("Name", out var name) && !string.IsNullOrEmpty(name))
                oldPerson.Name = name;
            
            if (Request.Headers.TryGetValue("Surname", out var surname) && !string.IsNullOrEmpty(name))
                oldPerson.Surname = surname;
            
            await _personRepo.UpdateAsync(oldPerson);
            
            return Ok(oldPerson);
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }
    
    [HttpDelete("{personId}")]
    public async Task<IActionResult> DeletePerson(long personId)
    {
        try
        {
            await _personRepo.DeleteAsync(personId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> CreatePerson()
    {
        try
        {
            var personId = await _personRepo.CreateEmptyAsync();
            var result = Created();
            result.Location = $"/api/v1/persons/{personId}";
            return result;
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }

    private List<Person> GenerateTestData()
    {
        return new()
        {
            new Person() { Id = 1, Name = "Петя", Surname = "Соломенков"},
            new Person() { Id = 2, Name = "Коля", Surname = "Сколков"},
            new Person() { Id = 3, Name = "Маша", Surname = "Василькова"},
        };
    }
}