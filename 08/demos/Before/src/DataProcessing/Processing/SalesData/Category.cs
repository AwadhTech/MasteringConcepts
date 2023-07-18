using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace DataProcessing;

internal readonly struct Category : IEquatable<Category>
{
    // language=Regex
    private const string CategoryPattern = @"^\s*([a-zA-Z]{3}[0-3]{3}):\s*(.+?)\s*$";
    private static readonly Regex _categoryRegex = new(CategoryPattern, RegexOptions.Compiled, TimeSpan.FromSeconds(3));
    
    public static Category Empty = new(string.Empty, string.Empty);

    public Category(string code, string description) => (Code, Description) = (code, description);

    public string Code { get; }

    public string Description { get; }

    public static bool TryParse(string source, [MaybeNullWhen(false)] out Category category)
    {
        category = Empty;

        if (string.IsNullOrWhiteSpace(source) || source.Length < 8)
            return false;

        var colonIndex = source.IndexOf(':');
        source = source.Trim();

        if (colonIndex > -1)
        {
            var categoryCode = source[..6].ToUpperInvariant();

            for (var index = 0; index < categoryCode.Length; index++)
            {
                if (index < 3 && !char.IsLetter(source, index))
                    return false;

                var character = categoryCode[index];
                if (index >= 3 &&
                    character != '0' && 
                    character != '1' &&
                    character != '2' &&
                    character != '3')
                {
                    return false;
                }
            }

            var categoryDescription = source.Substring(colonIndex + 1).TrimStart();
            category = new Category(categoryCode, categoryDescription);
            return true;
        }

        return false;
    }

    public static bool TryParseUsingRegex(string source, [MaybeNullWhen(false)] out Category category)
    {
        category = Empty;
        if (string.IsNullOrEmpty(source))
        {
            return false;
        }

        var match = _categoryRegex.Match(source);
        if (match.Success && match.Groups.Count == 3)
        {
            // first group represents the entire match
            // second group represents the first match before :
            // third group represents match after :

            var categoryCode = match.Groups[1].Value.ToUpperInvariant();
            var description = match.Groups[2].Value;

            category = new Category(categoryCode, description);
            return true;
        }
        return false;
    }

    public override bool Equals(object? obj) => obj is Category category && Equals(category);

    public bool Equals(Category other) => Code == other.Code && Description == other.Description;

    public override int GetHashCode() => HashCode.Combine(Code, Description);

    public static bool operator ==(Category left, Category right) => left.Equals(right);

    public static bool operator !=(Category left, Category right) => !(left == right);
}
