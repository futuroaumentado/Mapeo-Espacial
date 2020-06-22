using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using easyar;
using Common;

public class UIController : MonoBehaviour
{

    public ARSession Session;
    public TouchController touchController;
    private Color meshColor;
    private SparseSpatialMapWorkerFrameFilter sparse;
    private DenseSpatialMapBuilderFrameFilter dense;
    private bool mapa = false;
    public Button boton;
    public GameObject modelo3D;
    public GameObject offText;
    public GameObject onText;



    private void Awake()
    {
        sparse = Session.GetComponentInChildren<SparseSpatialMapWorkerFrameFilter>();
        dense = Session.GetComponentInChildren<DenseSpatialMapBuilderFrameFilter>();
        touchController.TurnOn(touchController.gameObject.transform, Camera.main, false, false, true, false);
    }



    // Start is called before the first frame update
    void Start()
    {
        meshColor = dense.MeshColor;
        Button btn = boton.GetComponent<Button>();
        btn.onClick.AddListener(delegate { ocultarMapeo(mapa); });
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) || !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && Input.touches[0].phase == TouchPhase.Moved)
        {
            modelo3D.SetActive(true);
            var touch = Input.touches[0];
            var viewPoint = new Vector2(touch.position.x / Screen.width, touch.position.y / Screen.height);
            if(sparse && sparse.LocalizedMap)
            {
                var points = sparse.LocalizedMap.HitTest(viewPoint);
                foreach(var point in points)
                {
                    touchController.transform.position = sparse.LocalizedMap.transform.TransformPoint(point);
                    break;
                }
            }
        }
    }

    void ocultarMapeo(bool estado)
    {
        if(estado == false)
        {
            offText.SetActive(false);
            onText.SetActive(true);
            dense.MeshColor = Color.clear;
            mapa = true;
        }
        else
        {
            offText.SetActive(true);
            onText.SetActive(false);
            dense.MeshColor = meshColor;
            mapa = false;
        }
    }
}
