namespace GameCreator.Core.Math
{
    using System;
    using System.IO;
    using System.Text;
    using System.Globalization;

    public class Tokenizer
    {
        public enum Token
        {
            EOF,
            Add,
            Subtract,
            Multiply,
            Divide,
            OpenParens,
            CloseParens,
            Number,
        }

        private static readonly CultureInfo CULTURE = new CultureInfo("en-US");

        // PROPERTIES: ----------------------------------------------------------------------------

        private TextReader reader;

        public char currentCharacter { get; private set; }
        public Token currentToken    { get; private set; }
        public float number          { get; private set; }
        public string identifier     { get; private set; }

        // INITIALIZER: ---------------------------------------------------------------------------

        public Tokenizer(string expression)
        {
            this.reader = new StringReader(expression);

            this.NextChar();
            this.NextToken();
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        void NextChar()
        {
            int character = reader.Read();
            this.currentCharacter = character < 0 ? '\0' : (char)character;
        }

        public void NextToken()
        {
            while (char.IsWhiteSpace(this.currentCharacter)) this.NextChar();
            switch (this.currentCharacter)
            {
                case '\0':
                    this.currentToken = Token.EOF;
                    return;

                case '+':
                    this.NextChar();
                    this.currentToken = Token.Add;
                    return;

                case '-':
                    this.NextChar();
                    this.currentToken = Token.Subtract;
                    return;

                case '*':
                    this.NextChar();
                    this.currentToken = Token.Multiply;
                    return;

                case '/':
                    this.NextChar();
                    this.currentToken = Token.Divide;
                    return;

                case '(':
                    this.NextChar();
                    this.currentToken = Token.OpenParens;
                    return;

                case ')':
                    this.NextChar();
                    this.currentToken = Token.CloseParens;
                    return;
            }

            if (char.IsDigit(this.currentCharacter) || this.currentCharacter == '.')
            {
                var sb = new StringBuilder();
                bool hasFloatingPoint = false;
                while (char.IsDigit(this.currentCharacter) || (!hasFloatingPoint && this.currentCharacter == '.'))
                {
                    sb.Append(this.currentCharacter);
                    hasFloatingPoint = this.currentCharacter == '.';
                    this.NextChar();
                }

                this.number = float.Parse(sb.ToString(), CULTURE);
                this.currentToken = Token.Number;
                return;
            }

            throw new Exception(string.Format("Unexpected character: {0}", this.currentCharacter));
        }
    }
}