using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using static NetworkManager.ServerStatus;
using System;


/// <summary>
/// Required components for this component.
/// </summary>

[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]
[RequireComponent(typeof(AREnvironmentProbeManager))]
[RequireComponent(typeof(InitialData))]
public class MultipleObjectPlacement : MonoBehaviour
{
    /// <summary>
    /// prefab for Pointer indicator
    /// </summary>
    [SerializeField]
    GameObject PointerIndicator;

    /// <summary>
    /// prefab for canvas
    /// </summary>
    [SerializeField]
    GameObject canvas;

    /// <summary>
    /// Referance for pointer indicator
    /// </summary>
    GameObject _PointerIndicator;

    /// <summary>
    /// Spawning object in the AR view
    /// </summary>
    public static GameObject spawnedObject;
    
    /// <summary>
    /// Max value for keep the object within the clipping values
    /// </summary>
    float MaxScaleNumber;

    /// <summary>
    /// Access the AR camera
    /// </summary>
    GameObject ArCamera;

    /// <summary>
    /// To check Ar object placed or not 
    /// </summary>
    public static bool isObjectPlaced = false;

    /// <summary>
    /// Initial scale of the Spawned Object
    /// </summary>
    static Vector3 initialScale;

    /// <summary>
    /// Previous rotation of the spawned Object
    /// </summary>
    Quaternion previousRotation;

    /// <summary>
    /// Initial rotation of the spawned object
    /// </summary>
    Quaternion InitialRotation;

    /// <summary>
    /// Previuos position of the spawned object
    /// </summary>
    Vector3 previousPosition;

    /// <summary>
    /// Hits of the AR raycast
    /// </summary>
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    /// <summary>
    /// Access the AR raycast Manager
    /// </summary>
    ARRaycastManager m_RaycastManager;


    /// <summary>
    /// To check positioning condition
    /// </summary>
    bool isPositioning = false;

    /// <summary>
    /// To check Multiple touches
    /// </summary>
    bool gotMultipleTouchs = false;

    /// <summary>
    /// Object Start position
    /// </summary>
    public Vector3 startMarker;

    /// <summary>
    /// Object destination position
    /// </summary>
    public Vector3 endMarker;

    /// <summary>
    /// Movement speed in units per second.
    /// </summary>
    public float speed = 100f;

    /// <summary>
    ///     Time when the movement started.
    /// </summary>
    private float startTime;

    /// <summary>
    ///     Time when the movement started.
    /// </summary>
    private float startTimeLiftDown;

    /// <summary>
    /// Total distance between the markers.
    /// </summary>
    private float journeyLength;

    /// <summary>
    /// Final Rotation of the object
    /// </summary>
    Quaternion toRotation;

    /// <summary>
    /// Starting rotaion of the object
    /// </summary>
    Quaternion FromRotation;

    /// <summary>
    /// Rotating speed in units per second.
    /// </summary>
    float speedR = 2.5f;

    /// <summary>
    /// Scalling speed in units per second.
    /// </summary>
    float speedS = 5f;
    /// <summary>
    /// To check object still rotating or not
    /// </summary>
    bool rotate = false;

    /// <summary>
    /// To check object still Moving to destination
    /// </summary>
    bool wentToPosition = false;

    /// <summary>
    /// To check object still Moving to destination
    /// </summary>
    bool wentToScale = false;

    public GameObject moveButton;


    /// <summary>
    /// Access shadow plane
    /// </summary>
    PrefabMaterialHandler prefabMaterialHandler;

    /// <summary>
    /// Inintial Position for Dragging
    /// </summary>
    Vector2 initialPosition = new Vector2(0, 0);

    /// <summary>
    /// current Position for Dragging
    /// </summary>
    Vector3 ObjectScreenPosition = new Vector2(0, 0);
    /// <summary>
    /// Difference between Inintial Position 
    /// </summary>
    Vector2 DistanceDifference = new Vector2(0, 0);

