using Android.Views.Animations;
using System;
using System.Threading.Tasks;

namespace Rumblebash.TomBareket
{
    public abstract class Ability
    {
        public string desc;
        public bool canAttack;
        public string chargeTxt;
        public Ability(string desc, bool canAttack = true, string chargeTxt = null)
        {
            this.desc = desc;
            this.canAttack = canAttack;
            this.chargeTxt = chargeTxt;
        }
        public int[] GetPositionInArray(SlotView toCheck)
        {
            for (int i = 0; i < 2; i++) for (int j = 0; j < 3; j++) if (MainActivity.svCardSlot[i, j] == toCheck) return new int[2] { i, j };
            return null;
        }
        public static bool CheckForAbilStone(SlotView toCheck) => toCheck.slotCard.cAbility == null || !(toCheck.slotCard.cAbility is AbilStone); //returns true if the checked card does not have AbilStone

        public virtual async Task OnStrike(SlotView bearer, SlotView target = null, int dmg = 0) { }

        public virtual async Task OnHit(SlotView bearer, SlotView attacker = null, int dmg = 0) { }

        public virtual async Task OnKill(SlotView bearer, SlotView target = null, int dmg = 0) { }

        public virtual async Task OnDie(SlotView bearer, SlotView attacker = null, int dmg = 0) { }

        public virtual async Task OnStartOfTurn(SlotView bearer) { }

        public virtual async Task OnPlace(SlotView bearer) { }

        public virtual async Task OnDrawnFromDeck(Card bearer) { }

        public virtual void OnUpdateHP(SlotView bearer, int amount, SlotView changer) { }

        public virtual void OnUpdateAT(SlotView bearer, int amount, SlotView changer) { }

        public virtual void OnUpdateSP(SlotView bearer, int amount, SlotView changer) { }
    }

    public class Projectile
    {
        public int turnsLeft;
        public int dmg;
        public SlotView targetSlot, sender;
        public Ability hitEffect = new AbilNone();

        public Projectile(int turnCount, int dmg, SlotView sender, SlotView targetSlot, Ability hitEffect = null)
        {
            this.turnsLeft = turnCount;
            this.dmg = dmg;
            this.sender = sender;
            this.targetSlot = targetSlot;
            if (hitEffect != null) this.hitEffect = hitEffect;
        }

        public async Task hit()
        {
            targetSlot.CardHit(dmg, sender);
        }
    }




    //Individual classes for each ability
    public class AbilNone : Ability
    {
        public AbilNone() : base("") { }
    }


    public class AbilScreech : Ability
    {
        public AbilScreech() : base("50% of AT splash damage") { }
        public override async Task OnStrike(SlotView bearer, SlotView target = null, int dmg = 0)
        {
            int[] position = GetPositionInArray(bearer.targetSlot);

            try
            {
                int aoe;
                if (bearer.slotCard.cAT % 2 != 0) aoe = (dmg + 1) / 2; else aoe = (dmg) / 2;
                if (position[1] > 0 && MainActivity.svCardSlot[position[0], position[1] - 1].slotCard != null && CheckForAbilStone(MainActivity.svCardSlot[position[0], position[1] - 1])) MainActivity.svCardSlot[position[0], position[1] - 1].CardHit(aoe, bearer);
                if (position[1] < 2 && MainActivity.svCardSlot[position[0], position[1] + 1].slotCard != null && CheckForAbilStone(MainActivity.svCardSlot[position[0], position[1] + 1])) MainActivity.svCardSlot[position[0], position[1] + 1].CardHit(aoe, bearer);

                Console.WriteLine(bearer.slotCard.cName); //Ideally, The command should pike up an error if the bearer dies while the ability is active, forcing it to trigger targetSlot's CardHit
                Console.WriteLine(MainActivity.worksTxt + "Ability");
            } catch (Exception e)
            {
                MainActivity.svCardSlot[position[0], position[1]].CardHit(dmg ,bearer);
            }
        }
    }

    public class AbilRam : Ability
    {
        public AbilRam() : base("Attacks in front when placed") { }
        public override async Task OnPlace(SlotView bearer)
        {
            int[] position = GetPositionInArray(bearer);
            bearer.targetSlot = MainActivity.svCardSlot[1 - position[0], position[1]];
            if (bearer.targetSlot != null && CheckForAbilStone(bearer.targetSlot))
            {
                bearer.CardStrike();
                await Task.Delay(300);
                bearer.targetSlot = null;
            }
        }
    }

