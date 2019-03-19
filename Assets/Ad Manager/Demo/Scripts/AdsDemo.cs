using UnityEngine;

public class AdsDemo : MonoBehaviour
{
    public void ShowInterstitial()
    {
        AdManager.Instance.ShowInterstitial();
    }

    public void ShowRewarded()
    {
        AdManager.Instance.ShowRewardedVideo(OnRewardedVideoCompleted, OnVideoAdNotAvailable);
    }
    
    private void OnRewardedVideoCompleted()
    {
        print("You have completed the rewarded video.");
    }

    private void OnVideoAdNotAvailable()
    {
        print("Sorry no video ad available.");
    }
}
