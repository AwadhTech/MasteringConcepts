using System.Diagnostics.CodeAnalysis;

namespace DataProcessing;

internal readonly struct Category : IEquatable<Category>
{
    public static Category Empty = new(string.Empty, string.Empty);

    public Category(string code, string description) => (Code, Description) = (code, description);

    public string Code { get; }

    public string Description { get; }

    public static bool TryParse(string source, [MaybeNullWhen(false)] out Category category)
    {
        category = Empty;
        var colonIndex = source.IndexOf(':');
        if (colonIndex > -1)
        {
            var categoryCode = source[..6];
            for (var index = 0; index < categoryCode.Length; index++)
            {
                if (index < 3 && !char.IsLetter(source, index))
                    return false;
                var character = categoryCode[index];
                if (index >= 3
                    && character != '0'
                    && character != '1'
                    && character != '3')
                {
                    return false;
                }
            }

            var categoryDescription = source.Substring(colonIndex + 1);
            category = new Category(categoryCode, categoryDescription);
            return true;
        }
        return false;
    }

    public static bool TryParseUsingRegex(string source, [MaybeNullWhen(false)] out Category category)
    {
        category = Empty;
        return false;
    }

    public override bool Equals(object? obj) => obj is Category category && Equals(category);

    public bool Equals(Category other) => Code == other.Code && Description == other.Description;

    public override int GetHashCode() => HashCode.Combine(Code, Description);

    public static bool operator ==(Category left, Category right) => left.Equals(right);

    public static bool operator !=(Category left, Category right) => !(left == right);
}
