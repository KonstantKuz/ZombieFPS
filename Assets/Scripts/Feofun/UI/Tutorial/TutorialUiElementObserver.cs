using System;
using System.Collections.Generic;
using System.Linq;
using SuperMaxim.Core.Extensions;
using UnityEngine.Assertions;

namespace Feofun.UI.Tutorial
{
    public delegate void UiElementDelegate(TutorialUiElement obj);
    
    public static class TutorialUiElementObserver
    {
        private static readonly HashSet<TutorialUiElement> _activeElements = new HashSet<TutorialUiElement>();
        
        public static event UiElementDelegate OnElementActivated;
        public static event UiElementDelegate OnElementClicked;

        public static void Add(TutorialUiElement element)
        {
            _activeElements.Add(element);
        }       
        public static void Remove(TutorialUiElement element)
        {
            _activeElements.Remove(element);
        }

        public static void DispatchOnActivated(TutorialUiElement element)
        { 
            OnElementActivated?.Invoke(element);
        }
        
        public static void DispatchOnClicked(TutorialUiElement element)
        {
            OnElementClicked?.Invoke(element);
        }

        //TODO: this is incorrect. Random element with given id is returned
        //Should be changed to scheme with unique elements ids later
        public static TutorialUiElement Get(string id) => _activeElements.First(it => it.Id == id);
        
        public static bool Contains(string id)
        {
            var item = _activeElements.FirstOrDefault(it => it.Id == id);
            return item != null;
        }
    }
}