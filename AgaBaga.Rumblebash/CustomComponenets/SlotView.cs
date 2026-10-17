namespace AgaBaga.Rumblebash.CustomComponenets
{
    //SlotView implementation, these are the slots on which cards are to be placed
    [Register("Rumblebash.SlotView")]
    public class SlotView : View, Android.Views.View.IOnDragListener, Animation.IAnimationListener
    {
        public Card slotCard = null;
        public SlotView targetSlot = null;
        public static SlotView lastPressed = null;
        public bool firstTurn = true;
        public static bool isInflated = false;
        public static View crosshair, infoTab;
        public int rotation = 0;
        public bool dragCrosshair = false;
        public TextView[] tvDisplayStats = new TextView[3];
        private LayoutInflater lInflater;
        private int[] coords = new int[2];
        private Animation aAttackAnimation, aTextBounce, aSlotSpin;
        private int animsRunning = 0;
        public static bool somethingPressed = false;

        public SlotView(Android.Content.Context context) : base(context)
        {
            this.SetOnDragListener(this);
            this.Click += SlotSelected;
            aSlotSpin = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_RotateSlot);
            aSlotSpin.Interpolator = new Android.Views.Animations.LinearInterpolator();
            StartAnimation(aSlotSpin);
        }
        public SlotView(Android.Content.Context context, IAttributeSet attrs) : base(context, attrs)
        {
            this.SetOnDragListener(this);
            this.Click += SlotSelected;
            aSlotSpin = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_RotateSlot);
            aSlotSpin.Interpolator = new Android.Views.Animations.LinearInterpolator();
            StartAnimation(aSlotSpin);
        }
        public SlotView(Android.Content.Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            this.SetOnDragListener(this);
            this.Click += SlotSelected;
            aSlotSpin = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_RotateSlot);
            aSlotSpin.Interpolator = new Android.Views.Animations.LinearInterpolator();
            StartAnimation(aSlotSpin);
        }
        protected SlotView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            this.SetOnDragListener(this);
            this.Click += SlotSelected;
            aSlotSpin = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_RotateSlot);
            aSlotSpin.Interpolator = new Android.Views.Animations.LinearInterpolator();
            StartAnimation(aSlotSpin);
        }


        //DragListener's OnDrag, checks for dragged cards to place down
        public bool OnDrag(View v, DragEvent e)
        {
            switch (e.Action)
            {
                case DragAction.Entered:
                    v.Invalidate();
                    return true;
                case DragAction.Exited:
                    v.Invalidate();
                    return true;
                case DragAction.Ended:
                    v.Invalidate();
                    return true;
                case DragAction.Drop:
                    if (slotCard == null) PlaceCard(CardViewHolder.cInStorage);
                    v.Invalidate();
                    return true;
                case DragAction.Location: return true;
            }
            return true;
        }

        public void PlaceCard(Card toPlace)
        {
            if (rotation == 0) PlaySFX(Resource.Raw.CardPlacedSFX, 150, 100); else PlaySFX(Resource.Raw.CardPlacedSFX, 100, 150);

            slotCard = toPlace;
            this.SetBackgroundDrawable(new BitmapDrawable(main.Resources, toPlace.cMonster));
            if (rotation == 1) this.ScaleX = -1;

            Animation aPlaceAnim = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_CardPlaced);
            StartAnimation(aPlaceAnim);

            cardPile.Remove(CardViewHolder.cInStorage);
            rvDeck.GetAdapter().NotifyDataSetChanged();
            tvDisplayStats[0].Text = Convert.ToString(slotCard.cAT);
            tvDisplayStats[0].Visibility = ViewStates.Visible;
            tvDisplayStats[1].Text = Convert.ToString(slotCard.cHP);
            tvDisplayStats[1].Visibility = ViewStates.Visible;
            tvDisplayStats[2].Text = Convert.ToString(slotCard.cSP);
            tvDisplayStats[2].Visibility = ViewStates.Visible;
            aTextBounce = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_textBounce);

            rvDeck.Visibility = ViewStates.Visible;

            if (slotCard.cAbility != null) slotCard.cAbility.OnPlace(this);
        }


        //Handles Selecting the slot to choose a target. On turn 1 of a card being placed you can also fetch the card back to
        private void SlotSelected(object sender, EventArgs e)
        {
            if (slotCard == null || isInCombat || isInSettings) return;
            rlParentLayout = main.FindViewById<RelativeLayout>(Resource.Id.rlGameMenu);
            if (isInflated)
            {
                rlParentLayout.RemoveView(crosshair);
                rlParentLayout.RemoveView(infoTab);
                if (lastPressed == this)
                {
                    isInflated = false;
                    return;
                }
            }
            lInflater = LayoutInflater.FromContext(main);
            crosshair = lInflater.Inflate(Resource.Layout.crosshair_layout, null);
            infoTab = lInflater.Inflate(Resource.Layout.info_layout, null);
            rlParentLayout.AddView(crosshair);
            rlParentLayout.AddView(infoTab);
            main.FindViewById<TextView>(Resource.Id.tvStatsDisplayName).Text = slotCard.cName;
            main.FindViewById<TextView>(Resource.Id.tvStatsDisplayName).Typeface = nameFont;
            main.FindViewById<TextView>(Resource.Id.tvStatsDisplayAbility).Text = slotCard.cAbility.desc;
            main.FindViewById<TextView>(Resource.Id.tvStatsDisplayAbility).Typeface = abilityFont;
            if (slotCard.cAbility.chargeTxt == null) main.FindViewById<TextView>(Resource.Id.tvStatsDisplayCharge).Visibility = ViewStates.Gone;
            else
            {
                main.FindViewById<TextView>(Resource.Id.tvStatsDisplayCharge).Visibility = ViewStates.Visible;
                main.FindViewById<TextView>(Resource.Id.tvStatsDisplayCharge).Text = slotCard.cAbility.chargeTxt;
                main.FindViewById<TextView>(Resource.Id.tvStatsDisplayCharge).Typeface = abilityFont;
            }
            if (targetSlot != null)
            {
                targetSlot.GetLocationOnScreen(coords);
                if (rotation == 1) crosshair.SetX(coords[0] + targetSlot.Width / 5 + targetSlot.Width / 10); else crosshair.SetX(coords[0] - targetSlot.Width / 2 - targetSlot.Width / 4);
            }
            else
            {
                (sender as View).GetLocationOnScreen(coords);
                if (rotation == 0) crosshair.SetX(coords[0] + (sender as View).Width / 5 + (sender as View).Width / 10); else crosshair.SetX(coords[0] - (sender as View).Width / 2 - (sender as View).Width / 4);
            }
            crosshair.SetY(coords[1] + (sender as View).Height / 5);
            int[] placerCoords = new int[2];
            (sender as View).GetLocationOnScreen(placerCoords);
            if (rotation == 0) infoTab.SetX(placerCoords[0]); else infoTab.SetX(placerCoords[0] - (sender as View).Width);
            infoTab.SetY(placerCoords[1] - (sender as View).Height / 4);
            isInflated = true;
            lastPressed = this;
            crosshair.Touch += MoveCrosshair;
        }


        //Handles creating the crosshair and moving it around
        private void MoveCrosshair(object sender, View.TouchEventArgs e)
        {
            switch (e.Event.Action)
            {
                case MotionEventActions.Move:
                    crosshair.SetX(e.Event.RawX - (sender as View).Width / 2);
                    crosshair.SetY(e.Event.RawY - (sender as View).Height / 2);
                    break;
                case MotionEventActions.Up:
                    SlotView toTarget = CrosshairCollision(e.Event.RawX - (sender as View).Width / 2, e.Event.RawY - (sender as View).Height);
                    if (toTarget != null)
                    {
                        toTarget.GetLocationOnScreen(coords);
                        if (rotation == 1) crosshair.SetX(coords[0] + toTarget.Width / 5 + toTarget.Width / 10); else crosshair.SetX(coords[0] - toTarget.Width / 2 - toTarget.Width / 4);
                        crosshair.SetY(coords[1] + (sender as View).Height / 6);
                        targetSlot = toTarget;
                    }
                    else
                    {
                        if (targetSlot != null)
                        {
                            targetSlot.GetLocationOnScreen(coords);
                            if (rotation == 1) crosshair.SetX(coords[0] + targetSlot.Width / 5 + targetSlot.Width / 10); else crosshair.SetX(coords[0] - targetSlot.Width / 2 - targetSlot.Width / 4);
                        }
                        else
                        {
                            lastPressed.GetLocationOnScreen(coords);
                            if (rotation == 0) crosshair.SetX(coords[0] + (sender as View).Width / 5 + (sender as View).Width / 9); else crosshair.SetX(coords[0] - (sender as View).Width - (sender as View).Width / 3);
                        }

                        crosshair.SetY(coords[1] + (sender as View).Height / 6);
                    }
                    break;
            }
        }

        public SlotView CrosshairCollision(double touchX, double touchY)
        {
            int checkRow = 1 - rotation;
            for (int i = 0; i < 3; i++) if (touchX >= svCardSlot[checkRow, i].Left && touchX <= svCardSlot[checkRow, i].Right)
                if (touchY <= svCardSlot[checkRow, i].Bottom && touchY >= svCardSlot[checkRow, i].Top)
                {
                    if (svCardSlot[checkRow, i].slotCard != null) return svCardSlot[checkRow, i];
                }
            return null;
        }


        //Handles the combat phase, attacking, getting hit and dying
        public async Task CardStrike()
        {
            if (targetSlot == null) return;

            if (rotation == 0) aAttackAnimation = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_StrikeRotation0);
            else aAttackAnimation = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_StrikeRotation1);
            StartAnimation(aAttackAnimation);
            await Task.Delay(185);

            try
            {
                slotCard.cAbility.OnStrike(this, targetSlot, slotCard.cAT);
                targetSlot.CardHit(slotCard.cAT, this);

            }
            catch (Exception e)
            {
                Console.WriteLine(errTxt);
                Console.WriteLine(e);
            }


        }

        public async Task CardHit(int dmg, SlotView attacker)
        {
            try
            {
                if (slotCard == null)
                {
                    attacker.targetSlot = null;
                    return;
                }
                slotCard.cHP -= dmg;
                if (slotCard.cHP <= 0)
                {
                    await PlaySFX(Resource.Raw.MonsterDeadSFX, 130, 130);

                    if (attacker.slotCard != null) await attacker.slotCard.cAbility.OnKill(attacker, this, dmg);
                    CardDead(attacker, dmg);
                }
                else await PlaySFX(Resource.Raw.MonsterHitSFX, 130, 130);

                await slotCard.cAbility.OnHit(this, attacker, dmg);
                tvDisplayStats[1].Text = Convert.ToString(slotCard.cHP);
                tvDisplayStats[1].StartAnimation(aTextBounce);
            }
            catch (Exception e)
            {
                Console.WriteLine(errTxt);
                Console.WriteLine(e);
            }
        }

        public async Task CardDead(SlotView attacker, int dmg)
        {
            try
            {
                List<SlotView> targeting = CheckForTargeting(this);
                for (int i = 0; targeting != null && i < targeting.Count; i++) targeting[i].targetSlot = null;
                if (slotCard.cAbility != null) await slotCard.cAbility.OnDie(this, attacker, Math.Abs(dmg));
                slotCard = null;
                targetSlot = null;

                ClearAnimation();
                for (int i = 0; i < 2; i++) tvDisplayStats[i].ClearAnimation();

                SetBackgroundDrawable(main.Resources.GetDrawable(Resource.Drawable.SlotPortal));
                StartAnimation(aSlotSpin);
                for (int i = 0; i < 3; i++) tvDisplayStats[i].Visibility = ViewStates.Invisible;

                curKills++;
                SetKills(kills + 1);

            }
            catch (Exception e)
            {
                Console.WriteLine(errTxt);
                Console.WriteLine(e);
            }
        }

        public async Task ChangeAT(int amount, SlotView changer)
        {
            slotCard.cAT += amount;
            if (slotCard.cAT < 0) slotCard.cAT = 0;
            if (slotCard.cAT > 99) slotCard.cAT = 99;
            if (slotCard.cAbility != null) slotCard.cAbility.OnUpdateAT(this, amount, changer);
            tvDisplayStats[0].Text = Convert.ToString(slotCard.cAT);
            tvDisplayStats[0].StartAnimation(aTextBounce);
        }
        public async Task SetAT(int amount, SlotView changer)
        {
            slotCard.cAT = amount;
            if (slotCard.cAT < 0) slotCard.cAT = 0;
            if (slotCard.cAT > 99) slotCard.cAT = 99;
            if (slotCard.cAbility != null) slotCard.cAbility.OnUpdateAT(this, amount, changer);
            tvDisplayStats[0].Text = Convert.ToString(slotCard.cAT);
            tvDisplayStats[0].StartAnimation(aTextBounce);
        }

        public async Task ChangeHP(int amount, SlotView changer, bool triggerOnChange = true)
        {
            slotCard.cHP += amount;
            if (slotCard.cHP <= 0) CardDead(changer, amount);
            if (slotCard.cHP > 99) slotCard.cHP = 99;
            if (slotCard.cAbility != null && triggerOnChange) slotCard.cAbility.OnUpdateHP(this, amount, changer);
            tvDisplayStats[1].Text = Convert.ToString(slotCard.cHP);
            tvDisplayStats[1].StartAnimation(aTextBounce);
        }
        public async Task SetHP(int amount, SlotView changer)
        {
            slotCard.cHP = amount;
            if (slotCard.cHP <= 0) CardDead(changer, amount);
            if (slotCard.cHP > 99) slotCard.cHP = 99;
            if (slotCard.cAbility != null) slotCard.cAbility.OnUpdateHP(this, amount, changer);
            tvDisplayStats[1].Text = Convert.ToString(slotCard.cHP);
            tvDisplayStats[1].StartAnimation(aTextBounce);
        }

        public async Task ChangeSP(int amount, SlotView changer, bool triggerOnChange = true)
        {
            slotCard.cSP += amount;
            if (slotCard.cSP <= 0) slotCard.cSP = 0;
            if (slotCard.cSP > 99) slotCard.cHP = 99;
            if (slotCard.cAbility != null && triggerOnChange) slotCard.cAbility.OnUpdateSP(this, amount, changer);
            tvDisplayStats[2].Text = Convert.ToString(slotCard.cSP);
            tvDisplayStats[2].StartAnimation(aTextBounce);
        }
        public async Task SetSP(int amount, SlotView changer)
        {
            slotCard.cSP = amount;
            if (slotCard.cSP <= 0) slotCard.cSP = 0;
            if (slotCard.cSP > 99) slotCard.cHP = 99;
            if (slotCard.cAbility != null) slotCard.cAbility.OnUpdateSP(this, amount, changer);
            tvDisplayStats[2].Text = Convert.ToString(slotCard.cSP);
            tvDisplayStats[2].StartAnimation(aTextBounce);
        }


        public async Task SetAbility(Ability toSet)
        {
            slotCard.cAbility = toSet;
            StartAnimation(aTextBounce);
        }

        public static List<SlotView> CheckForTargeting(SlotView target)
        {
            List<SlotView> toSend = new List<SlotView>();
            for (int i = 0; i < 2; i++) for (int j = 0; j < 3; j++) if (svCardSlot[i, j] != null && svCardSlot[i, j].targetSlot != null && svCardSlot[i, j].targetSlot == target) toSend.Add(svCardSlot[i, j]);
            return toSend;
        }

        public override void StartAnimation(Animation animation)
        {
            if (animateIdleAnims) animation.SetAnimationListener(this);
            base.StartAnimation(animation);

        }

        public void OnAnimationEnd(Animation animation)
        {
            animsRunning = 0;
            if (slotCard != null) { if (animsRunning == 0 && animateIdleAnims) base.StartAnimation(slotCard.aIdleAnimation); }
            else base.StartAnimation(aSlotSpin);
        }

        public void OnAnimationRepeat(Animation animation)
        {
        }

        public void OnAnimationStart(Animation animation)
        {
            if (animateIdleAnims) animsRunning = 1;
        }
    }
}
