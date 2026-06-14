namespace Repeater.Core
{
    public interface IInputAction
    {
        void Execute(IInputBackend backend);
    }
}
