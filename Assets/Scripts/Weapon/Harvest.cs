using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

public class Harvest : MonoBehaviour
{
    [SerializeField] private AudioClip successSound;
    private bool gatherAnimal = false;
    private AudioSource audioSource;

    public bool GatherAnimal { get => gatherAnimal; set => gatherAnimal = value; }

    private enum InputStatus {
        Grabbing,
        Holding,
        Realeasing,
        None
    }
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        Assert.IsNotNull(audioSource);
        Assert.IsNotNull(successSound);
    }


    void Update()
    {
       /* if (Input.GetMouseButtonUp(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Clicked on the UI");
                gatherAnimal = false;
            }
            else
            {
                gatherAnimal = true;
            }
        }*/

        if (GatherAnimal)
        {
            Animal animal = AnimalFactory.Instance.SelectedAnimal;
            var uI = FindObjectOfType<UIManager>();
            uI.showHuntedAnimalHPBox();
            animal.StartGatherInWordScene();
            
          
            if(animal.Hp == 0)
            {
                audioSource.PlayOneShot(successSound);
                uI.showHuntedAnimalHPBox(false);
                GatherAnimal = false;
            }
        }
    }


    // jezeli kolizja - nieaktualne
    /*
        private void OnTriggerStay(Collider other)
        {
            gemUnits += 1;
            if (other.name == "gem")
            {
                gemUnits += 1;
            }
            if (other.name == "Wolf")
            {
                wolfUnits += 1;
            }
            if (other.name == "zoomObj")
            {
                generatedObjects = other.gameObject.GetComponent<Animal>().getGeneratedObjects();
                gemUnits += 1;
            }

            Debug.Log("gem " + gemUnits + " wolf " + wolfUnits + " hp:" + other.gameObject.GetComponent<Animal>().Hp) ;
            //  GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);


            if (other.gameObject.CompareTag(HuntersConstants.TAG_ANIMAL))
            {
                // jezeli zwierze


                //jezeli koniec
                if (other.gameObject.GetComponent<Animal>().Hp == 1)
                {
                    audioSource.PlayOneShot(successSound);
                    dead();
                  //  Invoke("PowerDown", stallTime);
                }


            }

                }


            private void dead()
        {
            Debug.Log("WINEEER");
        }

        private void PowerDown()
        {
            Destroy(gameObject);
        }
        */




    //WCZESNIEJSZE
    /* 
     * //zmienne
     
     
         private  int gemUnits = 0;
   private  int wolfUnits = 0;
  public KeyCode fireLaser;

    public  string laserInUse = "n";
    private  int overheat = 0;
   
    List<Transform> generatedObjects = new List<Transform>();

    [SerializeField] private float collisionStallTime = 2.0f;
    [SerializeField] private float stallTime = 0.01f;

    [SerializeField] private AudioClip dropSound;

    [SerializeField] private AudioClip throwSound;

    private Rigidbody rigidbody;

    private InputStatus inputStatus;

    private float lastX;
    private float lastY;
    private bool released;
    private bool holding;
    private bool trackingCollisions = false;

    private float OverheatStrong = 300;
    */


    /*
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();

        Assert.IsNotNull(audioSource);
        Assert.IsNotNull(rigidbody);
        Assert.IsNotNull(dropSound);
        Assert.IsNotNull(successSound);
        Assert.IsNotNull(throwSound);
    }
    private void Update()
    {
        if (released)
        {
            return;
        }

        if (holding)
        {
            FollowInput();
        }

        // Update input status
        UpdateInputStatus();

        //react to that status
        switch (inputStatus)
        {
            case InputStatus.Grabbing:
           //     Grab();
                break;
            case InputStatus.Holding:
               // Drag();
                break;
            case InputStatus.Realeasing:
                Release();
                break;
            case InputStatus.None:
                break;
            default:
                return;
        }
    }

    private void Release()
    {
        if (lastY < GetInputPosition().y)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Throw(worldPosition);
        }

    }

    private void Throw(Vector3 targetPos)
    {
        rigidbody.useGravity = false;
        trackingCollisions = true;
       // GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 6);
       Vector3 direction = new Vector3(1.0f, 0.0f, GetInputPosition().x);
        direction = Camera.main.transform.TransformDirection(direction);

        rigidbody.AddForce((direction * 2 / 2.0f) + Vector3.up * 2);
        audioSource.PlayOneShot(throwSound);
        
        released = true;
        holding = false;

        Invoke("PowerDown", stallTime);
    }

    private void Drag()
    {
        lastX = GetInputPosition().x;
        lastY = GetInputPosition().y;
    }

    private void Grab()
    {
        Ray ray = Camera.main.ScreenPointToRay(GetInputPosition());
        RaycastHit point;

        if( Physics.Raycast(ray, out point, 100.0f)
            && point.transform == transform)
        {
            holding = true;
            transform.parent = null;
        }

    }

    private void UpdateInputStatus()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0)){
            inputStatus = InputStatus.Grabbing;
        }
        else if (Input.GetMouseButton(0)){
            inputStatus = InputStatus.Holding;
        }
        else if (Input.GetMouseButtonUp(0)){
            inputStatus = InputStatus.Realeasing;
        }
        else{
            inputStatus = InputStatus.None;
        }
#endif

#if NOT_UNITY_EDITOR

        if(Input.GetTouch(0).phase == TouchPhase.Began)
        {
            inputStatus = InputStatus.Grabbing;
        }
        else if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            inputStatus = InputStatus.Realeasing;
        }
        else if (Input.touchCount == 1)
        {
            inputStatus = InputStatus.Holding;
        }
        else
        {
            inputStatus = InputStatus.None;
        }
#endif
    }

    private void FollowInput()
    {
        Vector3 inputPos = GetInputPosition();
        inputPos.z = Camera.main.nearClipPlane * 7.5f;
        Vector3 pos = Camera.main.ScreenToWorldPoint(inputPos);
        transform.localPosition = Vector3.Lerp(transform.localPosition,
                                                pos,
                                                50.0f * Time.deltaTime);
     }

    private  Vector2 GetInputPosition()
    {
        Vector2 result = new Vector2();

#if UNITY_EDITOR
        result = Input.mousePosition;
#endif
#if NOT_UNITY_EDITOR
        result = Input.GetTouch(0).position;
#endif

        return result;
    }

    private void PowerDown()
    {
        Destroy(gameObject);
    }




    // FIRST VERSION
    /*
// Start is called before the first frame update
void Start()
{



}

// Update is called once per frame
void Update()
{

   if ((Input.GetKeyDown(fireLaser)) && (laserInUse == "n"))
   {
       GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 6);
       laserInUse = "y";
   }
   if (laserInUse == "y")
   {
       overheat += 1;
   }
   if ((laserInUse == "locked") && (overheat > 0))
   {
       overheat -= 1;

   }


   if (overheat > OverheatStrong)
   {
       laserInUse = "locked";
         GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -6);
       float waitTime = 3.0f;
       if (gemUnits > 0)
       {
           waitTime = 2;
       }
       StartCoroutine(StopLaser(waitTime));
   }
   Debug.Log("overhead: " + overheat);

}

private void OnTriggerStay(Collider other)
{
   gemUnits += 1;
   // -gwiazafka if (other.name == "gem")
   {
       gemUnits += 1;
   }
   if (other.name == "Wolf")
   {
       wolfUnits += 1;
   }
   if (other.name == "zoomObj")
   {
       generatedObjects=other.gameObject.GetComponent<Deplete>().getGeneratedObjects();
       gemUnits += 1;
   } end//
   Debug.Log("gem " + gemUnits + " wolf " + wolfUnits);
   GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

   Deplete deplte = FindObjectOfType<Deplete>();
   generatedObjects = deplte.getGeneratedObjects();

}

IEnumerator StopLaser(float timeToWait = 4f)
{

   yield return new WaitForSeconds(timeToWait);
   GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
   laserInUse = "n";

   Destroy(gameObject);

}
*/
}
