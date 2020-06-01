using platformer.attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace platformer.ui
{
    /// <summary>
    /// Display health of player
    /// </summary>
    public class HealthDisplay : MonoBehaviour
    {
        private Text textComponent; // Text attached to gameobject
        private Health healthComponent; // Health of player

        private void Awake()
        {
            textComponent = GetComponent<Text>();
            healthComponent = GameObject.FindWithTag("Player").GetComponent<Health>();
            if(healthComponent == null || textComponent == null)
            {
                Debug.LogError("Health or Text not initialized in HealthDisplay. Possible race conditions");
            }
        }

        // Update UI
        private void Update()
        {
            textComponent.text = string.Format("{0}/{1}", healthComponent.GetCurrentHealth(),
                healthComponent.maxHealth);
        }
    }

}