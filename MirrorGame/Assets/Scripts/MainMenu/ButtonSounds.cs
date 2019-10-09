﻿using UnityEngine;
using UnityEngine.EventSystems;
//required when dealing with event data

namespace MainMenu
{
    public class ButtonSounds : MonoBehaviour, ISelectHandler
        //, IPointerEnterHandler
    {  
        //behaviour inherited for on select and on press events and functions

        BaseEventData buttonEvent;
        public AudioClip buttonHover;
        public AudioClip buttonClick;
        //do this whene UI is hovered
        /*public void OnPointerEnter(PointerEventData eventData)
    {
        SoundController.instance.RandomPitchandsfx(0,buttonHover);
    }*/

        //do this whene UI is selected
        public void OnSelect(BaseEventData eventData)
        {
            if (buttonClick != null)
            {
                SoundController.instance.RandomPitchandsfx(0, buttonClick);
            }
        }

        //There needs to be one for when the UI is pressed to call on submit
    }
}
