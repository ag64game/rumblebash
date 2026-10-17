namespace AgaBaga.Rumblebash.Menus
{
    public static class MainMenu
    {
        public static void LoadMainMenu()
        {
            if (mpFightTheme != null) mpFightTheme.Stop();
            main.SetContentView(Resource.Layout.activity_start);
            rlMainMenu = main.FindViewById<RelativeLayout>(Resource.Id.rlStartMenu);
            rlParentLayout = null;
            ivTitle = main.FindViewById<ImageView>(Resource.Id.ivGameTitle);
            btnStart = main.FindViewById<Button>(Resource.Id.btnPlay);
            btnOptions = main.FindViewById<Button>(Resource.Id.btnOptions);
            aJumpTitle = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_TitleJump);
            ivTitle.StartAnimation(aJumpTitle);
            btnStart.Click += LoadGameMenu;
            btnOptions.Click += LoadSettingsMenu;
            btnStart.Typeface = statsFont;
            btnOptions.Typeface = statsFont;
            main.FindViewById<TextView>(Resource.Id.tvCreatorName).Typeface = statsFont;
            main.FindViewById<TextView>(Resource.Id.tvVersionName).Typeface = statsFont;
        }
    }
}
