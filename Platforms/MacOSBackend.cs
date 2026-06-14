using Repeater.Models;
using Repeater.Core;


namespace Repeater.Platforms
{
    public sealed class MacOSBackend : IInputBackend
    {
        public bool IsPressed(int keyCode)
            => throw new NotImplementedException();

        public void MouseClick(MouseButton button)
            => throw new NotImplementedException();

        public void KeyPress(int keyCode)
            => throw new NotImplementedException();

        public int GetPressedKey()
            => throw new NotImplementedException();
    }
}
