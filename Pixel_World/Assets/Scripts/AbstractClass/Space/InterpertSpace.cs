namespace AbstractClass.Space{
// Environment class inherits from Space 
    public abstract class InterpretSpace : Space{
        // Implementation of the abstract Initialize method
        public override void Initialize(){
        }

        protected override void DisplayDescription(){
            base.DisplayDescription();
        }
    }
}