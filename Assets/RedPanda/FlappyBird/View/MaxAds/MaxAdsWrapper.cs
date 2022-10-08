using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBird
{
	public class MaxAdsWrapper : MonoBehaviour
	{
		public void Initialize()
		{
			MaxSdk.SetSdkKey(AppLovinSettings.Instance.SdkKey);
			MaxSdk.SetUserId("DebugUser");
			MaxSdk.InitializeSdk();

			MaxSdkCallbacks.OnSdkInitializedEvent += OnSDKInitializedEventMethod;
		}

		private void OnSDKInitializedEventMethod(MaxSdk.SdkConfiguration config)
		{
			if(config.AppTrackingStatus == MaxSdkBase.AppTrackingStatus.Authorized) {
				
			}
		}
	}
}
