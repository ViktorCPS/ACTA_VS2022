using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace LRRremote
{
    public class TagsTimes
    {
        const int MAX_ENTRIES = 32000;
        public string[] tagID;
        public string[] detectionTime;
        public string[] name;
        public string[] TiDt;
        public string[] lastInsertionAttemptTime;
        public bool[] paired;
        public int noEntries;

        public TagsTimes()
        {
            tagID = new string[MAX_ENTRIES];
            detectionTime = new string[MAX_ENTRIES];
            name = new string[MAX_ENTRIES];
            TiDt = new string[MAX_ENTRIES];
            lastInsertionAttemptTime = new string[MAX_ENTRIES];
            paired = new bool[MAX_ENTRIES];
            noEntries = 0;
        }

        // add entry unconditionally
        public void AddEntry(string dt, string ti)
        {
            if (noEntries >= MAX_ENTRIES) return;
            tagID[noEntries] = ti;
            detectionTime[noEntries] = dt;
            TiDt[noEntries] = ti + dt;
            name[noEntries] = "";
            lastInsertionAttemptTime[noEntries] = dt;
            paired[noEntries] = false;
            noEntries++;
        }

        // add entry with debouncing
        public bool AddEntry(string dt, string ti, int debouncingPeriod)
        {
            if (noEntries == 0)
            {
                AddEntry(dt, ti);
                return true;
            }
            else if (debouncingPeriod == -1) // insert only if the previous entry is not the same
            {
                if (!((detectionTime[noEntries - 1] == dt) && (tagID[noEntries - 1] == ti)))
                {
                    AddEntry(dt, ti);
                    return true;
                }
            }
            else
            {
                int position = Array.LastIndexOf(tagID, ti, noEntries);
                //if ((position == -1) || (SecondsDiff(dt, detectionTime[position]) > debouncingPeriod))
                if ((position == -1) || (SecondsDiff(dt, lastInsertionAttemptTime[position]) > debouncingPeriod))
                {
                    AddEntry(dt, ti);
                    return true;
                }
                else
                {
                    if (position != -1) lastInsertionAttemptTime[position] = dt;
                }
            }
            return false;
        }

        public TagsTimes Debounced(int debouncingPeriod)
        {
            try
            {
                TagsTimes tagsTimesDebounced = new TagsTimes();
                for (int i = 0; i < this.noEntries; i++) tagsTimesDebounced.AddEntry(this.detectionTime[i], this.tagID[i], debouncingPeriod);
                return tagsTimesDebounced;
            }
            catch
            {
                return this;
            }
        }

        public string GetPair(string tagToPairID, string tagTime, int pairInterval)
        {
            string lastPositionAndTime = GetLastPositionAndTime(tagToPairID);
            if (lastPositionAndTime == "-1;00:00:00") return String.Empty;
            else
            {
                int position = Int32.Parse(lastPositionAndTime.Substring(0, lastPositionAndTime.IndexOf(";")));
                if (paired[position]) return String.Empty;
                string lastTagTime = lastPositionAndTime.Substring(lastPositionAndTime.IndexOf(";") + 1);
                if (SecondsDiff(tagTime, lastTagTime) > pairInterval) return String.Empty;
                else
                {
                    paired[position] = true;
                    return lastTagTime;
                }
            }
        }

        private int SecondsDiff(string t1, string t2)
        {
            TimeSpan ts1 = new TimeSpan(Int32.Parse(t1.Substring(0, 2)), Int32.Parse(t1.Substring(3, 2)), Int32.Parse(t1.Substring(6, 2)));
            TimeSpan ts2 = new TimeSpan(Int32.Parse(t2.Substring(0, 2)), Int32.Parse(t2.Substring(3, 2)), Int32.Parse(t2.Substring(6, 2)));
            int seconds = (int)(ts1.TotalSeconds - ts2.TotalSeconds); if (seconds < 0) seconds += 24 * 60 * 60;
            return seconds;
        }

        public string GetLastPositionAndTime(string ti)
        {
            try
            {
                int position = 0;
                for (position = noEntries - 1; position >= 0; position--) if (tagID[position] == ti) break;
                if (position == -1) return "-1;00:00:00"; return position.ToString() + ";" + detectionTime[position];
            }
            catch { return "-1;00:00:00"; }
        }

        public void FromFile(string fileName)
        {
            int noTries = 0;
            while (noTries < 3)
            {
                try
                {
                    string[] lines = File.ReadAllLines(fileName);
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string[] fields = lines[i].Split(' ');
                        AddEntry(fields[0].Trim(), fields[1].Trim());
                    }
                    return;
                }
                catch { Thread.Sleep(500); noTries++; }
            }
        }

        public void ToFile(string fileName)
        {
            int noTries = 0;
            while (noTries < 3)
            {
                try
                {
                    string[] lines = new string[noEntries];
                    for (int i = 0; i < noEntries; i++)
                    {
                        lines[i] = detectionTime[i] + " " + tagID[i];
                    }
                    File.WriteAllLines(fileName, lines);
                    return;
                }
                catch { Thread.Sleep(500); noTries++; }
            }
        }

        public string[] ToTagLines()
        {
            string[] lines = new string[noEntries];
            for (int i = 0; i < noEntries; i++) lines[i] = detectionTime[i] + " > " + tagID[i];
            return lines;
        }

        public string[] ToVisitorLines()
        {
            string[] lines = new string[noEntries];
            for (int i = 0; i < noEntries; i++) lines[i] = name[i].PadRight(25) + "   " + detectionTime[i];
            return lines;
        }

        public string ToCSVtext()
        {
            string lines = "";
            for (int i = 0; i < noEntries; i++) lines += tagID[i] + "," + name[i] + "," + detectionTime[i].PadLeft(8, '0') + Environment.NewLine;
            return lines;
        }

        public void SortByTagID()
        {
            string[] tagIDdetectionTime = new string[MAX_ENTRIES];
            TiDt.CopyTo(tagIDdetectionTime, 0); Array.Sort(tagIDdetectionTime, tagID, 0, noEntries);
            TiDt.CopyTo(tagIDdetectionTime, 0); Array.Sort(tagIDdetectionTime, detectionTime, 0, noEntries);
            TiDt.CopyTo(tagIDdetectionTime, 0); Array.Sort(tagIDdetectionTime, name, 0, noEntries);
            Array.Sort(TiDt);
        }

        public void SortByDetectionTime()
        {
            string[] dt = new string[MAX_ENTRIES];
            detectionTime.CopyTo(dt, 0); Array.Sort(dt, tagID, 0, noEntries);
            detectionTime.CopyTo(dt, 0); Array.Sort(dt, name, 0, noEntries);
            detectionTime.CopyTo(dt, 0); Array.Sort(dt, TiDt, 0, noEntries);
            Array.Sort(detectionTime);
        }
    }
}
