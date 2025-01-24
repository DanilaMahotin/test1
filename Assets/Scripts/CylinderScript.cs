using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;

public class CylinderScript : MonoBehaviour
{
    [SerializeField] private string _color;
    [SerializeField] private string _color2;
    [SerializeField] private string _color3;
    private int _ringsCount = 0;
    [SerializeField] private Light _pointLight;
    Transform parent;
    public bool isTrue;
    public int RingCount
    {
        get { return _ringsCount; }

    }
    private void Start()
    {
        isTrue = false;
        parent = transform;
        UpdateRingCount();
        CheckRingUp();
    }
    private void Update()
    {
        Light();
    }
    public void UpdateRingCount() 
    {
        parent = transform;
        _ringsCount = parent.childCount;
    }

    public void CheckRingUp()
    {
        Transform parent = transform;
        foreach (Transform child in parent)
        {
            Collider collider = child.GetChild(0).gameObject.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }
        }

        if (parent.childCount > 0)
        {
            Transform lastChild = parent.GetChild(parent.childCount - 1);
            Collider lastCollider = lastChild.GetChild(0).gameObject.GetComponent<Collider>();
            if (lastCollider != null)
            {
                lastCollider.enabled = true;
            }
        }
    }

    private void Light()
    {
        parent = transform;
        bool hasValidChild = true;

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (SceneManager.GetActiveScene().name == "low")
            {
                if (child.name != _color)
                {
                    hasValidChild = false;
                    break;
                }
            }
            else
            {
                if (_color2 == null)
                {
                    if (child.name != _color)
                    {
                        hasValidChild = false;
                        break;
                    }
                }
                else if (_color3 == null)
                {
                    if (child.name != _color && child.name != _color2)
                    {
                        hasValidChild = false;
                        break;
                    }
                }
                else 
                {
                    if (child.name != _color && child.name != _color2 && child.name != _color3)
                    {
                        hasValidChild = false;
                        break;
                    }
                }
            }
        }

        if (hasValidChild && parent.childCount > 0)
        {
            isTrue = true;
            _pointLight.color = Color.green;
            
        }
        else
        {
            _pointLight.color = Color.red;
            isTrue = false;
        }
    }
}
