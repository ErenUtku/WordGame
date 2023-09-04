using Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SwipeMenu : MonoBehaviour
    {
        [SerializeField] private GameObject scrollbar;
    
        //Container Values
        private float _scrollPos = 0;
        private float[] _pos;
        private float _distance;

        private void Start()
        {
            _pos = new float[transform.childCount];
            _distance = 1f / (_pos.Length - 1f);
            for (int i = 0; i < _pos.Length; i++)
            {
                _pos[i] = _distance * i;
            }

            var initialValue = CalculatePlayerLevelIndexPoint();
        
            scrollbar.GetComponent<Scrollbar>().value = initialValue;
            _scrollPos = initialValue;
        }


        private void Update()
        {
        
            if (Input.GetMouseButton(0))
            {
                _scrollPos = scrollbar.GetComponent<Scrollbar>().value;
            }
            else
            {
                for (int i = 0; i < _pos.Length; i++)
                {
                    if (_scrollPos < _pos[i] + (_distance / 2) && _scrollPos > _pos[i] - (_distance / 2))
                    {
                        scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, _pos[i], 0.1f);
                    }
                }
            }


            for (int i = 0; i < _pos.Length; i++)
            {
                if (_scrollPos < _pos[i] + (_distance / 2) && _scrollPos > _pos[i] - (_distance / 2))
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

        private static float CalculatePlayerLevelIndexPoint()
        {
            return (DataManager.Instance.LevelIndex - 1) / (float)DataManager.Instance.GetLevelsCount();
        }
    }
}