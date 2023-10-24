
namespace GAS
{
    public class BuffHandle
    {
        public Buff buff;
        public BuffComponent source;
        public BuffComponent parent;
        public float durationRemaining;
        public float totalDuration;
        public float timeUntilPeriod;
        public float level;
        
        public static BuffHandle Create(Buff buff, BuffComponent source, float level = 1)
        {
            return new BuffHandle(buff, source, level);
        }
        
        private BuffHandle(Buff buff, BuffComponent source, float level)
        {
            this.buff = buff;
            this.source = source;
            this.parent = source;
            this.level = level;
            
            foreach (var bm in this.buff.modifiers)
            {
                bm.magnitude.Initialise(this);
            }
            
            this.durationRemaining = this.buff.duration;
            this.totalDuration = this.buff.duration;
            this.timeUntilPeriod = this.buff.period;

            if (this.buff.executeImmediate)
            {
                this.timeUntilPeriod = 0;
            }
        }
        
        public bool TickPeriod(float deltaTime)
        {
            this.timeUntilPeriod -= deltaTime;
            if (this.timeUntilPeriod <= 0.01f)
            {
                this.timeUntilPeriod = this.buff.period;
                if (buff.IsPeriodic())
                {
                    return true;
                }
            }
            return false;
        }

        public void TickDuration(float deltaTime)
        {
            if (this.buff.durationType == DurationType.Infinite)
            {
                this.durationRemaining = 1;
            }
            else
            {
                this.durationRemaining -= deltaTime;
            }
        }
    }
}