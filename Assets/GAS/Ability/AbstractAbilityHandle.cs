namespace GAS
{
    public abstract class AbstractAbilityHandle
    {
        public AbilityDefine abilityDefine;
        public AbilityComponent owner;
        public float level;
        public bool isActive;
        
        public AbstractAbilityHandle(AbilityDefine abilityDefine, AbilityComponent owner, float level = 0)
        {
            this.abilityDefine = abilityDefine;
            this.owner = owner;
            this.level = level;
            this.isActive = false;
        }

        public virtual void Awake()
        {
        }

        public virtual void Active()
        {
            isActive = true;
        }

        public virtual bool Tick(float deltaTime)
        {
            return true;
        }
        
        public virtual bool CanActivate()
        {
            return true;
        }
        
        public virtual void End()
        {
            isActive = false;
        }
    }
}