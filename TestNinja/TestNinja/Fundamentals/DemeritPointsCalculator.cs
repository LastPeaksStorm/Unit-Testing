using System;

namespace TestNinja.Fundamentals
{
    public class DemeritPointsCalculator
    {
        private const int SpeedLimit = 60;
        private const int MaxSpeed = 240;
        
        public int CalculateDemeritPoints(int speed)
        {
            if (speed < 0 || speed > MaxSpeed) 
                throw new ArgumentOutOfRangeException();
            
            if (speed <= SpeedLimit) return 0; 
            
            const int kmPerDemeritPoint = 5;
            return (speed - SpeedLimit)/kmPerDemeritPoint;

        }        
    }
}