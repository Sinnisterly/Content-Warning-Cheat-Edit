


using DefaultNamespace;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Media3D;
using System.Runtime.CompilerServices;
using UnityEngine;
using Steamworks;
using UnityEngine.UI;
using Photon.Realtime;
using static UnityEngine.EventSystems.EventTrigger;
using static Photon.Voice.WebRTCAudioLib;
using ExitGames.Client.Photon;
using Zorro.Core.CLI;


namespace ContentWarningHax
{

    class Main : MonoBehaviour
    {



        
        bool esp = true;
        bool TransformMovement = false;
        public string customText = ""; // Custom text input field

        public static bool teleportFinderEnabled = false;
        public static Vector3 teleportPosition;
      
        
        public static List<Player> PlayerController = new List<Player>();
        public static List<Room> Room = new List<Room>();
    
        Photon.Realtime.Player[] otherPlayers = PhotonNetwork.PlayerListOthers;
        private string lobbyInfo = ""; // String to store lobby information


        float natNextUpdateTime;
      
  
        public static UnityEngine.Camera cam;
   
        private Rect windowRect = new Rect(0, 0, 400, 400); 
        private int tab = 0; 
        private Color backgroundColor = Color.black; 
        private bool showMenu = true;



        public List<string> monsterNames = new List<string>
{
    "BarnacleBall",    // spawns
    "BigSlap",         // spawns
    "Bombs",           // spawns
    "Dog",             // spawns
    "Ear",             // spawns
    "EyeGuy",          // spawns
    "Flicker",         // spawns
    "Ghost",           // spawns
    "Jelly",           // spawns
    "Knifo",           // spawns
    "Larva",           // spawns
    "Mouthe",          // spawns
    "Slurper",         // spawns
    "Snatcho",         // spawns
    "Spider",          // spawns
    "Snail",          // spawns
    "Toolkit_Fan",     // spawns
    "Toolkit_Hammer",  // spawns
    "Toolkit_Iron",    // spawns
    "Toolkit_Vaccuum", // spawns
    "Toolkit_Whisk",   // spawns
    "Toolkit_Wisk",    // spawns
    "Weeping",          // spawns


};


        public static Color TestColor
        {
            get
            {
                return new Color(1f, 0f, 1f, 1f);
            }
        }
      
        int selectedItemIndex = -1;
        
