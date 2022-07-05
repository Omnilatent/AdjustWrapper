using com.adjust.sdk;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omnilatent.AdjustUnity
{
    public class AdjustWrapper : MonoBehaviour
    {
        [SerializeField] string appToken;
        [SerializeField] AdjustLogLevel debugLogLevel = AdjustLogLevel.Verbose;

        void Start()
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

        /// <param name="amount">Amount of revenue</param>
        /// <param name="currencyCode">Currency of revenue</param>
        public static void TrackRevenueAdmob(double amount, string currencyCode)
        {
            AdjustAdRevenue adRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAdMob);
            adRevenue.setRevenue(amount / 1000000f, currencyCode);
            Adjust.trackAdRevenue(adRevenue);
        }

        public static void TrackRevenueMAX(double amount, string currencyCode, string adRevenueNetwork, string adRevenueUnit, string adRevenuePlacement)
        {
            var adRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAppLovinMAX);
            adRevenue.setRevenue(amount, currencyCode);
            adRevenue.setAdRevenueNetwork(adRevenueNetwork);
            adRevenue.setAdRevenueUnit(adRevenueUnit);
            adRevenue.setAdRevenuePlacement(adRevenuePlacement);
            Adjust.trackAdRevenue(adRevenue);
        }
    }
}