namespace AgaBaga.Rumblebash.Game
{
    public static class BoardSetup
    {
        public static async void LoadBoard()
        {
            svCardSlot[0, 0] = main.FindViewById<SlotView>(Resource.Id.svCardSlot11);
            svCardSlot[0, 1] = main.FindViewById<SlotView>(Resource.Id.svCardSlot12);
            svCardSlot[0, 2] = main.FindViewById<SlotView>(Resource.Id.svCardSlot13);
            svCardSlot[1, 0] = main.FindViewById<SlotView>(Resource.Id.svCardSlot21);
            svCardSlot[1, 1] = main.FindViewById<SlotView>(Resource.Id.svCardSlot22);
            svCardSlot[1, 2] = main.FindViewById<SlotView>(Resource.Id.svCardSlot23);
            for (int i = 0; i < 3; i++) svCardSlot[1, i].rotation = 1;

            svCardSlot[0, 0].tvDisplayStats[0] = main.FindViewById<TextView>(Resource.Id.tvMonsterAT11);
            svCardSlot[0, 0].tvDisplayStats[1] = main.FindViewById<TextView>(Resource.Id.tvMonsterHP11);
            svCardSlot[0, 0].tvDisplayStats[2] = main.FindViewById<TextView>(Resource.Id.tvMonsterSP11);

            svCardSlot[0, 1].tvDisplayStats[0] = main.FindViewById<TextView>(Resource.Id.tvMonsterAT12);
            svCardSlot[0, 1].tvDisplayStats[1] = main.FindViewById<TextView>(Resource.Id.tvMonsterHP12);
            svCardSlot[0, 1].tvDisplayStats[2] = main.FindViewById<TextView>(Resource.Id.tvMonsterSP12);

            svCardSlot[0, 2].tvDisplayStats[0] = main.FindViewById<TextView>(Resource.Id.tvMonsterAT13);
            svCardSlot[0, 2].tvDisplayStats[1] = main.FindViewById<TextView>(Resource.Id.tvMonsterHP13);
            svCardSlot[0, 2].tvDisplayStats[2] = main.FindViewById<TextView>(Resource.Id.tvMonsterSP13);

            svCardSlot[1, 0].tvDisplayStats[0] = main.FindViewById<TextView>(Resource.Id.tvMonsterAT21);
            svCardSlot[1, 0].tvDisplayStats[1] = main.FindViewById<TextView>(Resource.Id.tvMonsterHP21);
            svCardSlot[1, 0].tvDisplayStats[2] = main.FindViewById<TextView>(Resource.Id.tvMonsterSP21);

            svCardSlot[1, 1].tvDisplayStats[0] = main.FindViewById<TextView>(Resource.Id.tvMonsterAT22);
            svCardSlot[1, 1].tvDisplayStats[1] = main.FindViewById<TextView>(Resource.Id.tvMonsterHP22);
            svCardSlot[1, 1].tvDisplayStats[2] = main.FindViewById<TextView>(Resource.Id.tvMonsterSP22);

            svCardSlot[1, 2].tvDisplayStats[0] = main.FindViewById<TextView>(Resource.Id.tvMonsterAT23);
            svCardSlot[1, 2].tvDisplayStats[1] = main.FindViewById<TextView>(Resource.Id.tvMonsterHP23);
            svCardSlot[1, 2].tvDisplayStats[2] = main.FindViewById<TextView>(Resource.Id.tvMonsterSP23);

            for (int i = 0; i < 2; i++) for (int j = 0; j < 3; j++) for (int k = 0; k < 3; k++)
            {
                svCardSlot[i, j].tvDisplayStats[k].Visibility = ViewStates.Invisible;
                svCardSlot[i, j].tvDisplayStats[k].Typeface = statsFont;
            }
        }



        //Functions handling the combat button, as well as intializing combat itself
        private static void ButtonPressed(object sender, View.TouchEventArgs e)
        {
            if (isInCombat || isInSettings) return;
            switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    ivCombatButton.SetImageDrawable(main.GetDrawable(Resource.Drawable.PlayButtonPressed));
                    if (SlotView.crosshair != null) rlParentLayout.RemoveView(SlotView.crosshair);
                    if (SlotView.infoTab != null) rlParentLayout.RemoveView(SlotView.infoTab);
                    ivSettingsButton.SetImageDrawable(main.GetDrawable(Resource.Drawable.SettingsButton));
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
    }
}
