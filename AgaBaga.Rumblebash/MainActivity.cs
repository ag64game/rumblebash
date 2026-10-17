namespace AgaBaga.Rumblebash
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_start);
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            main = this;
            nameFont = Typeface.CreateFromAsset(Assets, "CardNameFont.TTF");
            abilityFont = Typeface.CreateFromAsset(Assets, "CardAbilityFont.ttf");
            statsFont = Typeface.CreateFromAsset(Assets, "CardStatsFont.ttf");
            starkillFont = Typeface.CreateFromAsset(Assets, "StarkillFont.otf");
            sp = GetSharedPreferences("details", FileCreationMode.Private);
            


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

            Card.SetCardCount();

            HideSystemUI();
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
            if (mpFightTheme != null) mpFightTheme.Start();
        }

        private void HideSystemUI()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
            {
                // Modern Android 11 (API 30) and above approach
                var controller = Window.InsetsController;
                if (controller != null)
                {
                    // Hide both status bar (top) and navigation bar (bottom)
                    controller.Hide(WindowInsets.Type.StatusBars() | WindowInsets.Type.NavigationBars());

                    // Allow user to swipe to bring them back temporarily without resizing the app
                    controller.SystemBarsBehavior = (int)WindowInsetsControllerBehavior.ShowTransientBarsBySwipe;
                }
            }
            else
            {
                // Legacy Android 10 and below approach
#pragma warning disable CS0618 // Type or member is obsolete
                var uiOptions = SystemUiFlags.HideNavigation
                                | SystemUiFlags.Fullscreen
                                | SystemUiFlags.ImmersiveSticky;
                Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;
#pragma warning restore CS0618
            }
        }
    }
}