using Test.DataModels;

namespace Test.Models;

public class PersonDto
{
    public string Name { get; set; } = "";
    public int Age { get; set; } = -1;
    public string Address { get; set; } = "";
    public string Work { get; set; } = "";

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