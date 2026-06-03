using System.Collections.Generic;

namespace nHash.Application.Ids;

public class UlidResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public List<string> Ulids { get; set; } = new();
}

public interface IUlidService
{
    UlidResult Generate(int count);
}
