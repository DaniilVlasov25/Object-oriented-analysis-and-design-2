namespace RacingCarTuner.Models
{
    public class Wheels
    {
        private string type;
        private int grip;

        public Wheels(string type, int grip)
        {
            this.type = type;
            this.grip = grip;
        }

        public string GetInfo()
        {
            return $"Колёса: {type}, Сцепление: {grip}";
        }
    }
}