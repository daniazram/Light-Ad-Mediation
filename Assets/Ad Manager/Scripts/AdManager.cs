using UnityEngine;
using System.Collections.Generic;

public enum AdType { Interstitial, Rewarded};
public class AdManager : MonoBehaviour
{
    [SerializeField]
    private NetworkType[] priority;

    private static AdManager instance;
    private AdNetwork[] attachedNetworks;
    private Dictionary<NetworkType, AdNetwork> availableNetworks;

    public bool RemoveAds
    {
        get { return PlayerPrefs.GetInt("RA") == 1; }

        set { PlayerPrefs.SetInt("RA", value ? 1 : 0); }
    }
    
    public bool RewardedAvailable
    {
        get
        {
            foreach (var network in attachedNetworks)
                if (network.RewardedAvailable)
                    return true;

            return false;

        }//GetPrioritizedNetwork(AdType.Rewarded).RewardedAvailable; }
    }

    public bool InterstitialAvailable
    {
        get
        {
            foreach (var network in attachedNetworks)
                if (network.RewardedAvailable)
                    return true;

            return false;

        }//GetPrioritizedNetwork(AdType.Interstitial).InterstitialAvailable; }
    }

    public static AdManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<AdManager>();

            return instance;
        }
    }

    private void Awake()
    {
        Init();
        DontDestroyOnLoad(this);
    }

    private void Init()
    {
        attachedNetworks = GetComponentsInChildren<AdNetwork>();
        availableNetworks = new Dictionary<NetworkType, AdNetwork>();

        foreach (var network in attachedNetworks)
        { network.Setup(); network.Init(); availableNetworks[network.networkType] = network; }
    }

    public void ShowInterstitial()
    {
        if(RemoveAds)
        {
            print("Ads: Can't show ads because Remove Ads Purchased.");
            return;
        }

        AdNetwork network = GetPrioritizedNetwork(AdType.Interstitial);
        if (network != null)
            network.ShowInterstitial();
        else
            print("Ads: No interstitial is ready.");
    }

    public void ShowRewardedVideo()
    {
        ShowRewardedVideo(null);
    }

    public void ShowRewardedVideo(System.Action OnVideoAdCompleted = null, System.Action OnVideoNotAvailable = null)
    {
        AdNetwork network = GetPrioritizedNetwork(AdType.Rewarded);

        if (network != null)
        {
            network.ShowRewardedVideo(OnVideoAdCompleted);
        }
        else
        {
            print("Ads: No rewarded video is ready.");
            OnVideoNotAvailable?.Invoke();
        }
    }

    private AdNetwork GetPrioritizedNetwork(AdType type)
    {
        var i = 0;
        var priortizedType = priority[i];
        AdNetwork network = availableNetworks[priortizedType];
        if (type == AdType.Rewarded)
        {
            while (network && !network.RewardedAvailable)
            {
                i++;
                if (i >= availableNetworks.Count)
                    return null;

                priortizedType = priority[i];
                network = availableNetworks[priortizedType];
            }
        }
        else if(type == AdType.Interstitial)
        {
            while (network && !network.InterstitialAvailable)
            {
                i++;
                if (i >= availableNetworks.Count)
                    return null;

                priortizedType = priority[i];
                network = availableNetworks[priortizedType];
            }
        }
        
        return network;
    }
}