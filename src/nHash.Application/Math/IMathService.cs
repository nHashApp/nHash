namespace nHash.Application.Maths;

public interface IMathService
{
    string CheckPrime(long number);
    string GenerateFibonacci(int count);
    string Factorize(long number);
    string Calculate(string expression);
}

