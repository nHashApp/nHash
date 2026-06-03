namespace nHash.Application.Maths.Models;

public class PrimeCheckResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public long Number { get; set; }
    public bool IsPrime { get; set; }
    public long SmallestFactor { get; set; } = -1;
    public bool IsPerfectNumber { get; set; }
    public bool IsFibonacciNumber { get; set; }
    public long DigitSum { get; set; }
}

public class FibonacciResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public int Count { get; set; }
    public List<ulong> Sequence { get; set; } = new();
}

public class FactorizeResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public long Number { get; set; }
    public Dictionary<long, int> Factors { get; set; } = new();
}

public class MathCalculateResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string Expression { get; set; } = string.Empty;
    public double Result { get; set; }
}
