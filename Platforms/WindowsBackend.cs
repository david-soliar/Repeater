using Repeater.Models;
using Repeater.Core;
using System.Runtime.InteropServices;


namespace Repeater.Platforms
{
    public sealed partial class WindowsBackend : IInputBackend
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            public uint type;
            public InputUnion U;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct InputUnion
        {
            [FieldOffset(0)] public MOUSEINPUT mi;
            [FieldOffset(0)] public KEYBDINPUT ki;
        }

        public bool IsPressed(int keyCode)
        {
            return (GetAsyncKeyState(keyCode) & 0x8000) != 0;
        }

        public int GetPressedKey()
        {
            while (true)
            {
                for (int i = 0; i < 256; i++)
                    if (IsPressed(i)) return i;
            }
        }

        public void MouseClick(MouseButton button)
        {
            var (down, up, mouseData) = GetMouseButtonFlags(button);
            SendMouseInput(down, mouseData);
            SendMouseInput(up, mouseData);
        }

        public void KeyPress(int keyCode)
        {
            uint down = 0; uint up = 0x0002;
            SendKeyboardInput(down, keyCode);
            SendKeyboardInput(up, keyCode);
        }

        private static void SendMouseInput(uint flags, uint mouseData = 0)
        {
            var input = new INPUT
            {
                type = 0,
                U = new InputUnion
                {
                    mi = new MOUSEINPUT
                    {
                        dwFlags = flags,
                        mouseData = mouseData
                    }
                }
            };
            _ = SendInput(1, [input], Marshal.SizeOf<INPUT>());
        }

        private static void SendKeyboardInput(uint flags, int keyCode)
        {
            var input = new INPUT
            {
                type = 1,
                U = new InputUnion
                {
                    ki = new KEYBDINPUT
                    {
                        wVk = (ushort)keyCode,
                        dwFlags = flags
                    }
                }
            };
            _ = SendInput(1, [input], Marshal.SizeOf<INPUT>());
        }

        private static (uint down, uint up, uint mouseData) GetMouseButtonFlags(MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => (0x0002, 0x0004, 0),
                MouseButton.Right => (0x0008, 0x0010, 0),
                MouseButton.Middle => (0x0020, 0x0040, 0),
                MouseButton.XButton1 => (0x0080, 0x0100, 0x0001),
                MouseButton.XButton2 => (0x0080, 0x0100, 0x0002),
                _ => throw new ArgumentException($"Unknown mouse button: {button}")
            };
        }
    }
}
