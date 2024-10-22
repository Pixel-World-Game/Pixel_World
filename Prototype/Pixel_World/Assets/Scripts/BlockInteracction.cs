using UnityEngine;

public class BlockInteraction : MonoBehaviour{
    public float range = 5.0f;
    public GameObject blockPrefab;

    void Update(){
        if (Input.GetMouseButtonDown(0)) // Left-click to break a block
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, range)){
                if (hit.collider.gameObject.CompareTag("Block")){
                    Destroy(hit.collider.gameObject);
                }
            }
        }

        if (Input.GetMouseButtonDown(1)) // Right-click to place a block
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, range)){
                // Calculate the position for placing the block
                Vector3 placePosition = hit.point + hit.normal * 0.5f;
                placePosition = new Vector3(Mathf.Round(placePosition.x), Mathf.Round(placePosition.y),
                    Mathf.Round(placePosition.z));

                Instantiate(blockPrefab, placePosition, Quaternion.identity);
            }
        }
    }
}