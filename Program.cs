// Retro console demo

try
{
    var screen = new RetroConsole.CubeScreen();
    var clock = Stopwatch.StartNew();

    while (true)
    {
        var w = AnsiConsole.Profile.Width / 2;
        var h = AnsiConsole.Profile.Height;
        var canvas = new Canvas(w, h);
        AnsiConsole.Live(canvas).Start(ldc => Updater(clock, canvas, screen, ldc));
    }
}
finally
{
}

static void Updater(Stopwatch clock, Canvas canvas, Screen screen, LiveDisplayContext ldc)
{
    var running = true;
    while (running)
    {
        var before = clock.ElapsedMilliseconds / 1000.0;
        var time = before;
        screen.Update(canvas, time);
        ldc.Refresh();
        if (Console.KeyAvailable)
        {
            var key = Console.ReadKey();
            running = key.Key switch
            {
                ConsoleKey.Spacebar => false,
                ConsoleKey.Escape => false,
                _ => true
            };
        }

        var after = clock.ElapsedMilliseconds / 1000.0;
        var waitFor = 1.0 / 120.0 - (after - before);
        if (waitFor > 0.0)
        {
            var ms = (int)(waitFor * 1000.0);
            Thread.Sleep(ms);
        }
    }
}

abstract class Screen
{
    public abstract string Name { get; }
    public abstract void Update(Canvas canvas, double time);
    public override string ToString() => Name;
}
