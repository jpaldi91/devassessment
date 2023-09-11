using System;
using System.Collections.Generic;

namespace SubtitleTimeshift;

public class Subtitle
{
  public Subtitle(int lineNumber, TimeSpan start, TimeSpan end, List<string> text)
  {
    LineNumber = lineNumber;
    Start = start;
    End = end;
    Text = text;
  }
  public int LineNumber { get; set; }
  public TimeSpan Start { get; set; }
  public TimeSpan End { get; set; }
  public List<string> Text { get; set; }

  public override string ToString()
  {
    List<string> lines = new List<string>();
    string timeSpanFormat = @"hh\:mm\:ss\.fff";
    string startString = Start.ToString(timeSpanFormat);
    string endString = End.ToString(timeSpanFormat);

    lines.Add(LineNumber.ToString());
    lines.Add($"{startString} --> {endString}");
    lines.AddRange(Text);
    lines.Add(Environment.NewLine);

    return string.Join(Environment.NewLine, lines);
  }
}