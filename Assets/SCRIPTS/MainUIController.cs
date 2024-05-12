using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour
{
    public static MainUIController instance;
    public RawImage sphereInstrument;
    public Texture2D sphereTexture;
    public GameObject instrumentStore;
    public bool isInstrumentStoreOpened;

    private void Awake()
    {
        instance = this;
    }

    public void OnClickInstrumentButton()
    {
        if (!isInstrumentStoreOpened)
        {
            instrumentStore.SetActive(true);
            isInstrumentStoreOpened = true;
        }
        else if (isInstrumentStoreOpened)
        {
            instrumentStore.SetActive(false);
            isInstrumentStoreOpened = false;
        }
        
    }
}
