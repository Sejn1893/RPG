using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;


namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string DeafaultSavingFile = "save";
        public float FadeInTime = 0.2f;


        
        IEnumerator Start()
        {

            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(0.5f);
            yield return GetComponent<SavingSystem>().LoadLastScene(DeafaultSavingFile);
            yield return fader.FadeIn(FadeInTime);
        }


        void Update()
        {

            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            
        }
        public void Load()
        {
            GetComponent<SavingSystem>().Load(DeafaultSavingFile);
        }
        public void Save()
        {
            GetComponent<SavingSystem>().Save(DeafaultSavingFile);
        }
    }
}
