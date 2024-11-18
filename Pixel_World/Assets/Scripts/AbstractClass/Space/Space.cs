using UnityEngine;
using AbstractClass.Element;

namespace AbstractClass.Space{
    public abstract class Space : MonoBehaviour{
        public string description;

        public Vector3[] map_boundaries;
        public Vector3 initial_position = new Vector3(0, 0, 0);

        public scope[] scope_list;
        public element[] object_list;
        public subject[] subject_list;

        public abstract void Initialize();

        public abstract void Update();

        protected virtual void DisplayDescription(){
            Debug.Log($"Space: {description}");
        }
    }
}