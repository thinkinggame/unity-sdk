using UnityEngine;
using ThinkingAnalytics;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class TAExample : MonoBehaviour
{
    public GUISkin skin;

    void OnGUI() {
        GUI.skin = this.skin;

        GUILayout.BeginArea(new Rect(Screen.width * 0.15f, Screen.height * 0.3f, Screen.width * 0.7f, Screen.height * 2.5f));

        if (GUILayout.Button("TRACK TEST_EVENT"))
        {
            // a simple tracking call
            Dictionary<string, object> properties = new Dictionary<string, object>()
            {
                {"KEY_DateTime", DateTime.Now.AddDays(1)},
                {"KEY_STRING", "B1"},
                {"KEY_BOOL", true},
                {"KEY_NUMBER", 50.65}
            };
            DateTime dateTime = DateTime.Now.AddDays(-1);
            ThinkingAnalyticsAPI.Track("TEST_EVENT", properties, dateTime);
        }

        if (GUILayout.Button("LOGIN UNITY_USER")) // 设置 account ID
        {
            ThinkingAnalytics.ThinkingAnalyticsAPI.Login("unity_user");
        }
        if (GUILayout.Button("LOGOUT")) // 清除 account ID
        {
            ThinkingAnalytics.ThinkingAnalyticsAPI.Logout();
        }
        if (GUILayout.Button("SET SUPER_PROPERTIES")) // 设置公共属性
        {
            Dictionary<string, object> superProperties = new Dictionary<string, object>()
            {
                {"SUPER_LEVEL", 0},
                {"SUPER_CHANNEL", "A3"}
            };
            ThinkingAnalyticsAPI.SetSuperProperties(superProperties);
        }

        if (GUILayout.Button("UNSET SUPER_CHANNEL")) // 清除某条公共属性
        {
            ThinkingAnalyticsAPI.UnsetSuperProperty("SUPER_CHANNEL");
        }

        if (GUILayout.Button("CLEAR SUPER_PROPERTIES")) // 清除公共属性
        {
            ThinkingAnalyticsAPI.ClearSuperProperties();
        }

        if (GUILayout.Button("SET USER_PROPERTIES")) // 设置用户属性
        {
            ThinkingAnalyticsAPI.UserSet(new Dictionary<string, object>(){
                {"USER_PROP_NUM", 0},
                {"USER_PROP_STRING", "A3"}
            });

            ThinkingAnalyticsAPI.UserSetOnce(new Dictionary<string, object>(){
                {"USER_PROP_NUM", -50},
                {"USER_PROP_STRING", "A3"}
            });

            ThinkingAnalyticsAPI.UserAdd(new Dictionary<string, object>(){
                {"USER_PROP_NUM", -100.9},
                {"USER_PROP_NUM2", 10.0}
            });
        }

        if (GUILayout.Button("FLUSH")) // an engage call
        {
            ThinkingAnalyticsAPI.Flush();
        }

        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "scene1")
        {
            // Show a button to allow scene2 to be switched to.
            if (GUILayout.Button("LOAD SAMPLE"))
            {
                SceneManager.LoadScene("Sample");
            }
        }
        else
        {
            // Show a button to allow scene1 to be returned to.
            if (GUILayout.Button("LOAD SCENE1"))
            {
                    SceneManager.LoadScene("scene1");
            }
        }
        GUILayout.EndArea();
    }

    void Start () {
        // 设置 Distinct ID
        ThinkingAnalyticsAPI.Identify("unity_id");
        ThinkingAnalyticsAPI.Identify("unity_debug_id", "debug-appid");
        Debug.Log("TA.TAExample - current disctinct ID is: " + ThinkingAnalyticsAPI.GetDistinctId());

        // 清除公共事件属性
        ThinkingAnalyticsAPI.ClearSuperProperties();

        // Track 简单事件
        Scene scene = SceneManager.GetActiveScene();
        ThinkingAnalyticsAPI.Track("unity_start", new Dictionary<string, object>() {
            {"SCENE_NAME", scene.name}
        });

        if (scene.name == "scene1")
        {
            // 设置 SuperProperties
            Dictionary<string, object> superProperties = new Dictionary<string, object>()
            {
                {"super_date", DateTime.Now.AddDays(1)},
                {"super_string", "B1"},
                {"super_bool", true},
                {"super_number", 100}
            };
            ThinkingAnalyticsAPI.SetSuperProperties(superProperties);
            Dictionary<string, object> response = ThinkingAnalyticsAPI.GetSuperProperties();

            // 测试公共事件属性返回值
            if (response != null)
            {
                foreach (KeyValuePair<string, object> kv in response)
                {
                    if (kv.Value is DateTime)
                    {
                        Debug.LogWarning("TA.TAExample - Returned super property date: " + ((DateTime)kv.Value).ToString("yyyy-MM-dd"));
                    }
                    if (kv.Value is bool)
                    {
                        Debug.LogWarning("TA.TAExample - Returned super property bool: " + Convert.ToBoolean(kv.Value));
                    }

                    Debug.LogWarning("TA.TAExample - Returned super property: " + kv.Key + ": " + kv.Value);
                }

            }
        }
    }
}
