using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace PixelArsenal
{
    public enum BeamType
    {
        Type1,
        Type2,
        Type3
    }

    public class PixelArsenalBeamScript : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject[] beamLineRendererPrefab;
        public GameObject[] beamStartPrefab;
        public GameObject[] beamEndPrefab;

        private BeamType currentBeam = BeamType.Type1;
        private GameObject beamStart;
        private GameObject beamEnd;
        private GameObject beam;
        private LineRenderer line;
        private Transform transform;
        private float textureScrollOffset;

        [Header("Adjustable Variables")]
        public float beamEndOffset = 1f;
        public float textureScrollSpeed = 8f;
        public float textureLengthScale = 3;

        [Header("Put Sliders here (Optional)")]
        public Slider endOffSetSlider;
        public Slider scrollSpeedSlider;

        [Header("Put UI Text object here to show beam name")]
        public Text textBeamName;

        private bool isFiringBeam = false;

        // Use this for initialization
        void Start()
        {
            transform = gameObject.transform;
            if (textBeamName)
                textBeamName.text = beamLineRendererPrefab[(int)currentBeam].name;
            if (endOffSetSlider)
                endOffSetSlider.value = beamEndOffset;
            if (scrollSpeedSlider)
                scrollSpeedSlider.value = textureScrollSpeed;
            CreateBeamObjects();
        }

        void CreateBeamObjects()
        {
            beamStart = Instantiate(beamStartPrefab[(int)currentBeam], new Vector3(0, 0, 0), Quaternion.identity, transform);
            beamEnd = Instantiate(beamEndPrefab[(int)currentBeam], new Vector3(0, 0, 0), Quaternion.identity, transform);
            beam = Instantiate(beamLineRendererPrefab[(int)currentBeam], new Vector3(0, 0, 0), Quaternion.identity, transform);
            line = beam.GetComponent<LineRenderer>();
            beamStart.SetActive(false);
            beamEnd.SetActive(false);
            beam.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();

            if (Input.GetMouseButtonDown(0))
            {
                isFiringBeam = true;
                beamStart.SetActive(true);
                beamEnd.SetActive(true);
                beam.SetActive(true);
            }
            if (Input.GetMouseButtonUp(0))
            {
                isFiringBeam = false;
                beamStart.SetActive(false);
                beamEnd.SetActive(false);
                beam.SetActive(false);
            }

            if (isFiringBeam)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray.origin, ray.direction, out hit))
                {
                    Vector3 tdir = hit.point - transform.position;
                    ShootBeamInDir(transform.position, tdir);
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) //Cycle beams
            {
                currentBeam = (BeamType)(((int)currentBeam + 1) % beamLineRendererPrefab.Length);
                UpdateBeam();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) //Cycle beams
            {
                currentBeam = (BeamType)(((int)currentBeam - 1 + beamLineRendererPrefab.Length) % beamLineRendererPrefab.Length);
                UpdateBeam();
            }
        }

        void UpdateBeam()
    {
        if (textBeamName)
            textBeamName.text = beamLineRendererPrefab[(int)currentBeam].name;
        Destroy(beamStart);
        Destroy(beamEnd);
        Destroy(beam);
        CreateBeamObjects();
    }

    void ShootBeamInDir(Vector3 start, Vector3 dir)
    {
        line.SetPosition(0, start);
        beamStart.transform.position = start;

        Vector3 end = Vector3.zero;
        RaycastHit hit;
        if (Physics.Raycast(start, dir, out hit))
            end = hit.point - (dir.normalized * beamEndOffset);
        else
            end = transform.position + (dir * 100);

        beamEnd.transform.position = end;
        line.SetPosition(1, end);

        beamStart.transform.LookAt(beamEnd.transform.position);
        beamEnd.transform.LookAt(beamStart.transform.position);

        float distance = Vector3.Distance(start, end);
        line.sharedMaterial.mainTextureScale = new Vector2(distance / textureLengthScale, 1);
        textureScrollOffset -= Time.deltaTime * textureScrollSpeed;
        if (textureScrollOffset < 0f)
            textureScrollOffset += 1f;
        line.sharedMaterial.mainTextureOffset = new Vector2(textureScrollOffset, 0);
    }
}
}