using Repeater.Models;


namespace Repeater.Core
{
    public class InputValidator
    {
        public static int GetPositiveInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int result) && result > 0)
                    return result;

                Console.WriteLine("Error: Enter a positive number.");
            }
        }

        public static int GetIntInRange(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int result) && result >= min && result <= max)
                    return result;

                Console.WriteLine($"Error: Enter a number between {min} and {max}.");
            }
        }

        public static int GetIntWithMin(string prompt, int min)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int result) && result > min)
                    return result;

                Console.WriteLine($"Error: Enter a number larger than {min}.");
            }
        }

        public static (int keyCode, IInputAction action) GetActionFromUser(string prompt, IInputBackend backend)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                Thread.Sleep(300);
                int key = backend.GetPressedKey();
                while (Console.KeyAvailable)
                    Console.ReadKey(true);

                IInputAction action = (key >= 0x01 && key <= 0x06)
                    ? new MouseAction(key switch
                    {
                        0x01 => MouseButton.Left,
                        0x02 => MouseButton.Right,
                        0x04 => MouseButton.Middle,
                        0x05 => MouseButton.XButton1,
                        0x06 => MouseButton.XButton2,
                        _ => throw new NotImplementedException(),
                    })
                    : new KeyboardAction(key);

                Console.WriteLine($"Using key: 0x{key:X2} ({(key <= 0x06 ? "Mouse" : "Keyboard")})");
                Console.Write("Proceed? (y/n): ");

                if (Console.ReadLine()?.Trim().ToLower() == "y")
                {
                    return (key, action);
                }
                Thread.Sleep(20);
            }
        }
    }
}
