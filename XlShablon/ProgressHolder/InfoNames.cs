namespace GH.XlShablon
{
    public enum InfoNames
    {
        [Iinfo("Информация", nameof(ProgressHolder.Message))]
        Info,
        [Iinfo("Прогресс", nameof(ProgressHolder.Progress))]
        Progress,
        [Iinfo("Длительность", nameof(ProgressHolder.Duration))]
        Duration,
        [Iinfo("Осталось", nameof(ProgressHolder.Remaining))]
        Remaining,
        [Iinfo("Отчёт", nameof(ProgressHolder.Summary))]
        Summary
    }
}
