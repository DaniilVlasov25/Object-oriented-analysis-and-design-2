namespace RacingCarTuner.Models
{
    public abstract class CarBuilder
    {
        private Car car;

        public void Reset()
        {
            car = new Car();
        }

        public abstract CarBuilder SetBody(string bodyType);
        public abstract CarBuilder SetEngine(string engineType);
        public abstract CarBuilder SetWheels(string wheelType);
        public abstract CarBuilder SetArmor(int? armor);
        public abstract CarBuilder SetSpeed(int? speed);

        public Car GetProduct()
        {
            Car result = car;
            return result;
        }

        protected Car GetCar() => car;
    }
}