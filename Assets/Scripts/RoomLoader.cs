using UnityEngine;

//makes this room loaded while its collider is touching the room loading collider around the player.
//otherwise, makes it unloaded.

public class RoomLoader : MonoBehaviour
{
    public Collider2D roomLoadCollider;
    GameObject roomObject; //enable/disable this as needed

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        roomObject = transform.GetChild(0).gameObject;
        roomObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider == roomLoadCollider)
        {
            //Debug.Log("hi");
            roomObject.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider == roomLoadCollider)
        {
            //Debug.Log("bye");
            roomObject.SetActive(false);
        }
    }
}
