using Test.DataModels;

namespace Test.Models;

public class PersonDto
{
    public required string Name { get; set; }
    public required int Age { get; set; }
    public required string Address { get; set; }
    public required string Work { get; set; }

    public Person ToPerson()
    {
        return new Person()
        {
            Name = Name,
            Age = Age,
            Address = Address,
            Work = Work
        };
    }
}