        Vector2 scrollPosition = Vector2.zero;
        Vector2 scrollPosition1 = Vector2.zero;

      
        void MenuWindow(int windowID)
        {
            GUILayout.BeginHorizontal();

            // Create toggle buttons for each tab
            GUILayout.BeginVertical(GUILayout.Width(100));
            if (GUILayout.Toggle(tab == 0, "Main Tab", "Button", GUILayout.ExpandWidth(true)))
            {
                tab = 0;
            }
            if (GUILayout.Toggle(tab == 1, "ESP Tab", "Button", GUILayout.ExpandWidth(true)))
            {
                tab = 1;
            }
            if (GUILayout.Toggle(tab == 2, "Item Spawner", "Button", GUILayout.ExpandWidth(true)))
            {
                tab = 2;
            }
            if (GUILayout.Toggle(tab == 3, "Monster Spawner", "Button", GUILayout.ExpandWidth(true)))
            {
                tab = 3;
            }
            if (GUILayout.Toggle(tab == 4, "Steam Tab", "Button", GUILayout.ExpandWidth(true)))
            {
                tab = 4;
            }
            if (GUILayout.Toggle(tab == 5, "Random Tab", "Button", GUILayout.ExpandWidth(true)))
            {
                tab = 5;
            }
            if (GUILayout.Toggle(tab == 6, "Player Tab", "Button", GUILayout.ExpandWidth(true)))
            {
                tab = 6;
            }
            if (GUILayout.Toggle(tab == 7, "Credits Tab", "Button", GUILayout.ExpandWidth(true)))
            {
                tab = 7;
            }
            GUILayout.EndVertical();

           

            GUILayout.BeginVertical();


       
            switch (tab)
            {
                case 0:
                 


                    // Content for tab 2
                    GUILayout.BeginVertical(GUI.skin.box);

                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                
                   

                    if (GUILayout.Button("Revive (self)"))
                    {

                        Player.localPlayer.CallRevive();
                    }

                   

                    if (GUILayout.Button("Recharge Battery"))
                    {
                        PlayerInventory playerInventory;
                        Player.localPlayer.TryGetInventory(out playerInventory);
                        foreach (InventorySlot inventorySlot in playerInventory.slots)
                        {
                            BatteryEntry batteryEntry;
                            if (inventorySlot.ItemInSlot.item != null && inventorySlot.ItemInSlot.data.TryGetEntry<BatteryEntry>(out batteryEntry) && batteryEntry.m_maxCharge > batteryEntry.m_charge)
                            {
                                batteryEntry.AddCharge(10000);
                            }
                        }
                    }

                   if (GUILayout.Button("Infinite Oxygen"))
                    {
                        Player.localPlayer.data.usingOxygen = false;
                    }

                    if (GUILayout.Button("Infinite Stamina"))
                    {
                        Player.localPlayer.data.staminaDepleated = false;
                    }
                   
                   


                    GUILayout.EndVertical();

                    GUILayout.Space(10);

                    GUILayout.BeginVertical();
                    
                  
                    if (GUILayout.Button("Fake Fail Quota"))
                    {
                        if (PhotonNetwork.IsMasterClient)
                        {                            
                            FindObjectOfType<EndGameScreen>().StartWatching();
                            WaitForSeconds waitForSeconds = new WaitForSeconds(10f);
                            for (int i = 0; i < PlayerHandler.instance.playerAlive.Count; i++)
                            {
                                PlayerHandler.instance.playerAlive[i].Die();
                            }
                        }
                    }



                    if (GUILayout.Button("Kill Self"))
                    {
                        for (int i = 0; i < PlayerHandler.instance.playerAlive.Count; i++)
                        {
                            PlayerHandler.instance.playerAlive[i].Die();
                        }
                    }

                    if (GUILayout.Button("Open Console"))
                    {
                        foreach (DebugUIHandler item in FindObjectsOfType<DebugUIHandler>())
                        {
                            item.Show();
                        }
                    }

                    if (GUILayout.Button("Close Console"))
                    {
                        foreach (DebugUIHandler item in FindObjectsOfType<DebugUIHandler>())
                        {
                            item.Hide();
                        }
                    }
                    
                    GUILayout.EndVertical();

                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();


                    break;
                case 1:
                    // Content for tab 2
                    GUILayout.BeginVertical(GUI.skin.box);

                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                    esp = GUILayout.Toggle(esp, "ESP");
                 
    


                    GUILayout.EndVertical();

                    GUILayout.Space(10);

                    GUILayout.BeginVertical();
                    TransformMovement = GUILayout.Toggle(TransformMovement, "TransMove");

                    if(TransformMovement)
                    {
                        GUILayout.Label("LeftControl forward");
                        GUILayout.Label("UpArrow Up");
                        GUILayout.Label("DownArrow Down");
                    }

                    GUILayout.EndVertical();

                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();

                    break;
                case 2:
                    // Items content
                    GUILayout.BeginVertical(GUI.skin.box);
                    if (GUILayout.Button("Dump items (do 1st)"))
                    {


                        string FilePath = "Items.txt";

                        foreach (var Items in ItemDatabase.Instance.lastLoadedItems)
                        {

                            string ItemInformation;
                            ItemInformation = "Name: " + Items.name;
                            ItemInformation += "\t | ID: " + Items.id;
                            File.AppendAllText(FilePath, ItemInformation + "\n");
                        }

                    }


                    scrollPosition = GUILayout.BeginScrollView(scrollPosition);

                    foreach (var item in ItemDatabase.Instance.lastLoadedItems)
                    {
                        if (GUILayout.Button(item.name))
                        {
                            EquipItem(item);
                        }
                    }

                    GUILayout.EndScrollView();
                    GUILayout.EndVertical();
                    break;
                case 3:
                    // Monsters content
                    GUILayout.BeginVertical(GUI.skin.box);

                  

                    scrollPosition1 = GUILayout.BeginScrollView(scrollPosition1);

                    foreach (string monsterName in monsterNames)
                    {
                        if (GUILayout.Button(monsterName))
                        {
                            SpawnMonster(monsterName);
                        }
                    }

                    GUILayout.EndScrollView();
                    GUILayout.EndVertical();
                    break;
                    case 4:
                    GUILayout.BeginVertical(GUI.skin.box);
                    SteamAPICall_t hAPICall = SteamMatchmaking.RequestLobbyList();
                    if (GUILayout.Button("Request Lobby List"))
                    {
                        hAPICall = SteamMatchmaking.RequestLobbyList();
                        Debug.Log("Requested Lobby List");
                    }

                    GUILayout.Label("SteamAPICall_t: " + SteamMatchmaking.RequestLobbyList());

                    if (GUILayout.Button("Random Join"))
                    {
                        MainMenuHandler.Instance.JoinRandom();
                  
                    }

                    if (GUILayout.Button("OnCreatedRoom"))
                    {
                        MainMenuHandler.Instance.OnCreatedRoom();

                    }
                    if (GUILayout.Button("Set Host"))
                    {

                        PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
                    }

                    GUILayout.EndVertical();
                    break;
                case 5:
                    GUILayout.BeginVertical(GUI.skin.box);
                    if (GUILayout.Button("Lots of Start Money (host)"))
                    {
                        BigNumbers.Instance.StartMoney = 99999999;

                    }

                    if (GUILayout.Button("Add 10k Money (host)"))
                    {
                        SurfaceNetworkHandler.RoomStats.AddMoney(10000);

                    }

                    if (GUILayout.Button("Remove 10k Money (host)"))
                    {
                        SurfaceNetworkHandler.RoomStats.RemoveMoney(10000);

                    }

                    if (GUILayout.Button("Add 10k to Views"))
                    {
                        SurfaceNetworkHandler.AddQuota(1000);
                    }

                    if (GUILayout.Button("BotHandler"))
                    {
                        BotHandler.instance.DestroyAll();

                    }


                    if (GUILayout.Button("Max Stats"))
                    {
                        Player.localPlayer.data.remainingOxygen = float.MaxValue;
                        Player.localPlayer.data.maxOxygen = float.MaxValue;
                        Player.localPlayer.data.health = float.MaxValue;
                        Player.localPlayer.data.currentStamina = float.MaxValue;
                        Player.localPlayer.data.rested = true;


                    }

                    if (GUILayout.Button("Clear Inventory"))
                    {
                        PlayerInventory playerInventory;
                        Player.localPlayer.TryGetInventory(out playerInventory);
                        foreach (InventorySlot inventorySlot in playerInventory.slots)
                        {
                            playerInventory.Clear();
                        }
                    }

                    if (GUILayout.Button("Crash"))
                    {
                        foreach (Player player in PlayerController)
                        {

                            if (!player.IsLocal)
                            {
                                PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
                                PhotonNetwork.DestroyPlayerObjects(player.refs.view.Controller);
                            }

                        }


                    }

                    GUILayout.EndVertical();

                    break;
                case 6:
                    GUILayout.BeginVertical(GUI.skin.box);
                     GUILayout.Label("Players in Lobby:");



                    lobbyInfo = ""; 

                    foreach (Photon.Realtime.Player player in otherPlayers)
                    {
                        string playerInfo = string.Format("{0} | {1}", player.NickName, player.CustomProperties.ToStringFull());
                        GUILayout.Label(playerInfo);
                        lobbyInfo += playerInfo + "\n";
                    }

                    if (GUILayout.Button("Save Lobby Info"))
                    {
                        SaveLobbyInfo();
                    }

                    GUILayout.EndVertical();

                    break;


            }
            switch (tab)
            { 
            case 7:
                GUILayout.BeginVertical(GUI.skin.box);
                GUILayout.Label("Credits:");
                GUILayout.Label("Sinnisterly (added things)- https://github.com/Sinnisterly ");
                GUILayout.Label("WoodgamerHD (for base/source)- https://github.com/WoodgamerHD ");
                GUILayout.EndVertical();
                break;
            }

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
            GUI.DragWindow(); 
        }
        public static void EquipItem(Item item)
        {
            Debug.Log("Spawn item: " + item.name);
            Vector3 debugItemSpawnPos = MainCamera.instance.GetDebugItemSpawnPos();


            Player.localPlayer.RequestCreatePickup(item, new ItemInstanceData(Guid.NewGuid()), debugItemSpawnPos, UnityEngine.Quaternion.identity);
        }
   
