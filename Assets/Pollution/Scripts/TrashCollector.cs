using UnityEngine;
using UnityEngine.InputSystem;

public class TrashCollector : MonoBehaviour
{
    [SerializeField] private Transform holdPoint;

    private InputAction interactAction;
    private GameObject nearbyTrash;
    private GameObject heldTrash;

    void Start()
    {
        interactAction = InputSystem.actions.FindAction("Interact");
    }

    void Update()
    {
        if (interactAction.WasPressedThisFrame())
        {
            if (heldTrash == null && nearbyTrash != null)
            {
                PickUp(nearbyTrash);
            }
            else if (heldTrash != null)
            {
                Drop();
            }
        }
    }

    void PickUp(GameObject trash)
    {
        heldTrash = trash;
        heldTrash.GetComponent<Rigidbody>().isKinematic = true;
        heldTrash.transform.SetParent(holdPoint);
        heldTrash.transform.localPosition = Vector3.zero;
        heldTrash.transform.localRotation = Quaternion.identity;
        nearbyTrash = null;
        Debug.Log("Déchet ramassé : " + heldTrash.tag);
    }

    void Drop()
    {
        heldTrash.transform.SetParent(null);
        heldTrash.GetComponent<Rigidbody>().isKinematic = false;
        heldTrash = null;
    }

    bool IsTrash(GameObject obj)
    {
        return obj.CompareTag("Plastic") || obj.CompareTag("Glass") || obj.CompareTag("Metal");
    }

    void OnTriggerStay(Collider other)
    {
        if (heldTrash == null && nearbyTrash == null && IsTrash(other.gameObject))
        {
            nearbyTrash = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == nearbyTrash)
        {
            nearbyTrash = null;
        }
    }
}