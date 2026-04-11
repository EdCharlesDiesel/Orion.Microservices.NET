namespace Orion.Helpers.Arrays
{
    public static class CalendarMatchingClass
    {
        // O(c1 + c2) time | O(c1 + c2) space - where c1 and c2 are the respective numbers of meetings in Calendar1 and Calendar2
        public static List<StringMeeting> CalendarMatching(
            List<StringMeeting> calendar1,
            StringMeeting dailyBounds1,
            List<StringMeeting> calendar2,
            StringMeeting dailyBounds2,
            int meetingDuration
        )
        {
            var updatedCalendar1 = UpdateCalendar(calendar1, dailyBounds1);
            var updatedCalendar2 = UpdateCalendar(calendar2, dailyBounds2);

            var mergedCalendar = MergeCalendars(updatedCalendar1, updatedCalendar2);
            var flattenedCalendar = FlattenCalendar(mergedCalendar);

            return GetMatchingAvailabilities(flattenedCalendar, meetingDuration);
        }

        private static List<Meeting> UpdateCalendar(List<StringMeeting> calendar, StringMeeting dailyBounds)
        {
            var updatedCalendar = new List<StringMeeting> { new StringMeeting("0:00", dailyBounds.Start) };
            updatedCalendar.AddRange(calendar);
            updatedCalendar.Add(new StringMeeting(dailyBounds.End, "23:59"));

            return updatedCalendar.Select(entry => new Meeting(TimeToMinutes(entry.Start), TimeToMinutes(entry.End))).ToList();
        }

        private static List<Meeting> MergeCalendars(List<Meeting> calendar1, List<Meeting> calendar2)
        {
            var merged = new List<Meeting>();
            int i = 0, j = 0;

            while (i < calendar1.Count && j < calendar2.Count)
            {
                merged.Add(calendar1[i].Start < calendar2[j].Start ? calendar1[i++] : calendar2[j++]);
            }

            while (i < calendar1.Count) merged.Add(calendar1[i++]);
            while (j < calendar2.Count) merged.Add(calendar2[j++]);

            return merged;
        }

        private static List<Meeting> FlattenCalendar(List<Meeting> calendar)
        {
            var flattened = new List<Meeting> { calendar[0] };

            for (var i = 1; i < calendar.Count; i++)
            {
                var current = calendar[i];
                var previous = flattened[flattened.Count - 1];

                if (previous.End >= current.Start)
                {
                    flattened[flattened.Count - 1] = new Meeting(
                        previous.Start,
                        Math.Max(previous.End, current.End)
                    );
                }
                else
                {
                    flattened.Add(current);
                }
            }
            return flattened;
        }

        private static List<StringMeeting> GetMatchingAvailabilities(List<Meeting> calendar, int meetingDuration)
        {
            var available = new List<StringMeeting>();

            for (var i = 1; i < calendar.Count; i++)
            {
                var start = calendar[i - 1].End;
                var end = calendar[i].Start;

                if (end - start >= meetingDuration)
                {
                    available.Add(new StringMeeting(
                        MinutesToTime(start),
                        MinutesToTime(end)
                    ));
                }
            }

            return available;
        }

        private static int TimeToMinutes(string time)
        {
            var parts = time.Split(':');
            var hours = int.Parse(parts[0]);
            var minutes = int.Parse(parts[1]);
            return hours * 60 + minutes;
        }

        private static string MinutesToTime(int minutes)
        {
            var hours = minutes / 60;
            var mins = minutes % 60;
            return $"{hours:D2}:{mins:D2}";
        }
    }

    // Meeting in raw strings ("09:00", "17:30")
    public class StringMeeting(string start, string end)
    {
        public string Start { get; set; } = start;
        public string End { get; set; } = end;
    }
    
}
