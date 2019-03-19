using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsNetwork : AdNetwork
{
    [SerializeField]
    private bool testing;
    [SerializeField]
    private string gameIDIos;
    [SerializeField]
    private string gameIDAndroid;
    [SerializeField]
    private string rewardedID = "rewardedVideo";
    [SerializeField]
    private string interstitialID = "video";
    
    public override bool RewardedAvailable { get { return Advertisement.IsReady(rewardedID); } }

    public override bool InterstitialAvailable { get { return Advertisement.IsReady(interstitialID); } }

    public override void Setup()
    {
        networkType = NetworkType.UnityAds;
    }

    public override void Init()
    {
        var id = gameIDAndroid;
        #if UNITY_IOS
        id = gameIDIos;
        #endif

        Advertisement.Initialize(id, testing);
    }

    public override void ShowInterstitial()
    {
        if (InterstitialAvailable)
            Advertisement.Show(interstitialID);
    }

    public override void ShowRewardedVideo(System.Action OnVideoAdComplete = null)
    {
        if (RewardedAvailable)
        {
            var options = new ShowOptions { resultCallback = RewardedCallbacks };

            this.OnVideoAdComplete = OnVideoAdComplete;
            Advertisement.Show(rewardedID, options);
        }
    }

    #region Helpers

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        networkType = NetworkType.UnityAds;
    }
#endif

    #endregion

    #region Ads Callbacks

    private void RewardedCallbacks(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:

                print("Ads: UnityAds | Rewarded Video Completed");
                if (OnVideoAdComplete != null)
                    OnVideoAdComplete();

                OnVideoAdComplete = null;
                break;

            case ShowResult.Skipped:

                print("Ads: UnityAds | Rewarded Video Skipped");
                break;

            case ShowResult.Failed:

                print("Ads: UnityAds | Rewarded Video failed to be shown");
                break;
        }
    }

    #endregion
}
