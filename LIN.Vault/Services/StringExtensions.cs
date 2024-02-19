using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIN.Vault.Services;

internal class StringExtensions
{

    public static string GetInitials(string name)
    {

        name = name.Trim();
        string init = "";

        while (name.Contains("  "))
            name = name.Replace("  ", " ");

        var split = name.Split(" ");

        if (split.Length == 1 && split[0].Length > 0)
        {
            init = split[0][0].ToString();
            return init.ToUpper();
        }

        else if (split.Length >= 2 && split[0].Length > 0 && split[1].Length > 0)
        {
            init = split[0][0].ToString();
            init += split[1][0].ToString();
            return init.ToUpper();
        }

        return "?";

    }

}
