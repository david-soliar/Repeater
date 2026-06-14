using Repeater.Models;


namespace Repeater.Core
{
    public sealed class MouseAction(MouseButton button) : IInputAction
    {
        public MouseButton Button { get; } = button;

        public void Execute(IInputBackend backend)
        {
            backend.MouseClick(Button);
        }
    }
}
