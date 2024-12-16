using CSLox.Utilities;
using System.Text;

public class Lox
{
    private static bool _hadError = false;

    public static void Main(string[] args)
    {
        args = new string[]
        {
            @"C:/Users/arno/OneDrive/Desktop/test.txt"
        };

        if (args.Length == 0)
        {
            Console.WriteLine("Usage: cslox [script]");
            Environment.Exit(64);
        } 
        else if (args.Length == 1)
        {
            RunFile(args[0]);
        } 
        else
        {
            RunPrompt();
        }
    }

    public static void Error(int line, string message)
    {
        Report(line, "", message);
    }

    // run interpreter on a single file 
    private static void RunFile(string path)
    {
        using StreamReader reader = new StreamReader(path, Encoding.UTF8);
        string content = reader.ReadToEnd(); // currently reads entire file's contents at once

        Run(content);

        if (_hadError )
        {
            Environment.Exit(65);
        }
    }

    // start interpreter in an interactive mode
    private static void RunPrompt()
    {
        while (true)
        {
            Console.Write("> ");
            string? line = Console.ReadLine();

            if (line == null) break;
            Run(line);
            _hadError = false;
        }
    }

    private static void Run(string source)
    {
        Scanner scanner = new Scanner(source);
        List<Token> tokens = scanner.ScanTokens();

        foreach (Token token in tokens)
        {
            Console.WriteLine(token.ToString());
        }
    }
    private static void Report(int line, string where, string message)
    {
        Console.Error.WriteLine($"[line {line}] Error {where}: {message}");
        _hadError = true;   
    }
}