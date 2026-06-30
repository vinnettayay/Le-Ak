using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUI;
    private bool inventoryOn = false;

    private void Awake()
    {
        //SetActives
        inventoryUI.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !inventoryOn)
        {
            inventoryOn = true;
            Cursor.lockState = CursorLockMode.Confined;
            inventoryUI.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && inventoryOn)
        {
            inventoryOn = false;
            Cursor.lockState = CursorLockMode.Locked;
            inventoryUI.SetActive(false);
        }
    }
}
