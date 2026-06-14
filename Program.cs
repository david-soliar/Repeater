using Repeater.Platforms;
using Repeater.Core;
using System.Diagnostics;


IInputBackend backend;

if (OperatingSystem.IsWindows())
    backend = new WindowsBackend();
else if (OperatingSystem.IsLinux())
    backend = new LinuxBackend();
else if (OperatingSystem.IsMacOS())
    backend = new MacOSBackend();
else
    throw new PlatformNotSupportedException();


int minRepeats = InputValidator.GetPositiveInt("Min repeats: ");
int maxRepeats = InputValidator.GetIntWithMin("Max repeats: ", minRepeats);
int idleCheck = InputValidator.GetIntInRange("Idle check (in milliseconds, 10-500): ", 10, 500);


(_, IInputAction action) = InputValidator.GetActionFromUser("Press key to repeat...", backend);
(int triggerKey, _) = InputValidator.GetActionFromUser("Press trigger key...", backend);


Console.WriteLine("Running...");


var rng = new Random();
int repeats = minRepeats;
int targetRepeats = rng.Next(minRepeats, maxRepeats + 1);
int clicksInCycle = 0;
int cycleLength = rng.Next(5, 20);
var sw = Stopwatch.StartNew();

while (true)
{
    if (backend.IsPressed(triggerKey))
    {
        long nextTick = sw.ElapsedTicks + (Stopwatch.Frequency / repeats);

        action.Execute(backend);
        clicksInCycle++;

        if (repeats < targetRepeats)
            repeats++;
        else if (repeats > targetRepeats)
            repeats--;

        if (clicksInCycle >= cycleLength)
        {
            clicksInCycle = 0;
            targetRepeats = rng.Next(minRepeats, maxRepeats + 1);
            cycleLength = rng.Next(5, 20);
        }

        while (sw.ElapsedTicks < nextTick)
            Thread.Sleep(1);
    }
    else
    {
        clicksInCycle = 0;
        if (repeats > minRepeats)
            repeats--;
        else
            targetRepeats = rng.Next(minRepeats, maxRepeats + 1);

        Thread.Sleep(idleCheck);
        sw.Restart();
    }
}