    public class AbilLeech : Ability
    {
        public AbilLeech() : base("+2 HP on kill") { }
        public override async Task OnKill(SlotView bearer, SlotView target = null, int dmg = 0)
        {
            Animation aPowerjump = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_AbilJump);
            bearer.StartAnimation(aPowerjump);
            bearer.slotCard.cHP += 2;
            bearer.tvDisplayStats[1].Text = Convert.ToString(bearer.slotCard.cHP);
        }
    }

    public class AbilEmpower : Ability
    {
        public AbilEmpower() : base("Empowers nearby allies for 1 AT") { }
        public override async Task OnPlace(SlotView bearer)
        {
            Animation aPowerjump = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_textBounce);
            bearer.StartAnimation(aPowerjump);
            int[] position = GetPositionInArray(bearer);
            if (position[1] > 0 && MainActivity.svCardSlot[position[0], position[1] - 1].slotCard != null && CheckForAbilStone(MainActivity.svCardSlot[position[0], position[1] - 1])) MainActivity.svCardSlot[position[0], position[1] - 1].ChangeAT(1, bearer);
            if (position[1] < 2 && MainActivity.svCardSlot[position[0], position[1] + 1].slotCard != null && CheckForAbilStone(MainActivity.svCardSlot[position[0], position[1] + 1])) MainActivity.svCardSlot[position[0], position[1] + 1].ChangeAT(1, bearer);
        }
    }

    public class AbilDouble : Ability
    {
        public AbilDouble() : base("Attacks strike again at the start of the next turn") { }
        public override async Task OnStrike(SlotView bearer, SlotView target = null, int dmg = 0)
        {
            if (bearer.targetSlot != null && CheckForAbilStone(bearer.targetSlot))
            {
                MainActivity.lProjectileBank.Add(new Projectile(1, bearer.slotCard.cAT, bearer, bearer.targetSlot));
            }
        }
    }

    public class AbilSteal : Ability
    {
        public AbilSteal() : base("Copies the ability in front when placed") { }
        public override async Task OnPlace(SlotView bearer)
        {
            
            int[] position = GetPositionInArray(bearer);

            if (MainActivity.svCardSlot[1 - position[0], position[1]] != null && !(MainActivity.svCardSlot[1 - position[0], position[1]].slotCard.cAbility is AbilSteal) && CheckForAbilStone(MainActivity.svCardSlot[1 - position[0], position[1]]))
            {
                Animation aLoadedAnimation;
                if (bearer.rotation == 0) aLoadedAnimation = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_StrikeRotation0);
                else aLoadedAnimation = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_StrikeRotation1);
                bearer.StartAnimation(aLoadedAnimation);
                bearer.slotCard.cAbility = (Ability)Activator.CreateInstance(MainActivity.svCardSlot[1 - position[0], position[1]].slotCard.cAbility.GetType());

                bearer.slotCard.cAbility.OnPlace(bearer);
            }
            else bearer.slotCard.cAbility = new AbilNone();
        }
    }

    public class AbilWither : Ability
    {
        public AbilWither() : base("Loses 1 HP at the start of a turn") { }
        public override async Task OnStartOfTurn(SlotView bearer)
        {
            if (bearer.slotCard.cHP > 1)
            {
                Animation aNotifyUpdate = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_MummyFade);
                bearer.StartAnimation(aNotifyUpdate);
            }
            bearer.ChangeHP(-1, bearer);
        }
    }

    public class AbilLonesome : Ability
    {
        private int addedAT = 0, oldAT = 0;

        public AbilLonesome() : base("Gets stronger for empty nearby ally slots") { }
        public override async Task OnStrike(SlotView bearer, SlotView target = null, int dmg = 0)
        {
            int[] position = GetPositionInArray(bearer);
            addedAT = 0;
            if (position[1] > 0 && MainActivity.svCardSlot[position[0], position[1] - 1].slotCard == null) addedAT++;
            if (position[1] < 2 && MainActivity.svCardSlot[position[0], position[1] + 1].slotCard == null) addedAT++;
            bearer.ChangeAT(addedAT - oldAT, bearer);
            oldAT = addedAT;
        }
    }

    public class AbilInflate : Ability
    {
        int charge = 0;
        public AbilInflate() : base("charges on stat change to release as AoE on death", chargeTxt: "Charge: 0") { }
        public override async Task OnDie(SlotView bearer, SlotView attacker = null, int dmg = 0)
        {
            for (int i = 0; i < 3; i++) if (MainActivity.svCardSlot[1 - bearer.rotation, i].slotCard != null && MainActivity.svCardSlot[1 - bearer.rotation, i].slotCard.cHP > 0 && CheckForAbilStone(MainActivity.svCardSlot[1 - bearer.rotation, i])) MainActivity.svCardSlot[1 - bearer.rotation, i].ChangeHP(-charge, bearer, false);
        }
        public override void OnUpdateAT(SlotView bearer, int amount, SlotView changer)
        {
            Animation aNotifyUpdate = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_AbilJump);
            bearer.StartAnimation(aNotifyUpdate);
            charge++;
            chargeTxt = "Charge: " + charge; 
        }
        public override void OnUpdateHP(SlotView bearer, int amount, SlotView changer)
        {
            Animation aNotifyUpdate = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_textBounce);
            bearer.StartAnimation(aNotifyUpdate);
            charge++;
            chargeTxt = "Charge: " + charge;
        }
        public override void OnUpdateSP(SlotView bearer, int amount, SlotView changer)
        {
            Animation aNotifyUpdate = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_textBounce);
            bearer.StartAnimation(aNotifyUpdate);
            charge++;
            chargeTxt = "Charge: " + charge;
        }
    }
    public class AbilPotion : Ability
    {
        public AbilPotion() : base("+1 AT to target when attacking") { }
        public override async Task OnStrike(SlotView bearer, SlotView target = null, int dmg = 0)
        {
            if(CheckForAbilStone(bearer.targetSlot))
            {
                bearer.targetSlot.ChangeAT(1, bearer);
                Animation aPowerjump = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_AbilJump);
                bearer.targetSlot.StartAnimation(aPowerjump);
            }
        }
    }
    public class AbilDrown : Ability
    {
        public AbilDrown() : base("Drags monster in front to death when dying") { }
        public override async Task OnDie(SlotView bearer, SlotView attacker = null, int dmg = 0)
        {
            int[] position = GetPositionInArray(bearer);

            Task.Delay(45);
            if (MainActivity.svCardSlot[1 - position[0], position[1]] != null && MainActivity.svCardSlot[1 - position[0], position[1]].slotCard.cHP > 0 && CheckForAbilStone(MainActivity.svCardSlot[1 - position[0], position[1]]))
            {
                MainActivity.svCardSlot[1 - position[0], position[1]].ChangeHP(-999, bearer, false);
                MainActivity.PlaySFX(Resource.Raw.HorseDieSFX, 130, 130);
            }
        }
    }
    public class AbilPoison : Ability
    {
        public AbilPoison() : base("Damages monster in front every round") { }
        public override async Task OnStartOfTurn(SlotView bearer)
        {
            Animation aPowerjump = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_textBounce);
            bearer.StartAnimation(aPowerjump);
            int[] position = GetPositionInArray(bearer);
            if (MainActivity.svCardSlot[1 - position[0], position[1]] != null && CheckForAbilStone(MainActivity.svCardSlot[1 - position[0], position[1]])) MainActivity.svCardSlot[1 - position[0], position[1]].ChangeHP(-bearer.slotCard.cAT, bearer);
        }
    }
    public class AbilDevour : Ability
    {
        public AbilDevour() : base("Attacks nearby allies at the start of a turn") { }
        public override async Task OnStartOfTurn(SlotView bearer)
        {
            int[] position = GetPositionInArray(bearer);
            if (position[1] > 0 && MainActivity.svCardSlot[position[0], position[1] - 1].slotCard != null && CheckForAbilStone(MainActivity.svCardSlot[position[0], position[1] - 1])) MainActivity.svCardSlot[position[0], position[1] - 1].CardHit(bearer.slotCard.cAT, bearer);
            if (position[1] < 2 && MainActivity.svCardSlot[position[0], position[1] + 1].slotCard != null && CheckForAbilStone(MainActivity.svCardSlot[position[0], position[1] + 1])) MainActivity.svCardSlot[position[0], position[1] + 1].CardHit(bearer.slotCard.cAT, bearer);
        }
    }
    public class AbilRage : Ability
    {
        public AbilRage() : base("+2 AT and +1 SP when hit") { }
        public override async Task OnHit(SlotView bearer, SlotView attacker = null, int dmg = 0)
        {
            bearer.ChangeAT(2, bearer);
            bearer.ChangeSP(1, bearer);
            Animation aNotifyUpdate = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_AbilJump);
            bearer.StartAnimation(aNotifyUpdate);

            bearer.slotCard.aIdleAnimation.Duration /= 2;
        }
    }
    public class AbilMirror : Ability
    {
        int tAT = 0;
        public AbilMirror() : base("+1 AT to monster in front and copies new AT on place") { }
        public override async Task OnPlace(SlotView bearer)
        {
            int[] position = GetPositionInArray(bearer);
            if (MainActivity.svCardSlot[1 - position[0], position[1]] != null && CheckForAbilStone(MainActivity.svCardSlot[1 - position[0], position[1]]))
            {
                MainActivity.svCardSlot[1 - position[0], position[1]].ChangeAT(1, bearer);
                tAT = MainActivity.svCardSlot[1 - position[0], position[1]].slotCard.cAT;
            }
            bearer.SetAT(tAT, bearer);

        }
    }
    public class AbilChain : Ability
    {
        public AbilChain() : base("Attacks hurt the monster in front of the target") { }
        public override async Task OnStrike(SlotView bearer, SlotView target = null, int dmg = 0)
        {
            int[] position = GetPositionInArray(bearer.targetSlot);
            int bearerAT = bearer.slotCard.cAT;

            if (MainActivity.svCardSlot[1 - position[0], position[1]].slotCard != null && CheckForAbilStone(MainActivity.svCardSlot[1 - position[0], position[1]]))
            {
                if (MainActivity.svCardSlot[1 - position[0], position[1]] == bearer) bearer.targetSlot.CardHit(bearer.slotCard.cAT, bearer);

                MainActivity.svCardSlot[1 - position[0], position[1]].ChangeHP(-bearerAT, bearer, false);
                if (MainActivity.svCardSlot[1 - position[0], position[1]].slotCard.cAbility != null) MainActivity.svCardSlot[1 - position[0], position[1]].slotCard.cAbility.OnHit(MainActivity.svCardSlot[1 - position[0], position[1]], bearer);
            }
        }
    }
    public class AbilRestore : Ability
    {
        public AbilRestore() : base("Heals nearby allies when attacking") { }
        public override async Task OnStrike(SlotView bearer, SlotView target = null, int dmg = 0)
        {
            int[] position = GetPositionInArray(bearer);
            if (position[1] > 0 && MainActivity.svCardSlot[position[0], position[1] - 1].slotCard != null && CheckForAbilStone(MainActivity.svCardSlot[position[0], position[1] - 1])) MainActivity.svCardSlot[position[0], position[1] - 1].ChangeHP(bearer.slotCard.cAT, bearer);
            if (position[1] < 2 && MainActivity.svCardSlot[position[0], position[1] + 1].slotCard != null && CheckForAbilStone(MainActivity.svCardSlot[position[0], position[1] + 1])) MainActivity.svCardSlot[position[0], position[1] + 1].ChangeHP(bearer.slotCard.cAT, bearer);
        }
    }
    public class AbilInfest : Ability
    {
        int storeHP = 2, storeAT = 0, storeSP = 5;
        public AbilInfest() : base("Swaps stats with target on start of turn. Can't attack", canAttack: false) { }
        public override async Task OnStartOfTurn(SlotView bearer)
        {
            if(bearer.targetSlot != null && CheckForAbilStone(bearer.targetSlot))
            {
                Animation aLoadedAnimation;
                if (bearer.rotation == 0) aLoadedAnimation = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_StrikeRotation0);
                else aLoadedAnimation = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_StrikeRotation1);

                storeHP = bearer.slotCard.cHP;
                storeAT = bearer.slotCard.cAT;
                storeSP = bearer.slotCard.cSP;

                bearer.SetHP(bearer.targetSlot.slotCard.cHP, bearer);
                bearer.SetAT(bearer.targetSlot.slotCard.cAT, bearer);
                bearer.SetSP(bearer.targetSlot.slotCard.cSP, bearer);

                bearer.targetSlot.SetHP(storeHP, bearer);
                bearer.targetSlot.SetAT(storeAT, bearer);
                bearer.targetSlot.SetSP(storeSP, bearer);
            }
        }
    }
    public class AbilMeteor : Ability
    {
        const int REQUIRED_KILLS = 3;
        int charge = 0;
        public AbilMeteor() : base("Board wipes after " + REQUIRED_KILLS + " kills", chargeTxt: "Kills left: " + REQUIRED_KILLS) { }
        public override async Task OnKill(SlotView bearer, SlotView target = null, int dmg = 0)
        {
            charge++;
            if (charge >= REQUIRED_KILLS)
            {
                for (int i = 0; i < 2; i++) for (int j = 0; j < 3; j++) if (MainActivity.svCardSlot[i, j].slotCard != null && MainActivity.svCardSlot[i, j] != bearer && CheckForAbilStone(MainActivity.svCardSlot[i, j])) MainActivity.svCardSlot[i, j].ChangeHP(-999, bearer, false);
                MainActivity.PlaySFX(Resource.Raw.WarlockMeteorSFX, 115, 115);
                charge = 0;
            }
            chargeTxt = "Kills left: " + (REQUIRED_KILLS - charge);
        }
    }
    public class AbilEater : Ability
    {
        public AbilEater() : base("Only attacks once every other turn") { }
        public override async Task OnStartOfTurn(SlotView bearer)
        {
            if (canAttack)
            {
                canAttack = false;
                chargeTxt = "Resting";
                bearer.slotCard.aIdleAnimation.Duration *= 10;
            }
            else
            {
                bearer.slotCard.aIdleAnimation.Duration /= 10;
                canAttack = true;
                chargeTxt = null;
            }
        }
    }
    public class AbilZombify: Ability
    {
        public AbilZombify() : base("Turns attacked targets into Zombies") { }
        public override async Task OnStrike(SlotView bearer, SlotView target  = null, int dmg = 0)
        {
            if (bearer.targetSlot.slotCard != null && bearer.targetSlot.slotCard.cHP > bearer.slotCard.cAT && CheckForAbilStone(bearer.targetSlot)) bearer.targetSlot.PlaceCard(new Card("Zombie", 3, bearer.slotCard.cAT, bearer.slotCard.cSP, "ZombiePortrait", "ZombieMonster", "AbilZombify", "anim_IdleAnimationBreath", 3200));
        }
    }
    public class AbilGnome : Ability
    {
        SlotView prevTarget = null;
        bool alreadyAttacked = false;
        public AbilGnome() : base("Attacks both currently and previously targeted slots when striking") { }

        public override async Task OnStrike(SlotView bearer, SlotView target = null, int dmg = 0) 
        {
            if (!alreadyAttacked)
            {
                SlotView curTarget = bearer.targetSlot;

                try
                {
                    await Task.Delay(15);
                    bearer.targetSlot = prevTarget;
                    alreadyAttacked = true;
                    bearer.CardStrike();
                    await Task.Delay(300);
                    if (curTarget.slotCard != null)   bearer.targetSlot = curTarget; else bearer.targetSlot = null;
                }
                catch (Exception e)
                {
                    Console.WriteLine(MainActivity.errTxt);
                    Console.WriteLine(e.ToString());
                }

                prevTarget = curTarget;
                chargeTxt = "Targeting slot No. " + (GetPositionInArray(prevTarget)[1] + 1);
                alreadyAttacked = false;
            }
        }

        public override async Task OnDie(SlotView bearer, SlotView attacker = null, int dmg = 0)
        {
            if (bearer.Animation != null) bearer.ClearAnimation();
        }
    }
    public class AbilStone: Ability
    {
        public AbilStone() : base("Immune to other abilities") { }
    }
    public class AbilInvert : Ability
    {
        public AbilInvert() : base("Swaps attackers' HP and AT around when hit") { }
        public override async Task OnHit(SlotView bearer, SlotView attacker = null, int dmg = 0)
        {
            if (attacker.slotCard.cAT > 0)
            {
                if (attacker != null && CheckForAbilStone(attacker))
                {
                    Animation aSlotSpin = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_invertigoBounce);
                    attacker.StartAnimation(aSlotSpin);
                    int storeHP = attacker.slotCard.cHP;
                    attacker.SetHP(attacker.slotCard.cAT, bearer);
                    attacker.SetAT(storeHP, bearer);
                }
            }
            else attacker.CardDead(attacker, 0);
        }
    }
    public class AbilSummon : Ability
    {
        public AbilSummon() : base("Grants extra cards every turn per AT + 1. Can't attack", canAttack: false) { }
        public override async Task OnStartOfTurn(SlotView bearer)
        {
            MainActivity.LoadCards(1+ bearer.slotCard.cAT);
        }
    }
    public class AbilUnleash : Ability
    {
        public AbilUnleash() : base("Attacks monsters granting it a stat boost") { }
        public override void OnUpdateAT(SlotView bearer, int amount, SlotView changer)
        {
            Task.Delay(200);
            Animation aLoadedAnimation;
            if (bearer.rotation == 0) aLoadedAnimation = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_StrikeRotation0);
            else aLoadedAnimation = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_StrikeRotation1);
            bearer.StartAnimation(aLoadedAnimation);
            Task.Delay(185);
            changer.CardHit(bearer.slotCard.cAT, bearer);
        }
        public override void OnUpdateHP(SlotView bearer, int amount, SlotView changer)
        {
            Task.Delay(200);
            Animation aLoadedAnimation;
            if (bearer.rotation == 0) aLoadedAnimation = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_StrikeRotation0);
            else aLoadedAnimation = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_StrikeRotation1);
            bearer.StartAnimation(aLoadedAnimation);
            Task.Delay(185);
            changer.CardHit(bearer.slotCard.cAT, bearer);
        }
        public override void OnUpdateSP(SlotView bearer, int amount, SlotView changer)
        {
            Task.Delay(200);
            Animation aLoadedAnimation;
            if (bearer.rotation == 0) aLoadedAnimation = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_StrikeRotation0);
            else aLoadedAnimation = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_StrikeRotation1);
            bearer.StartAnimation(aLoadedAnimation);
            Task.Delay(185);
            changer.CardHit(bearer.slotCard.cAT, bearer);
        }
    }
    public class AbilCauldron : Ability
    {
        const int REQUIRED_TURNS = 2;
        int charge = REQUIRED_TURNS;
        public AbilCauldron() : base("Drops a 10 DMG cauldron on slot in front of it after " + REQUIRED_TURNS + " turns", chargeTxt: "Turns Left: " + REQUIRED_TURNS) { }

        public override async Task OnPlace(SlotView bearer)
        {
            int[] position = GetPositionInArray(bearer);
            SlotView cauldronTarget = MainActivity.svCardSlot[1 - position[0], position[1]];

            if (CheckForAbilStone(cauldronTarget)) MainActivity.lProjectileBank.Add(new Projectile(REQUIRED_TURNS, 10, bearer, cauldronTarget));
            else charge = -1;
        }

        public override async Task OnStartOfTurn(SlotView bearer)
        {
            if(charge > 0)
            {
                charge--;
                if (charge == 0) chargeTxt = null;
                else chargeTxt = "Turns left: " + charge;
            }
        }
    }
    public class AbilFish : Ability
    {
        const int MAX_FISH = 6;
        int charge = 6;
        public AbilFish() : base("Respawns in deck after death with more stats. Up to " + MAX_FISH + " times") { }
        public override async Task OnDrawnFromDeck(Card bearer)
        {
            this.charge = MAX_FISH + 1 - bearer.cHP;
            if (bearer.cName == "Fin-Inito") chargeTxt = "Fish Left: " + charge;
            else chargeTxt = bearer.cName + " Left: " + charge;
        }

        public override async Task OnDie(SlotView bearer, SlotView attacker = null, int dmg = 0)
        {
            if (charge > 1)
            {
                int newStat = Math.Abs(MAX_FISH + 2 - charge);
                Card[] newCards = { new Card(bearer.slotCard.cPortrait, bearer.slotCard.cMonster,bearer.slotCard.cName, newStat, newStat, newStat, "AbilFish", bearer.slotCard.aIdleAnimation) };
                MainActivity.LoadCards(content: newCards);
            }
        }
    }

    public class AbilSnowstorm : Ability
    {
        public AbilSnowstorm() : base("-1 SP to all enemies when placed") { }

        public override async Task OnPlace(SlotView bearer)
        {
            int[] position = GetPositionInArray(bearer);

            for (int i = 0; i < 3; i++) if (MainActivity.svCardSlot[1 - position[0], i].slotCard != null) MainActivity.svCardSlot[1 - position[0], i].ChangeSP(-1, bearer);
        }
    }

    public class AbilTrickroom : Ability
    {
        public AbilTrickroom() : base("On start of turn, Each monster swaps SP with it's opposing monster") { }
        public override async Task OnStartOfTurn(SlotView bearer)
        {
            int[] position = GetPositionInArray(bearer);

            for (int i = 0; i < 3; i++) if (MainActivity.svCardSlot[1 - position[0], i].slotCard != null && MainActivity.svCardSlot[position[0], i].slotCard != null)
                {
                    int stored = MainActivity.svCardSlot[position[0], i].slotCard.cSP;
                    MainActivity.svCardSlot[position[0], i].SetSP(MainActivity.svCardSlot[1 - position[0], i].slotCard.cSP, bearer);
                    MainActivity.svCardSlot[1 - position[0], i].SetSP(stored, bearer);
                }
        }
    }

    public class AbilExplode : Ability 
    {
        private bool exploded = false;
        public AbilExplode() : base("Deals 50% of taken DMG to adjacent allies on hit/death") { }

        public override async Task OnHit(SlotView bearer, SlotView attacker = null, int dmg = 0)
        {
            int[] position = GetPositionInArray(bearer);

            if (!exploded)
            {
                exploded = true;

                Animation aNotifyExplosion = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_LandmineSquash);
                aNotifyExplosion.Interpolator = new OvershootInterpolator();
                bearer.StartAnimation(aNotifyExplosion);

                int aoe;
                if (bearer.slotCard.cAT % 2 != 0) aoe = (dmg + 1) / 2; else aoe = (dmg) / 2;
                if (position[1] > 0 && MainActivity.svCardSlot[position[0], position[1] - 1].slotCard != null && CheckForAbilStone(MainActivity.svCardSlot[position[0], position[1] - 1])) MainActivity.svCardSlot[position[0], position[1] - 1].CardHit(aoe, attacker);
                if (position[1] < 2 && MainActivity.svCardSlot[position[0], position[1] + 1].slotCard != null && CheckForAbilStone(MainActivity.svCardSlot[position[0], position[1] + 1])) MainActivity.svCardSlot[position[0], position[1] + 1].CardHit(aoe, attacker);
            }

            exploded = false;
        }

        public override async Task OnDie(SlotView bearer, SlotView attacker = null, int dmg = 0)
        {
            int[] position = GetPositionInArray(bearer);

            if (!exploded)
            {
                exploded = true;

                Animation aNotifyExplosion = AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_LandmineSquash);
                aNotifyExplosion.Interpolator = new OvershootInterpolator();
                bearer.StartAnimation(aNotifyExplosion);

                int aoe;
                if (bearer.slotCard.cAT % 2 != 0) aoe = (dmg + 1) / 2; else aoe = (dmg) / 2;
                if (position[1] > 0 && MainActivity.svCardSlot[position[0], position[1] - 1].slotCard != null && CheckForAbilStone(MainActivity.svCardSlot[position[0], position[1] - 1])) MainActivity.svCardSlot[position[0], position[1] - 1].CardHit(aoe, attacker);
                if (position[1] < 2 && MainActivity.svCardSlot[position[0], position[1] + 1].slotCard != null && CheckForAbilStone(MainActivity.svCardSlot[position[0], position[1] + 1])) MainActivity.svCardSlot[position[0], position[1] + 1].CardHit(aoe, attacker);
            }

            exploded = false;
        }
    }

    public class AbilReflection : Ability
    {
        public AbilReflection() : base("Reflects dealt damage to target when hit or dying") { }

        public override async Task OnHit(SlotView bearer, SlotView attacker = null, int dmg = 0)
        {
            if (bearer.targetSlot != null && CheckForAbilStone(bearer.targetSlot))
            {
                bearer.StartAnimation(AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_AbilJump));
                bearer.targetSlot.CardHit(dmg, bearer);
            }
        }

        public override async Task OnDie(SlotView bearer, SlotView attacker = null, int dmg = 0)
        {
            if (bearer.targetSlot != null && CheckForAbilStone(bearer.targetSlot))
            {
                bearer.StartAnimation(AnimationUtils.LoadAnimation(MainActivity.main, Resource.Animation.anim_AbilJump));
                bearer.targetSlot.CardHit(dmg, bearer);
            }
        }
    }
}