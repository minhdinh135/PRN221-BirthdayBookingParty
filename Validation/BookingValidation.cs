using System;

public class BookingValidation
{

    public static bool IsAfterCurentDateTime(DateTime partyDateTime)
    {
        // Rule 2: Minimum Date
        //DateTime minimumDate = DateTime.Now.AddDays(1); // Example: Minimum date is tomorrow
        //if (PartyDateTime < minimumDate)
        //{
        //    isValid = false;
        //}

        //// Rule 3: Maximum Date
        //DateTime maximumDate = DateTime.Now.AddMonths(6); // Example: Maximum date is 6 months from now
        //if (PartyDateTime > maximumDate)
        //{
        //    isValid = false;
        //}
        return partyDateTime > DateTime.Now;
    }
    public static bool IsPartyDateTimeValid(DateTime partyDateTime)
    {
        return partyDateTime > DateTime.Now.AddDays(2);
    }
}
