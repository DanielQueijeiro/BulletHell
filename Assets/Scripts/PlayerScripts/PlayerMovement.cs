using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float normalSpeed = 6f;
    public float slowSpeed = 4f;

    public KeyCode switchKey;
    
    void Update()
    {
        if (Input.GetKey(switchKey))
        {
            transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * slowSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * normalSpeed * Time.deltaTime;
        }
    }

}
