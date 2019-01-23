using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Server;
using System.Data;
using System.Data.SqlTypes;


namespace Inaugura.SqlServer
{    
    public class UDF
    {
        /// <summary>
        /// Calculates the distance between to lat/long points and returns the approximate distance in kilometers
        /// </summary>
        /// <param name="latitude1">The latitude of location 1</param>
        /// <param name="longitude1">The longitude of location 1</param>
        /// <param name="latitude2">The latitude of location 2</param>
        /// <param name="longitude2">The longitude of location 2</param>
        /// <returns>Distance in kilometers</returns>
        [SqlFunction(DataAccess = DataAccessKind.None)]
        public static SqlDouble Distance(SqlDouble latitude1, SqlDouble longitude1, SqlDouble latitude2, SqlDouble longitude2)
        {
            if (latitude1.IsNull || longitude1.IsNull || latitude2.IsNull || longitude2.IsNull)
                return SqlDouble.Null;

            if (latitude1 == latitude2 && longitude1 == longitude2)
                return 0;

            const double rad = 6371; //Earth radius in Km
            //Convert to radians
            double p1Longitude = longitude1.Value / 180 * Math.PI;
            double p1Latitude = latitude1.Value / 180 * Math.PI;
            double p2Longitude = longitude2.Value / 180 * Math.PI;
            double p2Latitude = latitude2.Value / 180 * Math.PI;

            return new SqlDouble(Math.Acos(Math.Sin(p1Latitude) * Math.Sin(p2Latitude) +
                Math.Cos(p1Latitude) * Math.Cos(p2Latitude) * Math.Cos(p2Longitude - p1Longitude)) * rad);
        }

        /// <summary>
        /// Creates a SqlDateTime object from a string representation
        /// </summary>
        /// <param name="str">The DateTime string</param>
        /// <returns>The sql datetime object, otherwise null if conversion failed.</returns>
        [SqlFunction(DataAccess = DataAccessKind.None)]
        public static SqlDateTime ConvertDateTime(SqlString str)
        {
            DateTime dateTime;
            if (DateTime.TryParse(str.ToString(), out dateTime))
            {
                return new SqlDateTime(dateTime);
            }
            return SqlDateTime.Null;
        }
    }
}