    /// <summary>
    /// Access the percentage indicator
    /// </summary>
    public static GameObject percentageIndicator;

    /// <summary>
    /// Access the percentage indicator prefab
    /// </summary>
    public static GameObject percentageIndicatorPrefab;

    /// <summary>
    /// Access the percentage scanSurface prefab
    /// </summary>
    public static GameObject scanSurface;

    /// <summary>
    /// Access the notification prefab
    /// </summary>
    GameObject notification;

    /// <summary>
    /// materials of the spawned object
    /// </summary>
    //public static Material[] getObjectMaterials;
    //public static Material[] setObjectMaterials;

    string detectedPlaneType = "";


    /// <summary>
    /// Access the object scalled or not
    /// </summary>
    bool iscalledToSpawn = false;

    /// <summary>
    /// Time status for object placement
    /// </summary>
    bool startTimeSet = false;

    /// <summary>
    /// Status of the indicator
    /// </summary>
    bool hideIndicator = false;

    /// <summary>
    /// AR Plane Manager Reference
    /// </summary>
    ARPlaneManager aRPlaneManager;
    
    public GameObject _lastObjectTouched = null;

    public Text totalPriceText;
    public float totalPrice = 0;

    public Text maxPriceText;
    public float maxPrice = 2500;

    public GameObject obj;
    /// <summary>
    /// store the server status
    /// </summary>
    public NetworkManager.ServerStatus status;

    /// <summary>
    /// return an instance of itself
    /// </summary>
    public static MultipleObjectPlacement instance;

    /// <summary>
    /// server pings
    /// </summary>
    public double lastping = 5;
    
    public GameObject catalogue;
    public GameObject infoPage;
    public GameObject infoButton;
    public List<GameObject> ModelsList;
    public List<Texture> TexturesList;
    
