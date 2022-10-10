using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBird
{
	public class MaxAdsWrapper : MonoBehaviour
	{
		public void Initialize()
		{
			//MaxSdk.SetSdkKey(AppLovinSettings.Instance.SdkKey);
			MaxSdk.SetSdkKey("");
			MaxSdk.SetUserId("DebugUser");
			MaxSdk.InitializeSdk();


#if UNITY_EDITOR || UNITY_IPHONE || UNITY_IOS
			
			MaxSdkCallbacks.OnSdkInitializedEvent += OnSDKInitializedEventMethod;

#endif
		}

#if UNITY_EDITOR || UNITY_IPHONE || UNITY_IOS
		private void OnSDKInitializedEventMethod(MaxSdk.SdkConfiguration config)
		{
			if(config.AppTrackingStatus == MaxSdkBase.AppTrackingStatus.Authorized) {
				
			}
		}
#endif
	}
}
