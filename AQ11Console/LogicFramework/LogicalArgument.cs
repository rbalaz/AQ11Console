namespace AQ11Console
{
    public interface LogicalArgument
    {
        // Interface implemented by all logical formula types in the framework
        string toString();
        bool isEqual(LogicalArgument argument);
    }
}
