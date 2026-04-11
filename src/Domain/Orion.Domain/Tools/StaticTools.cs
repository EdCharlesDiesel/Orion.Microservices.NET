using System;

namespace Orion.Domain.Tools;

public static class StaticTools
{
    public static int? TryGetErrorLine(Exception e)
    {
        if (string.IsNullOrEmpty(e.StackTrace))
            return null;
        var lineMatch = System.Text.RegularExpressions.Regex.Match(e.StackTrace, @"\:line (\d+)");
        if (lineMatch.Success && int.TryParse(lineMatch.Groups[1].Value, out var lineNumber))
        {
            return lineNumber;
        }

        return null;
    }
}