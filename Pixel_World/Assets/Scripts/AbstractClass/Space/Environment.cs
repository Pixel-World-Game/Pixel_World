using UnityEngine;

namespace AbstractClass.Space{
// Environment class inherits from Space 
    public abstract class Environment : Space
    {
        // Environment-specific properties
        public string environmentType;

        // Implementation of the abstract Initialize method
        public override void Initialize()
        {
            // Environment-specific initialization code
            Debug.Log("Initializing Environment: " + environmentType);
        }

        protected override void DisplayDescription()
        {
            base.DisplayDescription();
            Debug.Log("Environment Type: " + environmentType);
        }
    }
}