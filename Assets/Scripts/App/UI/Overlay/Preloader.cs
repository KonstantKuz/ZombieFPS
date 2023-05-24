using UnityEngine;

namespace App.UI.Overlay
{
    public class Preloader : MonoBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
        } 
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}