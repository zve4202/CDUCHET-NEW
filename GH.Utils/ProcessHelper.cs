using System;

namespace GH.Utils
{
    public class ProcessHelper
    {
        public static string ProcessedText(int processed, int total)
        {
            return string.Format("Обработано: {0} из {1}", processed, total);
        }

        public static string RemainingText(DateTime processStarted, int totalElements, int processedElements)
        {
            int secondsRemaining = 0;
            int totalSecond = (int)(DateTime.Now - processStarted).TotalSeconds;

            if (totalSecond > 0)
            {
                int itemsPerSecond = processedElements / totalSecond;

                if (itemsPerSecond > 0)
                    secondsRemaining = (totalElements - processedElements) / itemsPerSecond;
            }

            return string.Format("Оставшееся время обработки: {0}", new TimeSpan(0, 0, secondsRemaining).ToString(@"hh\:mm\:ss"));
        }

        public static string DurationText(DateTime processStarted)
        {
            return string.Format("Время с начала обработки: {0}", TimeSpan.FromTicks(DateTime.Now.Subtract(processStarted).Ticks).ToString(@"hh\:mm\:ss"));
        }

    }
}
