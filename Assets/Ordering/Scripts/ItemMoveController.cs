using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Ordering
{
    public class ItemMoveController : MonoBehaviour
    {
        public bool canmove, movingright = true;
        
        public float speed, distance;
        public Vector2 Offset;

        [Header("Move To Insect")]
        public bool moveToInsectMouth;
        public float speedToMouth;
        public GameObject Insect;
        Transform insectMouthPos;

        bool hastriggereatanim;
        [HideInInspector]
        public bool canCheckNeighour;
        // Start is called before the first frame update
        void Start()
        {
            canCheckNeighour = true;
            //while (Insect == null)
            //{
            Insect = GameObject.Find("InsectEnemy");
            //}
            insectMouthPos = Insect.transform.GetChild(0).transform;
        }

        // Update is called once per frame
        void Update()
        {
            if (!Timer.instance.GamePause)
            {
                if (canmove)
                {
                    MoveMethod();
                }

                if (moveToInsectMouth)
                {
                    if (Vector2.Distance(transform.position, insectMouthPos.position) > 0.1f)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, insectMouthPos.position, speedToMouth * Time.deltaTime);
                        if (Vector2.Distance(transform.position, insectMouthPos.position) < 3f && !hastriggereatanim )
                        {
                            Insect.GetComponent<Animator>().SetTrigger("eat2");
                            hastriggereatanim = true;
                        }

                    }
                    
                    else
                    {
                        //It means that mushroom has reached at insect mouth
                        GetComponent<Item>().CheckAnswer();
                        Destroy(this.gameObject);
                    }
                }

            }
        }

        //bool CheckAssPass()
        //{
        //    //Vector2 parenttempposright = new Vector2(transform.position.x + 2f, transform.position.y);
        //    //Vector2 parenttempposleft = new Vector2(transform.position.x - 2f, transform.position.y);


        //    //RaycastHit2D hit2Dright = Physics2D.Raycast(parenttempposright + Offset, Vector2.right, 1);
        //    //RaycastHit2D hit2Dleft = Physics2D.Raycast(parenttempposleft - Offset, -Vector2.right, 1);

        //    RaycastHit2D hit2Dright = Physics2D.Raycast((Vector2)transform.position + Offset, Vector2.right, distance);
        //    RaycastHit2D hit2Dleft = Physics2D.Raycast((Vector2)transform.position - Offset, -Vector2.right, distance);
        //    if (hit2Dright.collider == true || hit2Dleft.collider == true)
        //    {

        //        return false;
        //    }

        //    else
        //    {
        //        return true;
        //    }
        //}

        RaycastHit2D hit2Dright;
        RaycastHit2D hit2Dleft;

        void MoveMethod()
        {
            //Vector2 parenttempposright = new Vector2(transform.position.x + 2f, transform.position.y);
            //Vector2 parenttempposleft = new Vector2(transform.position.x - 2f, transform.position.y);

            //RaycastHit2D hit2Dright = Physics2D.Raycast(parenttempposright + Offset, Vector2.right, distance);
            //RaycastHit2D hit2Dleft = Physics2D.Raycast(parenttempposleft - Offset, -Vector2.right, distance);

            if(canCheckNeighour)
            {

             hit2Dright = Physics2D.Raycast((Vector2)transform.position + Offset, Vector2.right, distance);
             hit2Dleft = Physics2D.Raycast((Vector2)transform.position - Offset, -Vector2.right, distance);
            }

            

            if(hit2Dright.collider == true || hit2Dleft.collider == true)
            {
                //Debug.LogWarning("This is just collide with something"+ gameObject.name);
                if (!hit2Dleft.collider)
                {
                    if (movingright)
                    {

                        speed = -speed;
                        movingright = false;
                        Debug.Log("GotoLeft");
                    }

                }

                if (!hit2Dright.collider)
                {
                    if (!movingright)
                    {

                        speed = -speed;
                        movingright = true;
                        Debug.Log("GotoRight");
                    }

                }


                //else
                //{
                //    if (!hit2Dleft.collider)
                //    {
                //        speed = -speed;
                //        movingright = true;
                //        Debug.Log("Gotoright");
                //    }
                    
                //}
            }
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        private void OnDrawGizmos()
        {
            //Vector2 tempright = new Vector2(transform.position.x + 2f, transform.position.y);
            //Vector2 templeft = new Vector2(transform.position.x - 2f, transform.position.y);
            //Gizmos.DrawRay(tempright + Offset, Vector3.right * distance);
            //Gizmos.DrawRay(templeft - Offset, -Vector3.right * distance);

            Gizmos.color = Color.red;
            Gizmos.DrawRay((Vector2)transform.position + Offset , Vector3.right * distance);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay((Vector2)transform.position - Offset , -Vector3.right * distance);
        }
    }
}