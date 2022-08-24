using com.adjust.sdk;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omnilatent.AdjustUnity
{
    public class AdjustWrapper : MonoBehaviour
    {
        [SerializeField] bool initializeAutomatically = true;
        [SerializeField] string appToken;
        [SerializeField] AdjustLogLevel debugLogLevel = AdjustLogLevel.Verbose;

        [Tooltip("If true, automatically ask for ATT consent (iOS) on initialize. If false, you have to manually call CheckForNewAttStatus() after asking for ATT consent.")]
        [SerializeField] bool askTrackingConsentOnInit = false;

        [SerializeField] bool logConsoleDebugBuild = true;

        static AdjustWrapper instance;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            if (initializeAutomatically)
            {
                Init();
            }
        }

        public void Init()
        {
            AdjustEnvironment environment = AdjustEnvironment.Production;
            if (Debug.isDebugBuild)
            {
                environment = AdjustEnvironment.Sandbox;
            }
            else
            {
                debugLogLevel = AdjustLogLevel.Error;
            }
            AdjustConfig config = new AdjustConfig(appToken, environment);
            config.setLogLevel(debugLogLevel);
            config.setUrlStrategy(AdjustConfig.AdjustUrlStrategyIndia);
            Adjust.start(config);

            if (askTrackingConsentOnInit)
            {
                RequestAttConsent();
            }
            //Does not need to check user country from India, use strategy India by default for everyone
            /*LocationUtils.GetUserCountryAsync((LocationUtils.IpApiData ipData) =>
            {
                if (ipData != null && ipData.countryCode == "IN")
                {
                    config.setUrlStrategy(AdjustConfig.AdjustUrlStrategyIndia);
                }
                Adjust.start(config);
            });*/
        }

        public static void CheckForNewAttStatus(bool statusIsNotDetermined)
        {
            if (statusIsNotDetermined)
            {
                RequestAttConsent();
            }
            else
            {
                Adjust.checkForNewAttStatus();
            }
        }

        public static void RequestAttConsent()
        {
            Adjust.requestTrackingAuthorizationWithCompletionHandler((status) =>
            {
                switch (status)
                {
                    case 0:
                        // ATTrackingManagerAuthorizationStatusNotDetermined case
                        break;
                    case 1:
                        // ATTrackingManagerAuthorizationStatusRestricted case
                        break;
                    case 2:
                        // ATTrackingManagerAuthorizationStatusDenied case
                        break;
                    case 3:
                        // ATTrackingManagerAuthorizationStatusAuthorized case
                        break;
                }
            });
        }

        public static void LogEvent(string eventToken)
        {
            AdjustEvent adjustEvent = new AdjustEvent(eventToken);
            Adjust.trackEvent(adjustEvent);
        }

        public static void LogEventWithCallbackParam(string eventToken, string paramName, string paramValue)
        {
            AdjustEvent adjustEvent = new AdjustEvent(eventToken);
            adjustEvent.addCallbackParameter(paramName, paramValue);
            Adjust.trackEvent(adjustEvent);
        }

        public static void LogEventWithCallbackParams(string eventToken, List<List<string>> parameters)
        {
            AdjustEvent adjustEvent = new AdjustEvent(eventToken);
            for (int i = 0; i < parameters.Count; i++)
            {
                adjustEvent.addCallbackParameter(parameters[i][0], parameters[i][1]);
            }
            Adjust.trackEvent(adjustEvent);
        }

        /// <summary>
        /// Track revenue from Admob. Please pass raw amount value from Admob without any changes (DO NOT divide by 1000000).
        /// </summary>
        /// <param name="amount">Amount of revenue without modification</param>
        /// <param name="currencyCode">Currency of revenue</param>
        public static void TrackRevenueAdmob(double amount, string currencyCode)
        {
            AdjustAdRevenue adRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAdMob);
            double finalRevenue = amount / 1000000f;
            adRevenue.setRevenue(finalRevenue, currencyCode);
            Adjust.trackAdRevenue(adRevenue);
            LogToConsole($"Adjust tracked Admob revenue: {finalRevenue} {currencyCode}");
        }

        public static void TrackRevenueMAX(double amount, string currencyCode, string adRevenueNetwork, string adRevenueUnit, string adRevenuePlacement)
        {
            var adRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAppLovinMAX);
            adRevenue.setRevenue(amount, currencyCode);
            adRevenue.setAdRevenueNetwork(adRevenueNetwork);
            adRevenue.setAdRevenueUnit(adRevenueUnit);
            adRevenue.setAdRevenuePlacement(adRevenuePlacement);
            Adjust.trackAdRevenue(adRevenue);
            LogToConsole($"Adjust tracked MAX revenue: {amount} {currencyCode}-{adRevenueNetwork}-{adRevenueUnit}-{adRevenuePlacement}");
        }

        static void LogToConsole(string message)
        {
            if (Debug.isDebugBuild && instance.logConsoleDebugBuild)
            {
                Debug.Log(message);
            }
        }
    }
}