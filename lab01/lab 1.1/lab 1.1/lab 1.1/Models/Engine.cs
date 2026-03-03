namespace RacingCarTuner.Models
{
    public class Engine
    {
        private int power;
        private string type;

        public Engine(int power, string type)
        {
            this.power = power;
            this.type = type;
        }
        public string GetInfo()
        {
            return $"Двигатель: {type}, Мощность: {power} л.с.";
        }
    }
}