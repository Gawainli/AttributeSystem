namespace GAS.Magnitude
{
    public abstract class BaseMagnitude
    {
        public abstract void Initialise(BuffHandle handle);
        public abstract float CalculateMagnitude(BuffHandle handle);
    }
}