using System;

namespace GH.Utils
{
    public class ProgressHelper
    {
        public static string ProcessedText(int processed, int total)
        {
            return string.Format("{0:n0} из {1:n0}", processed, total);
        }

        public static string RemainingText(DateTime processStarted, int processed, int total)
        {
            int secondsRemaining = 0;
            int totalSecond = (int)(DateTime.Now - processStarted).TotalSeconds;

            if (totalSecond > 0)
            {
                int itemsPerSecond = processed / totalSecond;

                if (itemsPerSecond > 0)
                    secondsRemaining = (total - processed) * itemsPerSecond;
            }

            return TimeSpan.FromMilliseconds(secondsRemaining).ToString(@"hh\:mm\:ss");

            //return new TimeSpan(0, 0, secondsRemaining).ToString(@"hh\:mm\:ss");
        }

        public static string DurationText(DateTime processStarted)
        {
            return TimeSpan.FromTicks(DateTime.Now.Subtract(processStarted).Ticks).ToString(@"hh\:mm\:ss");
        }

    }
}
