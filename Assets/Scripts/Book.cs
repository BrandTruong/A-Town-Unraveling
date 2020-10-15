using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Book : MonoBehaviour
{
    //NOTE: MAKE SURE IMAGES START WITH BLANK IMAGE, THEN COVERPAGE, AND SIZE IS EVEN (END WITH BLANK IMAGE IF NOT)


    //ok that's it

    //yikes this code is spaghetti
    public GameObject[] ImageSource;
    //public Animator Animator;
    public GameObject[] Flip;
    public GameObject[] Page;
    private int pageno = 0;
    private int pagemax = 0;
    public void OnButtonPress()
    {
        Debug.Log("Book is opened");
        int i = 0;
        foreach (GameObject Button in Flip)
        {
            Button.SetActive(true);
            switch (i)
            {
                case 0:
                    Button.GetComponent<Button>().onClick.AddListener(delegate { FlipPageBack(); });
                    break;
                case 1:
                    Button.GetComponent<Button>().onClick.AddListener(delegate { FlipPageFront(); });
                    break;
                default:
                    break;
            }
            i++;
        }
        for (i = 0; i < Page.Length; i++)
        {
            Page[i].GetComponent<Image>().sprite = ImageSource[i].GetComponent<Image>().sprite;
            Page[i].SetActive(true);
        }
    }
    //0 for back, 1 for forward
    private void FlipPageBack() {
        if (pageno <= 0)
        {
            return;
        }
        for (int i = 0; i < Page.Length; i++)
        {
            Page[1 - i].GetComponent<Image>().sprite = ImageSource[--pageno].GetComponent<Image>().sprite;
        }
        Debug.Log("Flipped back pageno is" +pageno);
        return;
    }
    private void FlipPageFront()
    {
        if (pageno >= pagemax - 1)
        {
            return;
        }
        for (int i = 0; i < Page.Length; i++)
        {
            Page[(pageno % 2)].GetComponent<Image>().sprite = ImageSource[pageno + 2].GetComponent<Image>().sprite;
            pageno++;
        }
        Debug.Log("Flipped front pageno is" + pageno);
        return;
    }
  
    private void ExitOverlay()
    {
        deactivateObjects();
        pageno = 0;
    }
    private void deactivateObjects()
    {
        foreach (GameObject Button in Flip)
        {
            Button.SetActive(false);
        }
        foreach (GameObject Image in ImageSource)
        {
            Image.SetActive(false);
        }
        foreach (GameObject i in Page)
        {
            i.SetActive(false);
        }
    }
    void Awake()
    {
        Start();
    }
    void Start()
    {
        deactivateObjects();
        pagemax = ImageSource.Length - 1;
        Debug.Log(pagemax + " pagemax,pageno start " + pageno);
    }
    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            ExitOverlay();
        }
    }
}

