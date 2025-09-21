using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Test.Models;

namespace Test.DataModels;

[PrimaryKey("Id")]
public class Person : IDatabaseModel
{
    public long Id { get; set; }
    
    [StringLength(50)]
    public required string Name { get; set; }
    
    [StringLength(50)]
    public required string Surname { get; set; }
    
    public override string ToString() => $"Person Id: {Id}, Name: {Name}, Surname: {Surname}";
}
