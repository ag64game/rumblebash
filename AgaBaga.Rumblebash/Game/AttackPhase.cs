namespace AgaBaga.Rumblebash.Game
{
    public static class AttackPhase
    {
        //Functions handling the combat button, as well as intializing combat itself
        public static void ButtonPressed(object sender, View.TouchEventArgs e)
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

        public static bool AllHaveTarget()
        {
            int nullCount = 0;
            bool allHaveTarget = true;
            for (int i = 0; i < 2; i++) for (int j = 0; j < 3; j++)
            {
                if (svCardSlot[i, j].slotCard == null) nullCount++;
                else if (svCardSlot[i, j].targetSlot == null)
                {
                    Animation animMissing = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_textBounce);
                    svCardSlot[i, j].StartAnimation(animMissing);
                    allHaveTarget = false;
                }
            }
            if (!allHaveTarget || nullCount == 6)
            {
                ivCombatButton.SetImageDrawable(main.GetDrawable(Resource.Drawable.PlayButtonUnpressed));
            }
            if (nullCount == 5 && cardPile.Count == 0) LoadCards(1);
            canPress = true;
            if (nullCount != 6) return allHaveTarget; else return false;
        }

        public static async Task InitiateCombat()
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

            if (timeElapsed == 0)
            {
                isInCombat = false;
                isInSettings = false;
                LoadMainMenu();
            }
            else
            {
                isInCombat = false;
                ivCombatButton.SetImageDrawable(main.GetDrawable(Resource.Drawable.PlayButtonUnpressed));
            }

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


    }
}
