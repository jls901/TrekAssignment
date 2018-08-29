using System;
using System.Collections.Generic;
using System.Linq;

public class HouseholdBikesModel 
{
    public IEnumerable<string> bikes { get; set; }

    public override string ToString() 
    {
        if (bikes != null )
        {
            var tempBikes = bikes.OrderBy(x => x);
            return String.Join(',', tempBikes);
        }
        return null; 
    }
}