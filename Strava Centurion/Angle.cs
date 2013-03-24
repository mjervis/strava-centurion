namespace Strava_Centurion
{
    using System;

    public class Angle
    {
        public Angle()
        {
            this.Radians = 0;
        }

        public Angle(double angleInRadians)
        {
            this.Radians = angleInRadians;
        }

        public double Radians { get; set; }

        public double Degrees
        {
            get
            {
                return (this.Radians * 180) / Math.PI;
            }

            set
            {
                this.Radians = Math.PI * value / 180.0;
            }
        }

        public static implicit operator double(Angle a)
        {
            return a.Radians;
        }

        public static Angle operator +(Angle a, Angle b)
        {
            return new Angle(a.Radians + b.Radians);
        }

        public static Angle operator -(Angle a, Angle b)
        {
            return new Angle(a.Radians - b.Radians);
        }

        public static Angle operator /(Angle a, double divisor)
        {
            return new Angle(a.Radians / divisor);
        }
    }
}
