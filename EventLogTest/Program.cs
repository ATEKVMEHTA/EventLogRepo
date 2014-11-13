using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventLogTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // http://www.codeproject.com/Articles/12269/Distance-between-locations-using-latitude-and-long
            // http://mathforum.org/library/drmath/view/51879.html

            //var Lat1 = 60.72713;
            //var Long1 = -135.06297;
            //var Lat2 = 60.741808;
            //var Long2 = -135.072786;
            //double distance = Calc(Lat1, Long1, Lat2, Long2);
            //Console.WriteLine("Total Distance = " + distance); ;
            //Console.Read();

            try
            {
                string Source = "ATekLinkedIn";
                 //Create the source, if it does not already exist. 
                if (!EventLog.SourceExists(Source))
                {
                    //An event log source should not be created and immediately used. 
                    //There is a latency time to enable the source, it should be created 
                    //prior to executing the application that uses the source. 
                    //Execute this sample a second time to use the new source.
                    EventLog.CreateEventSource(Source, Source);
                    Console.WriteLine("CreatedEventSource");
                    Console.WriteLine("Exiting, execute the application a second time to use the source.");
                    // The source is created.  Exit the application to allow it to be registered. 
                    return;
                }

                EventLog.WriteEntry(Source, "This is test error on " + DateTime.Now, EventLogEntryType.Error);
                
                Console.WriteLine("Error has been added successfully into event log...");
                Console.WriteLine("Press any key to exit...");
                Console.Read();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error = " + ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.Read();
            }
        }

        public static double Calc(double Lat1,
                 double Long1, double Lat2, double Long2)
        {
            /*
                The Haversine formula according to Dr. Math.
                http://mathforum.org/library/drmath/view/51879.html
                
                dlon = lon2 - lon1
                dlat = lat2 - lat1
                a = (sin(dlat/2))^2 + cos(lat1) * cos(lat2) * (sin(dlon/2))^2
                c = 2 * atan2(sqrt(a), sqrt(1-a)) 
                d = R * c
                
                Where
                    * dlon is the change in longitude
                    * dlat is the change in latitude
                    * c is the great circle distance in Radians.
                    * R is the radius of a spherical Earth.
                    * The locations of the two points in 
                        spherical coordinates (longitude and 
                        latitude) are lon1,lat1 and lon2, lat2.
            */
            double dDistance = Double.MinValue;
            double dLat1InRad = Lat1 * (Math.PI / 180.0);
            double dLong1InRad = Long1 * (Math.PI / 180.0);
            double dLat2InRad = Lat2 * (Math.PI / 180.0);
            double dLong2InRad = Long2 * (Math.PI / 180.0);

            double dLongitude = dLong2InRad - dLong1InRad;
            double dLatitude = dLat2InRad - dLat1InRad;

            // Intermediate result a.
            double a = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) +
                       Math.Cos(dLat1InRad) * Math.Cos(dLat2InRad) *
                       Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);

            // Intermediate result c (great circle distance in Radians).
            double c = 2.0 * Math.Asin(Math.Sqrt(a));

            // Distance.
             const Double kEarthRadiusMiles = 3956.0;
            //const Double kEarthRadiusKms = 6376.5;
             dDistance = kEarthRadiusMiles * c;

            return dDistance;
        }
    }
}
