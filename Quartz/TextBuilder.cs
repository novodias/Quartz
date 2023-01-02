using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz
{
    internal class TextBuilder
    {
        private StringBuilder _stringBuilder = new();
        private string? _lastTextAppended = null;

        public event EventHandler<TextEventArgs>? TextChanged = null;
        public TextBuilder() { }

        public void AppendLine(string text)
        {
            _lastTextAppended = text;
            _stringBuilder.AppendLine(text);

            if (TextChanged is not null)
            {
                TextChanged.Invoke(this, new TextEventArgs(text));
            }
        }

        public IReadOnlyCollection<string> GetText() 
        {
            if (_stringBuilder.Length <= 0)
            {
                return Array.Empty<string>();
            }

            return _stringBuilder.ToString().Split(Environment.NewLine);
        }

        public async Task<bool> WaitForKeyword(int seconds, params string[] keywords)
        {
            if (_lastTextAppended is null)
            {
                throw new Exception();
            }

            var dateTime = DateTime.UtcNow.AddSeconds(seconds);

            while (true) 
            {
                if (_lastTextAppended is null)
                {
                    break;
                }

                if (keywords.Any(k => _lastTextAppended.ToLower().Contains(k.ToLower())))
                {
                    return true;
                }

                await Task.Delay(200);

                if (DateTime.UtcNow.CompareTo(dateTime) >= 0)
                {
                    break;
                }
            }

            return false;
        }
    }

    internal class TextEventArgs : EventArgs
    {
        public string Text;
        
        public TextEventArgs(string text) 
        {
            Text = text;
        }
    }
}
