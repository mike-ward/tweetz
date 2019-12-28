namespace tweetz.core.Infrastructure
{
    public interface ISystemState
    {
        bool IsSleeping { get; set; }
        bool IsRegisteredInStartup { get; set; }
    }
}