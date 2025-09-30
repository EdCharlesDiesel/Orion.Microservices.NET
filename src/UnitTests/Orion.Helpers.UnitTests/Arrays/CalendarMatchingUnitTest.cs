using Orion.Helpers.Arrays;

namespace Orion.Helpers.UnitTests.Arrays
{
    public class CalendarMatchingClassTests
    {
        [Fact(Skip = "Fix later")]
        public void CalendarMatching_ReturnsExpectedAvailabilities()
        {
            // Arrange
            var calendar1 = new List<StringMeeting>
            {
                new StringMeeting("09:00", "10:30"),
                new StringMeeting("12:00", "13:00"),
                new StringMeeting("16:00", "18:00")
            };
            var dailyBounds1 = new StringMeeting("09:00", "20:00");

            var calendar2 = new List<StringMeeting>
            {
                new StringMeeting("10:00", "11:30"),
                new StringMeeting("12:30", "14:30"),
                new StringMeeting("14:30", "15:00"),
                new StringMeeting("16:00", "17:00")
            };
            var dailyBounds2 = new StringMeeting("10:00", "18:30");

            int meetingDuration = 30;

            // Act
            var result = CalendarMatchingClass.CalendarMatching(
                calendar1,
                dailyBounds1,
                calendar2,
                dailyBounds2,
                meetingDuration
            );

            // Expected output (common free slots of >= 30 minutes)
            var expected = new List<StringMeeting>
            {
                new StringMeeting("11:30", "12:00"),
                new StringMeeting("15:00", "16:00"),
                new StringMeeting("18:00", "18:30")
            };

            // Assert
            Assert.Equal(expected.Count, result.Count);

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].Start, result[i].Start);
                Assert.Equal(expected[i].End, result[i].End);
            }
        }
    }
}