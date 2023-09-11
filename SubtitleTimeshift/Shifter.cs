using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleTimeshift
{
    public class Shifter
    {
        async static public Task Shift(Stream input, Stream output, TimeSpan timeSpan, Encoding encoding, int bufferSize = 1024, bool leaveOpen = false)
        {
            StreamReader reader = new StreamReader(input, encoding, true, bufferSize);
            StreamWriter writer = new StreamWriter(output, encoding, bufferSize, leaveOpen);
            int lineNumber;

            while (int.TryParse(reader.ReadLine(), out lineNumber))
            {
                string[] interval = reader.ReadLine().Split(" --> ", 2);
                List<string> textLines = new List<string>();
                string text = reader.ReadLine(); //reads the first line for the current subtitle, even if it's empty
                textLines.Add(text);
                text = reader.ReadLine();
                while (!string.IsNullOrWhiteSpace(text)) //reads non empty lines for the current subtitle
                {
                    textLines.Add(text);
                    text = reader.ReadLine();
                }
                string startString = interval[0].Replace(',', '.');
                string endString = interval[1].Replace(',', '.');
                TimeSpan start = TimeSpan.Parse(startString) + timeSpan;
                TimeSpan end = TimeSpan.Parse(endString) + timeSpan;
                Subtitle subtitle = new Subtitle(lineNumber, start, end, textLines);
                writer.Write(subtitle.ToString());
                await writer.FlushAsync();
            }
        }
    }
}
