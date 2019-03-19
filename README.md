# Light-Ad-Mediation

#### NOTE: This project is built with Unity 2018.3.4, if you face any problem try upgrading to Unity 2018.3 or above. Make sure you have set your build target to Android or iOS before importing. This project has only been tested with Internal Build System. Make sure you have added the apropriate keys before trying to show ads, keys that are already there are official testing keys. Read the relevant Network's docs for more info on how to show test ads. 

##### Update: All the official SDKs for the networks in use (UnityAds, Admob, Ironsource) are already included and tested. I will be updating the SDKs when necessary. 

## About
This is simple and easy to extend Ad Mediation for mobile platforms (Android and iOS only) currently it supports Unity Ads, Admob and Ironsource only and you can only display following ad type:

- Interstitial 
- Rewarded Video

But you can easily extend and make it compatible with banner ads as well.

## How To Use
- Drag and drop the **AdManager/Prefabs/AdManager.prefab** into the scene.  ![ads-prefab](https://user-images.githubusercontent.com/12896256/54632789-aed61800-4aa0-11e9-922e-290e530c9f6c.PNG)

- The prefab has the **AdManager.cs** script on it you can change the priority of each network here.   
![ads-prefabInspector](https://user-images.githubusercontent.com/12896256/54632915-e775f180-4aa0-11e9-8211-31f0660be84b.PNG)

- There is **Networks** GameObject inside the AdManager prefab this is where all the networks reside.  ![ads-prefabNetworks](https://user-images.githubusercontent.com/12896256/54632807-b4cbf900-4aa0-11e9-95db-02fb401c59a3.PNG)

- **AdManager.cs** has a static instance which you can use to display ads wherever you want.  ![ads-scriptDemo](https://user-images.githubusercontent.com/12896256/54633502-1d67a580-4aa2-11e9-962b-f0b58b5edd5b.PNG)

- With `ShowRewardedVideo(Action OnCompleted, Action OnNotAvailble)` function you can decouple the game logic from the ads logic and define what happens when user completes the ad or when there isn't any ad available.

- `AdManager.Instance.RewardedAvailable` and `AdManager.Instance.InterstitialAvailble` properties can be used to perform any checks before showing the ad. Note that the system will check if any ad is available when you make calls to show the ad, but if you want some logic based on the ads availability (e.g. disabling/enabling buttons that would reward based on the ads) you can use the properties too.

- A consistent Debug Log format has been used throughout the AdManager that can help you find logs once you want to debug the app on device. In the monitor search for **Ads:** and  you will start seeing only the relevant logs.

- This Ad Manager auto refills the ads so you don't need to explicitly make any API call to reload ads.

## Extension
If you wish to ad more networks you can do so by simply inherting from the **AdNetwork.cs** and overriding the virtual properties/functions. Before you start writing your own network there are few things you should take care of:

- `[InspectorReadOnly]NetworkType networkType` field of every network is intentionally markd as readonly so you don't accidently map the network type to wrong network (e.g. you might accidently set the AdmobNetwork's `networkType` to `NetworkType.UnityAds` in the inspector.)

- Priority works on the basis of `NetworkType` enum so make sure to update the enum with the new network that you add. For example if you add AdColonyNetwork.cs don't forget to update the enum `enum NetworkType {UnityAds, Admob, Ironsource, AdColony};`

- **AdNetwork.cs** has a `virtual Setup()` function this gets called before initializing any ad network, this is where you should assign the `networkType = NetwrokType.NewTypeYouCreated` just to make sure that we aren't mapping the wrong networks.
