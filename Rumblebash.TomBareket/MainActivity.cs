using Android.App;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using Android.Support.V7.Widget;
using Android.Content;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content.Res;
using Android.Graphics;
using Android.Views.Animations;
using Android.Media;
using Firebase.Database;
using Java.Util;
using static Rumblebash.TomBareket.ScoreboardViewHolder;
using Android.Util;

namespace Rumblebash.TomBareket
{
    [Activity(Label = "Rumblebash", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        //Variables for the start menu
        public ImageView ivTitle;
        public Button btnStart, btnOptions, btnHigh;
        public static Animation aJumpTitle;
        public static RelativeLayout rlMainMenu;

        //Constant variables for bug fixing
        public static string worksTxt = "-------------------MASSIVE DAY FOR THE UNEMPLOYED------------------", errTxt = "-------------------ERROR------------------";


        //Variables regarding the settings menu
        public LayoutInflater liInflateSettings;
        public static ISharedPreferences sp;
        public SeekBar sbVolumeMusic, sbVolumeSfx, sbVolumeDemon;
        public RadioGroup rgBgScroll, rgPlayIdleAnims;
        public RadioButton rbIdleChoice1, rbIdleChoice2;
        public RadioButton rbBGChoice1, rbBGChoice2;
        public ImageView ivSaveButton;
        public View vSettings, vModal;
        public static bool isInSettings = false;
        public ImageView ivSettingsPortrait;
        public static ViewGroup inflatedSettings;
        public static Action[] settingsPages;
        public static ImageView ivArrowLeft, ivArrowRight;
        public static int curPage = 1, lastImage = 0;
        public static bool firstInflate = true;


        //Variables regarding high scores and the score menu
        public ImageView ivReturnToMenu;
        public static FirebaseDatabase dbScoreStorage;
        public static ScoreboardListener slCreateScoreboard;
        public View vHigh;


        //The actual settings
        public static float volumeMusic = 100, volumeSFX = 100, volumeDemon = 100;
        public static bool animateBackground = true, animateIdleAnims = true;
        public static bool playStarkill = true;
        public static bool postOnline = true, endlessMode = false;


        //MainActivity statics, used consistantly in the card manager
        public static MainActivity main;
        public static Android.Content.Context context;
        public static bool canPress = true;
        

        //Recycler View variables, both the deck as well as the scoreboard
        public static List<Card> cardPile;
        public static RecyclerView rvDeck, rvScoreboard;
        public static CardAdapter cAdapter;
        public static List<Score> scoreboard;
        public static ScoreAdapter sAdapter;
        

        //Game board related variables
        public static SlotView[,] svCardSlot = svCardSlot = new SlotView[2, 3];
        public static List<Projectile> lProjectileBank = new List<Projectile>();
        public static TextView[,] tvMonsterStat = new TextView[6,2];
        public static ImageView ivCombatButton;
        public static MediaPlayer mpCombatHandler;
        public static TextView tvCountDown, tvCountKills;
        public static RelativeLayout rlParentLayout;
        public static Intent iDevil;
        public static System.Timers.Timer mTimer;
        public static int timeElapsed = 20;
        public static int stallCounter = 0;

        //Variables related to the save menu
        public LayoutInflater liInflateSave;
        public static EditText etPickName;
        public static Button btnSAVE;
        public static View vSaveMenu;


        //Combat related statistics
        public const int MAX_TURNS = 20;
        public static bool isInCombat = false;
        public static int kills = 0;
        public static int curKills = 0;
        public ImageView ivSettingsButton, ivCloseButton;


        //Statistics about the board, as well as settings and internal variables of the board
        public static ImageView ivForeground;
        public static LinearLayout llBackground;
        public static BackgroundScrollView svBgContainer;
        public static Animation aBackgroundMovement, aForegroundMovement;
        public static Typeface nameFont, abilityFont, statsFont, starkillFont;
        public MediaPlayer mpFightTheme;
        
        


        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_start);
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            main = this;
            nameFont = Typeface.CreateFromAsset(Assets, "CardNameFont.TTF");
            abilityFont = Typeface.CreateFromAsset(Assets, "CardAbilityFont.ttf");
            statsFont = Typeface.CreateFromAsset(Assets, "CardStatsFont.ttf");
            starkillFont = Typeface.CreateFromAsset(Assets, "StarkillFont.otf");
            sp = GetSharedPreferences("details", FileCreationMode.Private);
            AppDataHelper.GetDatabase();


            rlParentLayout = FindViewById<RelativeLayout>(Resource.Id.rlGameMenu);


            volumeMusic = sp.GetInt("musicVolume", 100);
            volumeSFX = sp.GetInt("sfxVolume", 100);
            volumeDemon = sp.GetInt("demonVolume", 100);
            animateBackground = sp.GetBoolean("animateBackground", true);
            animateIdleAnims = sp.GetBoolean("animateIdleAnims", true);
            playStarkill = sp.GetBoolean("playStarkill", true);
            postOnline = sp.GetBoolean("onlinePosting", true);
            endlessMode = sp.GetBoolean("isEndlessMode", false);

            curPage = sp.GetInt("currentPage", 0);
            settingsPages = new Action[] { VisualSettings, AudioSettings };
            lastImage = sp.GetInt("lastImage", Resource.Drawable.MusicSettings);


            LoadMainMenu();
            iDevil = new Intent(this, typeof(DevilService));
            scoreboard = new List<Score>() { };
            slCreateScoreboard = new ScoreboardListener();
            slCreateScoreboard.RetrieveScoresFromDatabase();

            Card.SetCardCount();
        }


