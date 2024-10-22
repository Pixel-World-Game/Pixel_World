using UnityEngine;

public class BlockInteraction : MonoBehaviour{
    public float range = 5.0f;
    public GameObject blockPrefab;
    public Transform terrainParent; // Parent GameObject for organizing generated blocks
    private Inventory inventory; // Reference to the Inventory script

    void Start() {
        inventory = FindObjectOfType<Inventory>(); // Find the inventory manager in the scene
    }
    
    void Update(){
        if (Input.GetMouseButtonDown(0)) { // Left-click to break a block
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, range)){
                if (hit.collider.gameObject.CompareTag("Block")){
                    Destroy(hit.collider.gameObject);
                }
            }
        }

        if (Input.GetMouseButtonDown(1)) { // Right-click to place a block
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, range)){
                // Calculate the position for placing the block
                Vector3 placePosition = hit.point + hit.normal * 0.5f;
                placePosition = new Vector3(Mathf.Round(placePosition.x), Mathf.Round(placePosition.y),
                    Mathf.Round(placePosition.z));

                GameObject blockToPlace = inventory.GetSelectedBlockPrefab();
                
                // Set the block's parent to be the terrainParent
                Instantiate(blockToPlace, placePosition, Quaternion.identity).transform.parent = terrainParent;
            }
        }
    }
}