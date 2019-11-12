using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MainMenu
{
    public class MenuButton : MonoBehaviour, IPointerEnterHandler
    {
        private ButtonSelect _buttonSelect;
        internal MenuButton prev;
        internal MenuButton next;
        private Color _default;
        private Text text;
        private Image[] _bars;
        public Vector3 Position { get; set; }


        private void Start()
        {
            text = GetComponentInChildren<Text>();
            _buttonSelect = GetComponentInParent<ButtonSelect>();
            Position = transform.position;
            _default = text.color;
            _bars = GetComponentsInChildren<Image>();
        }

        public void Set()
        {
            if (text != null)
                text.color = Color.white;
            if (_bars != null)
            {
                foreach (var bar in _bars)
                {
                    bar.color = Color.white;
                }
            }
        }

        public void UnSet()
        {
            if (text != null)
                text.color = _default;
            if (_bars != null)
            {
                foreach (var bar in _bars)
                {
                    bar.color = _default;
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _buttonSelect.SetButton(this);
        }
    }
}