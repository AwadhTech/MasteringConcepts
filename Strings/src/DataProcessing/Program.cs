global using System.Globalization;
global using DataProcessing;
global using DataProcessing.Input;
global using DataProcessing.Reporting;
global using Microsoft.Extensions.Logging;

// Marks when the app is past the user data collection phase.
var initialized = false; 

using var cts = new CancellationTokenSource();

// Register shutdown event handler.
Console.CancelKeyPress += (s, e) =>
{
    Console.WriteLine();
    var beforeColour = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Canceling...");
    Console.ForegroundColor = beforeColour;
    Console.WriteLine();
    cts.Cancel();

    if (initialized)
        e.Cancel = false;
};

try
{
    // Configure a logger factory that can provide loggers where required for logging progress.
    using var loggerFactory = LoggerFactory.Create(builder => builder
        .AddFilter("Microsoft", LogLevel.Warning)
        .AddFilter("System", LogLevel.Warning)
        .AddFilter("DataProcessing", LogLevel.Information)
        .AddConsole());

    // Set the output to Unicode to ensure we can display emojis consistently.
    Console.OutputEncoding = System.Text.Encoding.Unicode;

    // setting the application culture
    CultureInfo appCultureInfo= CultureInfo.InvariantCulture;
    CultureInfo.DefaultThreadCurrentCulture = appCultureInfo;
    var (foreName, surName, departmentId) = AcceptUserDetails();
    while (!ValidateUserDetails(foreName, surName, departmentId))
    {
        (foreName, surName, departmentId) = AcceptUserDetails();
    }

    var context = new SessionContext(foreName!, surName!, departmentId!);
     // Configure the options for the processor.
    // Adds two output writers used when reports are generated.
    var options = new ProcessingOptions(appCultureInfo, context, loggerFactory)
        .AddOutputWriter(new ThirdPartyOutputWriter())
        .AddOutputWriter(new ConsoleOutputWriter());

    // Mark app as initialized
    initialized = true;

    // Begin processing.
    await DataProcessor.ProcessAsync(options, cts.Token);

    // Allow enough time for logs to 'flush' to console before completing.
    await Task.Delay(250);

    Console.WriteLine();
    Console.WriteLine("COMPLETED: Press any key to exit.");
    Console.ReadKey();
}
catch (OperationCanceledException)
{
    Console.WriteLine();
    Console.WriteLine("CANCELLED: Press any key to exit.");
    Console.ReadKey();
}

static (string? foreName, string? surName, string? departmentId) AcceptUserDetails()
{
    Console.Clear();
    Console.Write("ForeName: ");
    var foreName = Console.ReadLine();
    Console.Write("SurName: ");
    var surName = Console.ReadLine();
    Console.Write("DepartmentId: ");
    var departmentId = Console.ReadLine();

    return (foreName, surName, departmentId);
}

static bool ValidateUserDetails(string? forName, string? surName, string? departmetnId)
{
    if (string.IsNullOrEmpty(forName) || string.IsNullOrEmpty(surName) || string.IsNullOrEmpty(departmetnId))
    {
        Console.WriteLine(@"Welcome to ""Data Muncher"" the Globomantics data processor! \U001F602");
        Console.WriteLine();

        Console.WriteLine("please provide some information");
        Console.Clear();
        Console.WriteLine("ERROR: You must supply all details");
        Console.WriteLine("Press any key to restart the application");
        Console.ReadKey();

        return false;
    }

    return true;
}