using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MainMenu
{
    public class ButtonSelect : MonoBehaviour
    {
        public EventSystem MenuSystem;
        private MenuButton _currentButton;
        public AudioClip buttonClick;
        public GameObject selector;
        private MenuButton[] _buttons;

        //this allows users to select buttons on the main menu using the keyboard only
        public void Awake()
        {
            _buttons = GetComponentsInChildren<MenuButton>();
            Debug.Log(_buttons.Length);
            for (var index = 0; index < _buttons.Length; index++)
            {
                if (index + 1 < _buttons.Length)
                {
                    _buttons[index].next = _buttons[index + 1];
                }
                else
                {
                    _buttons[index].next = _buttons[0];
                }

                if (index - 1 >= 0)
                {
                    _buttons[index].prev = _buttons[index - 1];
                }
                else
                {
                    _buttons[index].prev = _buttons[_buttons.Length - 1];
                }
            }
            
            SetButton(_buttons[0]);
        }

        private void OnEnable()
        {
            
            SetButton(_buttons[0]);
        }


        public void Submitted()
        {
            _currentButton.GetComponent<Button>().onClick.Invoke();
            //var pointer = new PointerEventData(EventSystem.current);
            //ExecuteEvents.Execute(Menu[currSelected], pointer, ExecuteEvents.pointerDownHandler);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                SetButton(_currentButton.prev);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                SetButton(_currentButton.next);
            }
            MenuSystem.SetSelectedGameObject(_currentButton.gameObject); 
        }

        public void SetButton(MenuButton menuButton)
        {
            _currentButton = menuButton;
            selector.transform.position = _currentButton.Position;
            SoundController.instance.RandomPitchandsfx(0, buttonClick);
        }
    }
}