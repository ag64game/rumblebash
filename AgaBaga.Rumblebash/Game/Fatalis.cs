using System;
using System.Collections.Generic;
using System.Text;

namespace AgaBaga.Rumblebash.Game
{
    public static class Fatalis
    {
        //Makes The Devil comment on the amount of kills you've gotten in a round
        public static async Task DevilCurKillComment()
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
            }
            catch (Exception e)
            {
                Console.WriteLine(errTxt);
                Console.WriteLine(e);
            }
            curKills = 0;
        }
        public static async Task<TimeSpan> DevilWarCry()
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
            throw new IndexOutOfRangeException("Faemin Fatalis' stallCounter is behaving unexpectadly. stallCounter = " + stallCounter);
        }
        public static async Task StarkillScreen(string displayText)
        {
            try
            {
                if (rlParentLayout != null)
                {
                    await Task.Delay(70);
                    await PlaySFX(Resource.Raw.StarkillSFX);
                    await Task.Delay(40);

                    LayoutInflater lInflater = LayoutInflater.From(main);
                    View starView = lInflater.Inflate(Resource.Layout.starkill_layout, null);

                    Animation animEffect = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_StarkillSpin);
                    Animation animMegamanEffect = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_FadeOut);
                    animMegamanEffect.Duration = animEffect.Duration / 2;
                    animMegamanEffect.AnimationEnd += (sender, e) =>
                    {
                        rlParentLayout.RemoveView(starView);
                    };

                    Animation animStart = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_FadeIn);
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
                    main.FindViewById<ImageView>(Resource.Id.ivStars).StartAnimation(animEffect);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(errTxt);
                Console.WriteLine(ex.ToString());
            }

        }
    }
}
