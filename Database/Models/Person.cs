using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using RsoiLab1.Models;

namespace RsoiLab1.DataModels;

[PrimaryKey("Id")]
public class Person : IDatabaseModel
{
    public long Id { get; set; }
    
    [StringLength(50)]
    public required string Name { get; set; }
    
    [Range(0, int.MaxValue)]
    public required int Age { get; set; }
    
    [StringLength(200)]
    public required string Address { get; set; }
    
    [StringLength(50)]
    public required string Work { get; set; }
    
    public override string ToString()
        => $"Person Id: {Id}, Name: {Name}, Age {Age}, Address: {Address}, Work: {Work}";

    public override bool Equals(object obj)
    {
        if (obj is not Person person)
            return false;
        
        return Id == person.Id &&
               Name == person.Name &&
               Age == person.Age &&
               Address == person.Address &&
               Work == person.Work;
    }
}
