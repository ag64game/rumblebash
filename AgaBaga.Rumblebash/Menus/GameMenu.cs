namespace AgaBaga.Rumblebash.Menus
{
    public static class GameMenu
    {
        public static void LoadGameMenu(object sender, EventArgs e)
        {
            main.SetContentView(Resource.Layout.activity_main);
            rlParentLayout = main.FindViewById<RelativeLayout>(Resource.Id.rlGameMenu);

            rvDeck = main.FindViewById<RecyclerView>(Resource.Id.rvCardDeck);
            LinearLayoutManager lmCardManager = new LinearLayoutManager(main, LinearLayoutManager.Horizontal, false);
            rvDeck.SetLayoutManager(lmCardManager);
            cardPile = new List<Card>() { };
            cAdapter = new CardAdapter(cardPile);
            rvDeck.SetAdapter(cAdapter);

            LoadCards(5);
            LoadCards(0, new Card[] { new Card(25) });

            LoadBoard();

            ivCombatButton = main.FindViewById<ImageView>(Resource.Id.ivPlayButton);
            ivCombatButton.Touch += ButtonPressed;

            llBackground = main.FindViewById<LinearLayout>(Resource.Id.llBackground);
            aBackgroundMovement = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_BackgroundScroll);
            aBackgroundMovement.Interpolator = new LinearInterpolator();
            if (animateBackground) llBackground.StartAnimation(aBackgroundMovement);

            svBgContainer = main.FindViewById<BackgroundScrollView>(Resource.Id.svBgContainer);
            svBgContainer.Touch += (sender, e) => { return; };
            svBgContainer.SmoothScrollingEnabled = false;

            svBgContainer.ScrollChange += (sender, e) => { return; };
            svBgContainer.Enabled = false;
            svBgContainer.VerticalScrollBarEnabled = false;
            svBgContainer.HorizontalScrollBarEnabled = false;
            svBgContainer.SetOnTouchListener(svBgContainer);

            mpFightTheme = MediaPlayer.Create(main, Resource.Raw.FightTheme);
            mpFightTheme.Looping = true;
            mpFightTheme.SetVolume(volumeMusic / 225, volumeMusic / 225);
            mpFightTheme.Start();

            timeElapsed = MAX_TURNS;

            tvCountDown = main.FindViewById<TextView>(Resource.Id.tvCountDown);
            tvCountDown.Typeface = statsFont;
            tvCountDown.Text = "Turn: " + Convert.ToString(timeElapsed);
            tvCountKills = main.FindViewById<TextView>(Resource.Id.tvCountKills);
            tvCountKills.Typeface = statsFont;
            SetKills(0);

            ivSettingsButton = main.FindViewById<ImageView>(Resource.Id.ivSettingsMenu);
            ivSettingsButton.Click += LoadSettingsMenu;

            isInCombat = false;
        }
    }
}
