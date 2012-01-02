using System;
using System.Globalization;
using System.Windows.Data;

namespace NGTweet.Convertors
{
    public class DateTimeToRelativeTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime createdDate;
            DateTime.TryParse(value.ToString(), out createdDate);

            TimeSpan timeDifference = DateTime.Now - createdDate;

            if (timeDifference.TotalMinutes < 1)
            {
                return string.Format("{0} secs ago", Math.Round(timeDifference.TotalSeconds));
            }

            if (timeDifference.TotalMinutes > 1 && timeDifference.TotalMinutes < 60)
            {
                return string.Format("{0} mins ago", Math.Round(timeDifference.TotalMinutes));
            }

            if (timeDifference.TotalHours > 1 && timeDifference.TotalHours <= 24)
            {
                return string.Format("{0} hours ago", Math.Round(timeDifference.TotalHours));
            }

            if (timeDifference.TotalDays > 1 && timeDifference.TotalDays <= 7)
            {
                return string.Format("{0} days ago", Math.Round(timeDifference.TotalDays));
            }

            return createdDate.ToString("m");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}