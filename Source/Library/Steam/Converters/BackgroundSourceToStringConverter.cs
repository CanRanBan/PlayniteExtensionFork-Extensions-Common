﻿using System;
using System.Windows.Data;
using System.Windows.Markup;
using Playnite.SDK;
using Steam;

namespace SteamLibrary
{
    public class BackgroundSourceToStringConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var source = (BackgroundSource)value;
            switch (source)
            {
                case BackgroundSource.Image:
                    return ResourceProvider.GetString("LOCSteamBackgroundSourceImage");
                case BackgroundSource.StoreScreenshot:
                    return ResourceProvider.GetString("LOCSteamBackgroundSourceScreenshot");
                case BackgroundSource.StoreBackground:
                    return ResourceProvider.GetString("LOCSteamBackgroundSourceStore");
                case BackgroundSource.Banner:
                    return ResourceProvider.GetString("LOCSteamBackgroundSourceBanner");
                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
