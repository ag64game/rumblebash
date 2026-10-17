namespace AgaBaga.Rumblebash.Menus
{
    public static class SettingsMenu
    {
        //Loads the settings menu, adds the required variables from SharedPreferences to it
        public static void LoadSettingsMenu(object sender, EventArgs e)
        {
            if (isInSettings) return;
            if (ivSettingsButton != null) ivSettingsButton.SetImageDrawable(main.GetDrawable(Resource.Drawable.SettingsButtonPressed));
            isInSettings = true;

            postOnline = sp.GetBoolean("onlinePosting", true);

            btnStart.Visibility = ViewStates.Gone;
            btnOptions.Visibility = ViewStates.Gone;

            liInflateSettings = LayoutInflater.From(main);

            vSettings = liInflateSettings.Inflate(Resource.Layout.activity_options, null);
            vSettings.SetY(main.WindowManager.DefaultDisplay.Height / 7);
            //vSettings.SetX(WindowManager.DefaultDisplay.Width / 30);

            vModal = liInflateSettings.Inflate(Resource.Layout.modal_layout, null);
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
            Animation animSettingsPop = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_SettingsMenuFloat);
            animSettingsPop.Interpolator = new DecelerateInterpolator();
            vSettings.StartAnimation(animSettingsPop);

            vModal.StartAnimation(AnimationUtils.LoadAnimation(main, Resource.Animation.anim_FadeIn));
            ivSettingsPortrait = main.FindViewById<ImageView>(Resource.Id.ivSettingsPortrait);
            ivSettingsPortrait.SetImageDrawable(main.GetDrawable(sp.GetInt("lastImage", Resource.Drawable.MusicSettings)));

            main.FindViewById<TextView>(Resource.Id.tvOptionsTitle).Typeface = nameFont;

            ivSaveButton = main.FindViewById<ImageView>(Resource.Id.ivSaveButton);
            ivSaveButton.Touch += SaveSettings;
            ivCloseButton = main.FindViewById<ImageView>(Resource.Id.ivCloseSettings);
            ivCloseButton.Touch += CloseSettings;

            inflatedSettings = main.FindViewById<ViewGroup>(Resource.Id.vDisplayedSettings);


            ivArrowLeft = main.FindViewById<ImageView>(Resource.Id.ivSettingsLeftButton);
            ivArrowRight = main.FindViewById<ImageView>(Resource.Id.ivSettingsRightButton);

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
                        curPage++;
                        SetInflatedMenu();
                        break;
                }

            };
        }

        public static void SetInflatedMenu()
        {
            inflatedSettings.RemoveAllViews();
            Console.WriteLine(curPage);
            settingsPages[curPage]();

            if (curPage == 0) ivArrowLeft.Visibility = ViewStates.Invisible; else ivArrowLeft.Visibility = ViewStates.Visible;
            if (curPage == settingsPages.Length - 1) ivArrowRight.Visibility = ViewStates.Invisible; else ivArrowRight.Visibility = ViewStates.Visible;
        }

        public static void VisualSettings()
        {
            inflatedSettings.AddView(LayoutInflater.From(main).Inflate(Resource.Layout.settings_visual, null));
            firstInflate = true;

            main.FindViewById<TextView>(Resource.Id.tvIdleAnimsDisplay).Typeface = nameFont;
            main.FindViewById<TextView>(Resource.Id.tvBgScroll).Typeface = nameFont;
            main.FindViewById<TextView>(Resource.Id.tvStarkillDisplay).Typeface = nameFont;

            rgBgScroll = main.FindViewById<RadioGroup>(Resource.Id.rgKeepBgScroll);
            rgBgScroll.CheckedChange += ChangeToBgScroll;
            rbBGChoice1 = main.FindViewById<RadioButton>(Resource.Id.rvBgScrollYes);
            rbBGChoice1.Typeface = abilityFont;
            rbBGChoice2 = main.FindViewById<RadioButton>(Resource.Id.rvBgScrollNo);
            rbBGChoice2.Typeface = abilityFont;

            rgPlayIdleAnims = main.FindViewById<RadioGroup>(Resource.Id.rgPlayIdleAnims);
            rgPlayIdleAnims.CheckedChange += ChangeToIdleAnims;
            rbIdleChoice1 = main.FindViewById<RadioButton>(Resource.Id.rvIdleAnimYes);
            rbIdleChoice1.Typeface = abilityFont;
            rbIdleChoice2 = main.FindViewById<RadioButton>(Resource.Id.rvIdleAnimNo);
            rbIdleChoice2.Typeface = abilityFont;

            if (animateBackground) rbBGChoice1.Checked = true;
            else rbBGChoice2.Checked = true;

            if (animateIdleAnims) rbIdleChoice1.Checked = true;
            else rbIdleChoice2.Checked = true;

            firstInflate = false;
        }
        public static void ChangeToBgScroll(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            if (!firstInflate && ivSettingsPortrait != null) ivSettingsPortrait.SetImageDrawable(main.GetDrawable(Resource.Drawable.ScrollBackgroundSettings));
            if (rbBGChoice1.Checked) animateBackground = true; else if (rbBGChoice2.Checked) animateBackground = false;

            lastImage = Resource.Drawable.SaveOnlineSettings;

            if (animateBackground && llBackground != null && llBackground.Animation == null) llBackground.StartAnimation(aBackgroundMovement);
            if (!animateBackground && llBackground != null) llBackground.ClearAnimation();
        }

        public static void ChangeToIdleAnims(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            if (!firstInflate && ivSettingsPortrait != null) ivSettingsPortrait.SetImageDrawable(main.GetDrawable(Resource.Drawable.IdleAnimationSettings));
            lastImage = Resource.Drawable.SaveOnlineSettings;
            if (rbIdleChoice1.Checked) animateIdleAnims = true; else if (rbIdleChoice2.Checked) animateIdleAnims = false;
            HandleIdleAnims(!animateIdleAnims);
        }
        public static void HandleIdleAnims(bool toStop)
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


        public static void AudioSettings()
        {
            inflatedSettings.AddView(LayoutInflater.From(main).Inflate(Resource.Layout.settings_audio, null));

            volumeMusic = sp.GetInt("musicVolume", 100);
            volumeSFX = sp.GetInt("sfxVolume", 100);
            volumeDemon = sp.GetInt("demonVolume", 100);

            sbVolumeMusic = main.FindViewById<SeekBar>(Resource.Id.sbMusicVolume);
            sbVolumeMusic.Progress = (int)volumeMusic;
            sbVolumeMusic.ProgressChanged += ChangeToMusic;
            sbVolumeSfx = main.FindViewById<SeekBar>(Resource.Id.sbSFXVolume);
            sbVolumeSfx.Progress = (int)volumeSFX;
            sbVolumeSfx.ProgressChanged += ChangeToSfx;
            sbVolumeDemon = main.FindViewById<SeekBar>(Resource.Id.sbDemonVolume);
            sbVolumeDemon.Progress = (int)volumeDemon;
            sbVolumeDemon.ProgressChanged += ChangeToDemon;

            main.FindViewById<TextView>(Resource.Id.tvMusicDisplay).Typeface = nameFont;
            main.FindViewById<TextView>(Resource.Id.tvSFXDisplay).Typeface = nameFont;
            main.FindViewById<TextView>(Resource.Id.tvDemonDisplay).Typeface = nameFont;
        }
        public static void ChangeToMusic(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            if (ivSettingsPortrait != null) ivSettingsPortrait.SetImageDrawable(main.GetDrawable(Resource.Drawable.MusicSettings));
            volumeMusic = sbVolumeMusic.Progress;
            if (mpFightTheme != null) mpFightTheme.SetVolume(volumeMusic / 225, volumeMusic / 225);
        }
        public static void ChangeToSfx(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            if (ivSettingsPortrait != null) ivSettingsPortrait.SetImageDrawable(main.GetDrawable(Resource.Drawable.SFXSettings));
            lastImage = Resource.Drawable.SFXSettings;

            volumeSFX = sbVolumeSfx.Progress;
        }
        public static void ChangeToDemon(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            volumeDemon = sbVolumeDemon.Progress;

            if (ivSettingsPortrait != null) switch (volumeDemon)
                {
                    case 0:
                        mpCombatHandler = MediaPlayer.Create(main, Resource.Raw.DevilVoiceSilenced);
                        mpCombatHandler.Start();
                        mpCombatHandler.Dispose();
                        ivSettingsPortrait.SetImageDrawable(main.GetDrawable(Resource.Drawable.DemonVoiceSettingsSilenced));
                        break;
                    case 100:
                        mpCombatHandler = MediaPlayer.Create(main, Resource.Raw.DevilVoiceMaxVolume);
                        ivSettingsPortrait.SetImageDrawable(main.GetDrawable(Resource.Drawable.DemonVoiceSettingsMaxVolume));
                        break;
                    default:
                        ivSettingsPortrait.SetImageDrawable(main.GetDrawable(Resource.Drawable.DemonVoiceSettings));
                        break;
                }
            lastImage = Resource.Drawable.DemonVoiceSettings;
        }



        public static void SaveSettings(object sender, View.TouchEventArgs e)
        {
            switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    ivSaveButton.SetImageDrawable(main.GetDrawable(Resource.Drawable.SaveButtonPressed));
                    break;
                case MotionEventActions.Up:
                    ivSaveButton.SetImageDrawable(main.GetDrawable(Resource.Drawable.SaveButtonUnpressed));
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
        public static void CloseSettings(object sender, View.TouchEventArgs e)
        {
            switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    ivCloseButton.SetImageDrawable(main.GetDrawable(Resource.Drawable.CancelButtonPressed));
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

                    ivCloseButton.SetImageDrawable(main.GetDrawable(Resource.Drawable.CancelButtonUnpressed));
                    btnStart.Visibility = ViewStates.Visible;
                    btnOptions.Visibility = ViewStates.Visible;

                    vModal.StartAnimation(AnimationUtils.LoadAnimation(main, Resource.Animation.anim_FadeOut));
                    vSettings.StartAnimation(AnimationUtils.LoadAnimation(main, Resource.Animation.anim_SettingsMenuFloatBack));

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


                    if (ivSettingsButton != null) ivSettingsButton.SetImageDrawable(main.GetDrawable(Resource.Drawable.SettingsButton));
                    isInSettings = false;
                    isInCombat = false;
                    break;
            }

        }

        public static void ChangeToSave(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            if (ivSettingsPortrait != null) ivSettingsPortrait.SetImageDrawable(main.GetDrawable(Resource.Drawable.SaveOnlineSettings));
        }
    }
}
