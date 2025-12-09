using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image[] healthPoints;
    public Sprite healthyLeaf;
    public Sprite deadLeaf;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //draw each leaf as either healthy or dead
        for(int x = 0; x < healthPoints.Length; x++)
        {
            if(PlayerStats.health > x)
            {
                healthPoints[x].sprite = healthyLeaf;
            }
            else
            {
                healthPoints[x].sprite = deadLeaf;
            }
        }
    }
}
