using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

using Spectre.Console.Rendering;
using RPGFramework.Enums;

namespace RPGFramework
{
    internal partial class Player : Character
    {
        #region --- Properties --- 
        // Properties to NOT save (don't serialize)
        [JsonIgnore]
        public bool IsAFK { get; set; } = false;

        [JsonIgnore]
        public bool IsOnline { get; set; }
        
        // Properties
        public DateTime LastLogin { get; set; }
        public TimeSpan PlayTime { get; set; } = new TimeSpan();
        public PlayerRole PlayerRole { get; set; } = PlayerRole.Player;
        #endregion

        public string DisplayName()
        {
            // We could add colors and other things later, for now, just afk
            return Name + (IsAFK ? " (AFK)" : "");

        }
        
        /// <summary>
        /// Things that should happen when a player logs in.
        /// </summary>
        public void Login()
        {
            IsOnline = true;
            LastLogin = DateTime.Now; 
            Console = CreateAnsiConsole();
        }

        /// <summary>
        /// Things that should happen when a player logs out. 
        /// </summary>
        public void Logout()
        {
            TimeSpan duration = DateTime.Now - LastLogin;
            PlayTime += duration;
            IsOnline = false;            
            Save();

            WriteLine("Bye!");
            Network?.Client.Close();
        }

        /// <summary>
        /// Save the player to the database.
        /// </summary>
        public void Save()
        {
            GameState.Instance.SavePlayer(this);
        }


        public void Write(string message)
        {
            Console.Write(message);
        }

        public void Write(IRenderable renderable)
        {
            Console.Write(renderable);
        }

        
        // This is just for convenience, we could access the Network.Writer directly
        // Should this be done throught the Comm class instead so it's all in one place?
        public void WriteLine(string message)
        {
            Console.MarkupLine(message);
            //Network?.Writer.WriteLine(message);
        }

    }


}
