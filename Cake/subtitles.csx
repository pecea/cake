using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System;

private const string subtitleFileName = @"subSample.srt";

new VoidJob("Subtitles").Does(() =>
{
Regex unit = new Regex(
   @"(?<sequence>\d+)\r\n(?<start>\d{2}\:\d{2}\:\d{2},\d{3}) --\> " + 
   @"(?<end>\d{2}\:\d{2}\:\d{2},\d{3})\r\n(?<text>[\s\S]*?\r\n\r\n)", 
   RegexOptions.Compiled | RegexOptions.ECMAScript);
double offset = 0;
Console.Write("offset, in seconds (+1,1, -2,75): ");
while (!Double.TryParse(Console.ReadLine(), out offset))
{
    Console.WriteLine("Invalid value, try again");
}

using (StreamReader input = new StreamReader(subtitleFileName, Encoding.Default))
{
    using (StreamWriter output = 
       new StreamWriter(subtitleFileName, false, Encoding.Default))
    {
        output.Write(
            unit.Replace(input.ReadToEnd(), delegate(Match m)
            {
			
	var seq = m.Groups["sequence"].Value;
                return m.Value.Replace(
                    String.Format("{0}\r\n{1} --> {2}\r\n",
                        m.Groups["sequence"].Value,
                        m.Groups["start"   ].Value,
                        m.Groups["end"     ].Value),
                    String.Format(
                        "{0}\r\n{1:HH\\:mm\\:ss\\,fff} --> " + 
                        "{2:HH\\:mm\\:ss\\,fff}\r\n",
                        int.Parse(seq),
                        DateTime.Parse(m.Groups["start"].Value.Replace(",","."))
                                .AddSeconds(offset),
                        DateTime.Parse(m.Groups["end"].Value.Replace(",","."))
                                .AddSeconds(offset)));
            }));
    }
}
});

JobManager.SetDefault("Subtitles");