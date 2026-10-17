namespace Constants
{
    public class SettingsConsts
    {
        //Variables regarding the settings menu
        public static LayoutInflater liInflateSettings;
        public static ISharedPreferences sp;
        public static SeekBar sbVolumeMusic, sbVolumeSfx, sbVolumeDemon;
        public static RadioGroup rgBgScroll, rgPlayIdleAnims;
        public static RadioButton rbIdleChoice1, rbIdleChoice2;
        public static RadioButton rbBGChoice1, rbBGChoice2;
        public static ImageView ivSaveButton;
        public static View vSettings, vModal;
        public static bool isInSettings = false;
        public static ImageView ivSettingsPortrait;
        public static ViewGroup inflatedSettings;
        public static Action[] settingsPages;
        public static ImageView ivArrowLeft, ivArrowRight;
        public static int curPage = 1, lastImage = 0;
        public static bool firstInflate = true;

        //Game settings
        public static float volumeMusic = 100, volumeSFX = 100, volumeDemon = 100;
        public static bool animateBackground = true, animateIdleAnims = true;
        public static bool playStarkill = true;
        public static bool postOnline = true, endlessMode = false;
    }

}