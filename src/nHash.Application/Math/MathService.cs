using System;
using System.Text;
using System.Collections.Generic;

namespace nHash.Application.Maths;

public class MathService : IMathService
{
    public string CheckPrime(long number)
    {
        if (number < 2)
            return $"{number} is not a prime number. (Primes must be >= 2)";

        long smallestFactor = -1;
        long limit = (long)System.Math.Sqrt(number);
        for (long i = 2; i <= limit; i++)
        {
            if (number % i == 0)
            {
                smallestFactor = i;
                break;
            }
        }

        var isPrime = smallestFactor == -1;
        var sb = new StringBuilder();
        sb.AppendLine($"Number: {number}");
        sb.AppendLine($"Is Prime: {(isPrime ? "Yes" : "No")}");
        if (!isPrime)
        {
            sb.AppendLine($"Smallest Factor: {smallestFactor}");
        }

        // Perfect check
        sb.AppendLine($"Is Perfect Number: {(IsPerfect(number) ? "Yes" : "No")}");

        // Fibonacci check
        sb.AppendLine($"Is Fibonacci Number: {(IsFibonacci(number) ? "Yes" : "No")}");

        // Digit sum
        sb.AppendLine($"Digit Sum: {DigitSum(number)}");

        return sb.ToString().TrimEnd();
    }

    private static bool IsPerfect(long n)
    {
        if (n <= 1) return false;
        long sum = 1;
        long limit = (long)System.Math.Sqrt(n);
        for (long i = 2; i <= limit; i++)
        {
            if (n % i == 0)
            {
                sum += i;
                long other = n / i;
                if (other != i)
                {
                    sum += other;
                }
            }
        }
        return sum == n;
    }

    private static bool IsFibonacci(long n)
    {
        if (n < 0) return false;
        try
        {
            double val1 = 5.0 * n * n + 4;
            double val2 = 5.0 * n * n - 4;
            return IsPerfectSquareDouble(val1) || IsPerfectSquareDouble(val2);
        }
        catch
        {
            return false;
        }
    }

    private static bool IsPerfectSquareDouble(double x)
    {
        if (x < 0) return false;
        double s = System.Math.Round(System.Math.Sqrt(x));
        return System.Math.Abs(s * s - x) < 0.0001;
    }

    private static long DigitSum(long n)
    {
        long sum = 0;
        n = System.Math.Abs(n);
        while (n > 0)
        {
            sum += n % 10;
            n /= 10;
        }
        return sum;
    }

    public string GenerateFibonacci(int count)
    {
        if (count <= 0) return "Count must be > 0";
        if (count > 93) count = 93; // 93 is the maximum Fibonacci number that fits in ulong

        var list = new List<ulong>();
        ulong a = 0;
        ulong b = 1;
        for (int i = 0; i < count; i++)
        {
            list.Add(a);
            ulong temp = a + b;
            a = b;
            b = temp;
        }

        return string.Join(", ", list);
    }

    public string Factorize(long number)
    {
        if (number <= 0) return "Number must be a positive integer >= 1";
        if (number == 1) return "1 = 1";

        long temp = number;
        var factors = new Dictionary<long, int>();
        
        // Count 2s
        while (temp % 2 == 0)
        {
            if (factors.TryGetValue(2, out var count))
                factors[2] = count + 1;
            else
                factors[2] = 1;
            temp /= 2;
        }

        // Count odd factors
        long limit = (long)System.Math.Sqrt(temp);
        for (long i = 3; i <= limit; i += 2)
        {
            while (temp % i == 0)
            {
                if (factors.TryGetValue(i, out var count))
                    factors[i] = count + 1;
                else
                    factors[i] = 1;
                temp /= i;
            }
            limit = (long)System.Math.Sqrt(temp);
        }

        if (temp > 1)
        {
            factors[temp] = 1;
        }

        var sb = new StringBuilder();
        sb.Append($"{number} = ");
        var parts = new List<string>();
        foreach (var pair in factors)
        {
            if (pair.Value == 1)
                parts.Add($"{pair.Key}");
            else
                parts.Add($"{pair.Key}^{pair.Value}");
        }
        sb.Append(string.Join(" * ", parts));
        return sb.ToString();
    }

    public string Calculate(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
            return "Error: Expression cannot be empty";

        try
        {
            var result = Evaluate(expression);
            return $"Expression: {expression}\nResult: {result}";
        }
        catch (Exception ex)
        {
            return $"Error calculating expression: {ex.Message}";
        }
    }

