using System;
using System.Collections.Generic;

namespace nHash.Application.Dev.Models;

public class CronParseResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string MinuteDescription { get; set; } = string.Empty;
    public string HourDescription { get; set; } = string.Empty;
    public string DayOfMonthDescription { get; set; } = string.Empty;
    public string MonthDescription { get; set; } = string.Empty;
    public string DayOfWeekDescription { get; set; } = string.Empty;
    public List<DateTime> NextExecutions { get; set; } = new();
}

public class RegexCaptureGroupDetail
{
    public int Index { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public class RegexMatchDetail
{
    public int MatchNumber { get; set; }
    public string Value { get; set; } = string.Empty;
    public int Index { get; set; }
    public int Length { get; set; }
    public List<RegexCaptureGroupDetail> CaptureGroups { get; set; } = new();
}

public class RegexTestResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string Pattern { get; set; } = string.Empty;
    public string Input { get; set; } = string.Empty;
    public bool IsMatch { get; set; }
    public int MatchCount { get; set; }
    public List<RegexMatchDetail> Matches { get; set; } = new();
}

public class ColorConvertResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string Hex { get; set; } = string.Empty;
    public string Rgb { get; set; } = string.Empty;
    public string Hsl { get; set; } = string.Empty;
    public string Cmyk { get; set; } = string.Empty;
    public int R { get; set; }
    public int G { get; set; }
    public int B { get; set; }
}

public class JwtBuildResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string Header { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;
    public string Signature { get; set; } = string.Empty;
}

public class SemverDetails
{
    public string Original { get; set; } = string.Empty;
    public int Major { get; set; }
    public int Minor { get; set; }
    public int Patch { get; set; }
    public string Prerelease { get; set; } = string.Empty;
    public string Build { get; set; } = string.Empty;
}

public class SemverCompareResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string Version1 { get; set; } = string.Empty;
    public string Version2 { get; set; } = string.Empty;
    public SemverDetails Details1 { get; set; } = new();
    public SemverDetails Details2 { get; set; } = new();
    public string ComparisonResult { get; set; } = string.Empty; // "v1 > v2", "v1 < v2", "v1 == v2"
}

public class NumberInspectResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string Input { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsInteger { get; set; }
    public long? IntegerValue { get; set; }
    public double? DoubleValue { get; set; }
    public string Decimal { get; set; } = string.Empty;
    public string Binary { get; set; } = string.Empty;
    public string Octal { get; set; } = string.Empty;
    public string Hexadecimal { get; set; } = string.Empty;
    public string Scientific { get; set; } = string.Empty;
    public bool? Positive { get; set; }
    public bool? Even { get; set; }
    public bool? IsNaN { get; set; }
    public bool? IsInfinity { get; set; }
    public bool? IsFinite { get; set; }
}
