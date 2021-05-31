using UnityEngine;

namespace TheEscapists.Core.Manager
{
    public class CreditsManager : MonoBehaviour
    {
        public GameObject credits;
        public void ShowCredits()
        {
            credits.SetActive(true);
        }
        public void HideCredits()
        {
            credits.SetActive(false);
        }
    }
}