using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Test.Models;

namespace Test.DataModels;

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
}
