using UnityEngine;

public class checkcollisionwall : MonoBehaviour
{
    public string walltouching;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Vector3 parentPosition = transform.position;
        Collider2D hit = Physics2D.OverlapBox(
        parentPosition + new Vector3(-0.8f, 0, 0),  // Center of the box
        new Vector2(0.5f, 0.8f), // Width and height
        0f                   // Rotation angle
        );
        walltouching = "";
        if (hit != null){
         if (hit.gameObject.layer == 3){
              walltouching = "left";
          }
        }   

        Collider2D hit2 = Physics2D.OverlapBox(
        parentPosition+ new Vector3(0.8f,0,0),  // Center of the box
        new Vector2(0.5f, 0.8f), // Width and height
        0f                   // Rotation angle
        );

        if (hit2 != null){
         if (hit2.gameObject.layer == 3){
                if (walltouching != "left" && walltouching != "both")
                {
                    walltouching = "right";
                }
                else
                {
                    walltouching = "both";
                }
          }
        }  
    Debug.Log(walltouching);
    }
}
