/*using System;
using UnityEngine;

namespace Managers
{
    public class IronSourceManager : MonoBehaviour
    {
        private string appKey = "YOUR_APP_KEY_HERE";

        // Action callbacks
        private Action onOpen;
        private Action onClose;
        private Action onFail;
        private Action<int> onReward;
        private Action onStart;
        private Action onEnd;
        private Action onClick;

        private void Start()
        {
            InitializeIronSource();
        }

        private void InitializeIronSource()
        {
            IronSource.Agent.init(appKey, IronSourceAdUnits.REWARDED_VIDEO);
            IronSource.Agent.validateIntegration();

            // Subscribe to IronSource events
            IronSourceEvents.onRewardedVideoAdOpenedEvent += HandleOnOpen;
            IronSourceEvents.onRewardedVideoAdClosedEvent += HandleOnClose;
            IronSourceEvents.onRewardedVideoAdShowFailedEvent += HandleOnFail;
            IronSourceEvents.onRewardedVideoAdRewardedEvent += HandleOnReward;
            IronSourceEvents.onRewardedVideoAdStartedEvent += HandleOnStart;
            IronSourceEvents.onRewardedVideoAdEndedEvent += HandleOnEnd;
            IronSourceEvents.onRewardedVideoAdClickedEvent += HandleOnClick;
        }

        public IronSourceManager ShowRewardedVideo()
        {
            if (IronSource.Agent.isRewardedVideoAvailable())
            {
                IronSource.Agent.showRewardedVideo();
            }
            else
            {
                Debug.LogWarning("Rewarded video not available");
                onFail?.Invoke();
            }
            return this;
        }

        #region Fluent Interface for Callbacks

        public IronSourceManager OnOpen(Action callback)
        {
            onOpen = callback;
            return this;
        }

        public IronSourceManager OnClosed(Action callback)
        {
            onClose = callback;
            return this;
        }

        public IronSourceManager OnFail(Action callback)
        {
            onFail = callback;
            return this;
        }

        public IronSourceManager OnReward(Action<int> callback)
        {
            onReward = callback;
            return this;
        }

        public IronSourceManager OnStart(Action callback)
        {
            onStart = callback;
            return this;
        }

        public IronSourceManager OnEnd(Action callback)
        {
            onEnd = callback;
            return this;
        }

        public IronSourceManager OnClick(Action callback)
        {
            onClick = callback;
            return this;
        }

        #endregion

        #region IronSource Callbacks

        private void HandleOnOpen()
        {
            Debug.Log("Rewarded Video Ad Opened");
            onOpen?.Invoke();
        }

        private void HandleOnClose()
        {
            Debug.Log("Rewarded Video Ad Closed");
            onClose?.Invoke();
        }

        private void HandleOnFail(IronSourceError error)
        {
            Debug.LogError($"Rewarded Video Ad Show Failed: {error.getDescription()}");
            onFail?.Invoke();
        }

        private void HandleOnReward(IronSourcePlacement placement)
        {
            int rewardAmount = placement.getRewardAmount();
            Debug.Log($"Reward received: {rewardAmount}");
            onReward?.Invoke(rewardAmount);
        }

        private void HandleOnStart()
        {
            Debug.Log("Rewarded Video Ad Started");
            onStart?.Invoke();
        }

        private void HandleOnEnd()
        {
            Debug.Log("Rewarded Video Ad Ended");
            onEnd?.Invoke();
        }

        private void HandleOnClick(IronSourcePlacement placement)
        {
            Debug.Log($"Rewarded Video Ad Clicked: {placement.getPlacementName()}");
            onClick?.Invoke();
        }

        #endregion

        private void OnDestroy()
        {
            // Unsubscribe from IronSource events
            IronSourceEvents.onRewardedVideoAdOpenedEvent -= HandleOnOpen;
            IronSourceEvents.onRewardedVideoAdClosedEvent -= HandleOnClose;
            IronSourceEvents.onRewardedVideoAdShowFailedEvent -= HandleOnFail;
            IronSourceEvents.onRewardedVideoAdRewardedEvent -= HandleOnReward;
            IronSourceEvents.onRewardedVideoAdStartedEvent -= HandleOnStart;
            IronSourceEvents.onRewardedVideoAdEndedEvent -= HandleOnEnd;
            IronSourceEvents.onRewardedVideoAdClickedEvent -= HandleOnClick;
        }
    }
}*/