namespace Errors
{
    public class BaseException(string mes) : Exception(mes);

    public class BackendException(string mes) : BaseException($"Program Exception: {mes}");

    public class BackendException_ArgumentIsNull(string mes) : BackendException($"Argument {mes} is null");
    public class BackendException_IncorrectArgumet(string mes) : BackendException($"Argument {mes} is incorrect");
    public class BackendException_RequiredArgumet(string mes) : BackendException($"Argument {mes} is required");

    public class BackendException_UnexpectedError(string mes) : BackendException($"А вот хрен его знает, что произошло: {mes}");
    
    public class DatabaseException(string mes) : BaseException($"DataBase Exception: {mes}");

    public class DatabaseException_EntityAlreadyExists(string entity) : DatabaseException($"Entity {entity} already exists!");
    
    // 404
    public class DatabaseException_EntityDoesNotExist(string entity) : DatabaseException($"Entity {entity} doesn't exist!");
    public class DatabaseException_ListIsNull(string entityName) : DatabaseException($"List {entityName} is null!");
    public class DatabaseException_ArgumentIsNull(string methodName, string argName) : DatabaseException($"Argument {argName} is null in Method {methodName}!");
    
    // 400
    public class DatabaseException_InvalidArgument(string methodName, string argName) : DatabaseException($"Argument {argName} is invalid in Request!");
}