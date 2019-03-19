using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdmobNetwork : AdNetwork
{
    [SerializeField]
    private string appID;
    [SerializeField]
    private string interstitialID = "ca-app-pub-3940256099942544/1033173712";
    [SerializeField]
    private string rewardedVideoID = "ca-app-pub-3940256099942544/5224354917";

    private RewardBasedVideoAd rewarded;
    private InterstitialAd interstitial;
    private Action OnRewardedCompleted;

    public override bool RewardedAvailable { get { return rewarded != null && rewarded.IsLoaded(); } }

    public override bool InterstitialAvailable { get { return interstitial != null && interstitial.IsLoaded(); } }

    public override void Setup()
    {
        networkType = NetworkType.Admob;
    }

    public override void Init()
    {
        MobileAds.SetiOSAppPauseOnBackground(true);
        MobileAds.Initialize("");

        rewarded = RewardBasedVideoAd.Instance;
        rewarded.OnAdLoaded += HandleRewardBasedVideoLoaded;
        rewarded.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        rewarded.OnAdOpening += HandleRewardBasedVideoOpened;
        rewarded.OnAdStarted += HandleRewardBasedVideoStarted;
        rewarded.OnAdRewarded += HandleRewardBasedVideoRewarded;
        rewarded.OnAdClosed += HandleRewardBasedVideoClosed;
        rewarded.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;

        RequestInterstitial();
        RequestRewardedVideo();
    }

    public override void ShowInterstitial()
    {
        if (!InterstitialAvailable)
        {
            RequestInterstitial();
            return;
        }

        interstitial.Show();
    }

    public override void ShowRewardedVideo(Action OnVideoAdComplete = null)
    {
        if (!RewardedAvailable)
        {
            RequestRewardedVideo();
            return;
        }

        OnRewardedCompleted = OnVideoAdComplete;
        rewarded.Show();
    }

    #region Helpers

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().
            //AddTestDevice("your test device id").
            Build();
    }

    private void RequestInterstitial()
    {
        if (interstitial != null)
            interstitial.Destroy();

        interstitial = new InterstitialAd(interstitialID);

        interstitial.OnAdLoaded += HandleOnAdLoaded;
        interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        interstitial.OnAdOpening += HandleOnAdOpened;
        interstitial.OnAdClosed += HandleOnAdClosed;
        interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        interstitial.LoadAd(CreateAdRequest());
    }

    private void RequestRewardedVideo()
    {
        rewarded.LoadAd(CreateAdRequest(), rewardedVideoID);
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        networkType = NetworkType.Admob;
    }
#endif
#endregion

    #region Interstitial Callbacks

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        print("Ads: Admob | HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("Ads: Admob | HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        print("Ads: Admob | HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        print("Ads: Admob | HandleAdClosed event received");
        RequestInterstitial();
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        print("Ads: Admob | HandleAdLeavingApplication event received");
    }

    #endregion

    #region Rewarded Callbacks

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        print("Ads: Admob | HandleRewardBasedVideoLoaded event received");
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("Ads: Admob | HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        print("Ads: Admob | HandleRewardBasedVideoOpened event received");
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        print("Ads: Admob | HandleRewardBasedVideoStarted event received");
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        print("Ads: Admob | HandleRewardBasedVideoClosed event received");
        RequestRewardedVideo();
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        print("Ads: Admob | HandleRewardBasedVideoRewarded event received");
        if (OnRewardedCompleted != null)
            OnRewardedCompleted();

        OnRewardedCompleted = null;
        RequestRewardedVideo();
    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        print("Ads: Admob | HandleRewardBasedVideoLeftApplication event received");
    }

    #endregion
}
