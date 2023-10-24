
namespace GAS
{
    public class GameplayEffectHandle
    {
        public GameplayEffect gameplayEffect;
        public AbilityComponent source;
        public AbilityComponent parent;
        public float durationRemaining;
        public float totalDuration;
        public float timeUntilPeriod;
        public float level;
        
        public static GameplayEffectHandle Create(GameplayEffect gameplayEffect, AbilityComponent source, float level = 1)
        {
            return new GameplayEffectHandle(gameplayEffect, source, level);
        }
        
        private GameplayEffectHandle(GameplayEffect gameplayEffect, AbilityComponent source, float level)
        {
            this.gameplayEffect = gameplayEffect;
            this.source = source;
            this.parent = source;
            this.level = level;
            
            foreach (var bm in this.gameplayEffect.modifiers)
            {
                bm.magnitude.Initialise(this);
            }
            
            this.durationRemaining = this.gameplayEffect.duration;
            this.totalDuration = this.gameplayEffect.duration;
            this.timeUntilPeriod = this.gameplayEffect.period;

            if (this.gameplayEffect.executeImmediate)
            {
                this.timeUntilPeriod = 0;
            }
        }
        
        public bool TickPeriod(float deltaTime)
        {
            this.timeUntilPeriod -= deltaTime;
            if (this.timeUntilPeriod <= 0.01f)
            {
                this.timeUntilPeriod = this.gameplayEffect.period;
                if (gameplayEffect.IsPeriodic())
                {
                    return true;
                }
            }
            return false;
        }

        public void TickDuration(float deltaTime)
        {
            if (this.gameplayEffect.durationType == DurationType.Infinite)
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