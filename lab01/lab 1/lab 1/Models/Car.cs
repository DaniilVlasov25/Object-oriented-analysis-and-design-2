namespace RacingCarTuner.Models
{
    public class Car
    {
        private string body;
        private Engine engine;
        private Wheels[] wheels;
        private int? armor;
        private int? speed;

        public Car(string body, Engine engine, Wheels[] wheels, int? armor, int? speed)
        {
            this.body = body;
            this.engine = engine;
            this.wheels = wheels;
            this.armor = armor;
            this.speed = speed;
        }

        public string GetSpecs()
        {
            var specs = $"=== Характеристики автомобиля ===\n";
            specs += $"Кузов: {body}\n";
            specs += (engine != null) ? engine.GetInfo() + "\n" : "Двигатель: не установлен\n";

            if (wheels != null && wheels.Length > 0)
            {
                specs += $"Колёса ({wheels.Length} шт.):\n";
                foreach (var wheel in wheels)
                {
                    specs += $"  - {wheel.GetInfo()}\n";
                }
            }
            else
            {
                specs += "Колёса: не установлены\n";
            }

            specs += armor.HasValue ? $"Броня: {armor} мм\n" : "Броня: не установлена\n";
            specs += speed.HasValue ? $"Макс. скорость: {speed} км/ч\n" : "Скорость: не указана\n";

            return specs;
        }
    }
}