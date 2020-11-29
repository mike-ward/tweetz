namespace tweetz.core.Interfaces
{
    public interface ISystemState
    {
        bool IsSleeping { get; set; }
        bool IsRegisteredInStartup { get; set; }
    }
}