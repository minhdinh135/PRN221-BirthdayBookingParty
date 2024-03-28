public class BookingValidation
{
    public static bool IsPartyDateTimeAfterTwoDays(DateTime partyDateTime)
    {
        return partyDateTime > DateTime.Now.AddDays(2);
    }

    public static bool IsPartyDateTimeWithinSixMonths(DateTime partyDateTime)
    {
        return partyDateTime < DateTime.Now.AddMonths(6);
    }

    public static bool IsPartyDateInWorkingHours(DateTime partyDateTime)
    {
        return !(partyDateTime.TimeOfDay >= TimeSpan.Zero && partyDateTime.TimeOfDay <= TimeSpan.FromHours(6));
    }
	public static bool IsEndTimeValid(DateTime partyDateTime, DateTime endTime)
	{
		DateTime minEndTime = partyDateTime.AddHours(1);
        
		return endTime >= minEndTime && endTime.Date == partyDateTime.Date;
	}
}
