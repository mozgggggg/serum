using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class checkcollisionwall : MonoBehaviour
{
    public string walltouching;
    Rigidbody2D body;
    public int playertouched = 0;

    [SerializeField] HealthComponent dmgsrc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        playertouched--;
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
    Collider2D hitplr = Physics2D.OverlapBox(
        parentPosition+ new Vector3(0,0,0),  // Center of the box
        new Vector2(1.3f, 1.3f), // Width and height
        0f                   // Rotation angle
        );
        if (hitplr != null)
        {
            if (hitplr.gameObject.layer == 6 && playertouched <= 0)
            {
                playertouched = 20;
                var damage = (int)Mathf.Round(body.linearVelocity.magnitude);
                dmgsrc.TakeDamage(damage);
            }
        }
    }
}
