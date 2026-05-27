using System.CommandLine;
using nHash.Application.Maths;
using nHash.Application.Abstraction;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Maths;

public class MathCommand(IMathService mathService, IOutputProvider outputProvider) : IMathCommand
{
    public BaseCommand Command => GetCommand();

    private BaseCommand GetCommand()
    {
        var command = new BaseCommand("math", "Mathematical utilities and calculators");
        command.Aliases.Add("calc");
        command.Subcommands.Add(GetPrimeCommand());
        command.Subcommands.Add(GetFibonacciCommand());
        command.Subcommands.Add(GetFactorCommand());
        command.Subcommands.Add(GetCalcCommand());
        return command;
    }

    private BaseCommand GetPrimeCommand()
    {
        var numberArg = new Argument<long>("number") { Description = "Number to check" };
        var cmd = new BaseCommand("prime", "Check if a number is prime, perfect, or Fibonacci, and calculate digit sum");
        cmd.Arguments.Add(numberArg);
        cmd.SetAction(parseResult =>
        {
            var num = parseResult.GetValue(numberArg);
            var res = mathService.CheckPrime(num);
            outputProvider.AppendLine(res);
        });
        return cmd;
    }

    private BaseCommand GetFibonacciCommand()
    {
        var countOption = new Option<int>("--count", "-c") { Description = "Count of Fibonacci numbers to generate", DefaultValueFactory = _ => 10 };
        var cmd = new BaseCommand("fibonacci", "Generate a sequence of Fibonacci numbers");
        cmd.Options.Add(countOption);
        cmd.SetAction(parseResult =>
        {
            var count = parseResult.GetValue(countOption);
            var res = mathService.GenerateFibonacci(count);
            outputProvider.AppendLine(res);
        });
        return cmd;
    }

    private BaseCommand GetFactorCommand()
    {
        var numberArg = new Argument<long>("number") { Description = "Number to factorize" };
        var cmd = new BaseCommand("factor", "Factorize a positive number into its prime factors");
        cmd.Arguments.Add(numberArg);
        cmd.SetAction(parseResult =>
        {
            var num = parseResult.GetValue(numberArg);
            var res = mathService.Factorize(num);
            outputProvider.AppendLine(res);
        });
        return cmd;
    }

    private BaseCommand GetCalcCommand()
    {
        var expressionArg = new Argument<string>("expression") { Description = "Math expression to evaluate" };
        var cmd = new BaseCommand("evaluate", "Evaluate a mathematical expression (+, -, *, /, %, ^, sin, cos, etc.)");
        cmd.Aliases.Add("eval");
        cmd.Arguments.Add(expressionArg);
        cmd.SetAction(parseResult =>
        {
            var expr = parseResult.GetValue(expressionArg) ?? string.Empty;
            var res = mathService.Calculate(expr);
            outputProvider.AppendLine(res);
        });
        return cmd;
    }
}
