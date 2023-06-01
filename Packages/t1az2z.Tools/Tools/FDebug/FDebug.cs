//Based on fancy debug by alicewithalex 

using System.Text;

using UnityEngine;

namespace t1az2z.Tools.FDebug
{
    public static class FDebug
    {
        private const char SPLITTER = '#';

        private static StringBuilder _stringBuilder = new StringBuilder();

        public static void Log(string message, int frameInterval, bool showFrame = true, params FColor[] colors)
        {
            if (Time.frameCount%frameInterval != 0)
                return;
            
            Log(message, showFrame, colors);
        }
        
        public static void Log(string message, params FColor[] colors)
        {
            Log(message, true, colors);
        }
        
        public static void Log(string message, bool showFrame = true, params FColor[] colors)
        {
#if F_DEBUG
            if (showFrame)
            {
                _stringBuilder.Append($"[{Time.frameCount}] ");
            }
            
            if (colors == null || colors.Length == 0)
            {
                _stringBuilder.Append(message);
                Debug.Log(_stringBuilder.ToString());
                _stringBuilder.Clear();
                return;
            }

            int i = 0;

            bool coloring = false;
            foreach (var ch in message)
            {
                if (ch == SPLITTER)
                {
                    if (coloring)
                    {
                        coloring = false;
                        i++;
                        _stringBuilder.Append("</color>");
                    }
                    else
                    {
                        coloring = true;

                        _stringBuilder.Append($"<color=#" +
                            $"{colors[i % colors.Length].ToHex()}>");
                    }

                    continue;
                }

                _stringBuilder.Append(ch);
            }

            Debug.Log(_stringBuilder.ToString());

            _stringBuilder.Clear();
#endif
        }

        public static void Log(string message, char enclosure = '#', params FColor[] colors)
        {
            if (colors == null || colors.Length == 0)
            {
                Debug.Log(message);
                return;
            }
        
            int i = 0;
        
            bool coloring = false;
            foreach (var ch in message)
            {
                if (ch == enclosure)
                {
                    if (coloring)
                    {
                        coloring = false;
                        i++;
                        _stringBuilder.Append("</color>");
                    }
                    else
                    {
                        coloring = true;
        
                        _stringBuilder.Append($"<color=#" +
                            $"{colors[i % colors.Length].ToHex()}>");
                    }
        
                    continue;
                }
        
                _stringBuilder.Append(ch);
            }
        
            Debug.Log(_stringBuilder.ToString());
        
            _stringBuilder.Clear();
        }
    }
}