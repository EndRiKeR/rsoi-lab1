using DataBaseAPI;
using Errors;
using Microsoft.AspNetCore.Mvc;
using Test.DataModels;
using Test.Models;

namespace Test.Controllers;


[ApiController]
[Route("api/v1/[controller]")]
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
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllPersons()
    {
        try
        {
            var persons = await _personRepo.GetListAsync();
            return Ok(persons);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateNewPerson()
    {
        try
        {
            int intAge = -1;
            
            if (!Request.Headers.TryGetValue("name", out var name))
                throw new BackendException_RequiredArgumet(nameof(name));
            
            if (string.IsNullOrEmpty(name) || name.Count > 20)
                throw new BackendException_IncorrectArgumet(nameof(name));

            if (Request.Headers.TryGetValue("age", out var age) && !string.IsNullOrEmpty(age))
            {
                intAge = Convert.ToInt32(age);
                
                if (intAge is < 0 or > 150)
                    intAge = -1;
            }
            
            if (!Request.Headers.TryGetValue("address", out var address) &&
                (string.IsNullOrEmpty(address) || address.Count > 200))
                address = string.Empty;
            
            if (!Request.Headers.TryGetValue("work", out var work) &&
                (string.IsNullOrEmpty(work) || work.Count > 50))
                work = string.Empty;

            Person person = new Person()
            {
                Name = name,
                Age = intAge,
                Address = address,
                Work = work,
            };
            
            var personId = await _personRepo.CreateAsync(person);
            
            string routeTemplate = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Template;
            string routeFull = routeTemplate + "/" + personId;
            
            var result = Created();
            result.Location = routeFull;
            return result;
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("{personId}")]
    public async Task<IActionResult> GetPersonById(long personId)
    {
        try
        {
            var person = await _personRepo.GetAsync(personId);
            return Ok(person);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    

    
    [HttpDelete("{personId}")]
    public async Task<IActionResult> RemovePersonById(long personId)
    {
        try
        {
            await _personRepo.DeleteAsync(personId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPatch("{personId}")]
    public async Task<IActionResult> UpdatePersonById(long personId)
    {
        try
        {
            var oldPerson = await _personRepo.GetAsync(personId);
            
            // Name
            if (!Request.Headers.TryGetValue("name", out var name))
                throw new BackendException_RequiredArgumet(nameof(name));

            if (string.IsNullOrEmpty(name) || name.Count > 20)
                throw new BackendException_IncorrectArgumet(nameof(name) + $"{string.IsNullOrEmpty(name)}");

            oldPerson.Name = name;
            
            // Age
            if (Request.Headers.TryGetValue("age", out var age) && !string.IsNullOrEmpty(age))
            {
                int intAge = Convert.ToInt32(age);
            
                if (intAge is >= 0 or <= 150)
                    oldPerson.Age = intAge;
            }
            
            // Address

            if (Request.Headers.TryGetValue("address", out var address) &&
                !(string.IsNullOrEmpty(address) || address.Count > 200))
            {
                oldPerson.Address = address;
            }
            
            // Work

            if (Request.Headers.TryGetValue("work", out var work) &&
                !(string.IsNullOrEmpty(work) || work.Count > 50))
            {
                oldPerson.Work = work;
            }
            
            await _personRepo.UpdateAsync(oldPerson);
            return Ok(oldPerson);
        }
        catch (Exception ex)
        {
            if (ex is BackendException_IncorrectArgumet or BackendException_RequiredArgumet)
                return BadRequest(ex.Message);

            if (ex is DatabaseException_EntityDoesNotExist)
                return NotFound(ex.Message);
                
            return BadRequest(ex.Message);
        }
    }

    private List<Person> GenerateTestData()
    {
        return new()
        {
            new Person() { Name = "Петя", Age = 14, Work = "Dev-OPS", Address = "Russia"},
            new Person() { Name = "Коля", Age = 25, Work = "Chef", Address = "Russia"},
            new Person() { Name = "Маша", Age = 40, Work = "Barista", Address = "Russia"},
            new Person() { Name = "Варя", Age = 38, Work = "Barber", Address = "Russia"},
            new Person() { Name = "Оля", Age = 12, Work = "Model", Address = "Russia"},
            new Person() { Name = "Саша", Age = 19, Work = "Student", Address = "Russia"},
            new Person() { Name = "Сережа", Age = 77, Work = "Doctor", Address = "Russia"},
        };
    }
}