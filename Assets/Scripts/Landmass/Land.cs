namespace Landmass
{
    public class Land
    {
        public float sample;
        public string type;

        public Land(float sample)
        {
            this.sample = sample;
            type = GetType(sample);
        }

        string GetType(float sample)
        {
            if (sample < 0.3f)
            {
                return "snow-grass";
            }
            else if (sample < 0.5f)
            {
                return "grass-ground";
            }
            else if (sample < 0.6f)
            {
                return "ground";
            }
            else
            {
                return "water";
            }
        }
    }
}
