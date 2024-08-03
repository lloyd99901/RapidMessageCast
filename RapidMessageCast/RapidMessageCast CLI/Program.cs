using System.Diagnostics;
using System.Text.RegularExpressions;

//Important: This is going to be removed in the future.
//NOTE: This program shouldn't be used.

string versionNumb = "v0.1 indev 2024";
Console.WriteLine("RapidMessageCast CLI Version " + versionNumb);
Console.WriteLine("--------------------------------------------------");
Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine("Warning: This program is not intended for use. Please use the GUI version of RapidMessageCast Dispatcher.");
Console.ForegroundColor = ConsoleColor.Gray;
//Print the starting message to the console
Console.WriteLine("Starting RapidMessageCast Dispatcher...");
string pcNames = "";
string Message = "";
int ExpireHourTime = 0;
int ExpireMinuteTime = 0;
int ExpireSecondTime = 0;
// Function to print the usage
void PrintUsage()
{
    Console.WriteLine("Usage: RapidMessageCast CLI [options]");
    Console.WriteLine("Options:");
    Console.WriteLine("  -h, --help    Show this message");
    Console.WriteLine("  -f, --file    Specify the file to broadcast");
    Console.WriteLine("  -v, --version Show the version of RapidMessageCast Dispatcher");
    Console.WriteLine("If no file is specified, the program will attempt to load default.rmsg if it exists where RMC Dispatcher is.");
    Console.WriteLine("If that file doesn't exist, this program will exit.");
}
void LoadRMSGFileFunction(string filePath)
{
    try
    {
        // Read the contents of the selected file
        string fileContents = File.ReadAllText(filePath);

        // Split the file contents by section headers
        string[] sections = fileContents.Split(new[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        // Extract message, PC list, and message duration from sections
        string message = GetValueFromSection(sections, "[Message]");
        string pcList = GetValueFromSection(sections, "[PCList]");
        string messageDuration = GetValueFromSection(sections, "[MessageDuration]");

        // Populate the TextBoxes with the extracted values
        Message = message;
        pcNames = pcList;

        string[] durationParts = messageDuration.Split(':');
        if (durationParts.Length == 3)
        {
            ExpireHourTime = (int)Convert.ToDecimal(durationParts[0]);
            ExpireMinuteTime = (int)Convert.ToDecimal(durationParts[1]);
            ExpireSecondTime = (int)Convert.ToDecimal(durationParts[2]);
        }
        //Add to loglist with the name of file that was loaded.
        Console.WriteLine("File loaded successfully: " + Path.GetFileName(filePath));
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Error loading message: " + ex.Message);
        Console.ForegroundColor = ConsoleColor.Gray;
    }
}

string GetValueFromSection(string[] sections, string sectionHeader)
{
    foreach (string section in sections)
    {
        if (section.StartsWith(sectionHeader))
        {
            // Get the value following the section header
            string value = section.Substring(sectionHeader.Length).Trim();

            // Filter out invalid characters only for PC list section
            if (sectionHeader == "[PCList]")
            {
                value = FilterInvalidCharacters(value);
            }

            return value;
        }
    }
    return string.Empty;
}

string FilterInvalidCharacters(string text)
{
    // Regular expression to match characters that are not allowed in NetBIOS or Windows hostnames
    string pattern = @"[^\p{L}\p{N}\-\._\n\r]";

    // Replace invalid characters with empty string
    return Regex.Replace(text, pattern, "");
}

void BeginMessageCast(string message, string pcList, int duration)
{
    //Print that broadcast is starting
    Console.WriteLine("Info - Broadcasting message to PCs... Once messaging is complete, press the enter key to exit.");
    //Print Message
    Console.WriteLine("Message: " + message);
    string[] pcNames = pcList.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
    Console.WriteLine("--------------------------------------------------");
    //check if msg.exe exists in system32
    if (!File.Exists("C:\\Windows\\System32\\msg.exe"))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Error: msg.exe not found in system32. Please ensure msg.exe is installed on this system.");
        Console.ForegroundColor = ConsoleColor.Gray;
        Environment.Exit(1);
    }

    foreach (string pcName in pcNames)
    {
        Task.Run(() =>
        {
            try
            {
                Console.WriteLine($"Initialising to message PC: {pcName}");

                var processInfo = new ProcessStartInfo
                {
                    //Run msg.exe from system32
                    FileName = "C:\\Windows\\System32\\msg.exe",
                    Arguments = $"* /TIME:{duration} /SERVER:{pcName} \"{message}\"",
                    CreateNoWindow = true,
                    UseShellExecute = false
                };

                var process = new Process { StartInfo = processInfo };

                // Start the process
                process.Start();

                Console.WriteLine($"MSG process sent for PC: {pcName}");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR - Failure to send command for PC: {pcName}");
                Console.WriteLine(ex.ToString());
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        });
    }
}


//read the arguments passed to the program, if no arguments are passed, print the usage but continue
if (args.Length == 0)
{
    //Print attempt to load default.rmsg
    Console.WriteLine("No arguments passed, attempting to load default.rmsg...");
    //If the file doesn't exist, print an error message and exit
    if (!File.Exists("default.rmsg"))
    {
        //color the error message red
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Error: default.rmsg not found and no arguments were specified. Showing help...");
        Console.ForegroundColor = ConsoleColor.Gray;
        PrintUsage();
        Environment.Exit(1);
    }
    //Parse the default.rmsg file
    LoadRMSGFileFunction("default.rmsg");
    //Begin the message cast
    int totalSeconds = ((int)ExpireHourTime * 3600) + ((int)ExpireMinuteTime * 60) + (int)ExpireSecondTime; //Calculate the total seconds from the hours, minutes and seconds for the message duration.
    BeginMessageCast(Message, pcNames, totalSeconds); //Start the message cast.
}
else
{
    //parse the arguments
    foreach (string arg in args)
    {
        if (arg == "-h" || arg == "--help")
        {
            PrintUsage();
        }
        else if (arg == "-v" || arg == "--version")
        {
            Console.WriteLine("RapidMessageCast Dispatcher Version " + versionNumb);
        }
        else if (arg == "-f" || arg == "--file")
        {
            string filePath = "";
            //Parse the file passed to the program
            try
            {
                filePath = args[Array.IndexOf(args, arg) + 1];
            }
            catch (Exception ex)
            {
                //Arguments failed. Print an error message and exit
                Console.ForegroundColor = ConsoleColor.Red;
                //Exception message
                Console.WriteLine("Error: " + ex.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
                PrintUsage();
                Environment.Exit(1);
            }

            //Parse the file
            LoadRMSGFileFunction(filePath);
            //Begin the message cast
            int totalSeconds = ((int)ExpireHourTime * 3600) + ((int)ExpireMinuteTime * 60) + (int)ExpireSecondTime; //Calculate the total seconds from the hours, minutes and seconds for the message duration.
            BeginMessageCast(Message, pcNames, totalSeconds); //Start the message cast.
        }
        else
        {
            PrintUsage();
        }
    }
}
//Pause the console so the user can read the output
Console.ReadLine();