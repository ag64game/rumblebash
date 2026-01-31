using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using Android.Content.Res;
using Android.Graphics;
using System.IO;
using Android.Graphics.Drawables;
using Android.Views.Animations;
using Android.Util;
using Android.Support.V7.Widget;
using System.Threading.Tasks;

namespace Rumblebash.TomBareket
{
    //Card implementation, the class containing the data of each card
    public class Card
    {
        public int cID = 0;
        public string cName = "Error";
        public int cHP = 1;
        public int cAT = 0;
        public int cSP = 0;
        public Bitmap cPortrait = BitmapFactory.DecodeResource(Application.Context.Resources, Resource.Drawable.defaultportrait);
        public Bitmap cMonster = BitmapFactory.DecodeResource(Application.Context.Resources, Resource.Drawable.defaultportrait);
        public Ability cAbility = new AbilNone();
        public Animation aIdleAnimation = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_IdleAnimationNone);

        private static int cardCount = 0;
        public static int CardCount
        {
            get 
            {
                return cardCount;
            }
        }

        public Card(int requestedId)
        {
            try
            {
                cID = requestedId;
                ///Reads data from a CSV file containing the cards' data
                AssetManager assets = Application.Context.Assets;
                System.IO.Stream s = assets.Open("CardInfo.csv");
                using (var r = new StreamReader(s))
                {
                    r.ReadLine();
                    for (int i = 0; i < requestedId; i++) r.ReadLine();
                    if (!r.EndOfStream)
                    {
                        Resources res = Resources.System;

                        string[] strSplitLine = r.ReadLine().Split(',');
                        cName = strSplitLine[0];

                        cHP = Convert.ToInt32(strSplitLine[1]);
                        cAT = Convert.ToInt32(strSplitLine[2]);
                        cSP = Convert.ToInt32(strSplitLine[3]);

                        Console.WriteLine(MainActivity.worksTxt);
                        Console.WriteLine(strSplitLine[4]);
                        Type abilType = Type.GetType("Rumblebash.TomBareket." + strSplitLine[4].Trim());
                        cAbility = (Ability)Activator.CreateInstance(abilType);

                        int idPortrait = (int)typeof(Resource.Drawable).GetField(strSplitLine[5].Trim()).GetValue(null);
                        cPortrait = BitmapFactory.DecodeResource(Application.Context.Resources, idPortrait);

                        int idMonster = (int)typeof(Resource.Drawable).GetField(strSplitLine[6].Trim()).GetValue(null);
                        cMonster = BitmapFactory.DecodeResource(Application.Context.Resources, idMonster);

                        int idAnimation = (int)typeof(Resource.Animation).GetField(strSplitLine[7].Trim()).GetValue(null);
                        aIdleAnimation = AnimationUtils.LoadAnimation(MainActivity.main, idAnimation);
                        aIdleAnimation.Duration = Convert.ToInt32(strSplitLine[8]);
                        if (cName == "Fin-Inito") aIdleAnimation.Interpolator = new LinearInterpolator();
                        else aIdleAnimation.RepeatMode = RepeatMode.Reverse;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(MainActivity.errTxt);
                Console.WriteLine(e);
            }
        }
        public Card(string requestedName = "Error", int requestedHP = 1, int requestedAT = 0, int requestedSP = 0, string requestedPortrait = "defaultportrait", string requestedMonster = "defaultportrait", string requestedAbil = "AbilNone", string requestedAnimation = "anim_IdleAnimationNone", int requestedDuration = 6500)
        {
            try
            {
                cName = requestedName;

                cHP = requestedHP;
                cAT = requestedAT;
                cSP = requestedSP;

                Type abilType = Type.GetType("Rumblebash.TomBareket." + requestedAbil.Trim());
                cAbility = (Ability)Activator.CreateInstance(abilType);

                int idPortrait = (int)typeof(Resource.Drawable).GetField(requestedPortrait.Trim()).GetValue(null);
                cPortrait = BitmapFactory.DecodeResource(Application.Context.Resources, idPortrait);

                int idMonster = (int)typeof(Resource.Drawable).GetField(requestedMonster.Trim()).GetValue(null);
                cMonster = BitmapFactory.DecodeResource(Application.Context.Resources, idMonster);

                int idAnimation = (int)typeof(Resource.Animation).GetField(requestedAnimation.Trim()).GetValue(null);
                aIdleAnimation = AnimationUtils.LoadAnimation(MainActivity.main, idAnimation);
                aIdleAnimation.Duration = Convert.ToInt32(requestedDuration);
                if (cName == "Fin-Inito") aIdleAnimation.Interpolator = new LinearInterpolator();
                else aIdleAnimation.RepeatMode = RepeatMode.Reverse;
            }
            catch (Exception e)
            {
                Console.WriteLine(MainActivity.errTxt);
                Console.WriteLine(e.Message);
            }
        }
        public Card(Bitmap requestedPortrait, Bitmap requestedMonster, string requestedName = "Error", int requestedHP = 1, int requestedAT = 0, int requestedSP = 0, string requestedAbil = "AbilNone", string requestedAnimation = "anim_IdleAnimationNone", int requestedDuration = 6500)
        {
            try
            {
                cName = requestedName;

                cHP = requestedHP;
                cAT = requestedAT;
                cSP = requestedSP;

                Type abilType = Type.GetType("Rumblebash.TomBareket." + requestedAbil.Trim());
                cAbility = (Ability)Activator.CreateInstance(abilType);

                cPortrait = requestedPortrait;
                cMonster = requestedMonster;

                int idAnimation = (int)typeof(Resource.Animation).GetField(requestedAnimation.Trim()).GetValue(null);
                aIdleAnimation = AnimationUtils.LoadAnimation(MainActivity.main, idAnimation);
                aIdleAnimation.Duration = Convert.ToInt32(requestedDuration);
                if (cName == "Fin-Inito") aIdleAnimation.Interpolator = new LinearInterpolator();
                else aIdleAnimation.RepeatMode = RepeatMode.Reverse;
            }
            catch (Exception e)
            {
                Console.WriteLine(MainActivity.errTxt);
                Console.WriteLine(e.Message);
            }
        }

        public Card(Bitmap requestedPortrait, Bitmap requestedMonster, string requestedName = "Error", int requestedHP = 1, int requestedAT = 0, int requestedSP = 0, string requestedAbil = "AbilNone", Animation requestedAnimation = null)
        {
            try
            {
                cName = requestedName;

                cHP = requestedHP;
                cAT = requestedAT;
                cSP = requestedSP;

                Type abilType = Type.GetType("Rumblebash.TomBareket." + requestedAbil.Trim());
                cAbility = (Ability)Activator.CreateInstance(abilType);

                cPortrait = requestedPortrait;
                cMonster = requestedMonster;

                aIdleAnimation = requestedAnimation;
            }
            catch (Exception e)
            {
                Console.WriteLine(MainActivity.errTxt);
                Console.WriteLine(e.Message);
            }
        }

        public static void SetCardCount()
        {
            int counter = 0;

            AssetManager assets = Application.Context.Assets;
            System.IO.Stream s = assets.Open("CardInfo.csv");
            using (var r = new StreamReader(s))
            {
                while (!r.EndOfStream)
                {
                    r.ReadLine();
                    counter++;
                }

                cardCount = counter - 1;
            }
        }
    }

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
            aSlotSpin = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_RotateSlot);
            aSlotSpin.Interpolator = new Android.Views.Animations.LinearInterpolator();
            StartAnimation(aSlotSpin);           
        }
        public SlotView(Android.Content.Context context, IAttributeSet attrs) : base(context, attrs)
        {
            this.SetOnDragListener(this);
            this.Click += SlotSelected;
            aSlotSpin = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_RotateSlot);
            aSlotSpin.Interpolator = new Android.Views.Animations.LinearInterpolator();
            StartAnimation(aSlotSpin);
        }
        public SlotView(Android.Content.Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            this.SetOnDragListener(this);
            this.Click += SlotSelected;
            aSlotSpin = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_RotateSlot);
            aSlotSpin.Interpolator = new Android.Views.Animations.LinearInterpolator();
            StartAnimation(aSlotSpin);
        }
        protected SlotView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            this.SetOnDragListener(this);
            this.Click += SlotSelected;
            aSlotSpin = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_RotateSlot);
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
            if (rotation == 0) MainActivity.PlaySFX(Resource.Raw.CardPlacedSFX, 150, 100); else MainActivity.PlaySFX(Resource.Raw.CardPlacedSFX, 100, 150);

            slotCard = toPlace;
            this.SetBackgroundDrawable(new BitmapDrawable(MainActivity.main.Resources, toPlace.cMonster));
            if (rotation == 1) this.ScaleX = -1;

            Animation aPlaceAnim = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_CardPlaced);
            StartAnimation(aPlaceAnim);
            
            MainActivity.cardPile.Remove(CardViewHolder.cInStorage);
            MainActivity.rvDeck.GetAdapter().NotifyDataSetChanged();
            tvDisplayStats[0].Text = Convert.ToString(slotCard.cAT);
            tvDisplayStats[0].Visibility = ViewStates.Visible;
            tvDisplayStats[1].Text = Convert.ToString(slotCard.cHP);
            tvDisplayStats[1].Visibility = ViewStates.Visible;
            tvDisplayStats[2].Text = Convert.ToString(slotCard.cSP);
            tvDisplayStats[2].Visibility = ViewStates.Visible;
            aTextBounce = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_textBounce);

            MainActivity.rvDeck.Visibility = ViewStates.Visible;

            if (slotCard.cAbility != null) slotCard.cAbility.OnPlace(this);
        }


        //Handles Selecting the slot to choose a target. On turn 1 of a card being placed you can also fetch the card back to
        private void SlotSelected(object sender, EventArgs e)
        {
            if (slotCard == null || MainActivity.isInCombat || MainActivity.isInSettings) return;
            MainActivity.rlParentLayout = MainActivity.main.FindViewById<RelativeLayout>(Resource.Id.rlGameMenu);
            if (isInflated)
            {
                MainActivity.rlParentLayout.RemoveView(crosshair);
                MainActivity.rlParentLayout.RemoveView(infoTab);
                if (lastPressed == this)
                {
                    isInflated = false;
                    return;
                }
            }
            lInflater = LayoutInflater.FromContext(MainActivity.main);
            crosshair = lInflater.Inflate(Resource.Layout.crosshair_layout, null);
            infoTab = lInflater.Inflate(Resource.Layout.info_layout, null);
            MainActivity.rlParentLayout.AddView(crosshair);
            MainActivity.rlParentLayout.AddView(infoTab);
            MainActivity.main.FindViewById<TextView>(Resource.Id.tvStatsDisplayName).Text = slotCard.cName;
            MainActivity.main.FindViewById<TextView>(Resource.Id.tvStatsDisplayName).Typeface = MainActivity.nameFont;
            MainActivity.main.FindViewById<TextView>(Resource.Id.tvStatsDisplayAbility).Text = slotCard.cAbility.desc;
            MainActivity.main.FindViewById<TextView>(Resource.Id.tvStatsDisplayAbility).Typeface = MainActivity.abilityFont;
            if (slotCard.cAbility.chargeTxt == null) MainActivity.main.FindViewById<TextView>(Resource.Id.tvStatsDisplayCharge).Visibility = ViewStates.Gone;
            else
            {
                MainActivity.main.FindViewById<TextView>(Resource.Id.tvStatsDisplayCharge).Visibility = ViewStates.Visible;
                MainActivity.main.FindViewById<TextView>(Resource.Id.tvStatsDisplayCharge).Text = slotCard.cAbility.chargeTxt;
                MainActivity.main.FindViewById<TextView>(Resource.Id.tvStatsDisplayCharge).Typeface = MainActivity.abilityFont;
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
            crosshair.SetY(coords[1] + (sender as View).Height / 9);
            int[] placerCoords = new int[2];
            (sender as View).GetLocationOnScreen(placerCoords);
            if(rotation == 0) infoTab.SetX(placerCoords[0]); else infoTab.SetX(placerCoords[0] - (sender as View).Width);
            infoTab.SetY(placerCoords[1]-(sender as View).Height / 4);
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
                    crosshair.SetY(e.Event.RawY - (sender as View).Height);
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
            for (int i = 0; i < 3; i++) if (touchX >= MainActivity.svCardSlot[checkRow, i].Left && touchX <= MainActivity.svCardSlot[checkRow, i].Right)
                    if (touchY <= MainActivity.svCardSlot[checkRow, i].Bottom && touchY >= MainActivity.svCardSlot[checkRow, i].Top)
                    {
                        if (MainActivity.svCardSlot[checkRow, i].slotCard != null) return MainActivity.svCardSlot[checkRow, i];
                    }
            return null;
        }


        //Handles the combat phase, attacking, getting hit and dying
        public async Task CardStrike()
        {
            if (targetSlot == null) return;

            if (rotation == 0) aAttackAnimation = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_StrikeRotation0);
            else aAttackAnimation = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_StrikeRotation1);
            StartAnimation(aAttackAnimation);
            await Task.Delay(185);

            try
            {
                slotCard.cAbility.OnStrike(this, targetSlot, slotCard.cAT);
                targetSlot.CardHit(slotCard.cAT, this);

            } catch (Exception e)
            {
                Console.WriteLine(MainActivity.errTxt);
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
                    await MainActivity.PlaySFX(Resource.Raw.MonsterDeadSFX, 130, 130);

                    if (attacker.slotCard != null) await attacker.slotCard.cAbility.OnKill(attacker, this, dmg);
                    CardDead(attacker, dmg);
                }
                else await MainActivity.PlaySFX(Resource.Raw.MonsterHitSFX, 130, 130);

                await slotCard.cAbility.OnHit(this, attacker, dmg);
                tvDisplayStats[1].Text = Convert.ToString(slotCard.cHP);
                tvDisplayStats[1].StartAnimation(aTextBounce);
            } catch (Exception e)
            {
                Console.WriteLine(MainActivity.errTxt);
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

                SetBackgroundDrawable(MainActivity.main.Resources.GetDrawable(Resource.Drawable.SlotPortal));
                StartAnimation(aSlotSpin);
                for (int i = 0; i < 3; i++) tvDisplayStats[i].Visibility = ViewStates.Invisible;

                MainActivity.curKills++;
                MainActivity.SetKills(MainActivity.kills + 1);

            } catch (Exception e)
            {
                Console.WriteLine(MainActivity.errTxt);
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
            for (int i = 0; i < 2; i++) for (int j = 0; j < 3; j++) if (MainActivity.svCardSlot[i, j] != null && MainActivity.svCardSlot[i,j].targetSlot != null && MainActivity.svCardSlot[i, j].targetSlot == target) toSend.Add(MainActivity.svCardSlot[i, j]);
            return toSend;
        }

        public override void StartAnimation(Animation animation)
        {
            if (MainActivity.animateIdleAnims) animation.SetAnimationListener(this);
            base.StartAnimation(animation);

        }

        public void OnAnimationEnd(Animation animation)
        {
            animsRunning = 0;
            if (slotCard != null) { if (animsRunning == 0 && MainActivity.animateIdleAnims) base.StartAnimation(slotCard.aIdleAnimation); }
            else base.StartAnimation(aSlotSpin);
        }

        public void OnAnimationRepeat(Animation animation)
        {
        }

        public void OnAnimationStart(Animation animation)
        {
            if(MainActivity.animateIdleAnims) animsRunning = 1;
        }
    }


    //RecyclerView implementation, the view that houses your deck of cards
    public class CardViewHolder : RecyclerView.ViewHolder
    {
        public static Card cInStorage;
        public static View vHeldCard;
        public static int iAdapterPosition;
        public TextView tvName { get; }
        public TextView tvHP { get; }
        public TextView tvAT { get; }
        public TextView tvSP { get; }
        public TextView tvDesc { get; }
        public ImageView ivPort { get; }
        public int iPosition { get; set; }
        public CardViewHolder(View itemView) : base(itemView)
        {
            //Animation aCardPlaced = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_CardPlaced);
            //itemView.StartAnimation(aCardPlaced);
            tvName = itemView.FindViewById<TextView>(Resource.Id.tvCardName);
            tvHP = itemView.FindViewById<TextView>(Resource.Id.tvCardHP);
            tvAT = itemView.FindViewById<TextView>(Resource.Id.tvCardAT);
            tvSP = itemView.FindViewById<TextView>(Resource.Id.tvCardSP);
            tvDesc = itemView.FindViewById<TextView>(Resource.Id.tvCardAbility);
            ivPort = itemView.FindViewById<ImageView>(Resource.Id.ivCardPortrait);
            tvName.Typeface = MainActivity.nameFont;
            tvAT.Typeface = MainActivity.statsFont;
            tvSP.Typeface = MainActivity.statsFont;
            tvHP.Typeface = MainActivity.statsFont;
            tvDesc.Typeface = MainActivity.abilityFont;
            itemView.LongClick += CardSelected;
        }

        //Functions handling the movement and placing of cards
        private void CardSelected(object sender, View.LongClickEventArgs e)
        {
            if (MainActivity.isInCombat || MainActivity.isInSettings) return;
            MainActivity.PlaySFX(Resource.Raw.CardPickSFX);

            vHeldCard = (View)sender;
            if (SlotView.crosshair != null) MainActivity.rlParentLayout.RemoveView(SlotView.crosshair);
            if (SlotView.infoTab != null) MainActivity.rlParentLayout.RemoveView(SlotView.infoTab);
            iAdapterPosition = MainActivity.rvDeck.GetChildAdapterPosition(MainActivity.rvDeck.FindChildViewUnder((sender as View).GetX(), (sender as View).GetY()));
            cInStorage = MainActivity.cardPile[iAdapterPosition];
            var item = new ClipData.Item((string)vHeldCard.Tag);
            string[] mimeType = new string[1] { ClipDescription.MimetypeTextPlain };
            var data = new ClipData("Aga", mimeType, item);
            var cardShadow = new View.DragShadowBuilder(vHeldCard);
            vHeldCard.Drag += DragCard;
            vHeldCard.StartDragAndDrop(data, cardShadow, vHeldCard, 0);

        }
        private void DragCard(object sender, View.DragEventArgs e)
        {
            switch (e.Event.Action)
            {
                case DragAction.Started:
                    MainActivity.rvDeck.Visibility = ViewStates.Invisible;
                    break;
                case DragAction.Entered:
                    (sender as View).Invalidate();
                    break;
                case DragAction.Exited:
                    (sender as View).Invalidate();
                    break;
                case DragAction.Drop:
                    MainActivity.rvDeck.Visibility = ViewStates.Visible;
                    break;
                case DragAction.Location:
                    break;
                case DragAction.Ended:
                    MainActivity.rvDeck.Visibility = ViewStates.Visible;
                    break;
            }
        }
    }
    public class CardAdapter : RecyclerView.Adapter
    {
        List<Card> data;
        public CardAdapter(List<Card> data)
        {
            this.data = data;
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.card_layout, parent, false);
            return new CardViewHolder(itemView);
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is CardViewHolder cardViewHolder)
            {
                Card item = data[position];
                cardViewHolder.tvName.Text = item.cName;
                cardViewHolder.tvHP.Text = Convert.ToString(item.cHP);
                cardViewHolder.tvAT.Text = Convert.ToString(item.cAT);
                cardViewHolder.tvSP.Text = Convert.ToString(item.cSP);
                if (item.cAbility != null) cardViewHolder.tvDesc.Text = item.cAbility.desc; else cardViewHolder.tvDesc.Text = "";
                cardViewHolder.ivPort.SetImageBitmap(item.cPortrait);
            }
        }
        public override int ItemCount => data.Count;

    }
}