        protected override void OnPause()
        {
            base.OnPause();
            if (mpFightTheme != null) mpFightTheme.Pause();
            iDevil = new Intent(this, typeof(DevilService));
            StartService(iDevil);
            
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (mpFightTheme != null)  mpFightTheme.Start();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void LoadMainMenu()
        {
            if(mpFightTheme != null) mpFightTheme.Stop();
            SetContentView(Resource.Layout.activity_start);
            rlMainMenu = FindViewById<RelativeLayout>(Resource.Id.rlStartMenu);
            rlParentLayout = null;
            ivTitle = FindViewById<ImageView>(Resource.Id.ivGameTitle);
            btnStart = FindViewById<Button>(Resource.Id.btnPlay);
            btnOptions = FindViewById<Button>(Resource.Id.btnOptions);
            btnHigh = FindViewById<Button>(Resource.Id.btnHighScore);
            aJumpTitle = AnimationUtils.LoadAnimation(this, Resource.Animation.anim_TitleJump);
            ivTitle.StartAnimation(aJumpTitle);
            btnStart.Click += LoadGameMenu;
            btnOptions.Click += LoadSettingsMenu;
            btnHigh.Click += LoadHighScoreMenu;
            btnStart.Typeface = statsFont;
            btnOptions.Typeface = statsFont;
            btnHigh.Typeface = statsFont;
            FindViewById<TextView>(Resource.Id.tvCreatorName).Typeface = statsFont;
            FindViewById<TextView>(Resource.Id.tvVersionName).Typeface = statsFont;
        }

        //Loads the settings menu, adds the required variables from SharedPreferences to it
        public void LoadSettingsMenu(object sender, EventArgs e)
        {
            if (isInSettings) return;
            if (ivSettingsButton != null) ivSettingsButton.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.SettingsButtonPressed));
            isInSettings = true;
            
            postOnline = sp.GetBoolean("onlinePosting", true);

            btnStart.Visibility = ViewStates.Gone;
            btnOptions.Visibility = ViewStates.Gone;
            btnHigh.Visibility = ViewStates.Gone;

            liInflateSettings = LayoutInflater.From(this);

            vSettings = liInflateSettings.Inflate(Resource.Layout.activity_options, null);
            vSettings.SetY(WindowManager.DefaultDisplay.Height / 7);
            //vSettings.SetX(WindowManager.DefaultDisplay.Width / 30);

            vModal = liInflateSettings.Inflate(Resource.Layout.modal_layout, null);
            vModal.LayoutParameters = new ViewGroup.LayoutParams(Resources.DisplayMetrics.WidthPixels, Resources.DisplayMetrics.HeightPixels);
            vModal.Alpha = 0.5f;

            if (rlParentLayout == null) 
            {
                rlMainMenu.AddView(vModal);
                rlMainMenu.AddView(vSettings);
            } 
            else
            {
                rlParentLayout.AddView(vModal);
                rlParentLayout.AddView(vSettings);
            }
            Animation animSettingsPop = AnimationUtils.LoadAnimation(this, Resource.Animation.anim_SettingsMenuFloat);
            animSettingsPop.Interpolator = new DecelerateInterpolator();
            vSettings.StartAnimation(animSettingsPop);
            
            vModal.StartAnimation(AnimationUtils.LoadAnimation(this, Resource.Animation.anim_FadeIn));
            ivSettingsPortrait = FindViewById<ImageView>(Resource.Id.ivSettingsPortrait);
            ivSettingsPortrait.SetImageDrawable(Resources.GetDrawable(sp.GetInt("lastImage", Resource.Drawable.MusicSettings)));

            FindViewById<TextView>(Resource.Id.tvOptionsTitle).Typeface = nameFont;

            ivSaveButton = FindViewById<ImageView>(Resource.Id.ivSaveButton);
            ivSaveButton.Touch += SaveSettings;
            ivCloseButton = FindViewById<ImageView>(Resource.Id.ivCloseSettings);
            ivCloseButton.Touch += CloseSettings;

            inflatedSettings = FindViewById<ViewGroup>(Resource.Id.vDisplayedSettings);


            ivArrowLeft = FindViewById<ImageView>(Resource.Id.ivSettingsLeftButton);
            ivArrowRight = FindViewById<ImageView>(Resource.Id.ivSettingsRightButton);

            SetInflatedMenu();

