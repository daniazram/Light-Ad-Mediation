using UnityEngine;

public enum NetworkType { UnityAds, Admob, Ironsource };
public class AdNetwork : MonoBehaviour
{
    [InspectorReadOnly] // Make sure you don't actually bind the network type to wrong network.
    public NetworkType networkType;
    protected System.Action OnVideoAdComplete;

    public virtual bool RewardedAvailable
    { get; }

    public virtual bool InterstitialAvailable
    { get; }

    public virtual void Setup()
    { }

    public virtual void Init()
    { }

    public virtual void ShowInterstitial()
    { }

    public virtual void ShowRewardedVideo(System.Action OnVideoAdComplete = null)
    { }

#if UNITY_EDITOR
    protected virtual void OnValidate()
    { }
#endif
}
