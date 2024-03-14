using System;

public class BookingValidation
{

        //// Rule 3: Maximum Date
        //DateTime maximumDate = DateTime.Now.AddMonths(6); // Example: Maximum date is 6 months from now
        //if (PartyDateTime > maximumDate)
        //{
        //    isValid = false;
        //}
    public static bool IsPartyDateTimeAfterTwoDays(DateTime partyDateTime)
    {
        return partyDateTime > DateTime.Now.AddDays(2);
    }

    public static bool IsPartyDateTimeWithinSixMonths(DateTime partyDateTime)
    {
        return partyDateTime < DateTime.Now.AddMonths(6);
    }
}
