using System.Text;

namespace nHash.Application.Texts;

public class TextDiffService : ITextDiffService
{
    public string Compare(string text1, string text2)
    {
        var lines1 = text1.Split(["\r\n", "\n"], StringSplitOptions.None);
        var lines2 = text2.Split(["\r\n", "\n"], StringSplitOptions.None);

        var sb = new StringBuilder();

        int n = lines1.Length;
        int m = lines2.Length;
        int[,] lcs = new int[n + 1, m + 1];

        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                if (lines1[i - 1] == lines2[j - 1])
                {
                    lcs[i, j] = lcs[i - 1, j - 1] + 1;
                }
                else
                {
                    lcs[i, j] = Math.Max(lcs[i - 1, j], lcs[i, j - 1]);
                }
            }
        }

        var diff = new List<string>();
        int x = n;
        int y = m;

        while (x > 0 || y > 0)
        {
            if (x > 0 && y > 0 && lines1[x - 1] == lines2[y - 1])
            {
                diff.Add($"  {lines1[x - 1]}");
                x--;
                y--;
            }
            else if (y > 0 && (x == 0 || lcs[x, y - 1] >= lcs[x - 1, y]))
            {
                diff.Add($"+ {lines2[y - 1]}");
                y--;
            }
            else if (x > 0 && (y == 0 || lcs[x, y - 1] < lcs[x - 1, y]))
            {
                diff.Add($"- {lines1[x - 1]}");
                x--;
            }
        }

        diff.Reverse();
        foreach (var line in diff)
        {
            sb.AppendLine(line);
        }

        return sb.ToString();
    }
}