    private static double Evaluate(string expr)
    {
        int pos = 0;
        double result = ParseExpression(expr, ref pos);
        
        while (pos < expr.Length && char.IsWhiteSpace(expr[pos])) pos++;
        
        if (pos < expr.Length)
            throw new ArgumentException($"Unexpected character: '{expr[pos]}' at index {pos}");
            
        return result;
    }

    private static double ParseExpression(string expr, ref int pos)
    {
        double val = ParseTerm(expr, ref pos);
        while (true)
        {
            SkipWhitespace(expr, ref pos);
            if (pos >= expr.Length) break;
            char op = expr[pos];
            if (op == '+' || op == '-')
            {
                pos++;
                double nextVal = ParseTerm(expr, ref pos);
                if (op == '+') val += nextVal;
                else val -= nextVal;
            }
            else
            {
                break;
            }
        }
        return val;
    }

    private static double ParseTerm(string expr, ref int pos)
    {
        double val = ParseFactor(expr, ref pos);
        while (true)
        {
            SkipWhitespace(expr, ref pos);
            if (pos >= expr.Length) break;
            char op = expr[pos];
            if (op == '*' || op == '/' || op == '%')
            {
                pos++;
                double nextVal = ParseFactor(expr, ref pos);
                if (op == '*') val *= nextVal;
                else if (op == '/')
                {
                    if (nextVal == 0) throw new DivideByZeroException("Division by zero");
                    val /= nextVal;
                }
                else
                {
                    if (nextVal == 0) throw new DivideByZeroException("Modulo by zero");
                    val %= nextVal;
                }
            }
            else
            {
                break;
            }
        }
        return val;
    }

    private static double ParseFactor(string expr, ref int pos)
    {
        double val = ParsePrimary(expr, ref pos);
        SkipWhitespace(expr, ref pos);
        if (pos < expr.Length && expr[pos] == '^')
        {
            pos++;
            double exponent = ParseFactor(expr, ref pos);
            val = System.Math.Pow(val, exponent);
        }
        return val;
    }

    private static double ParsePrimary(string expr, ref int pos)
    {
        SkipWhitespace(expr, ref pos);
        if (pos >= expr.Length)
            throw new ArgumentException("Unexpected end of expression");

        char c = expr[pos];

        if (c == '-')
        {
            pos++;
            return -ParsePrimary(expr, ref pos);
        }
        if (c == '+')
        {
            pos++;
            return ParsePrimary(expr, ref pos);
        }

        if (c == '(')
        {
            pos++;
            double val = ParseExpression(expr, ref pos);
            SkipWhitespace(expr, ref pos);
            if (pos >= expr.Length || expr[pos] != ')')
                throw new ArgumentException("Missing closing parenthesis");
            pos++;
            return val;
        }

        if (char.IsDigit(c) || c == '.')
        {
            int start = pos;
            bool hasDecimal = false;
            while (pos < expr.Length && (char.IsDigit(expr[pos]) || expr[pos] == '.'))
            {
                if (expr[pos] == '.')
                {
                    if (hasDecimal) throw new ArgumentException("Invalid number format");
                    hasDecimal = true;
                }
                pos++;
            }
            string numStr = expr[start..pos];
            if (!double.TryParse(numStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double num))
                throw new ArgumentException($"Invalid number: '{numStr}'");
            return num;
        }

        if (char.IsLetter(c))
        {
            int start = pos;
            while (pos < expr.Length && char.IsLetter(expr[pos]))
            {
                pos++;
            }
            string funcName = expr[start..pos].ToLowerInvariant();
            
            SkipWhitespace(expr, ref pos);
            if (pos >= expr.Length || expr[pos] != '(')
                throw new ArgumentException($"Missing opening parenthesis for function '{funcName}'");
            pos++;

            double arg = ParseExpression(expr, ref pos);
            
            SkipWhitespace(expr, ref pos);
            if (pos >= expr.Length || expr[pos] != ')')
                throw new ArgumentException($"Missing closing parenthesis for function '{funcName}'");
            pos++;

            return funcName switch
            {
                "sin" => System.Math.Sin(arg),
                "cos" => System.Math.Cos(arg),
                "tan" => System.Math.Tan(arg),
                "sqrt" => System.Math.Sqrt(arg),
                "abs" => System.Math.Abs(arg),
                "log" => System.Math.Log(arg),
                "floor" => System.Math.Floor(arg),
                "ceil" => System.Math.Ceiling(arg),
                "round" => System.Math.Round(arg),
                _ => throw new ArgumentException($"Unknown function: '{funcName}'")
            };
        }

        throw new ArgumentException($"Unexpected character: '{c}'");
    }

    private static void SkipWhitespace(string expr, ref int pos)
    {
        while (pos < expr.Length && char.IsWhiteSpace(expr[pos]))
        {
            pos++;
        }
    }
}
