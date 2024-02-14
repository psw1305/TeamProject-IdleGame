using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private float[] Layer_Speed = new float[7];
    [SerializeField] private GameObject[] Layer_Objects = new GameObject[7];

    #endregion

    //private void Update()
    //{
    //    LayerMove();
    //}

    public void LayerMove()
    {
        for (int i = 1; i < 7; i++)
        {
            Layer_Objects[i].transform.GetChild(0).position += Vector3.left * Time.deltaTime * Layer_Speed[i];
            Layer_Objects[i].transform.GetChild(1).position += Vector3.left * Time.deltaTime * Layer_Speed[i];
            
            if (Layer_Objects[i].transform.GetChild(0).localPosition.x <= -40.0f)
            {
                Layer_Objects[i].transform.GetChild(0).localPosition = new Vector2(Layer_Objects[i].transform.GetChild(0).localPosition.x + 80.0f, 0);
            }

            if (Layer_Objects[i].transform.GetChild(1).localPosition.x <= -40.0f)
            {
                Layer_Objects[i].transform.GetChild(1).localPosition = new Vector2(Layer_Objects[i].transform.GetChild(1).localPosition.x + 80.0f, 0);
            }
        }
        
    }
}