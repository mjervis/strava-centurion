using System;

namespace Strava_Centurion
{
    public interface INode
    {
        DateTime GetDateTime();

        double GetAltitude();

        string GetCadence();

        double GetTotalDistance();

        string GetHeartrate();

        double GetLongitude();

        double GetLatitude();

        double GetSpeed();

        double GetPower();

        void SetPower(double power);
    }
}
