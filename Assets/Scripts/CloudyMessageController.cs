using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CloudyMessageController : MonoBehaviour
    {
        [SerializeField] private Image image; // Controls the fade animation
        [SerializeField] private TMP_Text messageText;    // Displays the cloudy message text
        [SerializeField] private float fadeDuration = 1f; // Duration of the fade animation

        private bool boosterApplied = false; // Ensure the booster is applied only once


        // Initialize the cloudy message with the provided text and booster effect
        public void InitializeCloudyMessage(string message, float boosterEffect)
        {
            messageText.text = message;  // Set the message text
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);      // Start invisible
            messageText.color = new Color(messageText.color.r, messageText.color.g, messageText.color.b, 0);
            gameObject.SetActive(true); // Activate the GameObject

            RunMessage(boosterEffect);
        }

        private void RunMessage(float boosterEffect)
        {
            Sequence sequence = DOTween.Sequence();

            // Fade in both image and text
            sequence.AppendInterval(1f)
                .Join(image.DOFade(1, fadeDuration))
                .Join(messageText.DOFade(1, fadeDuration))
                .AppendInterval(fadeDuration)
                .OnComplete(() =>
                {
                    if (boosterApplied) return;
                    boosterApplied = true;
                    
                    // Fade out both image and text
                    Sequence fadeOutSequence = DOTween.Sequence();
                    fadeOutSequence
                        .Join(image.DOFade(0, fadeDuration))
                        .Join(messageText.DOFade(0, fadeDuration))
                        .OnComplete(() =>
                        {
                            gameObject.SetActive(false);
                            ApplyBoosterEffect(boosterEffect);
                        });
                });
        }

        // Example of applying the booster effect
        private void ApplyBoosterEffect(float boosterEffect)
        {
            Debug.Log($"Booster Effect Applied: {boosterEffect * 100}% increase!");
            // Add logic here to modify the coin's price or behavior
        }
    }
}