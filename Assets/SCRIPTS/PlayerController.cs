using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public CharacterController characterController;
    public float moveSpeed;
    public float mouseSensitivity;
    public GameObject mainUiPanel;
    public bool isMainUiEnable;
    public Animator anim;
    public Text showBoxName;
    public List<GameObject> showVfx_BoxWise;
    public List<AudioClip> showSfx_BoxWise;
    public AudioSource showAudioSource;
    public Transform firstMissionSpherePoint, secondMissionPoint;
    public List<Color> colorList;
    public float speed;
    public bool isFirstMissionCompleted;
    public float distanceFromPlayerToFirstSpher, distanceFromPlayerToSecondBoxs;
    public float minDistForFirstPoint, minDistForSecondPoint;

    private float xRot;

    private void Update()
    {
        distanceFromPlayerToFirstSpher = Vector3.Distance(this.transform.position, firstMissionSpherePoint.position);
        distanceFromPlayerToSecondBoxs = Vector3.Distance(this.transform.position, secondMissionPoint.position);
        if (!isMainUiEnable)
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            xRot -= mouseY;
            xRot = Mathf.Clamp(xRot, -90, 90);
            Camera.main.transform.localRotation = Quaternion.Euler(xRot, 0, 0);
            this.transform.Rotate(Vector3.up * mouseX);
            Vector3 moveDirection = transform.forward * moveZ + transform.right * moveX;
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
            if(moveX != 0 || moveZ != 0 )
            {
                anim.SetBool("iSWalking", true);
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            }
            else
            {
                anim.SetBool("iSWalking", false);
            }
        } 

        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit ))
            {
                if(hit.collider.gameObject.name == "Box_A" && isFirstMissionCompleted && distanceFromPlayerToSecondBoxs < minDistForSecondPoint)
                {
                    ShowBox("A", 0);
                }
                else if(hit.collider.gameObject.name == "Box_B" && isFirstMissionCompleted && distanceFromPlayerToSecondBoxs < minDistForSecondPoint)
                {
                    ShowBox("B", 1);
                }
                else if (hit.collider.gameObject.name == "Box_C" && isFirstMissionCompleted && distanceFromPlayerToSecondBoxs < minDistForSecondPoint)
                {
                    // sent back to first mission at sphere point and Main Ui Shows
                    var step = speed*Time.deltaTime;
                    this.transform.position = Vector3.MoveTowards(transform.position, firstMissionSpherePoint.position, step);
                    StartCoroutine(DelayToFirstMissionpoint());
                    mainUiPanel.SetActive(true);
                    ShowBox("C", 2);
                }
                else if(hit.collider.gameObject.name == "Sphere" && !isFirstMissionCompleted && distanceFromPlayerToFirstSpher < minDistForFirstPoint) // 
                {
                    FirstMissionGetSphereFunc();
                    hit.collider.gameObject.SetActive(false);
                    isFirstMissionCompleted = true;
                }
            }
        }
    }

    public void ShowBox(string boxName, int boxNumber)
    {
        showBoxName.gameObject.SetActive(true);
        showBoxName.text = "You Have Dropped in Box " + boxName;
        showBoxName.color = colorList[boxNumber];
        showVfx_BoxWise[boxNumber].SetActive(true);
        showAudioSource.PlayOneShot(showSfx_BoxWise[boxNumber], 0.75f);
        StartCoroutine(ResetText());
    }

    private IEnumerator ResetText()
    {
        yield return new WaitForSeconds(1.5f);
        showBoxName.gameObject.SetActive(false);
    }

    private IEnumerator DelayToFirstMissionpoint()
    {
        yield return new WaitForSeconds(1.5f);
        this.transform.position = firstMissionSpherePoint.position;
    }

    public void FirstMissionGetSphereFunc()
    {
        mainUiPanel.SetActive(true);
        MainUIController.instance.sphereInstrument.texture = MainUIController.instance.sphereTexture;
        isMainUiEnable = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        anim.SetBool("iSWalking", false);
    }

    public void OnClickMainUiCloseButton()
    {
        mainUiPanel.SetActive(false);
        isMainUiEnable = false;
    }
}
