using UnityEngine;

public class IronSourceNetwork : AdNetwork
{
    [SerializeField]
    private string appKey;

    public override bool RewardedAvailable { get { return IronSource.Agent.isRewardedVideoAvailable(); } }

    public override bool InterstitialAvailable { get { return IronSource.Agent.isInterstitialReady(); } }

    public override void Setup()
    {
        networkType = NetworkType.Ironsource;
    }

    public override void Init()
    {
        IronSource.Agent.init(appKey);
        HookEventDelegates();
        IronSource.Agent.validateIntegration();

        IronSource.Agent.loadInterstitial();
    }

    public override void ShowInterstitial()
    {
        IronSource.Agent.showInterstitial();
    }

    public override void ShowRewardedVideo(System.Action OnVideoAdComplete = null)
    {
        this.OnVideoAdComplete = OnVideoAdComplete;
        IronSource.Agent.showRewardedVideo();
    }

    #region Helpers

    private void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        networkType = NetworkType.Ironsource;
    }
#endif

    private void HookEventDelegates()
    {
        IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
        IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
        IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
        IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
        IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
        IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
        IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;

        IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
        IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
        IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
        IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
        IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
        IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
        IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;
    }

    #endregion

    #region Interstitial Callbacks

    void InterstitialAdReadyEvent()
    {
        print("Ads: IronSource | Interstitial Ad Ready");
    }

    void InterstitialAdLoadFailedEvent(IronSourceError error)
    {
        print("Ads: IronSource | Interstitial Ad Load Failed, code: " + error.getCode() + ", description : " + error.getDescription());
    }

    void InterstitialAdShowSucceededEvent()
    {
        print("Ads: IronSource | Interstitial Ad Show Succeeded");
    }

    void InterstitialAdShowFailedEvent(IronSourceError error)
    {
        print("Ads: IronSource | Interstitial Ad Show Failed , code :  " + error.getCode() + ", description : " + error.getDescription());
    }

    void InterstitialAdClickedEvent()
    {
        print("Ads: IronSource | Interstitial Ad Clicked");
    }

    void InterstitialAdOpenedEvent()
    {
        print("Ads: IronSource | Interstitial Ad Opened");
    }

    void InterstitialAdClosedEvent()
    {
        print("Ads: IronSource | Interstitial Ad Closed");
        IronSource.Agent.loadInterstitial();
    }

    void InterstitialAdRewardedEvent()
    {
        print("Ads: IronSource | Interstitial Ad Rewarded");
    }

    #endregion

    #region Rewarded Video Callbacks

    void RewardedVideoAvailabilityChangedEvent(bool canShowAd)
    {
        print("Ads: IronSource | Rewarded Video Availability Changed to " + canShowAd);
    }

    void RewardedVideoAdOpenedEvent()
    {
        print("Ads: IronSource | RewardedVideo Ad Opened");
    }

    void RewardedVideoAdRewardedEvent(IronSourcePlacement ssp)
    {
        print("Ads: IronSource | RewardedVideo Ad Rewarded");
        if (OnVideoAdComplete != null)
            OnVideoAdComplete();
        OnVideoAdComplete = null;
    }

    void RewardedVideoAdClosedEvent()
    {
        print("Ads: IronSource | RewardedVideo Ad Closed");
    }

    void RewardedVideoAdStartedEvent()
    {
        print("Ads: IronSource | RewardedVideo Ad Started");
    }

    void RewardedVideoAdEndedEvent()
    {
        print("Ads: IronSource | RewardedVideo Ad Ended");
    }

    void RewardedVideoAdShowFailedEvent(IronSourceError error)
    {
        print("Ads: IronSource | RewardedVideo Ad Show Failed, code :  " + error.getCode() + ", description : " + error.getDescription());
    }

    void RewardedVideoAdClickedEvent(IronSourcePlacement ssp)
    {
        print("Ads: IronSource | RewardedVideo Ad Clicked, name = " + ssp.getRewardName());
    }

    #endregion
}
