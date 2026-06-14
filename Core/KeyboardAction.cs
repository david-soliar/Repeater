namespace Repeater.Core
{
    public sealed class KeyboardAction(int keyCode) : IInputAction
    {
        public int KeyCode { get; } = keyCode;

        public void Execute(IInputBackend backend)
        {
            backend.KeyPress(KeyCode);
        }
    }
}
