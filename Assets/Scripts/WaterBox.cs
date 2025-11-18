using UnityEngine;

public class WaterBox : MonoBehaviour
{
    float waterForce = 5;
    float waterDragPercent = 0.8f;
    float stationaryPositionCutoff = 0.05f; //y level difference where it just locks the object at water level
    float stationarySpeedCutoff = 1f; //speed the player needs to be below to become stationary
    float topY; //y level at the top of the box, assuming the box isn't rotated

    private void Start()
    {
        topY = transform.position.y + transform.GetComponent<SpriteRenderer>().size.y / 2 + 0.25f; //the 0.25 makes it just a little higher than centered
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D body = collision.gameObject.GetComponent<Rigidbody2D>();
        if (body != null)
        {
            DoWaterStuff(body);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Rigidbody2D body = collision.gameObject.GetComponent<Rigidbody2D>();
        if (body != null)
        {
            DoWaterStuff(body);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("bees");
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (player != null)
        {
            Debug.Log("bees2");
            player.OnExitWater();
        }
    }

    void DoWaterStuff(Rigidbody2D body)
    {
        //special stuff if the body is attached to a player
        PlayerMovement player = body.GetComponent<PlayerMovement>();
        if(player != null)
        {
            player.WhileInWater();
        }
        //this if statement makes the player stop bobbing if really close to the water
        float diff = topY - body.transform.position.y;
        if (Mathf.Abs(diff) < stationaryPositionCutoff && Mathf.Abs(body.linearVelocityY) < stationarySpeedCutoff)
        {
            body.transform.position += new Vector3(0, diff, 0);
            body.linearVelocityY = 0;
        }
        //this if statement should make things hover at about the water level
        if (body.transform.position.y < topY)
        {
            body.linearVelocityY += waterForce;
            body.linearVelocityY *= waterDragPercent;
        }
    }
}
