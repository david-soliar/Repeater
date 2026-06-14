using Repeater.Models;


namespace Repeater.Core
{
    public interface IInputBackend
    {
        bool IsPressed(int keyCode);
        void MouseClick(MouseButton button);
        void KeyPress(int keyCode);
        int GetPressedKey();
    }
}
