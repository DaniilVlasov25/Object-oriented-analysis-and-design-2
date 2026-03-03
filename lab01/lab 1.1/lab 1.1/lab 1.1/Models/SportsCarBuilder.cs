namespace RacingCarTuner.Models
{
    public class SportsCarBuilder : CarBuilder
    {
        public override CarBuilder SetBody(string bodyType)
        {
            GetCar().SetBody("Спорт");
            return this;
        }

        public override CarBuilder SetEngine(string engineType)
        {
            Engine engine;
            switch (engineType)
            {
                case "V8 Атмо":
                    engine = new Engine(400, "V8 Спорт");
                    break;
                case "V6 Турбо":
                    engine = new Engine(300, "V6 Спорт");
                    break;
                case "Электро":
                    engine = new Engine(250, "Электро Спорт");
                    break;
                default:
                    engine = new Engine(400, "V8 Спорт");
                    break;
            }
            GetCar().SetEngine(engine);
            return this;
        }

        public override CarBuilder SetWheels(string wheelType)
        {
            Wheels[] wheels = new Wheels[4];
            int grip = wheelType == "Слик" ? 95 : 70;

            for (int i = 0; i < 4; i++)
            {
                wheels[i] = new Wheels(wheelType + " Спорт", grip);
            }
            GetCar().SetWheels(wheels);
            return this;
        }

        public override CarBuilder SetArmor(int? armor)
        {
            GetCar().SetArmor(armor ?? 30);
            return this;
        }

        public override CarBuilder SetSpeed(int? speed)
        {
            GetCar().SetSpeed(speed ?? 320);
            return this;
        }
    }
}