        void SaveLobbyInfo()
        {
            string filePath = "LobbyInfo.txt"; 

            try
            {
                // Write lobbyInfo string to a text file
                File.WriteAllText(filePath, lobbyInfo);
                Debug.Log("Lobby info saved to " + filePath);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Failed to save lobby info: " + ex.Message);
            }
        }
    


        public static void SpawnMonster(string monster)
        {
            RaycastHit raycastHit = HelperFunctions.LineCheck(MainCamera.instance.transform.position, MainCamera.instance.transform.position + MainCamera.instance.transform.forward * 30f, HelperFunctions.LayerType.TerrainProp, 0f);
            Vector3 vector = MainCamera.instance.transform.position + MainCamera.instance.transform.forward * 30f;
            if (raycastHit.collider != null)
            {
                vector = raycastHit.point;
            }
            vector = HelperFunctions.GetGroundPos(vector + Vector3.up * 1f, HelperFunctions.LayerType.TerrainProp, 0f);
            PhotonNetwork.Instantiate(monster, vector, UnityEngine.Quaternion.identity, 0, null);
        }
     
        public void OnGUI()
        {



            if (showMenu) // Only draw the menu when showMenu is true
            {
                // Set the background color
                GUI.backgroundColor = backgroundColor;

                windowRect = GUI.Window(0, windowRect, MenuWindow, "WoodSDK - edited @ Sinnisterly"); // Create the window with title "Menu"
             
                
            }

         


            if (esp)
            {
             

                foreach (Player player in PlayerController)
                {



                    Vector3 w2s = cam.WorldToScreenPoint(player.HeadPosition());
                    Vector3 enemyBottom = player.HeadPosition();
                    Vector3 enemyTop;
                    enemyTop.x = enemyBottom.x;
                    enemyTop.z = enemyBottom.z;
                    enemyTop.y = enemyBottom.y + 2f;
                    Vector3 worldToScreenBottom = cam.WorldToScreenPoint(enemyBottom);
                    Vector3 worldToScreenTop = cam.WorldToScreenPoint(enemyTop);

                    if (player.IsLocal)
                        return;

                   
           

                     if (ESPUtils.IsOnScreen(w2s))
                    {

                        float height = Mathf.Abs(worldToScreenTop.y - worldToScreenBottom.y);
                        float x = w2s.x - height * 0.3f;
                        float y = Screen.height - worldToScreenTop.y;


                        Vector2 namePosition = new Vector2(w2s.x, UnityEngine.Screen.height - w2s.y + 8f);
                        Vector2 hpPosition = new Vector2(x + (height / 2f) + 3f, y + 1f);

                    
                        namePosition -= new Vector2(player.HeadPosition().x - player.HeadPosition().x, 0f);
                        hpPosition -= new Vector2(player.HeadPosition().x - player.HeadPosition().x, 0f);

                        float distance = Vector3.Distance(UnityEngine.Camera.main.transform.position, player.HeadPosition());
                        int fontSize = Mathf.Clamp(Mathf.RoundToInt(12f / distance), 10, 20);



                        if (player.ai)
                        {
                            ESPUtils.DrawString(namePosition, player.name.Replace("(Clone)", ""), Color.red, true, fontSize, FontStyle.Bold);
                        }
                        else
                        {
                            ESPUtils.DrawString(namePosition, player.refs.view.Controller.ToString() + "\n" + "HP: " + player.data.health, Color.green, true, fontSize, FontStyle.Bold);
                            ESPUtils.DrawHealth(new Vector2(w2s.x, UnityEngine.Screen.height - w2s.y + 22f), player.data.health, 100f, 0.5f, true);

                        }
                     
                       

                    }
                }
            }
            

        }
       


        public void Start()
        {
            // Center the window on the screen
            windowRect.x = (Screen.width - windowRect.width) / 2;
            windowRect.y = (Screen.height - windowRect.height) / 2;           



        }






        public void Update()
        {
          
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                showMenu = !showMenu;
            }


            natNextUpdateTime += Time.deltaTime;

            if (natNextUpdateTime >= 1f)
            {


                PlayerController = Resources.FindObjectsOfTypeAll<Player>().ToList();
                Room = Resources.FindObjectsOfTypeAll<Room>().ToList();
              
                otherPlayers =  PhotonNetwork.PlayerListOthers;
              
              
                natNextUpdateTime = 0f;
            }

          
            if (TransformMovement)
            {

                if (Input.GetKey(KeyCode.LeftControl))
                {
                    Player.localPlayer.transform.position += 0.5f * UnityEngine.Camera.main.transform.forward;
                }
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    Player.localPlayer.transform.position += new Vector3(0f, 5f, 0f);
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    Player.localPlayer.transform.position += new Vector3(0f, -5f, 0f);
                } 


            }



            cam = UnityEngine.Camera.main;

        }
    }
}

