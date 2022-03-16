// Author: Anastasia Tumanov
// File Name: main.cs
// Project Name: PASS3PirateGame
// Creation Date: December 16th, 2021
// Modified Date: January 21st, 2022
// Description: This program is a roleplay, point and click 2D game.
// It follows a young pirate, Jim Wilds across Skull Island in search of a treasure.
// Throughout his journey, he talked to pirates, store clerks and captains to discover island secrets.
// He completes a quest at the cemetery, makes a purchase and travels in all corners of the island.

using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Animation2D;
using Helper;

namespace PASS3PirateGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        // Music for game
        Song introMusic;
        Song mapMusic;
        Song barMusic;
        Song cemeteryMusic;

        // Mousestate variables
        MouseState preMouse;

        // Left click variable for process speed up
        bool isLeftClick;

        // Constants for gameState changing
        const int INTRO = 0;
        const int HOME = 1;
        const int MAP = 2;
        const int BOAT_DECK = 3;
        const int PUB = 4;
        const int STORE = 5;
        const int INSIDE_PUB = 6;
        const int INSIDE_STORE = 7;
        const int CEMETERY = 8;
        const int FOREST = 9;
        const int ENDGAME = 11;
        const int PIRATE = 12;
        const int CAPTAIN = 13;
        const int QUIZ = 14;
        const int CORRECT = 15;
        const int WRONG = 16;

        // Current game state variable 
        int gameState = INTRO;

        // Menu page screen
        Texture2D homeScreen;
        Rectangle homeScreenRec;

        // Intro image screen
        Texture2D introImg;
        Animation introAnim;
        Vector2 introPos;

        // Timer for the introduction
        Timer animationEnd;
        int animationTime = 5200;

        // Fonts for speech, titles, etc.
        SpriteFont menuFont;
        SpriteFont textFont;
        SpriteFont speechFont;

        // Location for menu text
        Vector2 menuLoc = new Vector2(320, 360);

        // Start button
        string menuText = "Play Game";

        // Location variable
        Vector2 textLocation = new Vector2();

        // Array of different actions/locations
        string [] locations = new string[]{"", "Boat \nDeck", "Cemetery", "Forest", "Store",
                                          "Pub", "Enter", "Exit", "Pick Up", "Dig", "Map",
                                          "Talk to Pirate", "Talk to Captain", "  Talk to \n Store Clerk", "It's too dark to see anything...",
                                           "Look at sign"};
        // Holder variable for the string
        string textLocMessage = "";

        // Location for the coin message
        Vector2 coinMessageLoc = new Vector2(690, 30);

        // Rectangle creator
        Texture2D texture;

        // Menu text box - rectangle
        Rectangle menuTextBox;

        // Array of coins, their rectangles and their colours
        Texture2D[] coins = new Texture2D[3];
        Rectangle[] coinRec = new Rectangle[3];
        Color[] coinColours = new Color[3];

        // Map screen
        Texture2D map;
        Rectangle mapRec;

        // Map name picture and holder rectangle
        Texture2D mapName;
        Rectangle mapNameRec;

        // Map icon to bring you back to map
        Texture2D mapIcon;
        Rectangle mapIconRec;

        // Boxes for map locations
        Rectangle cemeteryBox;
        Rectangle boatDeckBox;
        Rectangle forestBox;

        // Grave rectangle
        Rectangle graveBox;

        // Speech options
        Rectangle option1;
        Rectangle option2;

        // Boat Deck
        Texture2D boatDeck;
        Rectangle boatDeckRec;

        // Outside pub screen
        Texture2D pub;
        Rectangle pubRec;
        Rectangle scummBar;

        // Outside store screen
        Texture2D store;
        Rectangle storeRec;
        Rectangle pirateStore;

        // Cemetery background
        Texture2D cemetery;
        Rectangle cemeteryRec;

        // Forest background
        Texture2D forest;
        Rectangle forestRec;

        // Left and right rectangles to change screens
        Rectangle leftBoxRec;
        Rectangle rightBoxRec;

        // Inside the pub animation
        Texture2D insidePub;
        Animation insidePubAnim;

        // Inside the store background
        Texture2D insideStore;
        Rectangle insideStoreBox;

        // Pirate information
        Texture2D pirate;
        Rectangle pirateRec;
        Color pirateCol = Color.White;

        // Pirate Digging
        Texture2D digging;
        Animation diggingAnim;
        Vector2 piratePos;
        Color digPirate = Color.Transparent;
        Timer diggingTimer;

        // Speech of the main character
        string[] jimSpeech = new string[] {"--> Ahoy. I am looking for the treasure of Skull Island!", "--> Sorry, you have rum on your breath. Goodbye.", "--> Thank you! ",
                                           "--> Goodbye.", "--> Aye!", "--> *gulp* I must have left them in my other pants", "--> Tell me more.",
                                           "--> Hi! I am looking for a shovel.", "--> I have 3 coins!", "--> I think I would just like to browse", "--> Uh. I don't have 3 coins.", "--> Sorry.",
                                           "In this sacred ground, there are three treasure boxes. \n Pick a hole, discover what treasure of \n Skull island is destined for you.", "You already \nhave the shovel. \n Go find your \ntreasure or whatever."};

        // Introduction text message and animation
        Texture2D introText;
        Animation introTextAnim;
        Vector2 introTextPos;

        // Coin counter
        int coinCounter = 0;

        // Shovel and bounding box
        Texture2D shovel;
        Rectangle shovelRec;

        // Lantern and bounding box
        Texture2D lantern;
        Rectangle lanternRec;

        // Pirate coordinates
        int pirateX = 300;
        int pirateY = 200;

        // Extra characters
        Rectangle pirate1Box;
        Rectangle captainBox;

        // Side character pirate
        Texture2D pirate1Screen;
        Rectangle pirate1Rec;
        string[] pirateSpeech = new string[] {"Ahoy Matey! How are ye today young lad?", "Only the captain will give you the \nexact location. But you need a shovel.",
                                              "Aye. I do know that you cannot \n enter the forest without a lantern. \n I have seen one in the cemetery, savvy?",
                                              "Here is a coin to help. Bye now lad"};

        // Side character captain
        Texture2D captainScreen;
        Rectangle captainRec;
        string[] captainSpeech = new string[] {"Arrrghh. Whada ya want lassie?", "The treasure, blimey! I'll tell you, \nbut you must bring a shovel and lantern. \n Do you have them?",
                                               "Excellent. The treasure is in the forest. \n I have marked the location with an X on your map.", "Aaaaarrghhhhh! Go find em lassie!"};

        // Speech holder variables
        string characterSpeech1 = "";
        string characterSpeech2 = "";

        // Speechbox to offer contrast
        Rectangle speechBox;

        // Character speech locations
        Vector2 characterSpeechLoc1 = new Vector2();
        Vector2 characterSpeechLoc2 = new Vector2();

        // Side character speech holder and location
        string sideSpeech = "";
        Vector2 sideSpeechLoc = new Vector2();

        // Variable for determining speech and actions
        int check = 0;

        // Have shovel
        bool haveShovel = false;
        Color shovelCol = Color.Transparent;

        // Have lantern
        bool haveLantern = false;
        Color lanternCol = Color.Transparent;

        // Have coins
        bool haveCoins = false;

        // Pirate level colours
        Color [] pirateLevCol = new Color[] {Color.Transparent, Color.Transparent, Color.Transparent};

        // Store clerk character
        Rectangle storeClerk;
        string [] storeClerkSpeech = new string[] {"What'cha lookin' at?", "A shovel huh?\n It's yours for 3 coins.",
                                                   "Great. It's yours", "Why are you wasting \nmy time, eh?"};

        // Level screens
        Texture2D level1Screen;
        Texture2D level2Screen;
        Texture2D level3Screen;

        // Colours of level screens
        Color[] levelScreen = new Color[3];

        // Rectangles for screens 
        Rectangle level1ScreenRec;
        Rectangle level2ScreenRec;
        Rectangle level3ScreenRec;

        // Correct and wrong quiz screens
        Texture2D correct;
        Texture2D wrong;

        // Rectangle for questions
        Rectangle questions;

        // Quiz score
        string score;

        // Number of tries for quiz
        int numTries;

        // Color of the forest 
        Color forestColor = Color.Black;

        // After location is known, X is displayed
        Texture2D mapX;
        Rectangle mapXRec;

        // Colour of the x
        Color XCol = Color.Transparent;

        // Is location known
        bool locationKnown = false;

        // Answer box options
        Rectangle[] answers = new Rectangle[4];

        // Question screen
        Texture2D questionScreen;

        // Dirt piles
        Texture2D[] dirtPiles = new Texture2D[3];
        Rectangle[] dirtPilesRec = new Rectangle[3];
        Color[] dirtPilesCol = new Color[3];

        // Rectangle for the forest sign
        Rectangle signBox;

        // Is boat deck visited
        bool boatDeckVisited = false;

        bool havePiratecoin = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Music for different game states
            introMusic = Content.Load<Song>("Audio/Music/IntroTheme");
            barMusic = Content.Load<Song>("Audio/Music/BarTheme");
            mapMusic = Content.Load<Song>("Audio/Music/GeneralMusic");
            cemeteryMusic = Content.Load<Song>("Audio/Music/CemeteryMusic");

            // Settings for music
            MediaPlayer.Volume = 0.8f;
            MediaPlayer.IsRepeating = true;

            // Homescreen background
            homeScreen = Content.Load<Texture2D>("Images/Backgrounds/Home");
            homeScreenRec = new Rectangle(0, 0, (int)(homeScreen.Width * 0.36), (int)(homeScreen.Height * 0.36));

            // Introduction image
            introImg = Content.Load<Texture2D>("Images/Backgrounds/GameIntro");
            introPos = new Vector2(0,0);
            introAnim = new Animation(introImg, 3, 1, 3, 0, 3, Animation.ANIMATE_ONCE, 300, introPos, 1.45f, true);

            // Timer for animation
            animationEnd = new Timer(animationTime, false);

            // Fonts for the text
            menuFont = Content.Load<SpriteFont>("Fonts/MenuFont");
            textFont = Content.Load<SpriteFont>("Fonts/LocFont");
            speechFont = Content.Load<SpriteFont>("Fonts/speechFont");

            // Menu text box load
            menuTextBox = new Rectangle(300, 350, 200, 70);

            // Texture made for rectangles
            texture = new Texture2D(GraphicsDevice, 1, 1);

            // Set texture to white colour
            texture.SetData(new[] { Color.White });

            piratePos = new Vector2(0, 0);

            // Map background
            map = Content.Load<Texture2D>("Images/Backgrounds/Island_Map");
            mapRec = new Rectangle(0, 0, map.Width, map.Height);

            // Skull island map name
            mapName = Content.Load<Texture2D>("Images/Sprites/MapName");
            mapNameRec = new Rectangle(5, 430, (int)(mapName.Width * 0.45), (int)(mapName.Height * 0.45));

            // Map icon to return to map
            mapIcon = Content.Load<Texture2D>("Images/Sprites/MapIcon");
            mapIconRec = new Rectangle(0, 0, (int)(mapIcon.Width * 0.20), (int)(mapIcon.Height * 0.20));

            // Map location rectangles
            cemeteryBox = new Rectangle(300, 170, 70, 70);
            boatDeckBox = new Rectangle(125, 295, 70, 70);
            forestBox = new Rectangle(210, 50, 70, 70);

            // Rectangle locations for the side characters
            pirate1Box = new Rectangle(700, 360, 70, 70);
            captainBox = new Rectangle(210, 290, 70, 70);
            storeClerk = new Rectangle(720, 300, 90, 100);

            // Rectangle for the grave
            graveBox = new Rectangle(460, 150, 70, 120);

            // Character speech options
            option1 = new Rectangle(130, 400, 600, 35);
            option2 = new Rectangle(130, 450, 600, 40);

            // Answer array (rectangles to be clicked)
            answers[0] = new Rectangle(150, 240, 200, 50);
            answers[1] = new Rectangle(150, 330, 200, 50);
            answers[2] = new Rectangle(460, 240, 200, 50);
            answers[3] = new Rectangle(460, 330, 200, 50);

            // Boat deck background
            boatDeck = Content.Load<Texture2D>("Images/Backgrounds/Boat_Deck");
            boatDeckRec = new Rectangle(0, 0, boatDeck.Width, boatDeck.Height);

            // Pirate background
            pub = Content.Load<Texture2D>("Images/Backgrounds/Pub");
            pubRec = new Rectangle(0, 0, (int)(pub.Width * 0.45), (int)(pub.Height * 0.45));

            // Store background
            store = Content.Load<Texture2D>("Images/Backgrounds/Store");
            storeRec = new Rectangle(0, 0, (int)(store.Width * 0.45), (int)(store.Height * 0.45));

            // Cemetery background
            cemetery = Content.Load<Texture2D>("Images/Backgrounds/Cemetery");
            cemeteryRec = new Rectangle(0, 0, cemetery.Width, cemetery.Height);

            // Forest background
            forest = Content.Load<Texture2D>("Images/Backgrounds/Forest");
            forestRec = new Rectangle(0, 0, forest.Width, forest.Height);

            // Left box rectangle
            leftBoxRec = new Rectangle(0, 30, 70, 480);

            // Right box rectangle
            rightBoxRec = new Rectangle(700, 0, 70, 480);

            // Speech box so there is contrast between text
            speechBox = new Rectangle(0, 400, 800, 120);

            // Store rectangles
            pirateStore = new Rectangle(60, 40, 400, 480);
            scummBar = new Rectangle(700, 325, 100, 150);

            // Inside store background
            insideStore = Content.Load<Texture2D>("Images/Backgrounds/Store_Inside");
            insideStoreBox = new Rectangle(0, 0, (int)(insideStore.Width * 0.335) , (int)(insideStore.Height * 0.32));

            // Inside pub animation
            insidePub = Content.Load<Texture2D>("Images/Backgrounds/Pub_Inside");
            insidePubAnim = new Animation(insidePub, 5, 18, 89, 0, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 5, introPos, 1.45f, true);

            // Digging animation
            digging = Content.Load<Texture2D>("Images/Sprites/Digging");
            diggingAnim = new Animation(digging, 4, 1, 4, 0, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 10, piratePos, 0.7f, true);
            diggingTimer = new Timer(2000, false);

            // Pirate image rectangle
            pirate = Content.Load<Texture2D>("Images/Sprites/Standing");
            pirateRec = new Rectangle(pirateX, pirateY, (int)(pirate.Width * 0.24), (int)(pirate.Height * 0.24));

            // Introduction text animation
            introText = Content.Load<Texture2D>("Images/Sprites/IntroTextAnim");
            introTextPos.X = pirateRec.X - 50;
            introTextPos.Y = pirateRec.Y - 120;
            introTextAnim = new Animation(introText, 5, 2, 7, 0, Animation.NO_IDLE, Animation.ANIMATE_ONCE, 115, introTextPos, 0.20f, true);

            // Coins images
            coins[0] = Content.Load<Texture2D>("Images/Sprites/Coin");
            coins[1] = Content.Load<Texture2D>("Images/Sprites/Coin");
            coins[2] = Content.Load<Texture2D>("Images/Sprites/Coin");

            // Coin bounding box
            coinRec[0] = new Rectangle(550, 380, (int)(coins[0].Width * 0.05), (int)(coins[0].Height * 0.05));
            coinRec[1] = new Rectangle(100, 240, (int)(coins[1].Width * 0.05), (int)(coins[1].Height * 0.05));
            coinRec[2] = new Rectangle(700, 0, (int)(coins[2].Width * 0.05), (int)(coins[2].Height * 0.05));

            // Pirate 1 screen - side character
            pirate1Screen = Content.Load<Texture2D>("Images/Backgrounds/Pirate1");
            pirate1Rec = new Rectangle(0, 0, (int)(pirate1Screen.Width * 0.33), (int)(pirate1Screen.Height * 0.375));

            // Captain screen
            captainScreen = Content.Load<Texture2D>("Images/Backgrounds/Captain");
            captainRec = new Rectangle(0, 0, (int)(captainScreen.Width * 0.35), (int)(captainScreen.Height * 0.32));

            // Level 1 screen
            level1Screen = Content.Load<Texture2D>("Images/Sprites/Level1Screen");
            level1ScreenRec = new Rectangle(0, 0, (int)(level1Screen.Width * 0.6), (int)(level1Screen.Height * 0.6));

            // Level 2 screen
            level2Screen = Content.Load<Texture2D>("Images/Sprites/Level2Screen");
            level2ScreenRec = new Rectangle(0, 0, (int)(level2Screen.Width * 0.6), (int)(level2Screen.Height * 0.6));

            // Level 3 screen
            level3Screen = Content.Load<Texture2D>("Images/Sprites/Level3Screen");
            level3ScreenRec = new Rectangle(0, 0, (int)(level3Screen.Width * 0.6), (int)(level3Screen.Height * 0.6));

            // Level screens colours
            levelScreen[0] = Color.Transparent;
            levelScreen[1] = Color.Transparent;
            levelScreen[2] = Color.Transparent;

            // Question square
            questionScreen = Content.Load<Texture2D>("Images/Sprites/Question3");

            // Correct and wrong screens
            correct = Content.Load<Texture2D>("Images/Sprites/Correct");
            wrong = Content.Load<Texture2D>("Images/Sprites/Wrong");

            // Questions bounding box
            questions = new Rectangle(0, 0, (int)(questionScreen.Width * 0.6), (int)(questionScreen.Height * 0.6));

            // Shovel and bounding rectangle
            shovel = Content.Load<Texture2D>("Images/Sprites/Shovel");
            shovelRec = new Rectangle(535, 0, (int)(shovel.Width * 0.05), (int)(shovel.Height * 0.05));

            // Lantern and bounding rectangle
            lantern = Content.Load<Texture2D>("Images/Sprites/Lantern");
            lanternRec = new Rectangle(610, 0, (int)(lantern.Width * 0.05), (int)(lantern.Height * 0.05));

            // Map red "X" and bounding rectangle 
            mapX = Content.Load<Texture2D>("Images/Sprites/X_On_Map");
            mapXRec = new Rectangle(185, 50, (int)(mapX.Width * 0.05), (int)(mapX.Height * 0.05));

            // Dirt piles images
            dirtPiles[0] = Content.Load<Texture2D>("Images/Sprites/Dirt");
            dirtPiles[1] = Content.Load<Texture2D>("Images/Sprites/Dirt");
            dirtPiles[2] = Content.Load<Texture2D>("Images/Sprites/Dirt");

            // Dirt piles bounding box
            dirtPilesRec[0] = new Rectangle(560, 300, (int)(dirtPiles[0].Width * 0.05), (int)(dirtPiles[0].Height * 0.05));
            dirtPilesRec[1] = new Rectangle(700, 400, (int)(dirtPiles[1].Width * 0.05), (int)(dirtPiles[1].Height * 0.05));
            dirtPilesRec[2] = new Rectangle(430, 380, (int)(dirtPiles[2].Width * 0.05), (int)(dirtPiles[2].Height * 0.05));

            // Dirt piles colours
            dirtPilesCol[0] = Color.Transparent;
            dirtPilesCol[1] = Color.Transparent;
            dirtPilesCol[2] = Color.Transparent;

            // Sign box bounding rectangle
            signBox = new Rectangle(430, 160, 250, 100);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Create variable for easy left click checks
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && preMouse.LeftButton != ButtonState.Pressed)
            {
                // Set left click to true
                isLeftClick = true;
            }

            // Get mouse state
            preMouse = Mouse.GetState();

            // Depending on gameState, update gameState
            switch (gameState)
            {
                case INTRO:
                    // Update introduction
                    UpdateIntro(gameTime);
                    break;
                case HOME:
                    // Update home
                    UpdateHome();
                    break;
                case MAP:
                    // Update map
                    UpdateMap();
                    break;
                case BOAT_DECK:
                    // Update boat deck
                    UpdateBoatDeck(gameTime);
                    break;
                case PUB:
                    // Update pub
                    UpdatePub();
                    break;
                case STORE:
                    // Update store
                    UpdateStore();
                    break;
                case INSIDE_PUB:
                    // Update inside pub
                    UpdateInsidePub(gameTime);
                    break;
                case INSIDE_STORE:
                    //Update inside store
                    UpdateInsideStore();
                    break;
                case CEMETERY:
                    // Update cemetery
                    UpdateCemetery();
                    break;
                case FOREST:
                    // Update forest
                    UpdateForest(gameTime);
                    break;
                case PIRATE:
                    // Update pirate
                    UpdatePirate();
                    break;
                case CAPTAIN:
                    // Update captain
                    UpdateCaptain();
                    break;
                case QUIZ:
                    // Update quiz
                    UpdateQuiz();
                    break;
                case CORRECT:
                    // Update correct screen
                    UpdateCorrect();
                    break;
                case WRONG:
                    // Update wrong screen
                    UpdateWrong();
                    break;
            }

            // If coin counter is three
            if(coinCounter == 3)
            {
                // Character has all three coins
                haveCoins = true;
            }

            // If character have shovel
            if (haveShovel)
            {
                // Have shovel appear on the screen
                shovelCol = Color.White;
            }

            // If character has a lantern
            if (haveLantern)
            {
                // Forest is not dark anymore
                forestColor = Color.White;

                // Lantern appears on inventory
                lanternCol = Color.White;
            }

            // If location is known
            if (locationKnown)
            {
                // X on the map is red
                XCol = Color.White;

                // Dirt piles are visible
                dirtPilesCol[0] = Color.White;
                dirtPilesCol[1] = Color.White;
                dirtPilesCol[2] = Color.White;
            }

            // Left click is false
            isLeftClick = false;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw gameState based on current gameState
            switch (gameState)
            {
                case INTRO:
                    // Draw intro
                    DrawIntro();
                    break;
                case HOME:
                    // Draw home screen
                    DrawHome();
                    break;
                case MAP:
                    // Draw map
                    DrawMap();
                    break;
                case BOAT_DECK:
                    // Draw boat deck
                    DrawBoatDeck();
                    break;
                case PUB:
                    // Draw pub
                    DrawPub();
                    break;
                case STORE:
                    // Draw store
                    DrawStore();
                    break;
                case INSIDE_PUB:
                    // Draw inside pub
                    DrawInsidePub();
                    break;
                case INSIDE_STORE:
                    // Draw inside store
                    DrawInsideStore();
                    break;
                case CEMETERY:
                    // Draw cemetery
                    DrawCemetery();
                    break;
                case FOREST:
                    // Draw forest
                    DrawForest();
                    break;
                case ENDGAME:
                    // Draw endgame
                    DrawEndGame();
                    break;
                case PIRATE:
                    // Draw pirate
                    DrawPirate();
                    break;
                case CAPTAIN:
                    // Draw captain
                    DrawCaptain();
                    break;
                case QUIZ:
                    // Draw quiz
                    DrawQuiz();
                    break;
                case CORRECT:
                    // Draw correct screen
                    DrawCorrect();
                    break;
                case WRONG:
                    // Update correct screen
                    DrawWrong();
                    break;
            }

            base.Draw(gameTime);
        }

        //Pre: None
        //Post: None
        //Desc: Update introduction
        void UpdateIntro(GameTime gameTime)
        {
            // If music is not playing
            if (MediaPlayer.State != MediaState.Playing)
            {
                // Play introduction music
                MediaPlayer.Play(introMusic);
            }

            // Update intro animation
            introAnim.Update(gameTime);

            // End animation
            animationEnd.Update(gameTime.ElapsedGameTime.TotalMilliseconds);

            // If intro animation is not animating
            if (!introAnim.isAnimating)
            {
                // Activate animation
                animationEnd.Activate();

                // If animation time remaining is less than or equal to zero
                if (animationEnd.GetTimeRemaining() <= 0)
                {
                    // change game state to home screen
                    gameState = HOME;
                }
            }
        }

        //Pre: None
        //Post: None
        //Desc: Draw introduction
        void DrawIntro()
        {
            spriteBatch.Begin();

            // Draw introduction animation
            introAnim.Draw(spriteBatch, Color.White, Animation.FLIP_NONE);

            spriteBatch.End();
        }

        //Pre: None
        //Post: None
        //Desc: Update home screen
        void UpdateHome()
        {
            // If is left click
            if (isLeftClick)
            {
                // If menu textbox contains mouse position
                if (menuTextBox.Contains(preMouse.Position))
                {
                    // Play map music
                    MediaPlayer.Play(mapMusic);

                    // Change gamestate to map
                    gameState = MAP;

                    // Make all coins visible
                    coinColours[0] = Color.White;
                    coinColours[1] = Color.White;
                    coinColours[2] = Color.White;
                }
            }
        }

        //Pre: None
        //Post: None
        //Desc: Draw home screen
        void DrawHome()
        {
            spriteBatch.Begin();

            // Draw homescreen 
            spriteBatch.Draw(homeScreen, homeScreenRec, Color.White);

            // Draw text 
            spriteBatch.DrawString(menuFont, menuText, menuLoc, Color.Magenta);

            // Create rectangle to be clicked to start game
            spriteBatch.Draw(texture, menuTextBox, Color.Transparent);

            spriteBatch.End();
        }

        //Pre: None
        //Post: None
        //Desc: Update map
        void UpdateMap()
        {
            // If left click
            if (isLeftClick)
            {
                // If cemetery location contains mouse position
                if (cemeteryBox.Contains(preMouse.Position) && boatDeckVisited)
                {
                    // Play cemetery music
                    MediaPlayer.Play(cemeteryMusic);

                    // Change game state to cemetery
                    gameState = CEMETERY;
                }
                // If boat deck location contains mouse position
                if (boatDeckBox.Contains(preMouse.Position))
                {
                    // Change gameState to boat deck
                    gameState = BOAT_DECK;
                }
                // IF forest location contains mouse position
                if (forestBox.Contains(preMouse.Position) && boatDeckVisited)
                {
                    // Change gamestate to forest
                    gameState = FOREST;

                    // Character speech contains blank
                    characterSpeech1 = "";
                }
            }

            // Depending on the mouse position, display location names
            if (boatDeckBox.Contains(preMouse.Position))
            {
                // Text follows mouse with name of location - boat deck
                textLocation.X = preMouse.X;
                textLocation.Y = preMouse.Y;
                textLocMessage = locations[1];
            }
            else if (cemeteryBox.Contains(preMouse.Position) && boatDeckVisited)
            {
                // Text follows mouse with name of location - cemetery
                textLocation.X = preMouse.X;
                textLocation.Y = preMouse.Y;
                textLocMessage = locations[2];
            }
            else if (forestBox.Contains(preMouse.Position) && boatDeckVisited)
            {
                // Text follows mouse with name of location - forest
                textLocation.X = preMouse.X;
                textLocation.Y = preMouse.Y;
                textLocMessage = locations[3];
            }
            else 
            {
                // Display nothing
                textLocMessage = locations[0];
            }
        }
        
        //Pre: None
        //Post: None
        //Desc: Draw map
        void DrawMap()
        {
            spriteBatch.Begin();

            // Draw map and map name
            spriteBatch.Draw(map, mapRec, Color.White);
            spriteBatch.Draw(mapName, mapNameRec, Color.White);

            // Create rectangles for all locations
            spriteBatch.Draw(texture, cemeteryBox, Color.Transparent);
            spriteBatch.Draw(texture, forestBox, Color.Transparent);
            spriteBatch.Draw(texture, boatDeckBox, Color.Transparent);

            // Text for location names
            spriteBatch.DrawString(textFont, textLocMessage, textLocation, Color.White);

            // Draw red x if location is known
            spriteBatch.Draw(mapX, mapXRec, XCol);

            spriteBatch.End();
        }

        //Pre: None
        //Post: None
        //Desc: Update boat deck
        void UpdateBoatDeck(GameTime gameTime)
        {
            // Character visited boat deck
            boatDeckVisited = true;

            // Set pirate locations
            pirateRec.X = 300;
            pirateRec.Y = 210;

            // Update intro text animation
            introTextAnim.Update(gameTime);

            // If left click
            if (isLeftClick && !introTextAnim.isAnimating)
            {
                // If left box contains mouse position
                if (leftBoxRec.Contains(preMouse.Position))
                {
                    // Change gameState to store screen
                    gameState = STORE;
                }
                // If right box contains mouse position
                if (rightBoxRec.Contains(preMouse.Position))
                {
                    // Change gameState to pub screen
                    gameState = PUB;
                }
                // If map icon contains mouse position
                if (mapIconRec.Contains(preMouse.Position))
                {
                    // Change gameState to map
                    gameState = MAP;
                }
                // If coin contains mouse position
                if (coinRec[0].Contains(preMouse.Position) && coinColours[0] == Color.White)
                {
                    // Increase coin counter
                    coinCounter++;

                    // Make coin colour invisible
                    coinColours[0] = Color.Transparent;
                }
            }

            if (!introTextAnim.isAnimating)
            {
                // Display location depending on where the mouse hovers
                if (leftBoxRec.Contains(preMouse.Position))
                {
                    // Display text location - left box
                    textLocation.X = preMouse.X;
                    textLocation.Y = preMouse.Y;
                    textLocMessage = locations[4];
                }
                else if (rightBoxRec.Contains(preMouse.Position))
                {
                    // Display text location - right box
                    textLocation.X = preMouse.X;
                    textLocation.Y = preMouse.Y;
                    textLocMessage = locations[5];
                }
                else if (mapIconRec.Contains(preMouse.Position))
                {
                    // Display text location - map icon
                    textLocation.X = preMouse.X;
                    textLocation.Y = preMouse.Y;
                    textLocMessage = locations[10];
                }
                else if (coinRec[0].Contains(preMouse.Position) && coinColours[0] == Color.White)
                {
                    // Display text location - left box 
                    textLocation.X = preMouse.X;
                    textLocation.Y = preMouse.Y;
                    textLocMessage = locations[8];
                }
                else
                {
                    // Display nothing
                    textLocMessage = locations[0];
                }
            }
            else
            {
                // Display nothing
                textLocMessage = locations[0];
            }
        }

        //Pre: None
        //Post: None
        //Desc: Draw boat deck
        void DrawBoatDeck()
        {
            spriteBatch.Begin();

            // Draw boat deck
            spriteBatch.Draw(boatDeck, boatDeckRec, Color.White);

            // Draw left and right boxes to switch screens
            spriteBatch.Draw(texture, leftBoxRec, Color.Transparent);
            spriteBatch.Draw(texture, rightBoxRec, Color.Transparent);

            // Draw map icon
            spriteBatch.Draw(mapIcon, mapIconRec, Color.White);

            // Draw pirate
            spriteBatch.Draw(pirate, pirateRec, Color.White);

            // Draw one of the coins
            spriteBatch.Draw(coins[2], coinRec[2], Color.White);

            // Draw intro text animation
            introTextAnim.Draw(spriteBatch, Color.Magenta, Animation.FLIP_NONE);

            // Draw one of the coins
            spriteBatch.Draw(coins[0], coinRec[0], coinColours[0]);

            // Display text 
            spriteBatch.DrawString(textFont, textLocMessage, textLocation, Color.White);
            spriteBatch.DrawString(textFont, " " + coinCounter, coinMessageLoc, Color.White);

            // Display lantern and shovel in inventory
            spriteBatch.Draw(lantern, lanternRec, lanternCol);
            spriteBatch.Draw(shovel, shovelRec, shovelCol);

            spriteBatch.End();
        }

        //Pre: None
        //Post: None
        //Desc: Update store screen
        void UpdateStore()
        {
            // Set pirate locations
            pirateRec.X = 550;
            pirateRec.Y = 260;

            // If left click
            if (isLeftClick)
            {
                // If right box contains mouse position
                if (rightBoxRec.Contains(preMouse.Position))
                {
                    // Change gamestate to boat deck
                    gameState = BOAT_DECK;
                }
                // If map icon contains mouse position
                if (mapIconRec.Contains(preMouse.Position))
                {
                    // Change gameState to map
                    gameState = MAP;
                }
                // If pirateStore contains mouse position
                if (pirateStore.Contains(preMouse.Position))
                {
                    // Set locations for text options
                    option1.X = 70;
                    option2.X = 70;
                    option1.Y = 410;
                    option2.Y = 450;

                    // Change gameState to inside store
                    gameState = INSIDE_STORE;

                    // Set check to 0
                    check = 0;

                    // Side character speech location
                    sideSpeechLoc.X = 550;
                    sideSpeechLoc.Y = 260;

                    // Chatacter speech locations
                    characterSpeechLoc1.X = 70;
                    characterSpeechLoc1.Y = 410;
                    characterSpeechLoc2.X = 70;
                    characterSpeechLoc2.Y = 447;
                }
            }

            // Depending on what mouse hovers over, display location
            if (pirateStore.Contains(preMouse.Position))
            {
                // Set current text location to pirate store
                textLocation.X = preMouse.X;
                textLocation.Y = preMouse.Y;
                textLocMessage = locations[6];
            }
            else if (rightBoxRec.Contains(preMouse.Position))
            {
                // Set current text location to right box
                textLocation.X = preMouse.X;
                textLocation.Y = preMouse.Y;
                textLocMessage = locations[1];
            }
            else
            {
                // Set text location nothing
                textLocMessage = locations[0];
            }
        }

        //Pre: None
        //Post: None
        //Desc: Draw store
        void DrawStore()
        {
            spriteBatch.Begin();

            // Draw store background
            spriteBatch.Draw(store, storeRec, Color.White);

            // Right box rectangle
            spriteBatch.Draw(texture, rightBoxRec, Color.Transparent);

            // Draw map icon
            spriteBatch.Draw(mapIcon, mapIconRec, Color.White);

            // Draw pirate store rectangle
            spriteBatch.Draw(texture, pirateStore, Color.Transparent);

            // Draw text
            spriteBatch.DrawString(textFont, textLocMessage, textLocation, Color.White);
            spriteBatch.DrawString(textFont, " " + coinCounter, coinMessageLoc, Color.White);

            // Draw coins at the top
            spriteBatch.Draw(coins[2], coinRec[2], Color.White);

            // Draw lantern and shovel in inventory
            spriteBatch.Draw(lantern, lanternRec, lanternCol);
            spriteBatch.Draw(shovel, shovelRec, shovelCol);

            // Draw pirate
            spriteBatch.Draw(pirate, pirateRec, Color.White);

            spriteBatch.End();
        }

        //Pre: None
        //Post: None
        //Desc: Update inside store
        void UpdateInsideStore()
        {
            // Set pirate location
            pirateRec.X = 470;
            pirateRec.Y = 250;

            // If left click
            if (isLeftClick)
            {
                // If left box contains mouse position
                if (leftBoxRec.Contains(preMouse.Position))
                {
                    // Play map music
                    MediaPlayer.Play(mapMusic);

                    // Change game state to store screen
                    gameState = STORE;
                }

                // Depending on inventory, dialogue is different for store clerk
                if (storeClerk.Contains(preMouse.Position) && haveShovel == false)
                {
                    // Set check to one
                    check = 1;
                }
                else if (storeClerk.Contains(preMouse.Position) && haveShovel == true)
                {
                    // Set check to 5
                    check = 5;
                }
            }

            // Depending on when mouse hovers, set text location
            if (leftBoxRec.Contains(preMouse.Position))
            {
                // Set text location to left box
                textLocation.X = preMouse.X;
                textLocation.Y = preMouse.Y;
                textLocMessage = locations[7];
            }
            else if (storeClerk.Contains(preMouse.Position) && check == 0)
            {
                // Set text location to store clerk
                textLocation.X = preMouse.X - 70;
                textLocation.Y = preMouse.Y;
                textLocMessage = locations[13];
            }
            else
            {
                // Set text location to nothing
                textLocMessage = locations[0];
            }

            // Depending on what check is, set proper text
            if (check == 0)
            {
                // Set speech texts to blank
                sideSpeech = "";
                characterSpeech1 = "";
                characterSpeech2 = "";
            }
            else if (check == 1)
            {
                // Set character and store clerk speech
                sideSpeech = storeClerkSpeech[0];
                characterSpeech1 = jimSpeech[7];
                characterSpeech2 = jimSpeech[9];
            }
            else if (check == 2)
            {
                // Set side character speech
                sideSpeech = storeClerkSpeech[1];

                // Check if character has coins
                if (haveCoins)
                {
                    // Set character speech
                    characterSpeech1 = jimSpeech[8];
                    characterSpeech2 = "";
                }
                else
                {
                    // Set character speech
                    characterSpeech1 = "";
                    characterSpeech2 = jimSpeech[10];
                }
            }
            else if (check == 3)
            {
                // Set side character and pirate speech
                sideSpeech = storeClerkSpeech[2];
                characterSpeech2 = jimSpeech[2];
                characterSpeech1 = "";
            }
            else if (check == 4)
            {
                // Set side character and pirate speech
                sideSpeech = storeClerkSpeech[3];
                characterSpeech2 = jimSpeech[11];
                characterSpeech1 = "";
            }
            else if (check == 5)
            {
                // Set side  character and pirate speech 
                sideSpeech = jimSpeech[13];
                characterSpeech2 = "";
                characterSpeech1 = "";
            }

            // If there is a left click and side character is talking
            if (isLeftClick && sideSpeech == storeClerkSpeech[0])
            {
                // Depending on the option chosen, display text
                if (option1.Contains(preMouse.Position))
                {
                    // Set check to two
                    check = 2;
                }
                else if (option2.Contains(preMouse.Position))
                {
                    // Set check to zero
                    check = 0;
                }
            }
            else if (isLeftClick && sideSpeech == storeClerkSpeech[1])
            {
                // Depending on the option chosen, display text
                if (option1.Contains(preMouse.Position) && haveCoins)
                {
                    // Set check to three
                    check = 3;

                    // Character has shovel
                    haveShovel = true;

                    // Set coin counter to zero
                    coinCounter = 0;
                }
                else if (option2.Contains(preMouse.Position))
                {
                    // Set check to four
                    check = 4;
                }
            }
            else if (isLeftClick && sideSpeech == storeClerkSpeech[2])
            {
                // Depending on the option chosen, display text
                if (option1.Contains(preMouse.Position))
                {
                    // Set check to three
                    check = 3;
                }
                if (option2.Contains(preMouse.Position))
                {
                    // Set check to zero
                    check = 0;
                }
            }
            else if (isLeftClick && sideSpeech == storeClerkSpeech[3])
            {
                // Display text based on choice
                if (option1.Contains(preMouse.Position) || option2.Contains(preMouse.Position))
                {
                    // Set check to zero
                    check = 0;
                }
            }
        }

        //Pre: None
        //Post: None
        //Desc: Draw inside store
        void DrawInsideStore()
        {
            spriteBatch.Begin();

            // Draw left box
            spriteBatch.Draw(texture, leftBoxRec, Color.Transparent);

            // Draw inside store background
            spriteBatch.Draw(insideStore, insideStoreBox, Color.White);

            // Draw text
            spriteBatch.DrawString(textFont, textLocMessage, textLocation, Color.White);

            // Draw store clerk rectangle
            spriteBatch.Draw(texture, storeClerk, Color.Transparent);

            // Draw side character speech
            spriteBatch.DrawString(speechFont, sideSpeech, sideSpeechLoc, Color.White);

            // Draw character speech
            spriteBatch.DrawString(speechFont, characterSpeech1, characterSpeechLoc1, Color.Magenta);
            spriteBatch.DrawString(speechFont, characterSpeech2, characterSpeechLoc2, Color.Magenta);

            // Draw option rectangles
            spriteBatch.Draw(texture, option1, Color.Transparent);
            spriteBatch.Draw(texture, option2, Color.Transparent);

            // Draw coin and counter
            spriteBatch.Draw(coins[2], coinRec[2], Color.White);
            spriteBatch.DrawString(textFont, " " + coinCounter, coinMessageLoc, Color.White);

            // Draw inventory
            spriteBatch.Draw(lantern, lanternRec, lanternCol);
            spriteBatch.Draw(shovel, shovelRec, shovelCol);

            // Draw pirate
            spriteBatch.Draw(pirate, pirateRec, Color.White);

            spriteBatch.End();
        }

        //Pre: None
        //Post: None
        //Desc: Update pub
        void UpdatePub()
        {
            // Set pirate location
            pirateRec.X = 270;
            pirateRec.Y = 250;

            // If there is a left click
            if (isLeftClick)
            {
                // Depending on what the mouse clicks on
                if (leftBoxRec.Contains(preMouse.Position))
                {
                    // Change gamestate to boat deck
                    gameState = BOAT_DECK; 
                }
                else if (scummBar.Contains(preMouse.Position))
                {
                    // Play bar music
                    MediaPlayer.Play(barMusic);

                    // Change gameState to inside pub
                    gameState = INSIDE_PUB;
                    
                }
                else if (mapIconRec.Contains(preMouse.Position))
                {
                    // Play map music
                    MediaPlayer.Play(mapMusic);

                    // Change gameState to map
                    gameState = MAP;
                }
            }
            else if (scummBar.Contains(preMouse.Position))
            {
                // Set text location
                textLocation.X = preMouse.X;
                textLocation.Y = preMouse.Y;
                textLocMessage = locations[6];
            }
            else if (leftBoxRec.Contains(preMouse.Position))
            {
                // Set text location
                textLocation.X = preMouse.X;
                textLocation.Y = preMouse.Y;
                textLocMessage = locations[1];
            }
            else
            {
                // Set text location to blank
                textLocMessage = locations[0];
            }
        }

        //Pre: None
        //Post: None
        //Desc: Draw pub
        void DrawPub()
        {
            spriteBatch.Begin();

            // Draw pub background
            spriteBatch.Draw(pub, pubRec, Color.White);

            // Draw left box
            spriteBatch.Draw(texture, leftBoxRec, Color.Transparent);

            // Draw map icon
            spriteBatch.Draw(mapIcon, mapIconRec, Color.White);

            // Draw rectangle for bar
            spriteBatch.Draw(texture, scummBar, Color.Transparent);

            // Draw text for locations
            spriteBatch.DrawString(textFont, textLocMessage, textLocation, Color.White);

            // Draw coins and counter
            spriteBatch.Draw(coins[2], coinRec[2], Color.White);
            spriteBatch.DrawString(textFont, " " + coinCounter, coinMessageLoc, Color.White);

            // Draw inventory objects
            spriteBatch.Draw(lantern, lanternRec, lanternCol);
            spriteBatch.Draw(shovel, shovelRec, shovelCol);

            // Draw pirate
            spriteBatch.Draw(pirate, pirateRec, Color.White);

            spriteBatch.End();
        }

        //Pre: None
        //Post: None
        //Desc: Update inside pub
        void UpdateInsidePub(GameTime gameTime)
        {
            // Set pirate location
            pirateRec.X = 300;
            pirateRec.Y = 320;

            // Update pub animation
            insidePubAnim.Update(gameTime);

            // If there is a left click
            if (isLeftClick)
            {
                // Depending on what is clicked on, do action
                if (leftBoxRec.Contains(preMouse.Position))
                {
                    // Play map music
                    MediaPlayer.Play(mapMusic);

                    // Change gameState to pub
                    gameState = PUB;
                }
                else if (pirate1Box.Contains(preMouse.Position))
                {
                    // Set locations for options
                    option1.X = 70;
                    option2.X = 70;
                    option1.Y = 410;
                    option2.Y = 450;

                    // Set check to zero
                    check = 0;

                    // Change gameState to pirate talking head
                    gameState = PIRATE;

                    // Change location of side speech
                    sideSpeechLoc.X = 80;
                    sideSpeechLoc.Y = 50;

                    // Set location of character speech
                    characterSpeechLoc1.X = 50;
                    characterSpeechLoc1.Y = 410;
                    characterSpeechLoc2.X = 50;
                    characterSpeechLoc2.Y = 447;
                }
                else if (captainBox.Contains(preMouse.Position))
                {
                    // Set locations for options
                    option1.X = 130;
                    option2.X = 130;
                    option1.Y = 400;
                    option2.Y = 450;

                    // Set check to zero
                    check = 0;

                    // Change gameState to captain talking head
                    gameState = CAPTAIN;
                    sideSpeechLoc.X = 150;
                    sideSpeechLoc.Y = 50;

                    // Set location of character speech
                    characterSpeechLoc1.X = 130;
                    characterSpeechLoc1.Y = 400;
                    characterSpeechLoc2.X = 130;
                    characterSpeechLoc2.Y = 440;
                }
            }

            // Depending on what the mouse hovers, set specific text
            if (leftBoxRec.Contains(preMouse.Position))
            {
                // Set text location 
                textLocation.X = preMouse.X;
                textLocation.Y = preMouse.Y;
                textLocMessage = locations[7];
            }
            else if (pirate1Box.Contains(preMouse.Position))
            {
                // Set text location
                textLocation.X = preMouse.X - 70;
                textLocation.Y = preMouse.Y;
                textLocMessage = locations[11];
            }
            else if (captainBox.Contains(preMouse.Position))
            {
                // Set text location
                textLocation.X = preMouse.X;
                textLocation.Y = preMouse.Y;
                textLocMessage = locations[12];
            }
            else
            {
                // Set text location to blank
                textLocMessage = locations[0];
            }
        }

        //Pre: None
        //Post: None
        //Desc: Draw inside pub
        void DrawInsidePub()
        {
            spriteBatch.Begin();

            // Draw left box rectangle
            spriteBatch.Draw(texture, leftBoxRec, Color.Transparent);

            // Draw inside pub animation
            insidePubAnim.Draw(spriteBatch, Color.White, Animation.FLIP_NONE);

            // Draw text for locations
            spriteBatch.DrawString(textFont, textLocMessage, textLocation, Color.White);

            // Draw pirate/captain rectangles
            spriteBatch.Draw(texture, pirate1Box, Color.Transparent);
            spriteBatch.Draw(texture, captainBox, Color.Transparent);

            // Draw coin and counter
            spriteBatch.Draw(coins[2], coinRec[2], Color.White);
            spriteBatch.DrawString(textFont, " " + coinCounter, coinMessageLoc, Color.White);

            // Draw inventory items
            spriteBatch.Draw(lantern, lanternRec, lanternCol);
            spriteBatch.Draw(shovel, shovelRec, shovelCol);

            // Draw pirate
            spriteBatch.Draw(pirate, pirateRec, Color.White);

            spriteBatch.End();
        }

        //Pre: None
        //Post: None
        //Desc: Update cemetery
        void UpdateCemetery()
        {
            // Set pirate location
            pirateRec.X = 300;
            pirateRec.Y = 90;

            // If left click
            if (isLeftClick)
            {
                // Depending on what the mouse has clicked on
                if (mapIconRec.Contains(preMouse.Position))
                {
                    // Play map music
                    MediaPlayer.Play(mapMusic);

                    // Change gameState to map
                    gameState = MAP;
                }
                else if (coinRec[1].Contains(preMouse.Position) && coinColours[1] == Color.White)
                {
                    // Coin counter is increased by one
                    coinCounter++;

                    // Change coin colour
                    coinColours[1] = Color.Transparent;
                }
                else if (graveBox.Contains(preMouse.Position) && haveLantern == false)
                {
                    // Set number of tries to 2
                    numTries = 2;

                    // Gamestate is quiz
                    gameState = QUIZ;
                }
            }

            // Depending on what the mouse hovers over, set text location
            if (coinRec[1].Contains(preMouse.Position) && coinColours[1] == Color.White)
            {
                // Set text location
                textLocation.X = preMouse.X;
                textLocation.Y = preMouse.Y;
                textLocMessage = locations[8];
            }
            else if (graveBox.Contains(preMouse.Position) && haveLantern == false)
            {
                // Set text location
                textLocation.X = preMouse.X;
                textLocation.Y = preMouse.Y;
                textLocMessage = "Look at grave";
            }
            else if (graveBox.Contains(preMouse.Position) && haveLantern == true)
            {
                // Set text location
                textLocation.X = preMouse.X;
                textLocation.Y = preMouse.Y;
                textLocMessage = "You already have the lantern.\n" + score;
            }
            else if (mapIconRec.Contains(preMouse.Position))
            {
                // Display text location - map icon
                textLocation.X = preMouse.X;
                textLocation.Y = preMouse.Y;
                textLocMessage = locations[10];
            }
            else
            {
                // Set text location to blank
                textLocMessage = locations[0];
            }
        }

        //Pre: None
        //Post: None
        //Desc: Draw cemetery
        void DrawCemetery()
        {
            spriteBatch.Begin();

            // Draw cemetery background
            spriteBatch.Draw(cemetery, cemeteryRec, Color.White);

            // Draw map icon
            spriteBatch.Draw(mapIcon, mapIconRec, Color.White);

            // Draw coin on ground
            spriteBatch.Draw(coins[1], coinRec[1], coinColours[1]);

            // Draw text location
            spriteBatch.DrawString(textFont, textLocMessage, textLocation, Color.White);

            // Draw coin and counter
            spriteBatch.DrawString(textFont, " " + coinCounter, coinMessageLoc, Color.White);
            spriteBatch.Draw(coins[2], coinRec[2], Color.White);

            // Draw grave rectangle
            spriteBatch.Draw(texture, graveBox, Color.Transparent);

            // Draw inventory items
            spriteBatch.Draw(lantern, lanternRec, lanternCol);
            spriteBatch.Draw(shovel, shovelRec, shovelCol);

            // Draw pirate
            spriteBatch.Draw(pirate, pirateRec, Color.White);

            spriteBatch.End();
        }

        //Pre: None
        //Post: None
        //Desc: Update forest
        void UpdateForest(GameTime gameTime)
        {
            // Based on whether lantern is owned
            if (haveLantern)
            {
                // Set pirate location
                pirateRec.X = 350;
                pirateRec.Y = 150;
            }
            else
            {
                // Set pirate location
                pirateRec.X = 0;
                pirateRec.Y = 250;
            }

            // Is left click
            if (isLeftClick)
            {
                // Depending on what the mouse contains, do action 
                if (mapIconRec.Contains(preMouse.Position))
                {
                    // show map on screen
                    gameState = MAP;
                }
                else if (signBox.Contains(preMouse.Position) && locationKnown)
                {
                    // Set text to location
                    characterSpeechLoc1.X = 100;
                    characterSpeechLoc1.Y = 100;
                    characterSpeech1 = jimSpeech[12];
                }

                // Go through dirt piles at index
                for (int i = 0; i < 3; i++)
                {
                    // If dirt piles at index contain mouse position
                    if (dirtPilesRec[i].Contains(preMouse.Position) && locationKnown)
                    {
                        // Change pirate level at index to visible
                        pirateLevCol[i] = Color.White;

                        // Change pirate to be visible
                        pirateCol = Color.Transparent;

                        // Depending on index, set position of digger
                        if (i == 0)
                        {
                            // Set digging position
                            diggingAnim.destRec.X = 440;
                            diggingAnim.destRec.Y = 120;
                        }
                        else if (i == 1)
                        {
                            // Set digging position
                            diggingAnim.destRec.X = 600;
                            diggingAnim.destRec.Y = 220;
                        }
                        else if (i == 2)
                        {
                            // Set digging position
                            diggingAnim.destRec.X = 380;
                            diggingAnim.destRec.Y= 200;
                        }

                        break;
                    }
                }
            }

            // If pirate is not visible on screen
            if (pirateCol == Color.Transparent)
            {
                // Activate timer
                diggingTimer.Activate();

                // Update animation
                diggingAnim.Update(gameTime);

                // End animation
                diggingTimer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);

                // Change dig pirate to be visible
                digPirate = Color.White;

                // Activate animation
                diggingAnim.isAnimating = true;

                // If animation time remaining is less than or equal to zero
                if (diggingTimer.GetTimeRemaining() <= 0)
                {
                    // Change gameState to end game
                    gameState = ENDGAME;
                    
                }
            }

            // Depending on mouse position, perform action
            if (mapIconRec.Contains(preMouse.Position) && coinColours[1] == Color.White)
            {
                // Set text location
                textLocation.X = preMouse.X;
                textLocation.Y = preMouse.Y;
                textLocMessage = locations[10];
            }
            else if (haveLantern == false)
            {
                // Set text location
                textLocation.X = preMouse.X;
                textLocation.Y = preMouse.Y;
                textLocMessage = locations[14];
            }
            else if (locationKnown && signBox.Contains(preMouse.Position))
            {
                // Set text location
                textLocation.X = preMouse.X;
                textLocation.Y = preMouse.Y;
                textLocMessage = locations[15];
            }
            else if (dirtPilesRec[0].Contains(preMouse.Position) && dirtPilesCol[0] == Color.White)
            {
                // Set text location
                textLocation.X = preMouse.X;
                textLocation.Y = preMouse.Y;
                textLocMessage = locations[9];
            }
            else if (dirtPilesRec[1].Contains(preMouse.Position) && dirtPilesCol[1] == Color.White)
            {
                // Set text location
                textLocation.X = preMouse.X;
                textLocation.Y = preMouse.Y;
                textLocMessage = locations[9];
            }
            else if (dirtPilesRec[2].Contains(preMouse.Position) && dirtPilesCol[2] == Color.White)
            {
                // Set text location
                textLocation.X = preMouse.X;
                textLocation.Y = preMouse.Y;
                textLocMessage = locations[9];
            }
            else
            {
                // Set text location to blank
                textLocMessage = locations[0];
            }
        }

        //Pre: None
        //Post: None
        //Desc: Draw forest
        void DrawForest()
        {
            spriteBatch.Begin();

            // Draw forest background
            spriteBatch.Draw(forest, forestRec, forestColor);

            // Draw map icon
            spriteBatch.Draw(mapIcon, mapIconRec, Color.White);

            // Draw dirt piles
            spriteBatch.Draw(dirtPiles[0], dirtPilesRec[0], dirtPilesCol[0]);
            spriteBatch.Draw(dirtPiles[1], dirtPilesRec[1], dirtPilesCol[1]);
            spriteBatch.Draw(dirtPiles[2], dirtPilesRec[2], dirtPilesCol[2]);

            // Draw coins and counter
            spriteBatch.Draw(coins[2], coinRec[2], Color.White);
            spriteBatch.DrawString(textFont, " " + coinCounter, coinMessageLoc, Color.White);

            // Draw text location
            spriteBatch.DrawString(textFont, textLocMessage, textLocation, Color.White);

            // Draw inventory
            spriteBatch.Draw(lantern, lanternRec, lanternCol);
            spriteBatch.Draw(shovel, shovelRec, shovelCol);

            // Draw character speech
            spriteBatch.DrawString(speechFont, characterSpeech1, characterSpeechLoc1, Color.Magenta);

            // Draw sign rectangle
            spriteBatch.Draw(texture, signBox, Color.Transparent);

            // Draw pirate
            spriteBatch.Draw(pirate, pirateRec, pirateCol);

            // Draw digging pub animation
            diggingAnim.Draw(spriteBatch, digPirate, Animation.FLIP_NONE);

            spriteBatch.End();
        }

        //Pre: None
        //Post: None
        //Desc: Draw end game
        void DrawEndGame()
        {
            spriteBatch.Begin();

            // Draw level screens
            spriteBatch.Draw(level1Screen, level1ScreenRec, pirateLevCol[0]);
            spriteBatch.Draw(level2Screen, level2ScreenRec, pirateLevCol[1]);
            spriteBatch.Draw(level3Screen, level3ScreenRec, pirateLevCol[2]);

            spriteBatch.End();
        }

        //Pre: None
        //Post: None
        //Desc: Update pirate talking head
        void UpdatePirate()
        {
            // Based on check value, set character speeches
            if (check == 0)
            {
                // Set side character speech
                sideSpeech = pirateSpeech[0];

                // Set main character speech
                characterSpeech1 = jimSpeech[0];
                characterSpeech2 = jimSpeech[1];
            }
            else if (check == 1)
            {
                // Set side character speech
                sideSpeech = pirateSpeech[1];

                // Set main character speech
                characterSpeech1 = jimSpeech[6];
                characterSpeech2 = jimSpeech[3];
            }
            else if (check == 2)
            {
                // Set side character speech
                sideSpeech = pirateSpeech[2];

                // Depending on whether character has cemetery coin
                if (havePiratecoin)
                {
                    // Set character speech to blank
                    characterSpeech1 = "";
                }
                else
                {
                    // Set character speech
                    characterSpeech1 = jimSpeech[2];
                }

                // Set character speech
                characterSpeech2 = jimSpeech[3];
            }
            else if (check == 3)
            {
                // Set side character speech
                sideSpeech = pirateSpeech[3];

                // Set main character speech
                characterSpeech1 = "--> Wow! Thanks!";
                characterSpeech2 = "";
            }

            // If is left click
            if (isLeftClick && sideSpeech == pirateSpeech[0])
            {
                // Depending on player option, perform action
                if (option1.Contains(preMouse.Position))
                {
                    // Set check to one
                    check = 1;
                }
                else if (option2.Contains(preMouse.Position))
                {
                    // Change gameState to inside pub
                    gameState = INSIDE_PUB;
                }
            }
            else if (isLeftClick && sideSpeech == pirateSpeech[1])
            {
                // Depending on player option, perform action
                if (option1.Contains(preMouse.Position))
                {
                    // Set check to two
                    check = 2;
                }
                else if (option2.Contains(preMouse.Position))
                {
                    // Change gameState to inside pub
                    gameState = INSIDE_PUB;
                }
            }
            else if (isLeftClick && sideSpeech == pirateSpeech[2])
            {
                // Depending on player option, perform action
                if (option1.Contains(preMouse.Position) && !havePiratecoin)
                {
                    // Set check to three
                    check = 3;

                    // Increase coinCounter by one
                    coinCounter++;

                    // Character has cemetery coin
                    havePiratecoin = true;
                }
                else if (option2.Contains(preMouse.Position))
                {
                    // Change gameState to inside pub
                    gameState = INSIDE_PUB;
                }
            }
            else if (isLeftClick && sideSpeech == pirateSpeech[3])
            {
                // If option one is clicked
                if (option1.Contains(preMouse.Position))
                {
                    // Change gameState to inside pub
                    gameState = INSIDE_PUB;
                }
            }
        }

        //Pre: None
        //Post: None
        //Desc: Draw pirate talking head
        void DrawPirate()
        {
            spriteBatch.Begin();

            // Draw pirate screen/background
            spriteBatch.Draw(pirate1Screen, pirate1Rec, Color.White);

            // Draw rectangle for speech
            spriteBatch.Draw(texture, speechBox, Color.Black);

            // Draw side character speech
            spriteBatch.DrawString(speechFont, sideSpeech, sideSpeechLoc, Color.White);

            // Draw main character speech
            spriteBatch.DrawString(speechFont, characterSpeech1, characterSpeechLoc1, Color.Magenta);
            spriteBatch.DrawString(speechFont, characterSpeech2, characterSpeechLoc2, Color.Magenta);

            // Draw option rectangles
            spriteBatch.Draw(texture, option1, Color.Transparent);
            spriteBatch.Draw(texture, option2, Color.Transparent);

            // Draw coin and counter
            spriteBatch.Draw(coins[2], coinRec[2], Color.White);
            spriteBatch.DrawString(textFont, " " + coinCounter, coinMessageLoc, Color.White);

            spriteBatch.End();
        }

        //Pre: None
        //Post: None
        //Desc: Update captain talking head
        void UpdateCaptain()
        {
            // Based on check value, update character speech
            if (check == 0)
            {
                // Set side character speech
                sideSpeech = captainSpeech[0];

                // Set main character speech
                characterSpeech1 = jimSpeech[0];
                characterSpeech2 = jimSpeech[1];
            }
            else if (check == 1)
            {
                // Set side character speech
                sideSpeech = captainSpeech[1];

                // If lantern and shovel are owned by character
                if (haveLantern && haveShovel)
                {
                    // Set main character speech
                    characterSpeech1 = jimSpeech[4];
                    characterSpeech2 = "";
                }
                else
                {
                    // Set main character speech
                    characterSpeech1 = "";
                    characterSpeech2 = jimSpeech[5];
                }
            }
            else if (check == 2)
            {
                // Set side character speech
                sideSpeech = captainSpeech[2];

                // Set main character speech
                characterSpeech1 = jimSpeech[2];
                characterSpeech2 = "";
            }
            else if (check == 3)
            {
                // Set side character speech
                sideSpeech = captainSpeech[3];

                // Set main character speech
                characterSpeech1 = "--> I'll find them!";
                characterSpeech2 = "";
            }

            // If left click
            if (isLeftClick)
            {
                if (sideSpeech == captainSpeech[0])
                {
                    // Depending on character option, perform action
                    if (option1.Contains(preMouse.Position))
                    {
                        // Set check to one
                        check = 1;
                    }
                    else if (option2.Contains(preMouse.Position))
                    {
                        // Change gameState to inside pub
                        gameState = INSIDE_PUB;
                    }
                }
                else if (sideSpeech == captainSpeech[1])
                {
                    // Depending on character option and conditions, perform action
                    if (option1.Contains(preMouse.Position) && haveShovel == true && haveLantern == true)
                    {
                        // Set check to two
                        check = 2;

                        // Location is known
                        locationKnown = true;
                    }
                    if (option2.Contains(preMouse.Position))
                    {
                        // Set check to three
                        check = 3;
                    }
                }
                else if (sideSpeech == captainSpeech[2])
                {
                    // If option 1 is clicked
                    if (option1.Contains(preMouse.Position))
                    {
                        // Change gameState to inside pub
                        gameState = INSIDE_PUB;
                    }
                }
                else if (sideSpeech == captainSpeech[3])
                {
                    // If option 1 is clicked
                    if (option1.Contains(preMouse.Position))
                    {
                        // Change gameState to inside pub
                        gameState = INSIDE_PUB;
                    }
                }
            }

            
            
        }

        //Pre: None
        //Post: None
        //Desc: Draw captain talking head
        void DrawCaptain()
        {
            spriteBatch.Begin();

            // Draw captain screen/background
            spriteBatch.Draw(captainScreen, captainRec, Color.White);

            // Draw speechbox rectangle
            spriteBatch.Draw(texture, speechBox, Color.Black);

            // Draw side character speech
            spriteBatch.DrawString(speechFont, sideSpeech, sideSpeechLoc, Color.White);

            // Draw main character speech
            spriteBatch.DrawString(speechFont, characterSpeech1, characterSpeechLoc1, Color.Magenta);
            spriteBatch.DrawString(speechFont, characterSpeech2, characterSpeechLoc2, Color.Magenta);

            // Draw option rectangles
            spriteBatch.Draw(texture, option1, Color.Transparent);
            spriteBatch.Draw(texture, option2, Color.Transparent);

            spriteBatch.End();
        }

        //Pre: None
        //Post: None
        //Desc: Update quiz screen
        void UpdateQuiz()
        {
            // If left click
            if (isLeftClick)
            {
               // Depending on number of tries, give respondent a result
                if (numTries == 2)
                {
                    // If answer at index 1 contains mouse position
                    if (answers[1].Contains(preMouse.Position))
                    {
                        // Change gameState to correct
                        gameState = CORRECT;

                        // Set score message
                        score = "You are a great pirate.\nIt took you one try!";
                    }
                    else
                    {
                        // Change gameState to wrong
                        gameState = WRONG;
                    }
                }
                else if (numTries == 1)
                {
                    // If answer at index 1 contains mouse position
                    if (answers[1].Contains(preMouse.Position))
                    {
                        // Change gameState is correct
                        gameState = CORRECT;

                        // Set score message
                        score = "Your pirating skills are decent.\n It took you two tries!";
                    }
                    else
                    {
                        // Change gameState to wrong
                        gameState = WRONG;
                    }
                }
            }
            else if (numTries == 0)
            {
               // Set numTries to zero
               numTries = 0;

               // Change gameState to wrong
               gameState = WRONG;
            }
        }

        //Pre: None
        //Post: None
        //Desc: Draw quiz screen
        void DrawQuiz()
        {
            spriteBatch.Begin();

            // Draw question screen
            spriteBatch.Draw(questionScreen, questions, Color.White);

            // Draw answer rectangles
            spriteBatch.Draw(texture, answers[0], Color.Transparent);
            spriteBatch.Draw(texture, answers[1], Color.Transparent);
            spriteBatch.Draw(texture, answers[2], Color.Transparent);
            spriteBatch.Draw(texture, answers[3], Color.Transparent);

            spriteBatch.End();
        }

        //Pre: None
        //Post: None
        //Desc: Update correct screen
        void UpdateCorrect()
        {
            if (isLeftClick)
            {
                // Change gameState to cemetery
                gameState = CEMETERY;

                // Character has lantern
                haveLantern = true;
            }
        }

        //Pre: None
        //Post: None
        //Desc: Draw correct screen
        void DrawCorrect()
        {
            spriteBatch.Begin();

            // Draw correct screen
            spriteBatch.Draw(correct, questions, Color.White);

            spriteBatch.End();
        }

        //Pre: None
        //Post: None
        //Desc: Update wrong screen
        void UpdateWrong()
        {
            // If is left click
            if (isLeftClick)
            {
                // Depending on number of tries, output result to player
                if (numTries == 2)
                {
                    // Reduce numTries by one
                    numTries--;

                    // Change gameState to quiz
                    gameState = QUIZ;

                    // Change left click to false
                    isLeftClick = false;
                }
                else if (numTries == 1)
                {
                    // Reduce numTries by one
                    numTries--;

                    // Change gameState to quiz
                    gameState = QUIZ;

                    // Change left click to false
                    isLeftClick = false;
                }
            }
            else if (numTries == 0)
            {
                // Change gameState to cemetery
                gameState = CEMETERY;

                // Player now has lantern
                haveLantern = true;

                // Set score message
                score = "You are not a great pirate.\n It took you three tries.";
            }
        }

        //Pre: None
        //Post: None
        //Desc: Draw wrong screen
        void DrawWrong()
        {
            spriteBatch.Begin();

            // Draw wrong screen
            spriteBatch.Draw(wrong, questions, Color.White);

            spriteBatch.End();
        }
    }
}