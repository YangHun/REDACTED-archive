using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

static public class Parser
{
    public static float lastBpm { get; private set; }
    public static List<List<Note>> ParseString(string s)
    {
        string[] lines = s.Split(
            new[] { "\r\n", "\r", "\n" },
            StringSplitOptions.None
        );
        int channels = Int32.Parse(lines[0].Split(' ')[1]);
        string noteLengthStr = lines[1].Split(' ')[1]; 
        float noteLength = (float)Int32.Parse(noteLengthStr.Split('/')[0]) / (float)Int32.Parse(noteLengthStr.Split('/')[1]);
        int bpm = Int32.Parse(lines[2].Split(' ')[1]);
        lastBpm = bpm;
        int lineLen = lines.Length;
        int lastTime = 0;

        List<List<Note>> notes = new List<List<Note>>();
        for (int channel = 0; channel < channels; channel++)
        {
            notes.Add(new List<Note>());
        }
        
        for (int i = 4; i < lineLen; i++)
        {
            int channel = (i - 4) % (channels + 1);
            if (channel == channels)
            {
                continue;
            }
            int time = lastTime;
            
            bool isInSubtime = false;
            int subTime = 0;
            List<Note> tempNotes = new List<Note>();
            foreach (char c in lines[i])
            {
                switch (c)
                {
                    case ' ':
                        break;
                    case '-':
                    case 'ㅡ':
                        if (isInSubtime) subTime++;
                        else time++;
                        break;
                    case '|':
                        break;
                    case '(':
                        isInSubtime = true;
                        break;
                    case ')':
                        isInSubtime = false;
                        foreach (Note note in tempNotes)
                        {
                            notes[channel].Add(new Note
                            {
                                timing = (int)((60000.0f / bpm * 4) * noteLength * (time + (float)note.timing / subTime)), 
                                charactor = note.charactor
                            });
                        }
                        tempNotes.Clear();
                        subTime = 0;
                        time++;
                        break;
                    default:
                        // read hangul char
                        if (isInSubtime)
                        {
                            tempNotes.Add(new Note {timing = subTime, charactor = c.ToString()});
                            subTime++;
                        }
                        else
                        {
                            notes[channel].Add(new Note
                            {
                                timing = (int)((60000.0f / bpm * 4) * noteLength * time),
                                charactor = c.ToString()
                            });
                            time++;
                        }
                        break;
                }
            }

            if (channel == channels - 1)
            {
                lastTime = time;
            }
        }
        
        return notes;
    }
}