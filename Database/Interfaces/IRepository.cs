using Test.Models;

namespace DataBaseAPI;

public interface IRepository<T> where T : IDatabaseModel
{
    /// <summary>
    /// Получение всех объектов данного репазитория
    /// </summary>
    /// <exception> DataBaseMindException_EntityIsNull </exception>
    /// <returns>Task<IEnumerable<Community>></returns>
    public Task<List<T>> GetListAsync();
    
    /// <summary>
    /// Получение одного объекта по ID
    /// </summary>
    /// <param name="id"></param>
    /// <exception> DataBaseMindException_EntityIsNull </exception>
    /// <returns>Task<Community?></returns>
    public Task<T> GetAsync(long id);
    
    /// <summary>
    /// создание объекта
    /// </summary>
    /// <param name="item"></param>
    /// <exception> DataBaseMindException_EntityIsNull </exception>
    /// <exception> DataBaseMindException_EntityAlreadyExists </exception>
    /// <returns>Task</returns>
    public Task<long> CreateAsync(string name);
    
    /// <summary>
    /// создание листа объектов
    /// </summary>
    /// <param name="item"></param>
    /// <exception> DataBaseMindException_EntityIsNull </exception>
    /// <exception> DataBaseMindException_EntityDoesNotExist </exception>
    /// <returns>Task</returns>
    public Task AddListAsync(List<T> item);
    
    /// <summary>
    /// обновление объекта
    /// </summary>
    /// <param name="item"></param>
    /// <exception> DataBaseMindException_EntityIsNull </exception>
    /// <exception> DataBaseMindException_EntityDoesNotExist </exception>
    /// <returns>Task</returns>
    public Task UpdateAsync(T item);
    
    /// <summary>
    /// удаление объекта по ID
    /// </summary>
    /// <exception> DataBaseMindException_EntityIsNull </exception>
    /// <exception> DataBaseMindException_EntityDoesNotExist </exception>
    /// <returns>Task</returns>
    public Task DeleteAsync(long itemId);
}