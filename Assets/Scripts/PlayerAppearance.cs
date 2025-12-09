using UnityEngine;

public class PlayerAppearance : MonoBehaviour
{
    public GameObject feetObject;
    public GameObject shoesObject;
    public GameObject hand1Object;
    public GameObject hand2Object;
    public GameObject hatObject;

    public PlayerMovement movement;

    bool facingLeft = false;

    Vector2 facingLeftScale = new(1, 1);
    Vector2 facingRightScale = new(-1, 1);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //shoes
        if(movement.speedModifier < 2)
        {
            feetObject.SetActive(true);
            shoesObject.SetActive(false);
        }
        else
        {
            feetObject.SetActive(false);
            shoesObject.SetActive(true);
        }
        //gloves
        if (movement.canWallJump)
        {
            hand1Object.SetActive(true);
            hand2Object.SetActive(true);
        }
        else
        {
            hand1Object.SetActive(false);
            hand2Object.SetActive(false);
        }
        //propeller hat
        if(movement.maxJumpFrames > 15)
        {
            hatObject.SetActive(true);
        }
        else
        {
            hatObject.SetActive(false);
        }
        //scale
        if(movement.horizontal + movement.extraXVelocity < 0)
        {
            facingLeft = true;
        }else if(movement.horizontal + movement.extraXVelocity > 0)
        {
            facingLeft = false;
        }
        if (facingLeft)
        {
            transform.localScale = facingLeftScale;
        }
        else
        {
            transform.localScale = facingRightScale;
        }
    }
}