            ivArrowLeft.Touch += (object sender, View.TouchEventArgs e) =>
            {
                switch (e.Event.Action)
                {
                    case MotionEventActions.Down:
                        (sender as View).ScaleX = 0.75f;
                        (sender as View).ScaleY = 0.75f;
                        break;
                    case MotionEventActions.Up:
                        (sender as View).ScaleX = 1f;
                        (sender as View).ScaleY = 1f;
                        curPage--;
                        SetInflatedMenu();
                        break;
                }
            };
            ivArrowRight.Touch += (object sender, View.TouchEventArgs e) =>
            {
                switch (e.Event.Action)
                {
                    case MotionEventActions.Down:
                        (sender as View).ScaleX = 0.75f;
                        (sender as View).ScaleY = 0.75f;
                        break;
                    case MotionEventActions.Up:
                        (sender as View).ScaleX = 1f;
                        (sender as View).ScaleY = 1f;
                        curPage ++;
                        SetInflatedMenu();
                        break;
                }
                
            };
        }

        public void SetInflatedMenu()
        {
            inflatedSettings.RemoveAllViews();
            Console.WriteLine(curPage);
            settingsPages[curPage]();

            if (curPage == 0) ivArrowLeft.Visibility = ViewStates.Invisible; else ivArrowLeft.Visibility = ViewStates.Visible;
            if (curPage == settingsPages.Length - 1) ivArrowRight.Visibility = ViewStates.Invisible; else ivArrowRight.Visibility = ViewStates.Visible;
        }

        public void VisualSettings()
        {
            inflatedSettings.AddView(LayoutInflater.From(this).Inflate(Resource.Layout.settings_visual, null));
            firstInflate = true;

            FindViewById<TextView>(Resource.Id.tvIdleAnimsDisplay).Typeface = nameFont;
            FindViewById<TextView>(Resource.Id.tvBgScroll).Typeface = nameFont;

            rgBgScroll = FindViewById<RadioGroup>(Resource.Id.rgKeepBgScroll);
            rgBgScroll.CheckedChange += ChangeToBgScroll;
            rbBGChoice1 = FindViewById<RadioButton>(Resource.Id.rvBgScrollYes);
            rbBGChoice1.Typeface = abilityFont;
            rbBGChoice2 = FindViewById<RadioButton>(Resource.Id.rvBgScrollNo);
            rbBGChoice2.Typeface = abilityFont;

            rgPlayIdleAnims = FindViewById<RadioGroup>(Resource.Id.rgPlayIdleAnims);
            rgPlayIdleAnims.CheckedChange += ChangeToIdleAnims;
            rbIdleChoice1 = FindViewById<RadioButton>(Resource.Id.rvIdleAnimYes);
            rbIdleChoice1.Typeface = abilityFont;
            rbIdleChoice2 = FindViewById<RadioButton>(Resource.Id.rvIdleAnimNo);
            rbIdleChoice2.Typeface = abilityFont;

            if (animateBackground) rbBGChoice1.Checked = true;
            else rbBGChoice2.Checked = true;

            if (animateIdleAnims) rbIdleChoice1.Checked = true;
            else rbIdleChoice2.Checked = true;

            firstInflate = false;
        }
        public void ChangeToBgScroll(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            if (!firstInflate && ivSettingsPortrait != null) ivSettingsPortrait.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.ScrollBackgroundSettings));
            if (rbBGChoice1.Checked) animateBackground = true; else if (rbBGChoice2.Checked) animateBackground = false;

            lastImage = Resource.Drawable.SaveOnlineSettings;

            if (animateBackground && llBackground != null && llBackground.Animation == null) llBackground.StartAnimation(aBackgroundMovement);
            if (!animateBackground && llBackground != null) llBackground.ClearAnimation();
        }

        public void ChangeToIdleAnims(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            if (!firstInflate && ivSettingsPortrait != null) ivSettingsPortrait.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.IdleAnimationSettings));
            lastImage = Resource.Drawable.SaveOnlineSettings;
            if (rbIdleChoice1.Checked) animateIdleAnims = true; else if (rbIdleChoice2.Checked) animateIdleAnims = false;
            HandleIdleAnims(!animateIdleAnims);
        }
        public void HandleIdleAnims(bool toStop)
        {
            foreach (SlotView slot in svCardSlot)
            {
                if (slot != null && slot.slotCard != null)
                {
                    if (toStop && slot.Animation != null && !isInCombat) slot.ClearAnimation();
                    if (!toStop && slot.Animation == null) slot.StartAnimation(slot.slotCard.aIdleAnimation);
                }
            }

        }


        public void AudioSettings()
        {
            inflatedSettings.AddView(LayoutInflater.From(this).Inflate(Resource.Layout.settings_audio, null));

            volumeMusic = sp.GetInt("musicVolume", 100);
            volumeSFX = sp.GetInt("sfxVolume", 100);
            volumeDemon = sp.GetInt("demonVolume", 100);

            sbVolumeMusic = FindViewById<SeekBar>(Resource.Id.sbMusicVolume);
            sbVolumeMusic.Progress = (int)volumeMusic;
            sbVolumeMusic.ProgressChanged += ChangeToMusic;
            sbVolumeSfx = FindViewById<SeekBar>(Resource.Id.sbSFXVolume);
            sbVolumeSfx.Progress = (int)volumeSFX;
            sbVolumeSfx.ProgressChanged += ChangeToSfx;
            sbVolumeDemon = FindViewById<SeekBar>(Resource.Id.sbDemonVolume);
            sbVolumeDemon.Progress = (int)volumeDemon;
            sbVolumeDemon.ProgressChanged += ChangeToDemon;

            FindViewById<TextView>(Resource.Id.tvMusicDisplay).Typeface = nameFont;
            FindViewById<TextView>(Resource.Id.tvSFXDisplay).Typeface = nameFont;
            FindViewById<TextView>(Resource.Id.tvDemonDisplay).Typeface = nameFont;
        }
        public void ChangeToMusic(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            if (ivSettingsPortrait != null) ivSettingsPortrait.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.MusicSettings));
            volumeMusic = sbVolumeMusic.Progress;
            if (mpFightTheme != null) mpFightTheme.SetVolume(volumeMusic / 225, volumeMusic / 225);
        }
        public void ChangeToSfx(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            if (ivSettingsPortrait != null) ivSettingsPortrait.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.SFXSettings));
            lastImage = Resource.Drawable.SFXSettings;

            volumeSFX = sbVolumeSfx.Progress;
        }
        public void ChangeToDemon(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            volumeDemon = sbVolumeDemon.Progress;

            if (ivSettingsPortrait != null) switch (volumeDemon)
                {
                    case 0:
                        MainActivity.mpCombatHandler = MediaPlayer.Create(MainActivity.main, Resource.Raw.DevilVoiceSilenced);
                        MainActivity.mpCombatHandler.Start();
                        MainActivity.mpCombatHandler.Dispose();
                        ivSettingsPortrait.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.DemonVoiceSettingsSilenced));
                        break;
                    case 100:
                        PlayDemon(Resource.Raw.DevilVoiceMaxVolume);
                        ivSettingsPortrait.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.DemonVoiceSettingsMaxVolume));
                        break;
                    default:
                        ivSettingsPortrait.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.DemonVoiceSettings));
                        break;
                }
            lastImage = Resource.Drawable.DemonVoiceSettings;
        }



        public void SaveSettings(object sender, View.TouchEventArgs e)
        {
            switch(e.Event.Action)
            {
                case MotionEventActions.Down:
                    ivSaveButton.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.SaveButtonPressed));
                    break;
                case MotionEventActions.Up:
                    ivSaveButton.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.SaveButtonUnpressed));
                    var editor = sp.Edit();
                    editor.PutInt("musicVolume", (int)volumeMusic);
                    editor.PutInt("sfxVolume", (int)volumeSFX);
                    editor.PutInt("demonVolume", (int)volumeDemon);
                    editor.PutBoolean("animateBackground", animateBackground);
                    editor.PutBoolean("animateIdleAnims", animateIdleAnims);
                    editor.PutInt("lastImage", lastImage);
                    editor.Commit();
                    if (mpFightTheme != null) mpFightTheme.SetVolume(volumeMusic / 225, volumeMusic / 225);
                    break;
            }
            
        }
        public void CloseSettings(object sender, View.TouchEventArgs e)
        {
            switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    ivCloseButton.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.CancelButtonPressed));
                    break;
                case MotionEventActions.Up:
                    volumeMusic = sp.GetInt("musicVolume", 100);
                    volumeSFX = sp.GetInt("sfxVolume", 100);
                    volumeDemon = sp.GetInt("demonVolume", 100);
                    animateBackground = sp.GetBoolean("animateBackground", true);
                    animateIdleAnims = sp.GetBoolean("animateIdleAnims", true);

                    if (mpFightTheme != null) mpFightTheme.SetVolume(volumeMusic / 225, volumeMusic / 225);

                    if (animateBackground && llBackground != null && llBackground.Animation == null) llBackground.StartAnimation(aBackgroundMovement);
                    if (!animateBackground && llBackground != null) llBackground.ClearAnimation();

                    HandleIdleAnims(!animateIdleAnims);

                    ivCloseButton.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.CancelButtonUnpressed));
                    btnStart.Visibility = ViewStates.Visible;
                    btnOptions.Visibility = ViewStates.Visible;
                    btnHigh.Visibility = ViewStates.Visible;

                    vModal.StartAnimation(AnimationUtils.LoadAnimation(this, Resource.Animation.anim_FadeOut));
                    vSettings.StartAnimation(AnimationUtils.LoadAnimation(this, Resource.Animation.anim_SettingsMenuFloatBack));

                    if (rlParentLayout == null)
                    {
                        rlMainMenu.RemoveView(vModal);
                        rlMainMenu.RemoveView(vSettings);
                    }
                    else
                    {
                        rlParentLayout.RemoveView(vModal);
                        rlParentLayout.RemoveView(vSettings);
                    }


                    if (ivSettingsButton != null) ivSettingsButton.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.SettingsButton));
                    isInSettings = false;
                    isInCombat = false;
                    break;
            }
            
        }
        
        public void ChangeToSave(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            if (ivSettingsPortrait != null) ivSettingsPortrait.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.SaveOnlineSettings));
        }




        public void LoadHighScoreMenu(object sender, EventArgs e)
        {
            if (btnHigh != null) btnHigh.Visibility = ViewStates.Gone;
            if (btnOptions != null) btnOptions.Visibility = ViewStates.Gone;
            if(btnStart != null) btnStart.Visibility = ViewStates.Gone;
            liInflateSettings = LayoutInflater.From(this);
            vHigh = liInflateSettings.Inflate(Resource.Layout.activity_highscores, null);
            vHigh.SetY(vHigh.GetY() + 150);
            rlMainMenu.AddView(vHigh);
            vHigh.StartAnimation(aJumpTitle);
            rvScoreboard = FindViewById<RecyclerView>(Resource.Id.rvScoreboard);
            LinearLayoutManager lmScoreManager = new LinearLayoutManager(this, LinearLayoutManager.Vertical, false);
            rvScoreboard.SetLayoutManager(lmScoreManager);
            slCreateScoreboard.RetrieveScoresFromDatabase();
            sAdapter = new ScoreAdapter(scoreboard);
            rvScoreboard.SetAdapter(sAdapter);
            ivReturnToMenu = FindViewById<ImageView>(Resource.Id.ivCloseHighScores);
            ivReturnToMenu.Touch += ExitHighScores;
        }

        public static void SaveScoreToDatabase(Score toSave)
        {
            HashMap sending = new HashMap();
            sending.Put("score", toSave.score);
            sending.Put("name", toSave.name);
            var scoreGather = dbScoreStorage.GetReference("scoreList").Push();
            scoreGather.SetValue(sending);
        }

        public void ExitHighScores(object sender, View.TouchEventArgs e)
        {
            switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    PlaySFX(Resource.Raw.ButtonPressedSFX);
                    ivReturnToMenu.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.CancelButtonPressed));
                    break;
                case MotionEventActions.Up:
                    PlaySFX(Resource.Raw.ButtonReleasedSFX);
                    ivReturnToMenu.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.CancelButtonUnpressed));
                    btnStart.Visibility = ViewStates.Visible;
                    btnOptions.Visibility = ViewStates.Visible;
                    btnHigh.Visibility = ViewStates.Visible;
                    rlMainMenu.RemoveView(vHigh);
                    break;
            }
        }


        public void LoadGameMenu(object sender, EventArgs e)
        {
            SetContentView(Resource.Layout.activity_main);
            rlParentLayout = FindViewById<RelativeLayout>(Resource.Id.rlGameMenu);

            rvDeck = FindViewById<RecyclerView>(Resource.Id.rvCardDeck);
            LinearLayoutManager lmCardManager = new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false);
            rvDeck.SetLayoutManager(lmCardManager);
            cardPile = new List<Card>() { };
            cAdapter = new CardAdapter(cardPile);
            rvDeck.SetAdapter(cAdapter);

            LoadCards(5);
            LoadCards(0, new Card[] { new Card(25) });

            LoadBoard();

            ivCombatButton = FindViewById<ImageView>(Resource.Id.ivPlayButton);
            ivCombatButton.Touch += ButtonPressed;

            llBackground = FindViewById<LinearLayout>(Resource.Id.llBackground);
            aBackgroundMovement = AnimationUtils.LoadAnimation(this, Resource.Animation.anim_BackgroundScroll);
            aBackgroundMovement.Interpolator = new LinearInterpolator();
            if (animateBackground) llBackground.StartAnimation(aBackgroundMovement);

            svBgContainer = FindViewById<BackgroundScrollView>(Resource.Id.svBgContainer);
            svBgContainer.Touch += (sender, e) => { return; };
            svBgContainer.SmoothScrollingEnabled = false;

            svBgContainer.ScrollChange += (sender, e) => { return; };
            svBgContainer.Enabled = false;
            svBgContainer.VerticalScrollBarEnabled = false;
            svBgContainer.HorizontalScrollBarEnabled = false;
            svBgContainer.SetOnTouchListener(svBgContainer);

            mpFightTheme = MediaPlayer.Create(this, Resource.Raw.FightTheme);
            mpFightTheme.Looping = true;
            mpFightTheme.SetVolume(volumeMusic / 225, volumeMusic / 225);
            mpFightTheme.Start();

            timeElapsed = MAX_TURNS;

            tvCountDown = FindViewById<TextView>(Resource.Id.tvCountDown);
            tvCountDown.Typeface = statsFont;
            tvCountDown.Text = "Turn: " + Convert.ToString(timeElapsed);
            tvCountKills = FindViewById<TextView>(Resource.Id.tvCountKills);
            tvCountKills.Typeface = statsFont;
            SetKills(0);

            ivSettingsButton = FindViewById<ImageView>(Resource.Id.ivSettingsMenu);
            ivSettingsButton.Click += LoadSettingsMenu;

            isInCombat = false;
        }

        public async void LoadBoard()
        {
            svCardSlot[0, 0] = FindViewById<SlotView>(Resource.Id.svCardSlot11);
            svCardSlot[0, 1] = FindViewById<SlotView>(Resource.Id.svCardSlot12);
            svCardSlot[0, 2] = FindViewById<SlotView>(Resource.Id.svCardSlot13);
            svCardSlot[1, 0] = FindViewById<SlotView>(Resource.Id.svCardSlot21);
            svCardSlot[1, 1] = FindViewById<SlotView>(Resource.Id.svCardSlot22);
            svCardSlot[1, 2] = FindViewById<SlotView>(Resource.Id.svCardSlot23);
            for (int i = 0; i < 3; i++) svCardSlot[1, i].rotation = 1;

            svCardSlot[0, 0].tvDisplayStats[0] = FindViewById<TextView>(Resource.Id.tvMonsterAT11);
            svCardSlot[0, 0].tvDisplayStats[1] = FindViewById<TextView>(Resource.Id.tvMonsterHP11);
            svCardSlot[0, 0].tvDisplayStats[2] = FindViewById<TextView>(Resource.Id.tvMonsterSP11);

            svCardSlot[0, 1].tvDisplayStats[0] = FindViewById<TextView>(Resource.Id.tvMonsterAT12);
            svCardSlot[0, 1].tvDisplayStats[1] = FindViewById<TextView>(Resource.Id.tvMonsterHP12);
            svCardSlot[0, 1].tvDisplayStats[2] = FindViewById<TextView>(Resource.Id.tvMonsterSP12);

            svCardSlot[0, 2].tvDisplayStats[0] = FindViewById<TextView>(Resource.Id.tvMonsterAT13);
            svCardSlot[0, 2].tvDisplayStats[1] = FindViewById<TextView>(Resource.Id.tvMonsterHP13);
            svCardSlot[0, 2].tvDisplayStats[2] = FindViewById<TextView>(Resource.Id.tvMonsterSP13);

            svCardSlot[1, 0].tvDisplayStats[0] = FindViewById<TextView>(Resource.Id.tvMonsterAT21);
            svCardSlot[1, 0].tvDisplayStats[1] = FindViewById<TextView>(Resource.Id.tvMonsterHP21);
            svCardSlot[1, 0].tvDisplayStats[2] = FindViewById<TextView>(Resource.Id.tvMonsterSP21);

            svCardSlot[1, 1].tvDisplayStats[0] = FindViewById<TextView>(Resource.Id.tvMonsterAT22);
            svCardSlot[1, 1].tvDisplayStats[1] = FindViewById<TextView>(Resource.Id.tvMonsterHP22);
            svCardSlot[1, 1].tvDisplayStats[2] = FindViewById<TextView>(Resource.Id.tvMonsterSP22);

            svCardSlot[1, 2].tvDisplayStats[0] = FindViewById<TextView>(Resource.Id.tvMonsterAT23);
            svCardSlot[1, 2].tvDisplayStats[1] = FindViewById<TextView>(Resource.Id.tvMonsterHP23);
            svCardSlot[1, 2].tvDisplayStats[2] = FindViewById<TextView>(Resource.Id.tvMonsterSP23);

            for (int i = 0; i < 2; i++) for (int j = 0; j < 3; j++) for (int k = 0; k < 3; k++)
                    {
                        svCardSlot[i, j].tvDisplayStats[k].Visibility = ViewStates.Invisible;
                        svCardSlot[i, j].tvDisplayStats[k].Typeface = statsFont;
                    }
        }



        //Functions handling the combat button, as well as intializing combat itself
        private void ButtonPressed(object sender, View.TouchEventArgs e)
        {
            if (isInCombat || isInSettings) return;
            switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    ivCombatButton.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.PlayButtonPressed));
                    if (SlotView.crosshair != null) rlParentLayout.RemoveView(SlotView.crosshair);
                    if (SlotView.infoTab != null) rlParentLayout.RemoveView(SlotView.infoTab);
                    ivSettingsButton.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.SettingsButton));
                    break;
                case MotionEventActions.Up:
                    if (AllHaveTarget())
                    {
                        rlParentLayout.RemoveView(SlotView.crosshair);
                        rlParentLayout.RemoveView(SlotView.infoTab);
                        InitiateCombat();
                    }
                    break;
            }
        }

        public bool AllHaveTarget()
        {
            int nullCount = 0;
            bool allHaveTarget = true;
            for (int i = 0; i < 2; i++) for (int j = 0; j < 3; j++)
                {
                    if (svCardSlot[i, j].slotCard == null) nullCount++;
                    else if (svCardSlot[i, j].targetSlot == null)
                    {
                        Animation animMissing = AnimationUtils.LoadAnimation(this, Resource.Animation.anim_textBounce);
                        svCardSlot[i, j].StartAnimation(animMissing);
                        allHaveTarget = false;
                    }
                }
            if (!allHaveTarget || nullCount == 6)
            {
                ivCombatButton.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.PlayButtonUnpressed));
            }
            if (nullCount == 5 && cardPile.Count == 0) LoadCards(1);
            canPress = true;
            if (nullCount != 6) return allHaveTarget; else return false;
        }

        public async Task InitiateCombat()
        {
            isInCombat = true;
            await Task.Delay(await DevilWarCry());

            foreach (SlotView slot in TurnOrder())
            {
                if (slot.slotCard != null && slot.slotCard.cAT > 0)
                    if (slot.slotCard.cAbility.canAttack) await slot.CardStrike();
            }

            LoadCards(3);

            mpCombatHandler.Dispose();

            CheckStartOfTurnAbilities();
            UnleashProjectiles();

            if (playStarkill) await DevilCurKillComment();

            mpCombatHandler.Dispose();

            timeElapsed--;
            tvCountDown.Text = "Turn: " + Convert.ToString(timeElapsed);
            curKills = 0;

            if (timeElapsed == 0) LoadSaveMenu();
            else
            {
                isInCombat = false;
                ivCombatButton.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.PlayButtonUnpressed));
            }
            
        }

        public static void SetKills(int newKills)
        {
            kills = newKills;
            tvCountKills.Text = "Kill: " + Convert.ToString(kills);
        }

        public static async Task CheckStartOfTurnAbilities()
        {
            for (int i = 0; i < 2; i++) for (int j = 0; j < 3; j++)
                {
                    SlotView toCheck = svCardSlot[i, j];
                    if (toCheck != null && toCheck.slotCard != null && toCheck.slotCard.cAbility != null) await toCheck.slotCard.cAbility.OnStartOfTurn(toCheck);
                }
                        
        }

        public static async Task UnleashProjectiles()
        {
            foreach (Projectile projectile in lProjectileBank)
            {
                await Task.Delay(150);
                projectile.turnsLeft--;
                
                if (projectile.turnsLeft == 0) await projectile.hit();
            }

            lProjectileBank.RemoveAll(proj => proj.turnsLeft == 0);
        }

        //Returns the slots sorted by each card's speed stat, provided the slot has a monster spawned and is able to attack
        public static IEnumerable<SlotView> TurnOrder()
        {
            List<SlotView> ordered = new List<SlotView>();
            for (int i = 0; i < 2; i++) for (int j = 0; j < 3; j++) if (svCardSlot[i, j].slotCard != null) ordered.Add(svCardSlot[i, j]);

            return ordered.OrderByDescending(slot => slot.slotCard.cSP).ToList();
        }

        public async Task<TimeSpan> DevilWarCry()
        {
            if (volumeDemon == 0) return TimeSpan.FromSeconds(0);

            if (AllCardsNoAT()) stallCounter++; else if (stallCounter > 0) stallCounter = -1; else stallCounter = 0;
                switch (stallCounter)
                {
                    case 0:
                        await PlayDemon(Resource.Raw.DevilVoiceFight);
                    return TimeSpan.FromSeconds(0.75);
                    case 1:
                        await PlayDemon(Resource.Raw.DevilVoiceStartStalling);
                    return TimeSpan.FromSeconds(2);
                    case 2:
                        await PlayDemon(Resource.Raw.DevilVoiceKeepStalling);
                    return TimeSpan.FromSeconds(2);
                    case -1:
                        await PlayDemon(Resource.Raw.DevilVoiceNoMoreStalling);
                    return TimeSpan.FromSeconds(2);
                default:
                    await PlaySFX(Resource.Raw.SaveSFX);
                    return TimeSpan.FromSeconds(0);
                }
            throw new IndexOutOfRangeException("Devil's stallCounter is behaving unexpectadly. stallCounter = " + stallCounter);
        }

        //returns true if none of the cards on board have 0AT / can't attack due to their ability
        public bool AllCardsNoAT()
        {
            for (int i = 0; i < 2; i++) for (int j = 0; j < 3; j++)
                {
                    if (svCardSlot[i, j].slotCard != null && svCardSlot[i, j].slotCard.cAT > 0 && svCardSlot[i,j].slotCard.cAbility.canAttack) return false;
                }

            return true;
        }

        //Makes The Devil comment on the amount of kills you've gotten in a round
        public async Task DevilCurKillComment()
        {
            try
            {
                switch (curKills)
                {
                    case 4:
                        await PlayDemon(Resource.Raw.DevilVoice3Kills);
                        await StarkillScreen("NICE SHOT");
                        break;

                    case 5:
                        await PlayDemon(Resource.Raw.DevilVoice5Kills);
                        await StarkillScreen("SHEER BRUTE");
                        break;

                    case 1:
                    case 2:
                    case 3:
                    case 0:
                        break;

                    default:
                        await PlayDemon(Resource.Raw.DevilVoice6Kills);
                        await StarkillScreen("HOLE IN ONE");
                        break;
                }
            } catch (Exception e)
            {
                Console.WriteLine(errTxt);
                Console.WriteLine(e);
            }
            curKills = 0;
        }
        public async Task StarkillScreen(string displayText)
        {
            try
            {
                if (rlParentLayout != null)
                {
                    await Task.Delay(70);
                    await PlaySFX(Resource.Raw.StarkillSFX);
                    await Task.Delay(40);

                    LayoutInflater lInflater = LayoutInflater.From(this);
                    View starView = lInflater.Inflate(Resource.Layout.starkill_layout, null);

                    Animation animEffect = AnimationUtils.LoadAnimation(this, Resource.Animation.anim_StarkillSpin);
                    Animation animMegamanEffect = AnimationUtils.LoadAnimation(this, Resource.Animation.anim_FadeOut);
                    animMegamanEffect.Duration = animEffect.Duration /2;
                    animMegamanEffect.AnimationEnd += (sender, e) =>
                    {
                        rlParentLayout.RemoveView(starView);
                    };

                    Animation animStart = AnimationUtils.LoadAnimation(this, Resource.Animation.anim_FadeIn);
                    animStart.Duration = 165;
                    animStart.AnimationEnd += (sender, e) =>
                    {
                        starView.StartAnimation(animMegamanEffect);
                        starView.StartAnimation(animMegamanEffect);
                    };

                    TextView tvComment = starView.FindViewById<TextView>(Resource.Id.tvStarkillText);
                    tvComment.Text = displayText;
                    tvComment.Typeface = starkillFont;
                    tvComment.Alpha = 1f;

                    rlParentLayout.AddView(starView);
                    starView.StartAnimation(animStart);
                    FindViewById<ImageView>(Resource.Id.ivStars).StartAnimation(animEffect);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(errTxt);
                Console.WriteLine(ex.ToString());
            }

        }

        //Has 3 functions all of which related to adding cards to the deck: It can give out a selected amount of random cards from either the entire or a selected card pool, or give a predetermined list of cards
        public static void LoadCards(int amount = 0, Card[] content = null, int[] range = null)
        {
            if (content != null) for (int i = 0; i < content.Count(); i++)
                {
                    cardPile.Add(content[i]);
                    if (content[i].cAbility != null) content[i].cAbility.OnDrawnFromDeck(content[i]);
                }

            System.Random rand = new System.Random();
            if (range != null) for (int i = 0; i < amount; i++)
                {
                    Card newCard = new Card(rand.Next(0, range.Length - 1));
                    cardPile.Add(newCard);
                    if (newCard.cAbility != null) newCard.cAbility.OnDrawnFromDeck(newCard);
                }

            else for(int i = 0; i < amount; i++)
                {
                    Card newCard = new Card(rand.Next(0, Card.CardCount));
                    cardPile.Add(newCard);
                    if (newCard.cAbility != null) newCard.cAbility.OnDrawnFromDeck(newCard);
                }
            rvDeck.GetAdapter().NotifyDataSetChanged();
        }

        public static async Task PlaySFX(int sfxId, int volDivLeft = 100, int volDivRight = 100)
        {
            MainActivity.mpCombatHandler = MediaPlayer.Create(MainActivity.main, sfxId);
            MainActivity.mpCombatHandler.Start();
            MainActivity.mpCombatHandler.SetVolume(MainActivity.volumeSFX / volDivLeft, MainActivity.volumeSFX / volDivRight);
            MainActivity.mpCombatHandler.Dispose();
        }

        public static async Task PlayDemon(int sfxId, int volDivLeft = 100, int volDivRight = 100)
        {
            MainActivity.mpCombatHandler = MediaPlayer.Create(MainActivity.main, sfxId);
            MainActivity.mpCombatHandler.Start();
            MainActivity.mpCombatHandler.SetVolume(MainActivity.volumeDemon / volDivLeft, MainActivity.volumeDemon / volDivRight);
            MainActivity.mpCombatHandler.Dispose();
        }

        public void LoadSaveMenu()
        {
            if (!postOnline)
            {
                LoadMainMenu();
                return;
            }
            isInSettings = true;
            for (int i = 0; i < 2; i++) for (int j = 0; j < 3; j++) svCardSlot[i, j].ClearAnimation();
            mpFightTheme.Stop();
            liInflateSettings = LayoutInflater.From(this);
            vSaveMenu = liInflateSettings.Inflate(Resource.Layout.activity_savescore, null);
            rlParentLayout.AddView(vSaveMenu);
            vSaveMenu.SetX(vSaveMenu.GetX() + 120);
            vSaveMenu.SetY(vSaveMenu.GetY() + 700);
            vSaveMenu.StartAnimation(aJumpTitle);
            etPickName = FindViewById<EditText>(Resource.Id.etInsertName);
            etPickName.Typeface = abilityFont;
            btnSAVE = FindViewById<Button>(Resource.Id.btnSaveToFirebase);
            btnSAVE.Click += ReturnToTitle;
        }

        public void ReturnToTitle(object sender, EventArgs e)
        {
            if (etPickName.Text == "")
            {
                Animation animMissing = AnimationUtils.LoadAnimation(this, Resource.Animation.anim_textBounce);
                etPickName.StartAnimation(animMissing);
                return;
            }
            isInCombat = false;
            isInSettings = false;
            rlParentLayout.RemoveView(vSaveMenu);
            SaveScoreToDatabase(new Score(kills, etPickName.Text));
            PlaySFX(Resource.Raw.SaveSFX);
            etPickName.Text = "";
            kills = 0;
            timeElapsed = 20;
            LoadMainMenu();
        }
    }

    [Register("Rumblebash.BackgroundScrollView")]
    public class BackgroundScrollView : HorizontalScrollView, Android.Views.View.IOnTouchListener
    {
        public BackgroundScrollView(Context context) : base(context) { }

        public BackgroundScrollView(Context context, IAttributeSet attrs) : base(context, attrs) { }

        public BackgroundScrollView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

        public BackgroundScrollView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes) { }

        protected BackgroundScrollView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

        public bool OnTouch(View v, MotionEvent e)
        {
            return false;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            return false;
        }
    }

}