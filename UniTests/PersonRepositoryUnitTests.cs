using DataBaseAPI;
using DataBaseContext;
using DataBaseContext.Repositories;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using Moq.EntityFrameworkCore;
using RsoiLab1.DataModels;
using RsoiLab1.UnitTests.Factories;
using Xunit;

namespace Rsoilab1.UnitTests;

public class PersonRepositoryUnitTests
{
    private readonly IRepository<Person> _personRepository;
    private readonly Mock<Context> _mockPersonContext = new();

    public PersonRepositoryUnitTests()
    {
        // создаем репозиторий с мокнутым контекстом
        // То есть контекст, который был у нас БД теперь заменен на объект, эмулирующий действия оригинала
        // Эти действия прописываются в каждом тесте
        _personRepository = new PersonRepository(_mockPersonContext.Object);
    }
    
    [Fact]
    public async Task GetAllPersonAsync_Basic_Ok()
    {
        // Arrange
        List<Person> persons = [PersonFactory.CreatePerson(id: 1), PersonFactory.CreatePerson(id: 2)];

        // Пример создания функции для мокнутого контекста
        // Точнее мы задает тут то, что вместо обращения к базе и получения persons
        // Вы возвращаем заготовленные данные "из БД"
        _mockPersonContext.Setup(c => c.Persons).ReturnsDbSet(persons);
        
        List<Person> expectedPersons = [PersonFactory.CreatePerson(id: 1), PersonFactory.CreatePerson(id: 2)];
        
        // Act
        // Мы пробуем получить заготовленные данные, проверяя работу именно репы, а не контекста
        var actualPersons = await _personRepository.GetListAsync();
        
        // Assert
        // Проверям, что все ок
        Assert.Equal(expectedPersons, actualPersons);
    }
    
    [Fact]
    public async Task DeletePersonAsync_Basic_Success()
    {
        // Arrange
        List<Person> persons = [PersonFactory.CreatePerson(id: 1), PersonFactory.CreatePerson(id: 2)];
        Person expectedPerson = PersonFactory.CreatePerson(id: 3);
    
        _mockPersonContext.Setup(c => c.Persons).ReturnsDbSet(persons);
        
        var expectedId = persons.Max(p => p.Id) + 1;
        
        // Act
        var actualPerson = await _personRepository.CreateAsync(PersonFactory.CreatePerson());
        var actualId = actualPerson.Id;
        
        // Assert
        Assert.Equal(expectedId, actualId);
    }
    
    [Fact]
    public async Task GetPersonByIdAsync_Basic_Success()
    {
        // Arrange
        List<Person> persons = [PersonFactory.CreatePerson(id: 1), PersonFactory.CreatePerson(id: 2)];
    
        _mockPersonContext.Setup(c => c.Persons).ReturnsDbSet(persons);
    
        var id = 2;
        var expectedPersonResponse = PersonFactory.CreatePerson(id: 2);
        
        // Act
        var actualPersonResponse = await _personRepository.GetAsync(id);
        
        // Assert
        Assert.Equal(expectedPersonResponse, actualPersonResponse);
    }
    
    [Fact]
    public async Task UpdatePersonByIdAsync_Basic_Success()
    {
        // Arrange
        var personDb = PersonFactory.CreatePerson(id: 1);
        
        _mockPersonContext.Setup(c => c.Persons).ReturnsDbSet([personDb]);
        
        var personRequest = PersonFactory.CreatePersonDto(name: "Roma", age: 22, address: "Khimki", work: "IT");
        var expectedPersonResponse = PersonFactory.CreatePerson(id: 1, name: "Roma", age: 22, address: "Khimki", work: "IT");

        var person = personRequest.ToPerson();
        // get Id from path
        person.Id = 1;
        
        // Act
        var actualPersonResponse = await _personRepository.UpdateAsync(person);
        
        // Assert
        Assert.NotNull(actualPersonResponse);
        Assert.Equal(expectedPersonResponse, actualPersonResponse);
    }
}