    public GameObject mainButtons;
    public GameObject scanText;
    
    
    public GameObject objectToPlaceAutomaticly;
    public void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        InitialData._singleObjectPlacement = false;
        aRPlaneManager = FindObjectOfType<ARPlaneManager>();
        ArCamera = GameObject.FindWithTag("MainCamera");
        m_RaycastManager = GetComponent<ARRaycastManager>();
        scanSurface = GameObject.FindWithTag("ScanSurfaceAnim");
        notification = GameObject.FindWithTag("NotificationPanel");
        scanSurface.SetActive(true);
//        totalPriceText.text = totalPrice.ToString("F2");
  //      maxPriceText.text = maxPrice.ToString("F2");
        StartCoroutine(NetworkManager.instance.GetServerStatus());
    }

    void Update()
    {
        if (Time.time - lastping >= 5)
        {
            StartCoroutine(NetworkManager.instance.GetServerStatus());
            lastping = Time.time;
        }
        //status.reachable = true;

        if (!TouchIndicatorHandler.isTouchedTheObject)
        {
            if (_lastObjectTouched != null)
            {
                if (infoButton.activeSelf == false)
                {
                    infoButton.SetActive(true);
                    setInfoItemValues(_lastObjectTouched);
                }
            }
            else
                infoButton.SetActive(false);

            Vector3 rayEmitPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            if (m_RaycastManager.Raycast(rayEmitPosition, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = s_Hits[0].pose;
                Console.WriteLine("Hit Pose: " + hitPose.position);
                if (_PointerIndicator == null)
                {
                    _PointerIndicator = Instantiate(PointerIndicator);
                }

                if (_PointerIndicator.activeSelf == false && !hideIndicator)
                {
                    _PointerIndicator.SetActive(true);
                }

                _PointerIndicator.transform.position = hitPose.position;
                _PointerIndicator.transform.rotation = hitPose.rotation;
                scanSurface.SetActive(false);
                if (Vector3.Distance(ArCamera.transform.position, hitPose.position) < 0.8f)
                {
                    _PointerIndicator.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }
                else
                {
                    _PointerIndicator.transform.localScale = new Vector3(1, 1, 1);
                }

                if (iscalledToSpawn && ((PlaneRecognizor(s_Hits[0].trackable.transform) ==
                                         spawnedObject.GetComponent<SpawningObjectDetails>().planeDetectionMode
                                             .ToString()) ||
                                        spawnedObject.GetComponent<SpawningObjectDetails>().planeDetectionMode
                                            .ToString() == "Everything"))
                {
                    isObjectPlaced = true;
                    spawnedObject.SetActive(true);
                    if (spawnedObject.GetComponent<SpawningObjectDetails>().planeDetectionMode.ToString() == "Vertical")
                    {
                        Quaternion orientation = Quaternion.identity;
                        Quaternion zUp = Quaternion.identity;
                        GetWallPlacement(s_Hits[0], out orientation, out zUp);
                        spawnedObject.transform.rotation = zUp;
                    }
                    else
                    {
                        spawnedObject.transform.rotation = hitPose.rotation;
                    }

                    spawnedObject.GetComponent<SpawningObjectDetails>().initialPlacedRotation =
                        spawnedObject.transform.rotation;
                    //getObjectMaterials = EventSystem.current.currentSelectedGameObject.GetComponent<PrefabMaterialHandler>().ObjectMaterials;
                    //setObjectMaterials = getObjectMaterials;
                    //for (int i = 0; i < getObjectMaterials.Length; i++)
                    //{
                    //    getObjectMaterials[i].shader = (Shader)Resources.Load("StandradCustomShader", typeof(Shader));
                    //    getObjectMaterials[i].color = new Color32(255, 255, 255, 255);
                    //}


                    previousRotation = hitPose.rotation;
                    previousPosition = hitPose.position;
                    spawnedObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    spawnedObject.transform.localPosition = previousPosition +
                                                            new Vector3(0,
                                                                spawnedObject.GetComponent<Collider>().bounds.size.y /
                                                                4, 0);
                    startMarker = spawnedObject.transform.position +
                                  new Vector3(0, spawnedObject.GetComponent<Collider>().bounds.size.y / 4, 0);
                    endMarker = previousPosition;
                    journeyLength = Vector3.Distance(startMarker, endMarker);
                    startTime = Time.time;
                    rotate = true;
                    wentToScale = true;
                    iscalledToSpawn = false;

                }

                if (iscalledToSpawn && (PlaneRecognizor(s_Hits[0].trackable.transform) !=
                                        spawnedObject.GetComponent<SpawningObjectDetails>().planeDetectionMode
                                            .ToString()))
                {
                    notification.transform.GetChild(0).GetComponent<Text>().text = "Scan a " +
                        spawnedObject.GetComponent<SpawningObjectDetails>().planeDetectionMode + " suface";
                    notification.GetComponent<Animator>().Play("notificationAnim");
                    iscalledToSpawn = false;
                    spawnedObject = null;
                }
            }
        }
        else
        {
            _PointerIndicator.SetActive(false);
        }

        //Finding the Dragging position 
        if (TouchIndicatorHandler.isTouchedTheObject && (Input.touchCount < 2) && !gotMultipleTouchs)
        {
            Debug.Log("Dragging Object at speed " + spawnedObject.GetComponent<SpawningObjectDetails>()._speedFactor);
            _lastObjectTouched = TouchIndicatorHandler.hitObject;
            if (!TryGetTouchPosition(out Vector2 touchPosition))
                return;

            if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                if (isPositioning)
                {
                    var hitPose = s_Hits[0].pose;
                    if (TouchIndicatorHandler.hitObject != null &&
                        (PlaneRecognizor(s_Hits[0].trackable.transform) == TouchIndicatorHandler.hitObject
                             .GetComponent<SpawningObjectDetails>().planeDetectionMode.ToString() ||
                         spawnedObject.GetComponent<SpawningObjectDetails>().planeDetectionMode.ToString() ==
                         "Everything") && spawnedObject.GetComponent<SpawningObjectDetails>()._speedFactor > 10)
                    {
                        if (TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().enbleDragFeature)
                            TouchIndicatorHandler.hitObject.transform.position = hitPose.position;
                    }

                    previousPosition = hitPose.position;
                }
            }
        }

        if (TouchIndicatorHandler.isTouchedTheObject)
        {
            TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().scalePersentageIndicator.transform
                .rotation = Quaternion.Euler(ArCamera.transform.rotation.eulerAngles.x,
                ArCamera.transform.rotation.eulerAngles.y, 0);
        }

        MultipleTouchHandler();
        freezePositionWhenRotate();
        SendObjectToDetectedPosition();
    }
    
    public void EditSpeedState()
    {
        Debug.Log("Touched button");
        if (_lastObjectTouched.GetComponent<SpawningObjectDetails>()._speedFactor >= 50)
        {
            _lastObjectTouched.GetComponent<SpawningObjectDetails>()._speedFactor = 0;
        }
        else
        {
            _lastObjectTouched.GetComponent<SpawningObjectDetails>()._speedFactor = 100;
        }
    }

    private void GetWallPlacement(ARRaycastHit _planeHit, out Quaternion orientation, out Quaternion zUp)
    {
        TrackableId planeHit_ID = _planeHit.trackableId;
        ARPlane planeHit = aRPlaneManager.GetPlane(planeHit_ID);
        Vector3 planeNormal = planeHit.normal;
        orientation = Quaternion.FromToRotation(Vector3.up, planeNormal);
        Vector3 forward = _planeHit.pose.position - (_planeHit.pose.position + Vector3.down);
        zUp = Quaternion.LookRotation(forward, planeNormal);
    }
    void MultipleTouchHandler()
    {
        if (Input.touchCount == 0)
        {
            gotMultipleTouchs = false;
            DistanceDifference = new Vector2(0, 0);
        }
        else if (Input.touchCount > 1)
        {
            gotMultipleTouchs = true;
            DistanceDifference = new Vector2(0, 0);
        }
    }

    /// <summary>
    /// To Get the touch position
    /// </summary>
    /// <param name="touchPosition"></param>
    /// <returns></returns>
    bool TryGetTouchPosition(out Vector2 touchPosition)
    {

        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                initialPosition = Input.GetTouch(0).position;
                ObjectScreenPosition = ArCamera.GetComponent<Camera>().WorldToScreenPoint(TouchIndicatorHandler.hitObject.transform.position);
                DistanceDifference = new Vector2(ObjectScreenPosition.x, ObjectScreenPosition.y) - initialPosition;
                touchPosition = Input.GetTouch(0).position + DistanceDifference;
                return true;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if (DistanceDifference != new Vector2(0, 0))
                {
                    isPositioning = true;
                    touchPosition = Input.GetTouch(0).position + DistanceDifference;
                    return true;
                }
                else
                {
                    initialPosition = Input.GetTouch(0).position;
                    ObjectScreenPosition = ArCamera.GetComponent<Camera>().WorldToScreenPoint(TouchIndicatorHandler.hitObject.transform.position);
                    DistanceDifference = new Vector2(ObjectScreenPosition.x, ObjectScreenPosition.y) - initialPosition;
                    touchPosition = Input.GetTouch(0).position + DistanceDifference;
                    return true;
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                TouchIndicatorHandler.isTouchedTheObject = false;
                isPositioning = false;
                initialPosition = new Vector2(0, 0);
                touchPosition = default;
                return false;
            }
            else
            {
                touchPosition = default;
                initialPosition = new Vector2(0, 0);
                return false;
            }
        }
        else
        {
            TouchIndicatorHandler.isTouchedTheObject = false;
            isPositioning = false;
        }
        touchPosition = default;
        return false;
    }


    /// <summary>
    /// Sending the object to the detected position
    /// </summary>
    void SendObjectToDetectedPosition()
    {
        if (wentToPosition || wentToScale)
        {
            
            if (spawnedObject.transform.localScale == initialScale || spawnedObject.transform.localScale.magnitude > initialScale.magnitude)
            {
                spawnedObject.transform.localScale = initialScale;
                wentToScale = false;
                wentToPosition = true;
                speed = Vector3.Distance(startMarker, endMarker);
                startMarker = spawnedObject.transform.position;
                spawnedObject.GetComponent<SpawningObjectDetails>().initialScale = spawnedObject.transform.localScale;
                if (spawnedObject.GetComponent<SpawningObjectDetails>().enableShadowPlane)
                {
                    spawnedObject.GetComponent<SpawningObjectDetails>().shadowPlane.SetActive(true);
                }
                else
                {
                    spawnedObject.GetComponent<SpawningObjectDetails>().shadowPlane.SetActive(false);
                }
                if ((Time.time - startTimeLiftDown) > 0.15f)
                {
                    if (!startTimeSet) { startTime = Time.time; startTimeSet = true; }
                    float distCovered = (Time.time - startTime) * speed * 300 * Time.deltaTime;
                    float fractionOfJourney = distCovered / journeyLength;
                    spawnedObject.transform.position = Vector3.Lerp(startMarker, endMarker, fractionOfJourney);
                    if ((int)(spawnedObject.transform.position.magnitude - endMarker.magnitude) == 0)
                    {
                        spawnedObject.transform.position = endMarker;
                        wentToPosition = false;
                        startTimeSet = false;
                        rotate = false;
                    }
                }

            }
            else
            {
                spawnedObject.transform.localScale = Vector3.Lerp(new Vector3(0.1f, 0.1f, 0.1f), initialScale, (Time.time - startTime) * speedS);
                startTimeLiftDown = Time.time;
            }
        }
       

    }

    /// <summary>
    /// Hide the touch scale Percenntage indicator
    /// </summary>
    public static void hideScalePercentageIndicator()
    {
        if (TouchIndicatorHandler.hitObject != null) { 
        TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().scalePersentageIndicator.SetActive(false);
        }
    }

    /// <summary>
    /// Hide the touch scale Percenntage indicator
    /// </summary>
    public static void ShowScalePercentageIndicator(string Percentage)
    {
        TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().scalePersentageIndicator.SetActive(true);
        TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().scalePersentageIndicator.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = Percentage + "%";
    }

    public void destroyObject() {
        Destroy(_lastObjectTouched);
    }


    /// <summary>
    /// Hide the touch indicator gameobject
    /// </summary>
    public static void hideTouchIndicator()
    {
        if (TouchIndicatorHandler.hitObject != null)
        {
            TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().touchIndicator.SetActive(false);
            Destroy(TouchIndicatorHandler.hitObject.GetComponent<Outline>());
        }

    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
    /// <summary>
    /// Show the touch indicator gameobject
    /// </summary>
    public static void showTouchIndicator()
    {
        if (isObjectPlaced  && TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().enableTouchIndicator)
        {
            TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().touchIndicator.SetActive(true);
            TouchIndicatorHandler.hitObject.AddComponent<Outline>();
        }

    }


    /// <summary>
    /// Freeze the position when object is rotating
    /// </summary>
    void freezePositionWhenRotate()
    {
        if (isObjectPlaced && (Input.touchCount > 1))
        {
            if (previousRotation != spawnedObject.transform.rotation)
            {
                TouchIndicatorHandler.hitObject.transform.position = previousPosition;
                previousRotation = TouchIndicatorHandler.hitObject.transform.rotation;
                TouchIndicatorHandler.isTouchedTheObject = true;
            }
            else if (previousRotation == TouchIndicatorHandler.hitObject.transform.rotation)
            {
                previousPosition = TouchIndicatorHandler.hitObject.transform.position;
            }
        }
    }


    /// <summary>
    /// Reset scale to initial scale
    /// </summary>
    public static void resetToInitialScale()
    {
            TouchIndicatorHandler.hitObject.transform.localScale = TouchIndicatorHandler.hitObject.GetComponent<SpawningObjectDetails>().initialScale;
    }

    /// <summary>
    /// Instabciate the choosed object to spawn
    /// </summary>
    /// <param name="go"></param>
    public void spawnObject(GameObject go, int price = 0, int id = 0, string color = "", string style = "", string room = "")
    {
        Debug.Log("Spawn object");
        this.catalogue.SetActive(false);
        iscalledToSpawn = true;
        isObjectPlaced = false;
        spawnedObject = Instantiate(go);
        initialScale = spawnedObject.transform.localScale;
        spawnedObject.GetComponent<SpawningObjectDetails>().scalePersentageIndicator.transform.localScale = spawnedObject.GetComponent<Collider>().bounds.size * 0.0015f;
        spawnedObject.GetComponent<SpawningObjectDetails>().scalePersentageIndicator.transform.position = new Vector3(0, spawnedObject.GetComponent<Collider>().bounds.size.y * 1.2f, 0);
        spawnedObject.GetComponent<SpawningObjectDetails>().totalPrice = (spawnedObject.GetComponent<SpawningObjectDetails>().priceValue + spawnedObject.GetComponent<SpawningObjectDetails>().deliveryPrice) + (spawnedObject.GetComponent<SpawningObjectDetails>().priceValue / 100) * 5;
        spawnedObject.GetComponent<SpawningObjectDetails>().price = price;
        spawnedObject.GetComponent<SpawningObjectDetails>().id = id;
        spawnedObject.GetComponent<SpawningObjectDetails>().color = color;
        spawnedObject.GetComponent<SpawningObjectDetails>().style = style;
        spawnedObject.GetComponent<SpawningObjectDetails>().room = room;
    
        if (totalPrice + spawnedObject.GetComponent<SpawningObjectDetails>().totalPrice > maxPrice)
        {
            Destroy(spawnedObject);
            return;
        }
        totalPrice += spawnedObject.GetComponent<SpawningObjectDetails>().totalPrice;
        totalPriceText.text = totalPrice.ToString("F2");
//        setInfoItemValues(spawnedObject.)
        spawnedObject.SetActive(false);
        catalogue.SetActive(false);
    }

//copy _lastObjectTouched position to spawn a new object
    public void spawnAtPosition(GameObject go, int price, int id, string color, string style, string room) {
        if (_lastObjectTouched != null) {
            //create new object at _lastObjectTouched position
            GameObject newObject = Instantiate(go);
            newObject.transform.position = _lastObjectTouched.transform.position;
            newObject.transform.rotation = _lastObjectTouched.transform.rotation;
            newObject.transform.localScale = _lastObjectTouched.transform.localScale;
            newObject.GetComponent<SpawningObjectDetails>().scalePersentageIndicator.transform.localScale = newObject.GetComponent<Collider>().bounds.size * 0.0015f;
            newObject.GetComponent<SpawningObjectDetails>().scalePersentageIndicator.transform.position = new Vector3(0, newObject.GetComponent<Collider>().bounds.size.y * 1.2f, 0);
            newObject.GetComponent<SpawningObjectDetails>().totalPrice = (newObject.GetComponent<SpawningObjectDetails>().priceValue + newObject.GetComponent<SpawningObjectDetails>().deliveryPrice) + (newObject.GetComponent<SpawningObjectDetails>().priceValue / 100) * 5;
            newObject.GetComponent<SpawningObjectDetails>().price = price;
            newObject.GetComponent<SpawningObjectDetails>().id = id;
            newObject.GetComponent<SpawningObjectDetails>().color = color;
            newObject.GetComponent<SpawningObjectDetails>().style = style;
            newObject.GetComponent<SpawningObjectDetails>().room = room;
            Destroy(_lastObjectTouched);
            _lastObjectTouched = newObject;
        }
    }
    
    // spawn a random number of objects from 1 to 5 not same position but first is on cursor position
    public IEnumerator spawnRandomObjects(GameObject go) {
        int number = UnityEngine.Random.Range(1, 6);
        for (int i = 0; i < number; i++) {
            GameObject newObject = Instantiate(go);
            newObject.transform.position = _PointerIndicator.transform.position;
            newObject.transform.rotation = newObject.transform.rotation;
            newObject.transform.localScale = newObject.transform.localScale;
            newObject.GetComponent<SpawningObjectDetails>().scalePersentageIndicator.transform.localScale = newObject.GetComponent<Collider>().bounds.size * 0.0015f;
            newObject.GetComponent<SpawningObjectDetails>().scalePersentageIndicator.transform.position = new Vector3(0, newObject.GetComponent<Collider>().bounds.size.y * 1.2f, 0);
            newObject.SetActive(true);
            yield return new WaitForSeconds(1);
        }
    }
    
    public void callSpawnRandomObjects(GameObject go) {
        StartCoroutine(spawnRandomObjects(go));
    }
    
    public void movePanel(GameObject panel)
    {
        Vector3 pos = panel.GetComponent<RectTransform>().position;
        Vector3 canvasPos = obj.GetComponent<RectTransform>().position;

        if (pos.x < 450) // & active panel
        {
            panel.GetComponent<RectTransform>().position = canvasPos;
        }
        else
        {
            panel.GetComponent<RectTransform>().position = new Vector3(-850, pos.y, pos.z);
        }
    }
    
    public void EditMaxPrice(float value)
    {
        if (maxPrice + value < totalPrice)
            return;
        maxPrice += value;
        maxPriceText.text = maxPrice.ToString("F2");
    }
    
    /// <summary>
    /// Detect the ray hit plane type
    /// </summary>
    /// <param name="ArPlaneTransform"></param>
    /// <returns></returns>
    string PlaneRecognizor(Transform ArPlaneTransform)
    {

        if ((Mathf.Round(ArPlaneTransform.eulerAngles.x) == 0 || Mathf.Round(ArPlaneTransform.eulerAngles.x) == 360) && (Mathf.Round(ArPlaneTransform.eulerAngles.z) == 0 || Mathf.Round(ArPlaneTransform.eulerAngles.z) == 360))
        {
            detectedPlaneType = "Horizontal";
        }
        else
        {
            detectedPlaneType = "Vertical";
        }
        return detectedPlaneType;
    }

    /// <summary>
    /// Pointer indicator handler
    /// </summary>
    public void showHideCanvas()
    {
        hideIndicator = !hideIndicator;
        canvas.SetActive(!canvas.activeSelf);
        _PointerIndicator.SetActive(!_PointerIndicator.activeSelf);
    }

    public void setInfoItemValues(GameObject objectToCheck) {
        string name = objectToCheck.GetComponent<SpawningObjectDetails>().name;
        string dimensions = objectToCheck.GetComponent<SpawningObjectDetails>().dimensions.x.ToString()+"x"+objectToCheck.GetComponent<SpawningObjectDetails>().dimensions.y.ToString()+"x"+objectToCheck.GetComponent<SpawningObjectDetails>().dimensions.z.ToString()+"cm";
        string brand = objectToCheck.GetComponent<SpawningObjectDetails>().brand;
        string price = ((objectToCheck.GetComponent<SpawningObjectDetails>().priceValue + objectToCheck.GetComponent<SpawningObjectDetails>().deliveryPrice) + (objectToCheck.GetComponent<SpawningObjectDetails>().priceValue / 100) * 5).ToString() + "€ frais de livraison inclus";
        
        catalogue.SetActive(false);
        infoPage.SetActive(true);
        StartCoroutine(NetworkManager.instance.GetCatalogue());
        infoPage.GetComponent<infoItem>().miniature.GetComponent<Image>().sprite = objectToCheck.GetComponent<SpawningObjectDetails>().miniature;
      // double price = furniture.GetComponent<SpawningObjectDetails>().totalPrice();
    }

    public void UpdateCatalogue() {
        CatalogueScript.instance.ClearList();
        StartCoroutine(CatalogueScript.instance.CreateItemsWithFilters());
    }

    public void callOpenCatalogue() {
        StartCoroutine(LoadAndOpenCatalogue());
    }
    
    public void closeCatalogue() {
        catalogue.SetActive(false);
        mainButtons.SetActive(true);    
        scanText.SetActive(true);

    }

    public IEnumerator LoadAndOpenCatalogue() 
    {
        GameObject newItem;

        yield return StartCoroutine(NetworkManager.instance.GetCatalogue());
        catalogue.SetActive(true);
        CatalogueScript catalogueScript = catalogue.GetComponent<CatalogueScript>();
        catalogueScript.ClearList();
        NetworkManager.instance.allCatalog.ForEach((item) =>
        {
            newItem = Instantiate(catalogueScript.Prefab, catalogueScript.listView);
            newItem.GetComponent<CatalogueItemScript>().Name.text = item.name;
            newItem.GetComponent<CatalogueItemScript>().Price.text = item.price.ToString() + "€";
            newItem.GetComponent<CatalogueItemScript>().BrandName.text = item.company_name;
            newItem.GetComponent<CatalogueItemScript>().list = catalogueScript.cartList;
            newItem.GetComponent<CatalogueItemScript>().id = item.id;
            newItem.GetComponent<CatalogueItemScript>().color = item.colors;
            newItem.GetComponent<CatalogueItemScript>().style = item.styles;
            newItem.GetComponent<CatalogueItemScript>().room = item.rooms;


            Transform thumbnailTransform = newItem.transform.Find("Thumbnail");
            int randomItem = UnityEngine.Random.Range(0, TexturesList.Count);
            if (thumbnailTransform != null)
            {
                RawImage thumbnailImage = thumbnailTransform.GetComponent<RawImage>();
                if (thumbnailImage != null)
                {
                    thumbnailImage.texture = TexturesList[randomItem];
                }
            }


            GameObject TryAR = newItem.transform.Find("TryAR").gameObject;
            Button TryARButton = TryAR.GetComponent<Button>();
            TryARButton.onClick.AddListener(() => {
                mainButtons.SetActive(true);
                scanText.SetActive(true);
                spawnObject(ModelsList[randomItem], Convert.ToInt32(item.price), item.id, item.colors, item.styles, item.rooms);
            });
        });
        mainButtons.SetActive(false);
        scanText.SetActive(false);
    }


    public void changeMaterials() {
    int randomMatPrincipal = UnityEngine.Random.Range(0, spawnedObject.GetComponent<SpawningObjectDetails>().matPrincipal.Count);
    int randomMatSecondaire = UnityEngine.Random.Range(0, spawnedObject.GetComponent<SpawningObjectDetails>().matSecondary.Count);

    List<int> matPrincipalIndex = spawnedObject.GetComponent<SpawningObjectDetails>().matPrincipalIndex;
    List<int> matSecondaireIndex = spawnedObject.GetComponent<SpawningObjectDetails>().matSecondaryIndex;

    foreach (Transform child in spawnedObject.transform)
    {
        Renderer childRenderer = child.GetComponent<Renderer>();
        if (childRenderer != null)
        {
            if (matPrincipalIndex.Contains(child.GetSiblingIndex()))
            {
                childRenderer.material = spawnedObject.GetComponent<SpawningObjectDetails>().matPrincipal[randomMatPrincipal];
                spawnedObject.GetComponent<SpawningObjectDetails>().indexMatPrincipal = randomMatPrincipal;
            }
            else if (matSecondaireIndex.Contains(child.GetSiblingIndex()))
            {
                childRenderer.material = spawnedObject.GetComponent<SpawningObjectDetails>().matSecondary[randomMatSecondaire];
            }
        }
    }
    }

}
