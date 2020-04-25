using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

namespace DeveloperTools
{
    /// <summary>
    /// Enviro runtime weather and time modifier.
    /// Allows you to switch weather and time via keyboard shortcuts.
    /// Optionally weather and time can be displayed in text (menu UI -> "Text - TextMeshPro") objects in the UI.
    /// 
    /// Usage:
    /// 
    /// NumPad +/-: shift through predefined list of weather presets
    /// Alpha [1..0]: directly access a weather preset from a list of weather presets
    /// PageUp/Down: change time of day by hour
    /// 
    /// </summary>
    public class EnviroKeyHandler : MonoBehaviour
    {
        // predefined list in given order
        /*
         Weather Prefabs:
            0: Clear Sky(EnviroWeatherPrefab)
            1: Cloudy 1 (EnviroWeatherPrefab)
            2: Cloudy 2 (EnviroWeatherPrefab)
            3: Cloudy 3 (EnviroWeatherPrefab)
            4: Cloudy 4 (EnviroWeatherPrefab)
            5: Foggy 1 (EnviroWeatherPrefab)
            6: Light Rain(EnviroWeatherPrefab)
            7: Heavy Rain(EnviroWeatherPrefab)
            8: Light Snow(EnviroWeatherPrefab)
            9: Heavy Snow(EnviroWeatherPrefab)
            10: Storm(EnviroWeatherPrefab)

            (got the info via Log() method below) 
        */
        string[] weatherPrefabIdList = new string[] { "Clear Sky", "Cloudy 1", "Cloudy 2", "Cloudy 3", "Cloudy 4", "Foggy", "Light Rain", "Heavy Rain", "Storm", "Light Snow", "Heavy Snow" };
        int currentWeatherIdIndex = -1;

        public TextMeshProUGUI weatherTextMesh;
        public TextMeshProUGUI timeTextMesh;

        /// <summary>
        /// If this is set to true, then the keyboard combinations will only work when ctrl is pressed
        /// </summary>
        public bool modifierRequired = false;

        void Start()
        {
            UpdateUiText();
        }

        void Update()
        {
            if (EnviroSkyMgr.instance == null || !EnviroSkyMgr.instance.IsStarted())
                return;

            #region Modifier check

            bool modifierActive = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

            #endregion Modifier check

            #region Input Handler
            if (!modifierRequired || (modifierRequired && modifierActive))
            {

                #region Time of Day
                if (Input.GetKeyDown(KeyCode.PageUp))
                {
                    EnviroSkyMgr.instance.SetTimeOfDay(EnviroSkyMgr.instance.GetTimeOfDay() + 1f);
                }
                else if (Input.GetKeyDown(KeyCode.PageDown))
                {
                    EnviroSkyMgr.instance.SetTimeOfDay(EnviroSkyMgr.instance.GetTimeOfDay() - 1f);
                }
                #endregion Time of Day

                #region Weather
                if (Input.anyKeyDown)
                {
                    currentWeatherIdIndex = weatherPrefabIdList.ToList().IndexOf(EnviroSkyMgr.instance.GetCurrentWeatherPreset().name);

                    int originalWeatherIndexId = currentWeatherIdIndex;

                    if (Input.GetKeyDown(KeyCode.KeypadPlus))
                    {
                        currentWeatherIdIndex++;
                        if (currentWeatherIdIndex == weatherPrefabIdList.Length)
                        {
                            currentWeatherIdIndex = 0;
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.KeypadMinus))
                    {
                        currentWeatherIdIndex--;
                        if (currentWeatherIdIndex < 0)
                        {
                            currentWeatherIdIndex = weatherPrefabIdList.Length - 1;
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        currentWeatherIdIndex = 0;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        currentWeatherIdIndex = 1;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        currentWeatherIdIndex = 2;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha4))
                    {
                        currentWeatherIdIndex = 3;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha5))
                    {
                        currentWeatherIdIndex = 4;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha6))
                    {
                        currentWeatherIdIndex = 5;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha7))
                    {
                        currentWeatherIdIndex = 6;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha8))
                    {
                        currentWeatherIdIndex = 7;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha9))
                    {
                        currentWeatherIdIndex = 8;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha0))
                    {
                        currentWeatherIdIndex = 9;
                    }

                    // weather changed
                    if (currentWeatherIdIndex != originalWeatherIndexId)
                    {
                        EnviroSkyMgr.instance.ChangeWeather(weatherPrefabIdList[currentWeatherIdIndex]);
                    }
                }
                #endregion Weather
            }
            #endregion Input Handler

            // update ui weather and time information
            UpdateUiText();

        }

        /// <summary>
        /// Update the text objects in the UI with information about time and weather
        /// </summary>
        void UpdateUiText()
        {
            if (weatherTextMesh != null)
            {
                // show weather in gui; getting the preset and using that one in order to avoid nullpointer exceptions
                EnviroWeatherPreset preset = EnviroSkyMgr.instance.Weather.currentActiveWeatherPreset;
                if (preset != null)
                {
                    weatherTextMesh.text = preset.name;
                }
            }

            // time
            if (timeTextMesh != null)
            {
                timeTextMesh.text = EnviroSkyMgr.instance.GetTimeString();
            }
        }

    }
}