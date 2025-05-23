using System.Text;
using DSharpPlus.Entities;

namespace Discord.Common.Helpers;

public static class StringUtils {
    /// <summary>
    /// Replaces each character in a string with an asterisk
    /// </summary>
    /// <param name="thisString"></param>
    /// <returns>Each character as an asterisk</returns>
    public static string Redact(this string? thisString) {
        string? temp = null;
        temp = thisString!.ToCharArray().Aggregate(temp, (current, empty) => current + "*");
        return temp ?? "***************";
    }

    /// <summary>
    /// Replaces all the old characters (as char) to anything of your choosing.
    /// </summary>
    /// <param name="theStringToBeEdited">this</param>
    /// <param name="oldCharacters">Characters (char[]) to be replaced</param>
    /// <param name="newCharacters">What the old characters will be replaced to. If left blank, outcome to be blank</param>
    /// <returns>new string with the replaced chars</returns>
    public static string ReplaceAll(this string theStringToBeEdited, string oldCharacters, string newCharacters = "") // Idea from Java String.ReplaceAll()
        => oldCharacters.ToCharArray().Aggregate(theStringToBeEdited, (current, c) => current.Replace($"{c}", newCharacters));

    /// <summary>
    /// Returns a bool if the inputted string is "true" or not
    /// </summary>
    /// <param name="input">this</param>
    /// <returns>Boolean indicating if the inputted string is "true" or not</returns>
    public static bool AsBool(this string input) => input.ToLower().Equals("true");

    /// <summary>
    /// Returns a bool if the inputted string is "true" or "false"
    /// </summary>
    /// <param name="boolean">this</param>
    /// <returns>"true" or "false"</returns>
    public static string AsString(this bool boolean) => boolean ? "true" : "false";

    /// <summary>
    /// Trys to parse string data as a double
    /// </summary>
    /// <param name="str">this</param>
    /// <returns>double</returns>
    public static double AsDouble(this string? str) => double.Parse(str!);

    /// <summary>
    /// Trys to parse string data as an integer
    /// </summary>
    /// <param name="str">this</param>
    /// <returns>integer</returns>
    public static int AsInt(this string? str) => int.Parse(str!);

    /// <summary>
    /// Checks if the string contains multiple string values
    /// </summary>
    /// <param name="str1">this</param>
    /// <param name="chars">As many strings as you want to compare to the target string</param>
    /// <returns>Boolean indicating that any and all of your specified strings are contained in the target string (this)</returns>
    public static bool AndContainsMultiple(this string str1, params string[] chars) => chars.All(str1.ToLower().Contains);
    
    /// <summary>
    /// Checks if the string contains multiple character values
    /// </summary>
    /// <param name="str1">this</param>
    /// <param name="chars">As many characters as you want to compare to the target string</param>
    /// <returns></returns>
    public static bool AndContainsMultiple(this string str1, params char[] chars) => chars.All(str1.ToLower().Contains);

    /// <summary>
    /// Checks if the string contains multiple string values using OR
    /// </summary>
    /// <param name="str1">this</param>
    /// <param name="strs">As many strings as you want to compare to the target string</param>
    /// <returns>Boolean that iterates through each string instance and compares it to that target using OR</returns>
    public static bool OrContainsMultiple(this string str1, params string[] strs) => strs.Any(str1.ToLower().Contains);

    /// <summary>
    /// Checks if the string contains multiple character values using OR
    /// </summary>
    /// <param name="str1">this</param>
    /// <param name="chars">As many characters as you want to compare to the target string</param>
    /// <returns></returns>
    public static bool OrContainsMultiple(this string str1, params char[] chars) => chars.Any(str1.ToLower().Contains);

    /// <summary>
    /// Checks if the string is equal to multiple values
    /// </summary>
    /// <param name="str1">this</param>
    /// <param name="strs">As many strings as you want to compare to the target string</param>
    /// <returns>Boolean whether the multiple strings are equal to each other (or the same) as the target string (this)</returns>
    public static bool EqualsMultiple(this string str1, params string[] strs) => strs.All(str1.Equals);

