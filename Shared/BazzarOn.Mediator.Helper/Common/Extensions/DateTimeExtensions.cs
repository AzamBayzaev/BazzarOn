namespace BazzarOn.Mediator.Helper.Common.Extensions;

public static class DateTimeExtensions
{
    public static DateTime GetUtcDateTimeNow(this TimeProvider timeProvider)
        => timeProvider.GetUtcNow().UtcDateTime;
}
