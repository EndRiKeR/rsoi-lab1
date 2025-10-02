
using RsoiLab1.DataModels;
using RsoiLab1.Models;

namespace RsoiLab1.UnitTests.Factories;

public static class PersonFactory
{
    public static Person CreatePerson(
        int id = 0,
        string name = "Roma",
        int age = 22,
        string address = "Khimki",
        string work = "IT")
    {
        return new Person()
        {
            Id = id,
            Name = name,
            Age = age,
            Address = address,
            Work = work
        };
    }
    
    public static PersonDto CreatePersonDto(
        string name = "Roma",
        int age = 22,
        string address = "Khimki",
        string work = "IT")
    {
        return new PersonDto()
        {
            Name = name,
            Age = age,
            Address = address,
            Work = work
        };
    }
}