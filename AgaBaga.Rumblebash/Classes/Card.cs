namespace AgaBaga.Rumblebash.Classes
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
        public Animation aIdleAnimation = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_IdleAnimationNone);

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

                        Console.WriteLine(worksTxt);
                        Console.WriteLine(strSplitLine[4]);
                        Type abilType = Type.GetType(ABILITY_DIR + strSplitLine[4].Trim());
                        cAbility = (Ability)Activator.CreateInstance(abilType);

                        int idPortrait = (int)typeof(Resource.Drawable).GetField(strSplitLine[5].Trim()).GetValue(null);
                        cPortrait = BitmapFactory.DecodeResource(Application.Context.Resources, idPortrait);

                        int idMonster = (int)typeof(Resource.Drawable).GetField(strSplitLine[6].Trim()).GetValue(null);
                        cMonster = BitmapFactory.DecodeResource(Application.Context.Resources, idMonster);

                        int idAnimation = (int)typeof(Resource.Animation).GetField(strSplitLine[7].Trim()).GetValue(null);
                        aIdleAnimation = AnimationUtils.LoadAnimation(main, idAnimation);
                        aIdleAnimation.Duration = Convert.ToInt32(strSplitLine[8]);
                        if (cName == "Fin-Inito") aIdleAnimation.Interpolator = new LinearInterpolator();
                        else aIdleAnimation.RepeatMode = RepeatMode.Reverse;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(errTxt);
                Console.WriteLine(typeof(AbilNone));
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

                Type abilType = Type.GetType(ABILITY_DIR + requestedAbil.Trim());
                cAbility = (Ability)Activator.CreateInstance(abilType);

                int idPortrait = (int)typeof(Resource.Drawable).GetField(requestedPortrait.Trim()).GetValue(null);
                cPortrait = BitmapFactory.DecodeResource(Application.Context.Resources, idPortrait);

                int idMonster = (int)typeof(Resource.Drawable).GetField(requestedMonster.Trim()).GetValue(null);
                cMonster = BitmapFactory.DecodeResource(Application.Context.Resources, idMonster);

                int idAnimation = (int)typeof(Resource.Animation).GetField(requestedAnimation.Trim()).GetValue(null);
                aIdleAnimation = AnimationUtils.LoadAnimation(main, idAnimation);
                aIdleAnimation.Duration = Convert.ToInt32(requestedDuration);
                if (cName == "Fin-Inito") aIdleAnimation.Interpolator = new LinearInterpolator();
                else aIdleAnimation.RepeatMode = RepeatMode.Reverse;
            }
            catch (Exception e)
            {
                Console.WriteLine(errTxt);
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

                Type abilType = Type.GetType(ABILITY_DIR + requestedAbil.Trim());
                cAbility = (Ability)Activator.CreateInstance(abilType);

                cPortrait = requestedPortrait;
                cMonster = requestedMonster;

                int idAnimation = (int)typeof(Resource.Animation).GetField(requestedAnimation.Trim()).GetValue(null);
                aIdleAnimation = AnimationUtils.LoadAnimation(main, idAnimation);
                aIdleAnimation.Duration = Convert.ToInt32(requestedDuration);
                if (cName == "Fin-Inito") aIdleAnimation.Interpolator = new LinearInterpolator();
                else aIdleAnimation.RepeatMode = RepeatMode.Reverse;
            }
            catch (Exception e)
            {
                Console.WriteLine(errTxt);
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

                Type abilType = Type.GetType(ABILITY_DIR + requestedAbil.Trim());
                cAbility = (Ability)Activator.CreateInstance(abilType);

                cPortrait = requestedPortrait;
                cMonster = requestedMonster;

                aIdleAnimation = requestedAnimation;
            }
            catch (Exception e)
            {
                Console.WriteLine(errTxt);
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
}
