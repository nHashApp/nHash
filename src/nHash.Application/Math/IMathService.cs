using nHash.Application.Maths.Models;

namespace nHash.Application.Maths;

public interface IMathService
{
    PrimeCheckResult CheckPrime(long number);
    FibonacciResult GenerateFibonacci(int count);
    FactorizeResult Factorize(long number);
    MathCalculateResult Calculate(string expression);
}

