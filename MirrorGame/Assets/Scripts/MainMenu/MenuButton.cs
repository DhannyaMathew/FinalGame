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
        public Vector3 Position { get; set; }


        private void Start()
        {
            _buttonSelect = GetComponentInParent<ButtonSelect>();
            Position = GetComponentInChildren<Text>().transform.position;
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log(this);
            _buttonSelect.SetButton(this);
        }
    }
}
