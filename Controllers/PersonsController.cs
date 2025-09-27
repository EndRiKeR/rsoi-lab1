using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
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
            string address = string.Empty;
            string work = string.Empty;
            string name = string.Empty;
            
            Stream req = Request.Body;
            req.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();
            Person input = null;
            input = JsonSerializer.Deserialize<Person>(json);
            
            if (string.IsNullOrEmpty(input.Name) || input.Name.Length > 20)
                throw new BackendException_IncorrectArgumet(nameof(input.Name));
            else
                name = input.Name;
            
            if (input == null)
                throw new BackendException_IncorrectArgumet(nameof(input));

            if (input.Age is >= 0 or <= 150)
                intAge = input.Age;
            
            if (!string.IsNullOrEmpty(input.Address) && input.Address.Length <= 200)
                address = input.Address;
            
            if (!string.IsNullOrEmpty(work) && input.Work.Length <= 50)
                work = input.Work;

            Person person = new Person()
            {
                Name = input.Name,
                Age = intAge,
                Address = address,
                Work = work,
            };
            
            var createdPerson = await _personRepo.CreateAsync(person);
            
            string routeTemplate = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Template;
            string routeFull = routeTemplate + "/" + createdPerson.Id;
            
            return Created(routeFull, createdPerson);
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
            
            Stream req = Request.Body;
            req.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();
            Person input = null;
            input = JsonSerializer.Deserialize<Person>(json);
            
            if (string.IsNullOrEmpty(input.Name) || input.Name.Length > 20)
                throw new BackendException_IncorrectArgumet(nameof(input.Name) + $"{string.IsNullOrEmpty(input.Name)}");

            oldPerson.Name = input.Name;
            
            // Age
            oldPerson.Age = input.Age is >= 0 or <= 150 ? input.Age : -1;
            
            // Address
            oldPerson.Address = (string.IsNullOrEmpty(input.Address) || input.Address.Length > 200) ? "" : input.Address;
            
            // Work
            oldPerson.Work = (string.IsNullOrEmpty(input.Work) || input.Work.Length > 50) ? "" : input.Work;
            
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