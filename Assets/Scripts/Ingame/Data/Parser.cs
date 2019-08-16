using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


static public class Parser
{
    public static List<List<Note>> ParseString(string s)
    {
        var lines_ = s.Replace("\r","").Split('\n');
        var lines = new List<string>(lines_);
        var l1 = lines[0].Split('\t');
        int channelcnt = l1.Length - 1;
        List<List<Note>> channels = new List<List<Note>>(l1.Length - 1);
        
        Debug.Log(l1);
        lines.RemoveAt(0);

        for (int i = 0; i < channelcnt; i++) channels.Add(new List<Note>());
            foreach (var line in lines)
        {
            if (Regex.Match(line, "^[\\s]*$").Success) // if blank, ignore
                continue;
            var line_ = line.Split('\t');
            int timing = int.Parse(line_[0]);
            for (int i = 0; i < channelcnt; i++)
            {
                if (Regex.Match(line_[i + 1], "^[\\s]*$").Success) // if blank, ignore
                    continue;
                channels[i].Add(new Note() { timing = timing, charactor = line_[i + 1] });
            }
        }
        return channels;
    }
}