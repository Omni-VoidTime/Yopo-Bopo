using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float bounceForce = 16;
    public bool updateConstantly = false; //set to true if we have a moving obstacle ever

    Vector2 forceVector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CalculateForceVector();
    }

    // Update is called once per frame
    void Update()
    {
        if (updateConstantly)
        {
            CalculateForceVector();
        }
    }

    void CalculateForceVector()
    {
        //calculate direction vector from angle
        float angle = (transform.rotation.eulerAngles.z + 90) * Mathf.PI / 180;
        forceVector = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * bounceForce;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //bounce the object if it has a rigidbody
        Rigidbody2D body = collision.gameObject.GetComponent<Rigidbody2D>();
        if (body == null) return;

        //add force to the body but make sure it doesn't go too high from "double dipping" the bounce pad's collision
        float xVel = body.linearVelocityX;
        float yVel = body.linearVelocityY;
        if (xVel < forceVector.x)
        {
            xVel = forceVector.x;
        }
        if (yVel < forceVector.y)
        {
            yVel = forceVector.y;
        }
        body.linearVelocity = new Vector2(0,yVel);
        PlayerMovement player = body.GetComponent<PlayerMovement>();
        if(player != null)
        {
            player.extraXVelocity = xVel;
        }
        //body.AddForce(forceVector);
    }
}
