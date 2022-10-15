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
            int milliSecondsForRemaining = 0;
            int totalMilliseconds = (int)(DateTime.Now - processStarted).TotalMilliseconds;

            if (processed > 0)
            {
                int milliSecondsForItem = totalMilliseconds / processed;

                if (milliSecondsForItem > 0)
                    milliSecondsForRemaining = (total - processed) * milliSecondsForItem;
            }

            return TimeSpan.FromMilliseconds(milliSecondsForRemaining).ToString(@"hh\:mm\:ss");
        }

        public static string DurationText(DateTime processStarted)
        {
            return TimeSpan.FromTicks(DateTime.Now.Subtract(processStarted).Ticks).ToString(@"hh\:mm\:ss");
        }
    }
}
