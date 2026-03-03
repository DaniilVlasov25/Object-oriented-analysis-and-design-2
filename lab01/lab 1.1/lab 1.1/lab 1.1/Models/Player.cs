using RacingCarTuner.Models;

namespace RacingCarTuner
{
    public class Player
    {
        private Garage garage;

        public Player(Garage garage)
        {
            this.garage = garage;
        }
        public Car CreateCar(CarBuilder builder, string carType, string engine = null, string wheels = null, int? armor = null, int? speed = null)
        {
            garage.SetBuilder(builder);

            if (carType == "Спорт")
            {
                return garage.ConstructSportsCar();
            }
            else if (carType == "Внедорожник")
            {
                return garage.ConstructOffroadCar();
            }
            else 
            {
                return garage.ConstructCustomCar("Седан", engine, wheels, armor, speed);
            }
        }
    }
}