    /// <summary>
    /// Checks if the string is not equal to a value
    /// </summary>
    /// <param name="string1">this</param>
    /// <param name="string2">String to compare to the target</param>
    /// <returns>Boolean whether the two string are equal to each other (or the same)</returns>
    public static bool NotEquals(this string string1, string string2) => string1 != string2;

    /// <summary>
    /// Checks to see if the left number is equals the right number
    /// </summary>
    /// <param name="num1">this</param>
    /// <param name="num2">number to compare with the target number</param>
    /// <returns>Boolean whether the two numbers are equal to each other</returns>
    public static bool Is(this int num1, int num2) => num1 == num2;

    /// <summary>
    /// Checks to see if the left number is not equals the right number
    /// </summary>
    /// <param name="num1">this</param>
    /// <param name="num2">number to compare with the target number</param>
    /// <returns>Boolean whether the two numbers are not equal to each other</returns>
    public static bool IsNot(this int num1, int num2) => num1 != num2;
    
    /// <summary>
    /// Checks to see if the left number is not equals the right number
    /// </summary>
    /// <param name="num1">this</param>
    /// <param name="num2">number to compare with the target number</param>
    /// <returns>Boolean whether the two numbers are not equal to each other</returns>
    public static bool NotEqual(this int num1, int num2) => num1 != num2;

