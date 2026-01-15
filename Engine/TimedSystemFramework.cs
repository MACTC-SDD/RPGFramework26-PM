using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace RPGFramework.Engine
{
    // CODE REVIEW: Rylan (PR #16)
    // I think this is all work that you re-did in GameState threads, right?
    // I'm commenting out for compiling purposes since it looks unused. If so, we can just remove it.
    /*
    internal class TimedSystemFramework
    {

        Random CoreRandom = new Random();

        public class WeatherConditionsClass
        {
            public string WeatherCondition { get; set; }
            public string Season { get; set; }
            public string TimeOfDay { get; set; }
            public string GroundCondition { get; set; }
        }

        WeatherConditionsClass CurrentWeather = new WeatherConditionsClass();
        public void WeatherTicks()
        {
            if(ticks % 60 == 0) {
                CurrentWeather.WeatherCondition = WeatherStates[CoreRandom.Next(WeatherStates.Count - 1)];
                CurrentWeather.GroundCondition = GroundWeatherConditions[CoreRandom.Next(GroundWeatherConditions.Count - 1)];
            }
        }

        List<string> WeatherStates = new List<string>()
        {
            "light rain",
            "heavy rain",
            "thunderstorm",
            "clear skies",
            "partly cloudy",
            "overcast",
            "snow",
            "blizzard",
            "tornado"
        };

        

        List<string> Seasons = new List<string>()
        {
            "spring",
            "summer",
            "autumn",
            "winter"
        };

        

        List<string> TimesOfDay = new List<string>()
        {
            "dawn",
            "morning",
            "afternoon",
            "evening",
            "night",
            "midnight"
        };

        

        List<string> GroundWeatherConditions = new List<string>()
        {
            "windy",
            "icy",
            "foggy",
            "snow-covered",
            "muddy",
            "wet",
            "flooded",
            "dry"
        };

        public void ItemDecay()
        {
            // Every 30 ticks, items decay
            if (ticks % 1200 == 0)
            {
                foreach (var item in GameState.Instance.Items.Values)
                {
                    if (item.IsPerishable)
                    {
                        item.UsesRemaining--;
                    }
                    if (item is Weapon)
                    {
                        item.Durability--;
                    }
                }
            }
        }
        public int ticks { get; private set; } = 0;
        public void TickTimer()
        {
            ticks++;
            WeatherTicks();
            ItemDecay();
            Announcments();
        }
        
        
        
    }*/
    
    }

