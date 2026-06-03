using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace nHash.Application.Ids;

public class NanoIdService : INanoIdService
{
    private const string DefaultAlphabet = "_~abcdefghijklmnopqrstuvwxyz0123456789_~ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public NanoIdResult Generate(int length, string alphabet, int count)
    {
        var result = new NanoIdResult();

        if (length <= 0)
        {
            result.Success = false;
            result.ErrorMessage = "Length must be greater than 0.";
            return result;
        }

        if (count <= 0)
        {
            result.Success = false;
            result.ErrorMessage = "Count must be greater than 0.";
            return result;
        }

        if (string.IsNullOrEmpty(alphabet))
        {
            alphabet = DefaultAlphabet;
        }

        if (alphabet.Length > 256)
        {
            result.Success = false;
            result.ErrorMessage = "Alphabet length cannot be larger than 256 characters.";
            return result;
        }

        try
        {
            for (int i = 0; i < count; i++)
            {
                result.NanoIds.Add(GenerateSingleNanoId(length, alphabet));
            }
            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"NanoID Generation Error: {ex.Message}";
            return result;
        }
    }

    private static string GenerateSingleNanoId(int size, string alphabet)
    {
        var mask = (2 << (int)Math.Log(alphabet.Length - 1, 2)) - 1;
        var step = (int)Math.Ceiling(1.6 * mask * size / alphabet.Length);

        var idBuilder = new char[size];
        var bytes = new byte[step];
        var pointer = 0;

        while (pointer < size)
        {
            RandomNumberGenerator.Fill(bytes);
            for (var i = 0; i < step; i++)
            {
                var alphabetIndex = bytes[i] & mask;
                if (alphabetIndex < alphabet.Length)
                {
                    idBuilder[pointer] = alphabet[alphabetIndex];
                    pointer++;
                    if (pointer == size)
                    {
                        break;
                    }
                }
            }
        }

        return new string(idBuilder);
    }
}
