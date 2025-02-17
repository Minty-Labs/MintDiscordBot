namespace Discord.Common.Helpers;

public static class Preconditions {
    #region Objects

    /// <exception cref="ArgumentNullException"><paramref name="obj"/> must not be <see langword="null"/>.</exception>
    public static void NotNull<T>(T obj, string name, string msg = null) where T : class {
        if (obj == null) throw CreateNotNullException(name, msg);
    }

    private static ArgumentNullException CreateNotNullException(string name, string? msg) 
        => msg == null ? new ArgumentNullException(paramName: name) : new ArgumentNullException(msg, name);

    #endregion

    #region Strings

    /// <exception cref="ArgumentException"><paramref name="obj"/> cannot be blank.</exception>
    public static void NotEmpty(string obj, string name, string msg = null) {
        if (obj.Length == 0) throw CreateNotEmptyException(name, msg);
    }

    /// <exception cref="ArgumentException"><paramref name="obj"/> cannot be blank.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="obj"/> must not be <see langword="null"/>.</exception>
    public static void NotNullOrEmpty(string obj, string name, string msg = null) {
        if (obj == null) throw CreateNotNullException(name, msg);
        if (obj.Length == 0) throw CreateNotEmptyException(name, msg);
    }

    /// <exception cref="ArgumentException"><paramref name="obj"/> cannot be blank.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="obj"/> must not be <see langword="null"/>.</exception>
    public static void NotNullOrWhitespace(string obj, string name, string msg = null) {
        if (obj == null) throw CreateNotNullException(name, msg);
        if (obj.Trim().Length == 0) throw CreateNotEmptyException(name, msg);
    }

    private static ArgumentException CreateNotEmptyException(string name, string? msg)
        => new(msg ?? "Argument cannot be blank.", name);

    #endregion

    #region Numerics

    #region NotEqual

