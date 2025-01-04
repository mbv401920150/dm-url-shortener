﻿namespace UrlShortener.Core;

public static class Base62EncodingExtension
{
    private const string Alphanumeric = "0123456789" +
                                        "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                                        "abcdefghijklmnopqrstuvwxyz";
    public static string EncodeToBase62(this long number)
    {
        if(number == 0) return Alphanumeric[0].ToString();
        
        var result = new Stack<char>();
        while (number > 0)
        {
            result.Push(Alphanumeric[(int)number % Alphanumeric.Length]);
            number /= Alphanumeric.Length;
        }
        
        return new string(result.ToArray());
    }
}