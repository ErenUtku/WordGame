using System;
using UnityEngine;
using UnityEngine.UI;
public class SwipeMenu : MonoBehaviour
{
    public GameObject scrollbar;
    private float _scrollPos = 0;
    private float[] _pos;
    private float distance;

    private void Start()
    {
        _pos = new float[transform.childCount];
        distance = 1f / (_pos.Length - 1f);
        for (int i = 0; i < _pos.Length; i++)
        {
            _pos[i] = distance * i;
        }

        float initialValue = (DataManager.Instance.LevelIndex - 1) / (float)4;//4 is test
        
        //(float)(DataManager.Instance.GetLevelsCount())
        scrollbar.GetComponent<Scrollbar>().value = initialValue;
        _scrollPos = initialValue;
    }

    
    void Update()
    {
        
        if (Input.GetMouseButton(0))
        {
            _scrollPos = scrollbar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < _pos.Length; i++)
            {
                if (_scrollPos < _pos[i] + (distance / 2) && _scrollPos > _pos[i] - (distance / 2))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, _pos[i], 0.1f);
                }
            }
        }


        for (int i = 0; i < _pos.Length; i++)
        {
            if (_scrollPos < _pos[i] + (distance / 2) && _scrollPos > _pos[i] - (distance / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1.2f, 1.2f), 0.1f);
                for (int j = 0; j < _pos.Length; j++)
                {
                    if (j != i)
                    {
                        transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                    }
                }
            }
        }
       

    }
    
}