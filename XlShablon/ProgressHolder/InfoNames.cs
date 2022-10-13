namespace GH.XlShablon
{
    public enum InfoNames
    {
        [Iinfo("Информация", nameof(ProgressHolder.Info))]
        Info,
        [Iinfo("Текущий прогресс", nameof(ProgressHolder.Progress))]
        Progress,
        [Iinfo("Прошло с начала", nameof(ProgressHolder.Duration))]
        Duration,
        [Iinfo("Осталось до конца", nameof(ProgressHolder.Remaining))]
        Remaining,
        [Iinfo("Итого", nameof(ProgressHolder.Summary))]
        Summary
    }
}
