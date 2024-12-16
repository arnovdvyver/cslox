
using CSLox.Constants;

namespace CSLox.Utilities;

public class Scanner
{
    private string _source;
    private int _start = 0;
    private int _current = 0;
    private int _line = 1;
    private List<Token> _tokens = new List<Token>();

    public Scanner(string source)
    {
        // strip out any whitespace
        _source = new String(source
            .ToCharArray()
            .Where(x => x != 32)
            .ToArray()
        );
    }

    public List<Token> ScanTokens()
    {
        while (!EndOfSource())
        {
            _start = _current; 
            ScanToken();
        }

        // add EOF token
        _tokens.Add(new Token
        {
            Type = TokenType.Eof,
            Lexeme = "",
            Literal = null,
            Line = _line
        });

        return _tokens; 
    }

    private void ScanToken()
    {
        char c = Advance();

        switch (c)
        {
            // single-character tokens
            case '(':
                AddToken(TokenType.LeftParen, null); break;

            case ')':
                AddToken(TokenType.RightParen, null); break;

            case '{':
                AddToken(TokenType.LeftBrace, null); break;

            case '}':
                AddToken(TokenType.RightBrace, null); break;

            case ',':
                AddToken(TokenType.Comma, null); break;

            case '.':
                AddToken(TokenType.Dot, null); break;

            case '-':
                AddToken(TokenType.Minus, null); break;

            case '+':
                AddToken(TokenType.Plus, null); break;

            case ';':
                AddToken(TokenType.Semicolon, null); break;

            case '*':
                AddToken(TokenType.Star, null); break;

            // composite tokens
            case '!':
                AddToken(Match('=') ? TokenType.BangEqual : TokenType.Bang, null);
                break;

            case '=':
                AddToken(Match('=') ? TokenType.EqualEqual : TokenType.Equal, null);
                break;

            case '<':
                AddToken(Match('=') ? TokenType.LessEqual : TokenType.Less, null);
                break;

            case '>':
                AddToken(Match('=') ? TokenType.GreaterEqual : TokenType.Greater, null);
                break;

            // divide is a special case since its used for comments
            case '/':
                if (Match('/'))
                {
                    while (Peek() != '\n' && !EndOfSource())
                    {
                        Advance();
                    }
                }
                else
                {
                    AddToken(TokenType.Slash, null);
                }
                break;

            // escape characters
            case ' ':
            case '\r':
            case '\t':
                break;

            // newline
            case '\n':
                _line++;
                break;

            // literals
            case '"':
                StringLiteral();
                break;

            default:
                Lox.Error(_line, "Unexpected character"); break;
        }
    }

    private bool EndOfSource() => _current >= _source.Count();

    private char Advance()
    {
        _current++;
        return _source[_current - 1];
    }

    private void AddToken(TokenType type, object? literal)
    {
        string text = _source.Substring(_start, _current - _start); 
        _tokens.Add(new Token()
        {
            Type = type,
            Lexeme = text,
            Literal = literal,
            Line = _line
        });
    }

    private bool Match(char expected)
    {
        if (EndOfSource() || _source[_current] != expected)
        {
            return false;
        }

        _current++; // we do an 'early' advance here to skip over a expected char
        return true;
    }

    private char Peek()
    {
        if (EndOfSource())
        {
            return '\0'; // return that we have reached the end of the line
        }

        return _source[_current]; // keep advancing 
    }

    private void StringLiteral()
    {
        _current++;
    }
}
