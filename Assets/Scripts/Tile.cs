using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer sr;
    private bool isActivated = false;

    public bool IsAlive()
    {
        return isActivated;
    }

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseDown()
    {
        if (!isActivated)
        {
            ActivateTile();
        }
        else if (isActivated)
        {
            ResetTile();
        }
    }

    public void ActivateTile()
    {
        isActivated = true;
        sr.color = Color.black;
    }

    public void ResetTile()
    {
        sr.color = Color.white;
        isActivated = false;
    }
}
