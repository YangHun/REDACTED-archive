using System;
using System.Collections.Generic;
using UnityEngine;

public class Song
{
    // Song info (persistent between multiple scenes)
    static public Song currentSong;
    
    public string Name { get; private set; }
    public int NumChannels { get; private set; }
    public int Bpm { get; private set; }
    public List<List<Note>> Channels { get; private set; }
    public AudioClip Clip { get; set; }

    public Song(string name, int numChannels, int bpm, AudioClip clip)
    {
        this.Name = name;
        this.NumChannels = numChannels;
        this.Bpm = bpm;
        this.Clip = clip;

        this.Channels = new List<List<Note>>();
        for (int i = 0; i < numChannels; i++)
        {
            this.Channels.Add(new List<Note>());
        }
    }

    public static Song LoadSong(string songName)
    {
        TextAsset textAsset = Resources.Load<TextAsset>($"Songs/{songName}_notes");
        AudioClip clip = Resources.Load<AudioClip>($"Songs/{songName}");

        string[] lines = textAsset.text.Split(
            new[] {"\r\n", "\r", "\n"},
            StringSplitOptions.None
        );

        int channels = Int32.Parse(lines[0].Split(' ')[1]);
        string noteLengthStr = lines[1].Split(' ')[1];
        float noteLength = (float) Int32.Parse(noteLengthStr.Split('/')[0]) /
                           (float) Int32.Parse(noteLengthStr.Split('/')[1]);
        int bpm = Int32.Parse(lines[2].Split(' ')[1]);
        int lineLen = lines.Length;
        int lastTime = 0;

        Song song = new Song(songName, channels, bpm, clip);

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
                            song.Channels[channel].Add(new Note
                            {
                                timing = (int) ((60000.0f / bpm * 4) * noteLength *
                                                (time + (float) note.timing / subTime)),
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
                            song.Channels[channel].Add(new Note
                            {
                                timing = (int) ((60000.0f / bpm * 4) * noteLength * time),
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

        return song;
    }
}
