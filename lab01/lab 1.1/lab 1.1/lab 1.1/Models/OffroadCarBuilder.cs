namespace RacingCarTuner.Models
{
    public class OffroadCarBuilder : CarBuilder
    {
        public override CarBuilder SetBody(string bodyType)
        {
            GetCar().SetBody("Внедорожник");
            return this;
        }

        public override CarBuilder SetEngine(string engineType)
        {
            Engine engine;
            switch (engineType)
            {
                case "V8 Атмо":
                    engine = new Engine(350, "V8 Внедорожник");
                    break;
                case "V6 Турбо":
                    engine = new Engine(300, "V6 Внедорожник");
                    break;
                case "Электро":
                    engine = new Engine(200, "Электро Внедорожник");
                    break;
                default:
                    engine = new Engine(350, "V6 Внедорожник");
                    break;
            }
            GetCar().SetEngine(engine);
            return this;
        }

        public override CarBuilder SetWheels(string wheelType)
        {
            Wheels[] wheels = new Wheels[4];
            int grip = wheelType == "Грязь" ? 85 : 70;

            for (int i = 0; i < 4; i++)
            {
                wheels[i] = new Wheels(wheelType + " Внедорожник", grip);
            }
            GetCar().SetWheels(wheels);
            return this;
        }

        public override CarBuilder SetArmor(int? armor)
        {
            GetCar().SetArmor(armor ?? 80);
            return this;
        }

        public override CarBuilder SetSpeed(int? speed)
        {
            GetCar().SetSpeed(speed ?? 200);
            return this;
        }
    }
}