    /// <summary>
    /// Returns a random string of a specified length
    /// </summary>
    /// <param name="length">Target length of randomized string</param>
    /// <returns>Random string of alphanumeric characters</returns>
    public static string GetRandomString(int length = 15) =>
        new(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz", length)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());
    
    /// <summary>
    /// Returns a bool to see if the number is zero
    /// </summary>
    /// <param name="ulong">this ulong</param>
    /// <returns>true or false</returns>
    public static bool IsZero(this ulong @ulong) => @ulong is 0;
    
    /// <summary>
    /// Returns a bool to see if the number is zero
    /// </summary>
    /// <param name="double">this double</param>
    /// <returns>true or false</returns>
    public static bool IsZero(this double @double) => @double is 0;
    
    /// <summary>
    /// Returns a bool to see if the number is zero
    /// </summary>
    /// <param name="int">this int</param>
    /// <returns>true or false</returns>
    public static bool IsZero(this int @int) => @int is 0;
    
    /// <summary>
    /// Returns a bool to see if the number is zero
    /// </summary>
    /// <param name="float">this float</param>
    /// <returns>true or false</returns>
    public static bool IsZero(this float @float) => @float is 0;
    
    /// <summary>
    /// Returns a bool to see if the number is zero
    /// </summary>
    /// <param name="long">this long</param>
    /// <returns>true or false</returns>
    public static bool IsZero(this long @long) => @long is 0;
    
    /// <summary>
    /// Returns a bool to see if the number is zero
    /// </summary>
    /// <param name="decimal">this decimal</param>
    /// <returns>true or false</returns>
    public static bool IsZero(this decimal @decimal) => @decimal is 0;
    
    /// <summary>
    /// Returns a bool to see if the number is zero
    /// </summary>
    /// <param name="str">this string</param>
    /// <returns>true or false</returns>
    public static bool IsZero(this string str) => str is "0";

    /// <summary>
    /// Checks if the string contains the given string, if it does, it replaces it with your given string.
    /// </summary>
    /// <param name="theStringToBeEdited">this</param>
    /// <param name="contains">string to check if it contains to</param>
    /// <param name="replaceWith">string to replace contains string with</param>
    /// <returns>new string with replaced content</returns>
    public static string ContainsAndReplace(this string theStringToBeEdited, string contains, string replaceWith)
        => !theStringToBeEdited.Contains(contains) ? theStringToBeEdited : theStringToBeEdited.Replace(contains, replaceWith);

    /// <summary>
    /// Checks if the string contains the given string, if it does, it replaces the entire string with your given string.
    /// </summary>
    /// <param name="theStringToBeEdited">this</param>
    /// <param name="contains">string to check if it contains to</param>
    /// <param name="replaceWith">string to replace entire string with</param>
    /// <returns>new string with replaced content</returns>
    public static string ContainsAndReplaceWhole(this string theStringToBeEdited, string contains, string replaceWith)
        => !theStringToBeEdited.Contains(contains) ? theStringToBeEdited : replaceWith;

    /// <summary>
    /// Escapes Discord text modifiers (bold, italics, strikethrough, code, and spoiler) in a string.
    /// </summary>
    /// <param name="thisString">this</param>
    /// <returns>Returns a string with the modifers escaped.</returns>
    public static string EscapeTextModifiers(this string thisString) {
        var asChars = thisString.ToCharArray();
        var hasTwoOrMoreUnderscores = asChars.Count(c => c == '_') >= 2;
        var hasTwoOrMoreAsterisks = asChars.Count(c => c == '*') >= 2;
        var hasTwoOrMoreTildes = asChars.Count(c => c == '~') >= 2;
        var hasTwoOrMoreBackticks = asChars.Count(c => c == '`') >= 2;
        var hasBackSlash = asChars.Count(c => c == '\\') >= 1;
        var hasQuote = asChars.Count(c => c == '\"') >= 1;

        if (hasTwoOrMoreUnderscores)
            thisString = thisString.ReplaceAll("_", "\\_");
        if (hasTwoOrMoreAsterisks)
            thisString = thisString.ReplaceAll("*", "\\*");
        if (hasTwoOrMoreTildes)
            thisString = thisString.ReplaceAll("~", "\\~");
        if (hasTwoOrMoreBackticks)
            thisString = thisString.ReplaceAll("`", "\\`");
        if (hasBackSlash)
            thisString = thisString.ReplaceAll("\\", @"\\");
        if (hasQuote)
            thisString = thisString.ReplaceAll("\"", "\\\"");
        return thisString;
    }

    private static string VerifyString(object data, Type? error = null, string? errorMessage = null, bool allowEmpty = true) {
        errorMessage ??= $"Expected a string, got {data} instead.";
        error ??= typeof(Exception);

        var s = (string)data;
        if (s == null) throw (Exception)Activator.CreateInstance(error, errorMessage)!;
        if (!allowEmpty && s.Length == 0) throw (Exception)Activator.CreateInstance(error, errorMessage)!;
        return s;
    }

    /// <summary>
    /// Safely splits a string into multiple messages.
    /// </summary>
    /// <param name="text">Main message text</param>
    /// <param name="maxLength">Length to where you want the text to split at</param>
    /// <param name="charSeparator">char separator</param>
    /// <param name="prepend">Text to add before the next message</param>
    /// <param name="append">Text to add after the first and all new split messages</param>
    /// <returns>List of strings to be used in a for or foreach loop to send messages</returns>
    /// <exception cref="Exception">Provided string was empty, nothing to split</exception>
    public static List<string> SplitMessage(this string text, int maxLength = 2000, string charSeparator = "\n", string prepend = "", string append = "") {
        text = VerifyString(text);

        if (text.Length <= maxLength) return [text];

        var messages = new List<string>();
        var msg = "";

        var splitText = text.Split( [ charSeparator ], StringSplitOptions.None).ToList();

        if (splitText.Any(elem => elem.Length > maxLength)) throw new Exception("SPLIT_MAX_LENGTH_EXCEEDED");

        foreach (var chunk in splitText) {
            if (!string.IsNullOrEmpty(msg) && (msg + charSeparator + chunk + append).Length > maxLength) {
                messages.Add(msg + append);
                msg = prepend;
            }

            msg += (string.IsNullOrEmpty(msg) || msg != prepend ? "" : charSeparator) + chunk;
        }

        messages.Add(msg);
        return messages.Where(m => !string.IsNullOrEmpty(m)).ToList();
    }


    /// <summary>
    /// Get Discord ActivityType from string
    /// </summary>
    /// <param name="type">activity as string</param>
    /// <returns>DSharpPlus.Entities.ActivityType</returns>
    public static DiscordActivityType GetActivityType(string type) {
        return type.ToLower() switch {
            "playing" => DiscordActivityType.Playing,
            "listening" => DiscordActivityType.ListeningTo,
            "watching" => DiscordActivityType.Watching,
            "streaming" => DiscordActivityType.Streaming,
            "competing" => DiscordActivityType.Competing,
            "play" => DiscordActivityType.Playing,
            "listen" => DiscordActivityType.ListeningTo,
            "watch" => DiscordActivityType.Watching,
            "stream" => DiscordActivityType.Streaming,
            "other" => DiscordActivityType.Custom,
            "compete" => DiscordActivityType.Competing,
            "custom" => DiscordActivityType.Custom,
            _ => DiscordActivityType.Custom
        };
    }

    /// <summary>
    /// Get Discord UserStatus from string
    /// </summary>
    /// <param name="status">status as string</param>
    /// <returns>DSharpPlus.Entities.UserStatus</returns>
    public static DiscordUserStatus GetUserStatus(string status) {
        return status.ToLower() switch {
            "online" => DiscordUserStatus.Online,
            "idle" => DiscordUserStatus.Idle,
            "dnd" => DiscordUserStatus.DoNotDisturb,
            "do_not_disturb" => DiscordUserStatus.DoNotDisturb,
            "invisible" => DiscordUserStatus.Invisible,
            "offline" => DiscordUserStatus.Invisible,
            _ => DiscordUserStatus.Online
        };
    }

    /// <summary>
    /// Get a specified number of characters from the left side of a string
    /// </summary>
    /// <param name="input">this string</param>
    /// <param name="length">how many characters you want to get</param>
    /// <returns>Returns the left most specified characters</returns>
    public static string Left(this string input, int length) => (input.Length < length) ? input : input[..length];

    /// <summary>
    /// Only accepts letters and numbers used for hex colors
    /// </summary>
    /// <param name="input">this string</param>
    /// <returns>string with only letters and numbers used for hex color codes</returns>
    public static string ValidateHexColor(this string input) {
        char[] allowedChars = ['a', 'b', 'c', 'd', 'e', 'f', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
        return new string(input.ToLower().ToCharArray().Where(c => allowedChars.Contains(c)).ToArray());
        ;
    }

    /// <summary>
    /// Remove all letters from a string and casts it to an integer
    /// </summary>
    /// <param name="input">this string</param>
    /// <returns>integer</returns>
    public static int? RemoveAllLetters(this string input) => int.Parse(input.Where(char.IsDigit).Aggregate(string.Empty, (current, c) => current + c));

    public static string Sanitize(this string input) {
        input = input.Replace("\\", "");
        input = input.Replace("`", "");
        input = input.Replace("|", "");
        input = input.Replace("%", "");

        return input;
    }

    /// <summary>Indents using whitespace like.</summary>
    /// <param name="text">The text you want to be indented.</param>
    /// <param name="indentationAmount">The amount of spaces you want the <paramref name="text"/> to be intented with.</param>
    /// <returns>Indented text.</returns>
    public static string IndentWithWhitespace(string text, int indentationAmount = 4) => IndentWithCharacter(text, " ", indentationAmount);

    /// <summary>Indents using a string input.</summary>
    /// <param name="text">The text you want to be indented.</param>
    /// <param name="indentationAmount">The amount of spaces or char you want the <paramref name="text"/> to be intented with.</param>
    /// <param name="indentationCharacter">The amount of spaces you want it to be intented.</param>
    /// <returns>Indented text.</returns>
    public static string IndentWithCharacter(string text, string indentationCharacter, int indentationAmount = 4) {
        Preconditions.NotNullOrEmpty(indentationCharacter, nameof(indentationCharacter));

        var sb = new StringBuilder(text);
        sb.Insert(0, indentationCharacter, indentationAmount);

        return sb.ToString();
    }
}