    /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
    public static void NotEqual(sbyte obj, sbyte value, string name, string msg = null) {
        if (obj == value) throw CreateNotEqualException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
    public static void NotEqual(byte obj, byte value, string name, string msg = null) {
        if (obj == value) throw CreateNotEqualException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
    public static void NotEqual(short obj, short value, string name, string msg = null) {
        if (obj == value) throw CreateNotEqualException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
    public static void NotEqual(ushort obj, ushort value, string name, string msg = null) {
        if (obj == value) throw CreateNotEqualException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
    public static void NotEqual(int obj, int value, string name, string msg = null) {
        if (obj == value) throw CreateNotEqualException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
    public static void NotEqual(uint obj, uint value, string name, string msg = null) {
        if (obj == value) throw CreateNotEqualException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
    public static void NotEqual(long obj, long value, string name, string msg = null) {
        if (obj == value) throw CreateNotEqualException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
    public static void NotEqual(ulong obj, ulong value, string name, string msg = null) {
        if (obj == value) throw CreateNotEqualException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
    public static void NotEqual(sbyte? obj, sbyte value, string name, string msg = null) {
        if (obj == value) throw CreateNotEqualException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
    public static void NotEqual(byte? obj, byte value, string name, string msg = null) {
        if (obj == value) throw CreateNotEqualException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
    public static void NotEqual(short? obj, short value, string name, string msg = null) {
        if (obj == value) throw CreateNotEqualException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
    public static void NotEqual(ushort? obj, ushort value, string name, string msg = null) {
        if (obj == value) throw CreateNotEqualException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
    public static void NotEqual(int? obj, int value, string name, string msg = null) {
        if (obj == value) throw CreateNotEqualException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
    public static void NotEqual(uint? obj, uint value, string name, string msg = null) {
        if (obj == value) throw CreateNotEqualException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
    public static void NotEqual(long? obj, long value, string name, string msg = null) {
        if (obj == value) throw CreateNotEqualException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value may not be equal to <paramref name="value"/>.</exception>
    public static void NotEqual(ulong? obj, ulong value, string name, string msg = null) {
        if (obj == value) throw CreateNotEqualException(name, msg, value);
    }

    private static ArgumentException CreateNotEqualException<T>(string name, string? msg, T value)
        => new(msg ?? $"Value may not be equal to {value}.", name);

    #endregion

    #region AtLeast

    /// <exception cref="ArgumentException">Value must be at least <paramref name="value"/>.</exception>
    public static void AtLeast(sbyte obj, sbyte value, string name, string msg = null) {
        if (obj < value) throw CreateAtLeastException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value must be at least <paramref name="value"/>.</exception>
    public static void AtLeast(byte obj, byte value, string name, string msg = null) {
        if (obj < value) throw CreateAtLeastException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value must be at least <paramref name="value"/>.</exception>
    public static void AtLeast(short obj, short value, string name, string msg = null) {
        if (obj < value) throw CreateAtLeastException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value must be at least <paramref name="value"/>.</exception>
    public static void AtLeast(ushort obj, ushort value, string name, string msg = null) {
        if (obj < value) throw CreateAtLeastException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value must be at least <paramref name="value"/>.</exception>
    public static void AtLeast(int obj, int value, string name, string msg = null) {
        if (obj < value) throw CreateAtLeastException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value must be at least <paramref name="value"/>.</exception>
    public static void AtLeast(uint obj, uint value, string name, string msg = null) {
        if (obj < value) throw CreateAtLeastException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value must be at least <paramref name="value"/>.</exception>
    public static void AtLeast(long obj, long value, string name, string msg = null) {
        if (obj < value) throw CreateAtLeastException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value must be at least <paramref name="value"/>.</exception>
    public static void AtLeast(ulong obj, ulong value, string name, string msg = null) {
        if (obj < value) throw CreateAtLeastException(name, msg, value);
    }

    private static ArgumentException CreateAtLeastException<T>(string name, string? msg, T value)
        => new(msg ?? $"Value must be at least {value}.", name);

    #endregion

    #region GreaterThan

    /// <exception cref="ArgumentException">Value must be greater than <paramref name="value"/>.</exception>
    public static void GreaterThan(sbyte obj, sbyte value, string name, string msg = null) {
        if (obj <= value) throw CreateGreaterThanException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value must be greater than <paramref name="value"/>.</exception>
    public static void GreaterThan(byte obj, byte value, string name, string msg = null) {
        if (obj <= value) throw CreateGreaterThanException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value must be greater than <paramref name="value"/>.</exception>
    public static void GreaterThan(short obj, short value, string name, string msg = null) {
        if (obj <= value) throw CreateGreaterThanException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value must be greater than <paramref name="value"/>.</exception>
    public static void GreaterThan(ushort obj, ushort value, string name, string msg = null) {
        if (obj <= value) throw CreateGreaterThanException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value must be greater than <paramref name="value"/>.</exception>
    public static void GreaterThan(int obj, int value, string name, string msg = null) {
        if (obj <= value) throw CreateGreaterThanException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value must be greater than <paramref name="value"/>.</exception>
    public static void GreaterThan(uint obj, uint value, string name, string msg = null) {
        if (obj <= value) throw CreateGreaterThanException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value must be greater than <paramref name="value"/>.</exception>
    public static void GreaterThan(long obj, long value, string name, string msg = null) {
        if (obj <= value) throw CreateGreaterThanException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value must be greater than <paramref name="value"/>.</exception>
    public static void GreaterThan(ulong obj, ulong value, string name, string msg = null) {
        if (obj <= value) throw CreateGreaterThanException(name, msg, value);
    }

    private static ArgumentException CreateGreaterThanException<T>(string name, string? msg, T value)
        => new(msg ?? $"Value must be greater than or equals {value}.", name);

    #endregion

    #region AtMost

    /// <exception cref="ArgumentException">Value must be at most <paramref name="value"/>.</exception>
    public static void AtMost(sbyte obj, sbyte value, string name, string msg = null) {
        if (obj > value) throw CreateAtMostException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value must be at most <paramref name="value"/>.</exception>
    public static void AtMost(byte obj, byte value, string name, string msg = null) {
        if (obj > value) throw CreateAtMostException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value must be at most <paramref name="value"/>.</exception>
    public static void AtMost(short obj, short value, string name, string msg = null) {
        if (obj > value) throw CreateAtMostException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value must be at most <paramref name="value"/>.</exception>
    public static void AtMost(ushort obj, ushort value, string name, string msg = null) {
        if (obj > value) throw CreateAtMostException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value must be at most <paramref name="value"/>.</exception>
    public static void AtMost(int obj, int value, string name, string msg = null) {
        if (obj > value) throw CreateAtMostException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value must be at most <paramref name="value"/>.</exception>
    public static void AtMost(uint obj, uint value, string name, string msg = null) {
        if (obj > value) throw CreateAtMostException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value must be at most <paramref name="value"/>.</exception>
    public static void AtMost(long obj, long value, string name, string msg = null) {
        if (obj > value) throw CreateAtMostException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value must be at most <paramref name="value"/>.</exception>
    public static void AtMost(ulong obj, ulong value, string name, string msg = null) {
        if (obj > value) throw CreateAtMostException(name, msg, value);
    }

    private static ArgumentException CreateAtMostException<T>(string name, string? msg, T value)
        => new(msg ?? $"Value must be at most {value}.", name);

    #endregion

    #region LessThan

    /// <exception cref="ArgumentException">Value must be less than <paramref name="value"/>.</exception>
    public static void LessThan(sbyte obj, sbyte value, string name, string msg = null) {
        if (obj >= value) throw CreateLessThanException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value must be less than <paramref name="value"/>.</exception>
    public static void LessThan(byte obj, byte value, string name, string msg = null) {
        if (obj >= value) throw CreateLessThanException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value must be less than <paramref name="value"/>.</exception>
    public static void LessThan(short obj, short value, string name, string msg = null) {
        if (obj >= value) throw CreateLessThanException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value must be less than <paramref name="value"/>.</exception>
    public static void LessThan(ushort obj, ushort value, string name, string msg = null) {
        if (obj >= value) throw CreateLessThanException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value must be less than <paramref name="value"/>.</exception>
    public static void LessThan(int obj, int value, string name, string msg = null) {
        if (obj >= value) throw CreateLessThanException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value must be less than <paramref name="value"/>.</exception>
    public static void LessThan(uint obj, uint value, string name, string msg = null) {
        if (obj >= value) throw CreateLessThanException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value must be less than <paramref name="value"/>.</exception>
    public static void LessThan(long obj, long value, string name, string msg = null) {
        if (obj >= value) throw CreateLessThanException(name, msg, value);
    }

    /// <exception cref="ArgumentException">Value must be less than <paramref name="value"/>.</exception>
    public static void LessThan(ulong obj, ulong value, string name, string msg = null) {
        if (obj >= value) throw CreateLessThanException(name, msg, value);
    }

    private static ArgumentException CreateLessThanException<T>(string name, string? msg, T value)
        => new(msg ?? $"Value must be less than or equals {value}.", name);

    #endregion

    #endregion

    #region Bulk Delete

    /// <exception cref="ArgumentOutOfRangeException">Messages are younger than 2 weeks.</exception>
    public static void YoungerThanTwoWeeks(IEnumerable<ulong> collection, string name) {
        var minimum = (ulong)DateTimeOffset.UtcNow.Subtract(TimeSpan.FromDays(14)).ToUnixTimeSeconds();
        if (collection.Where(t => t != 0).Any(t => t <= minimum)) {
            throw new ArgumentOutOfRangeException(name, "Messages must be younger than two weeks old.");
        }
    }

    /// <exception cref="ArgumentException">The everyone role cannot be assigned to a user.</exception>
    public static void NotEveryoneRole(IEnumerable<ulong> roles, ulong guildId, string name) {
        if (roles.Any(t => t == guildId)) {
            throw new ArgumentException("The everyone role cannot be assigned to a user.", name);
        }
    }

    #endregion
}