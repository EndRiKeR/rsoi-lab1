using System.Text.Json;
using RsoiLab1.DataModels;

namespace RsoiLab1.Models;

public static class Converter
{
    public static string ConvertToJson(IDatabaseModel databaseModel) => JsonSerializer.Serialize(databaseModel);
    public static string ConvertToJson(List<Person> databaseModels) => JsonSerializer.Serialize(databaseModels);
}