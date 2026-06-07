namespace nHash.Application.Cryptos.Passwords;

public class BCryptService : IBCryptService
{
    public BCryptHashResult Hash(string password, int workFactor)
    {
        var result = new BCryptHashResult();
        if (password == null)
        {
            result.Success = false;
            result.ErrorMessage = "Password cannot be null.";
            return result;
        }

        if (workFactor < 4 || workFactor > 31)
        {
            result.Success = false;
            result.ErrorMessage = "Work factor must be between 4 and 31.";
            return result;
        }

        try
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(password, workFactor);
            result.Hash = hash;
            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"BCrypt Hashing Error: {ex.Message}";
            return result;
        }
    }

    public BCryptVerifyResult Verify(string password, string hashedPassword)
    {
        var result = new BCryptVerifyResult();
        if (password == null)
        {
            result.Success = false;
            result.ErrorMessage = "Password cannot be null.";
            return result;
        }

        if (string.IsNullOrWhiteSpace(hashedPassword))
        {
            result.Success = false;
            result.ErrorMessage = "Hashed password cannot be empty.";
            return result;
        }

        try
        {
            var isValid = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            result.IsValid = isValid;
            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"BCrypt Verification Error: {ex.Message}";
            return result;
        }
    }
}
