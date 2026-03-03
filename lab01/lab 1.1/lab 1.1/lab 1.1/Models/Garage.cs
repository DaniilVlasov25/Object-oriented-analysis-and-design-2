using RacingCarTuner.Models;

namespace RacingCarTuner
{
    public class Garage
    {
        private CarBuilder builder;
        public void SetBuilder(CarBuilder builder)
        {
            this.builder = builder;
        }
        public Car ConstructSportsCar()
        {
            builder.Reset();
            builder.SetBody("Спорт");
            builder.SetEngine("V8 Атмо");
            builder.SetWheels("Слик");
            builder.SetArmor(30);
            builder.SetSpeed(320);
            return builder.GetProduct();
        }
        public Car ConstructOffroadCar()
        {
            builder.Reset();
            builder.SetBody("Внедорожник");
            builder.SetEngine("V6 Турбо");
            builder.SetWheels("Грязь");
            builder.SetArmor(80);
            builder.SetSpeed(200);
            return builder.GetProduct();
        }
        public Car ConstructCustomCar(string body, string engine, string wheels, int? armor, int? speed)
        {
            builder.Reset();
            builder.SetBody(body);

            if (engine != null)
                builder.SetEngine(engine);

            if (wheels != null)
                builder.SetWheels(wheels);

            if (armor.HasValue)
                builder.SetArmor(armor.Value);

            if (speed.HasValue)
                builder.SetSpeed(speed.Value);

            return builder.GetProduct();
        }
    }
}