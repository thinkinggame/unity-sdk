﻿
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ThinkingAnalytics.Utils;
using UnityEngine;

namespace ThinkingAnalytics.Wrapper
{
    public partial class ThinkingAnalyticsWrapper
    {
#if UNITY_IOS && !(UNITY_EDITOR)
        [DllImport("__Internal")]
        private static extern void start(string app_id, string server_url);
        [DllImport("__Internal")]
        private static extern void identify(string app_id, string unique_id);
        [DllImport("__Internal")]
        private static extern string get_distinct_id(string app_id);
        [DllImport("__Internal")]
        private static extern void login(string app_id, string account_id);
        [DllImport("__Internal")]
        private static extern void logout(string app_id);
        [DllImport("__Internal")]
        private static extern void track(string app_id, string event_name, string properties, long time_stamp_millis);
        [DllImport("__Internal")]
        private static extern void set_super_properties(string app_id, string properties);
        [DllImport("__Internal")]
        private static extern void unset_super_property(string app_id, string property_name);
        [DllImport("__Internal")]
        private static extern void clear_super_properties(string app_id);
        [DllImport("__Internal")]
        private static extern string get_super_properties(string app_id);
        [DllImport("__Internal")]
        private static extern void time_event(string app_id, string event_name);
        [DllImport("__Internal")]
        private static extern void user_set(string app_id, string properties);
        [DllImport("__Internal")]
        private static extern void user_set_once(string app_id, string properties);
        [DllImport("__Internal")]
        private static extern void user_add(string app_id, string properties);
        [DllImport("__Internal")]
        private static extern void user_delete(string app_id);
        [DllImport("__Internal")]
        private static extern void flush(string app_id);
        [DllImport("__Internal")]
        private static extern void set_network_type(int type);
        [DllImport("__Internal")]
        private static extern void enable_log(bool is_enable);

        private void init(string app_id, string serverUrl, bool enableLog)
        {
            start(app_id, serverUrl);
            enable_log(enableLog);
        }

        private void identify(string uniqueId)
        {
            identify(token.appid, uniqueId);
        }

        private string getDistinctId()
        {
            return get_distinct_id(token.appid);
        }

        private void login(string accountId)
        {
            login(token.appid, accountId);
        }

        private void logout()
        {
            logout(token.appid);
        }


        private void flush()
        {
            flush(token.appid);
        }

        private void track(string eventName, Dictionary<string, object> properties)
        {
            track(token.appid, eventName, TD_MiniJSON.Serialize(properties), 0);
        }

        private void track(string eventName, Dictionary<string, object> properties, DateTime dateTime)
        {
            long dateTimeTicksUTC = TimeZoneInfo.ConvertTimeToUtc(dateTime).Ticks;

            DateTime dtFrom = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            long currentMillis = (dateTimeTicksUTC - dtFrom.Ticks) / 10000;

            track(token.appid, eventName, TD_MiniJSON.Serialize(properties), currentMillis);
        }

        private void setSuperProperties(Dictionary<string, object> superProperties)
        {
            string properties = TD_MiniJSON.Serialize(superProperties);
            set_super_properties(token.appid, properties);
        }

        private void unsetSuperProperty(string superPropertyName)
        {
            unset_super_property(token.appid, superPropertyName);
        }

        private void clearSuperProperty()
        {
            clear_super_properties(token.appid);
        }

        private Dictionary<string, object> getSuperProperties()
        {
            string superPropertiesString = get_super_properties(token.appid);
            return TD_MiniJSON.Deserialize(superPropertiesString);
        }

        private void timeEvent(string eventName)
        {
            time_event(token.appid, eventName);
        }

        private void userSet(Dictionary<string, object> properties)
        {
            user_set(token.appid, TD_MiniJSON.Serialize(properties));
        }

        private void userSetOnce(Dictionary<string, object> properties)
        {
            user_set_once(token.appid, TD_MiniJSON.Serialize(properties));
        }

        private void userAdd(Dictionary<string, object> properties)
        {
            user_add(token.appid, TD_MiniJSON.Serialize(properties));
        }

        private void userDelete()
        {
            user_delete(token.appid);
        }

        private void setNetworkType(ThinkingAnalyticsAPI.NetworkType networkType)
        {
            set_network_type((int)networkType);
        }
#endif
    }
}