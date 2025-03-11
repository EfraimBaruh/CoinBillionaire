/*using System;
using UnityEngine;

namespace Managers
{
    public class IronSourceManager : MonoBehaviour
    {
        private string appKey = "214ccb4a5";

        // Action callbacks
        private Action onOpen;
        private Action onClose;
        private Action onFail;
        private Action<int> onReward;
        private Action onLoadFail;
        private Action onUnAvailable;
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
            IronSourceRewardedVideoEvents.onAdRewardedEvent += HandleOnReward;
            IronSourceRewardedVideoEvents.onAdClickedEvent +=  HandleOnClick;
            IronSourceRewardedVideoEvents.onAdOpenedEvent +=   HandleOnOpen;
            IronSourceRewardedVideoEvents.onAdClosedEvent +=   HandleOnClose;
            IronSourceRewardedVideoEvents.onAdUnavailableEvent += HandleOnAdUnAvailable;
            IronSourceRewardedVideoEvents.onAdShowFailedEvent  += HandleOnFail;
            IronSourceRewardedVideoEvents.onAdLoadFailedEvent  += HandleOnLoadFail;
            
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

        public IronSourceManager OnClick(Action callback)
        {
            onClick = callback;
            return this;
        }

        public IronSourceManager OnUnavailable(Action callback)
        {
            onUnAvailable = callback;
            return this;
        }

        public IronSourceManager OnLoadFail(Action callback)
        {
            onLoadFail = callback;
            return this;
        }

        #endregion

        #region IronSource Callbacks

        private void HandleOnOpen(IronSourceAdInfo adInfo)
        {
            Debug.Log("Rewarded Video Ad Opened");
            onOpen?.Invoke();
        }

        private void HandleOnClose(IronSourceAdInfo adInfo)
        {
            Debug.Log("Rewarded Video Ad Closed");
            onClose?.Invoke();
        }

        private void HandleOnFail(IronSourceError error, IronSourceAdInfo adInfo)
        {
            Debug.LogError($"Rewarded Video Ad Show Failed: {error.getDescription()}");
            onFail?.Invoke();
        }

        private void HandleOnReward(IronSourcePlacement placement, IronSourceAdInfo adInfo)
        {
            int rewardAmount = placement.getRewardAmount();
            Debug.Log($"Reward received: {rewardAmount}");
            onReward?.Invoke(rewardAmount);
        }

        private void HandleOnClick(IronSourcePlacement placement, IronSourceAdInfo adInfo)
        {
            Debug.Log($"Rewarded Video Ad Clicked: {placement.getPlacementName()}");
            onClick?.Invoke();
        }

        private void HandleOnAdUnAvailable()
        {
            Debug.Log($"Rewarded Video Ad Unavailable:");
        }

        private void HandleOnLoadFail(IronSourceError error)
        {
            Debug.Log($"Rewarded Video Ad Load failed: {error.getDescription()}");
        }
        #endregion

        private void OnDestroy()
        {
            // Unsubscribe from IronSource events
            IronSourceRewardedVideoEvents.onAdRewardedEvent -= HandleOnReward;
            IronSourceRewardedVideoEvents.onAdClickedEvent  -= HandleOnClick;
            IronSourceRewardedVideoEvents.onAdOpenedEvent   -= HandleOnOpen;
            IronSourceRewardedVideoEvents.onAdClosedEvent   -= HandleOnClose;
            IronSourceRewardedVideoEvents.onAdUnavailableEvent -= HandleOnAdUnAvailable;
            IronSourceRewardedVideoEvents.onAdShowFailedEvent  -= HandleOnFail;
            IronSourceRewardedVideoEvents.onAdLoadFailedEvent  -= HandleOnLoadFail;
        }
    }
}*/