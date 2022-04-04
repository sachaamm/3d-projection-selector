using UnityEngine;

namespace _3dProjectionSelection.Scripts.Camera
{
    public class MySimpleCamera : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed = 1;
        
        [SerializeField]
        private float rotateSpeed = 1;
        
        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(new Vector3(0,-1,0) * rotateSpeed * Time.deltaTime);
            }
            
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(new Vector3(0,1,0) * rotateSpeed * Time.deltaTime);
            }
            
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(new Vector3(0,0,1) * moveSpeed * Time.deltaTime);
            }
            
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(new Vector3(0,0,-1) * moveSpeed * Time.deltaTime);
            }
        }
    }
}