﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace ELittoral.Helpers
{
    public static class Geographical
    {
        public static Geopoint GetCentralGeopoint(IList<Geopoint> geoPoints)
        {
            if (geoPoints.Count == 1)
            {
                return geoPoints.Single();
            }

            double x = 0;
            double y = 0;
            double z = 0;

            foreach (var geoPoint in geoPoints)
            {
                var latitude = geoPoint.Position.Latitude * Math.PI / 180;
                var longitude = geoPoint.Position.Longitude * Math.PI / 180;

                x += Math.Cos(latitude) * Math.Cos(longitude);
                y += Math.Cos(latitude) * Math.Sin(longitude);
                z += Math.Sin(latitude);
            }

            var total = geoPoints.Count;

            x = x / total;
            y = y / total;
            z = z / total;

            var centralLongitude = Math.Atan2(y, x);
            var centralSquareRoot = Math.Sqrt(x * x + y * y);
            var centralLatitude = Math.Atan2(z, centralSquareRoot);

            var centralGeopoint = new Geopoint(new BasicGeoposition
            {
                Latitude = centralLatitude * 180 / Math.PI,
                Longitude = centralLongitude * 180 / Math.PI
            });

            return centralGeopoint;
        }
    }
}
