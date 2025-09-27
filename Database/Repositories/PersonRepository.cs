using DataBaseAPI;
using Errors;
using Microsoft.EntityFrameworkCore;
using Test.DataModels;

namespace DataBaseContext.Repositories;

public class PersonRepository : IRepository<Person>
{
    private readonly Context _context;

    public PersonRepository(Context context)
    {
        _context = context;
    }
    
    public async Task<List<Person>> GetListAsync()
    {
        try
        {
            List<Person> persons = await _context.Persons.ToListAsync();

            if (persons == null)
                throw new DatabaseException_ListIsNull(nameof(Person));

            return persons;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<Person> GetAsync(long id)
    {
        try
        {
            List<Person> persons = await _context.Persons.ToListAsync();

            if (persons == null)
                throw new DatabaseException_ListIsNull(nameof(Person));
            
            Person? target = persons.Find(p => p.Id == id);
            
            if (target == null)
                throw new DatabaseException_EntityDoesNotExist(id.ToString());

            return target;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<Person> CreateAsync(Person person)
    {
        try
        {
            if (person == null)
                throw new DatabaseException_ArgumentIsNull(nameof(CreateAsync), nameof(person));

            List<Person> persons = await _context.Persons.ToListAsync();

            if (persons == null)
                throw new DatabaseException_ListIsNull(nameof(Person));

            var createdPerson = await _context.Persons.AddAsync(person);
            await _context.SaveChangesAsync();
            
            return createdPerson.Entity;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task AddListAsync(List<Person> items)
    {
        try
        {
            if (items == null)
                throw new DatabaseException_ArgumentIsNull(nameof(AddListAsync), nameof(items));

            List<Person> persons = await _context.Persons.ToListAsync();

            if (persons == null)
                throw new DatabaseException_ListIsNull(nameof(Person));

            foreach (Person item in items)
            {
                bool exists = persons.Any(p => p.Id == item.Id && p.Name == item.Name);
            
                if (!exists)
                    await _context.Persons.AddAsync(item);
            }
           
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<Person> UpdateAsync(Person item)
    {
        try
        {
            if (item == null)
                throw new DatabaseException_ArgumentIsNull(nameof(UpdateAsync), nameof(item));
            
            List<Person> persons = await _context.Persons.ToListAsync();

            if (persons == null)
                throw new DatabaseException_ListIsNull(nameof(Person));
            
            Person? target = persons.Find(p => p.Id == item.Id);
            
            if (target == null)
                throw new DatabaseException_EntityDoesNotExist(item.ToString());
            
            target.Name = item.Name;
            target.Address = item.Address;
            target.Work = item.Work;
           
            await _context.SaveChangesAsync();
            return target;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task DeleteAsync(long itemId)
    {
        try
        {
            List<Person> persons = await _context.Persons.ToListAsync();

            if (persons == null)
                throw new DatabaseException_ListIsNull(nameof(Person));
            
            Person? target = persons.Find(p => p.Id == itemId);
            
            if (target == null)
                throw new DatabaseException_EntityDoesNotExist(itemId.ToString());
            
            _context.Remove(target);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}