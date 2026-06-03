using System.CommandLine;
using nHash.Application.Maths;
using nHash.Application.Maths.Models;
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
        cmd.Aliases.Add("pr");
        cmd.Aliases.Add("p");
        cmd.SetAction(parseResult =>
        {
            var num = parseResult.GetValue(numberArg);
            var res = mathService.CheckPrime(num);
            if (!res.Success)
            {
                outputProvider.AppendLine(res.ErrorMessage);
                return;
            }

            outputProvider.AppendLine($"Number: {res.Number}");
            outputProvider.AppendLine($"Is Prime: {(res.IsPrime ? "Yes" : "No")}");
            if (!res.IsPrime)
            {
                outputProvider.AppendLine($"Smallest Factor: {res.SmallestFactor}");
            }
            outputProvider.AppendLine($"Is Perfect Number: {(res.IsPerfectNumber ? "Yes" : "No")}");
            outputProvider.AppendLine($"Is Fibonacci Number: {(res.IsFibonacciNumber ? "Yes" : "No")}");
            outputProvider.AppendLine($"Digit Sum: {res.DigitSum}");
        });
        return cmd;
    }

    private BaseCommand GetFibonacciCommand()
    {
        var countOption = new Option<int>("--count", "-c") { Description = "Count of Fibonacci numbers to generate", DefaultValueFactory = _ => 10 };
        var cmd = new BaseCommand("fibonacci", "Generate a sequence of Fibonacci numbers");
        cmd.Options.Add(countOption);
        cmd.Aliases.Add("fib");
        cmd.Aliases.Add("f");
        cmd.SetAction(parseResult =>
        {
            var count = parseResult.GetValue(countOption);
            var res = mathService.GenerateFibonacci(count);
            if (!res.Success)
            {
                outputProvider.AppendLine(res.ErrorMessage);
                return;
            }

            outputProvider.AppendLine(string.Join(", ", res.Sequence));
        });
        return cmd;
    }

    private BaseCommand GetFactorCommand()
    {
        var numberArg = new Argument<long>("number") { Description = "Number to factorize" };
        var cmd = new BaseCommand("factor", "Factorize a positive number into its prime factors");
        cmd.Arguments.Add(numberArg);
        cmd.Aliases.Add("fac");
        cmd.Aliases.Add("fact");
        cmd.SetAction(parseResult =>
        {
            var num = parseResult.GetValue(numberArg);
            var res = mathService.Factorize(num);
            if (!res.Success)
            {
                outputProvider.AppendLine(res.ErrorMessage);
                return;
            }

            outputProvider.Append($"{res.Number} = ");
            var parts = new List<string>();
            foreach (var pair in res.Factors)
            {
                if (pair.Value == 1)
                    parts.Add($"{pair.Key}");
                else
                    parts.Add($"{pair.Key}^{pair.Value}");
            }
            outputProvider.AppendLine(string.Join(" * ", parts));
        });
        return cmd;
    }

    private BaseCommand GetCalcCommand()
    {
        var expressionArg = new Argument<string>("expression") { Description = "Math expression to evaluate" };
        var cmd = new BaseCommand("evaluate", "Evaluate a mathematical expression (+, -, *, /, %, ^, sin, cos, etc.)");
        cmd.Aliases.Add("eval");
        cmd.Aliases.Add("c");
        cmd.Arguments.Add(expressionArg);
        cmd.SetAction(parseResult =>
        {
            var expr = parseResult.GetValue(expressionArg) ?? string.Empty;
            var res = mathService.Calculate(expr);
            if (!res.Success)
            {
                outputProvider.AppendLine(res.ErrorMessage);
                return;
            }

            outputProvider.AppendLine($"Expression: {res.Expression}");
            outputProvider.AppendLine($"Result: {res.Result}");
        });
        return cmd;
    }
}
