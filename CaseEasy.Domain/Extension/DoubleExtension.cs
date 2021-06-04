namespace CaseEasy.Domain.Extension
{
    public static class DoubleExtension
    {
        public static double SubtractPercent(this double amount, double percent)
        {
            return amount - amount.Percent(percent);
        }

        public static double Percent(this double amount, double percent)
        {
            return amount * (percent / 100);
        }
    }
}
