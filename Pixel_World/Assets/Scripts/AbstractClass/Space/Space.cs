using UnityEngine;
using AbstractClass;

namespace AbstractClass.Space{
    public abstract class Space : MonoBehaviour{
        public string description;

        public Vector3[] map_boundaries;
        public Vector3 initial_position = new Vector3(0, 0, 0);

        public Scope.Scope[] scope_list;
        public Element.Element[] object_list;
        public Subject.Subject[] subject_list;

        public abstract void Initialize();

        public abstract void Update();

        protected virtual void DisplayDescription(){
            Debug.Log($"Space: {description}");
        }
    }
}