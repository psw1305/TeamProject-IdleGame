using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    #region Serialize Fields

    [SerializeField] private float[] layer_Speed = new float[6];
    [SerializeField] private GameObject[] layer_Objects = new GameObject[6];

    #endregion

    #region Properties

    public float[] Layer_Speed => layer_Speed;
    public GameObject[] Layer_Objects => layer_Objects;

    #endregion

    public void LayerMove()
    {
        for (int i = 1; i < layer_Objects.Length; i++)
        {
            layer_Objects[i].transform.GetChild(0).position += Vector3.left * Time.deltaTime * layer_Speed[i];
            layer_Objects[i].transform.GetChild(1).position += Vector3.left * Time.deltaTime * layer_Speed[i];
            
            if (layer_Objects[i].transform.GetChild(0).localPosition.x <= -40.0f)
            {
                layer_Objects[i].transform.GetChild(0).localPosition = new Vector2(layer_Objects[i].transform.GetChild(0).localPosition.x + 80.0f, 0);
            }

            if (layer_Objects[i].transform.GetChild(1).localPosition.x <= -40.0f)
            {
                layer_Objects[i].transform.GetChild(1).localPosition = new Vector2(layer_Objects[i].transform.GetChild(1).localPosition.x + 80.0f, 0);
            }
        }        
    }
}