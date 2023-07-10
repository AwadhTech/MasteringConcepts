# Notes.
### Introduction to strings
- strings  are basically a sequence of characters.
- Built-in type in C#
- Their design is optimized for peformance
- Are reference types stored in the heap
- Have value based equality semantics

### Immutability of string
- Means its internal state cannot be modified after initialization
- String are immutable and once created cannot be modified
- character buffer cannot grow.
- Methods and operators return a new string
- Strings are Threads safe by default.

### Character Encoding
- Encoding is the means which characters are mapped to bytes of binary data stored in memory, on disk
  or transmitted over a network.
- #### Encoding Standards
    - ###### Ascii
        - American Standard for Information Interchange
        - uses 7 bits to encode upto 128 characters
        - Limited to only English Characters
        - used for HTTP/1 request line and headers
    - ###### Unicode
        - uses 16-bit character mode
    - ###### Unicode Transformation Format(UTF)
        - Encodes Unicode Code points to and from binary data
        - .Net uses UTF-16 encoding for string
        - Requires atleast two bytes per character
        - **UTF-8** can encode all Unicode code points
        - lengths varies between 1 and 4 bytes
        - 1 byte: Sufficent for US-Ascii
        - 2 bytes : supports an extra 1,920 characters
        - 3 bytes: Suffient for the whole BMP Range
        - 4 bytes: Supports the supplementary range
        - For most text, UTF-8 results in less data to transmit vs UTF-16
      ###### System.Text.Encoding Classes
        - Includes classes to work with different encoding schemes
        - Encording classes are accessible from static Properties(ASCII, UTF-8)
        - Encode characters/string to bytes
        - Decode bytes back to char
### Introduction to Regular Expressions
-   Tool for parsing complex strings.
- Formed by a sequence of characters
