namespace Strava_Centurion
{
    using System;

    public class Distance
    {
        public Distance()
        {
            this.Metres = 0;
        }

        public Distance(double distanceInMetres)
        {
            this.Metres = distanceInMetres;
        }

        public double Metres { get; set; }

        public double Kilometres
        {
            get
            {
                return this.Metres / 1000;
            }

            set
            {
                this.Metres = value * 1000;
            }
        }

        public double Miles
        {
            get
            {
                return this.Metres * 0.000621371192;
            }

            set
            {
                this.Metres = value / 0.000621371192;
            }
        }

        public static implicit operator double(Distance a)
        {
            return a.Metres;
        }

        public static Distance operator +(Distance a, Distance b)
        {
            return new Distance(a.Metres + b.Metres);
        }

        public static Distance operator -(Distance a, Distance b)
        {
            return new Distance(a.Metres - b.Metres);
        }

        public static Distance operator /(Distance a, Distance divisor)
        {
            return new Distance(a.Metres / divisor.Metres);
        }
    }
}
