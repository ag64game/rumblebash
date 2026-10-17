namespace Constants
{
    public static class FuncConsts
    {
        public static async Task PlaySFX(int sfxId, int volDivLeft = 100, int volDivRight = 100)
        {
            mpCombatHandler = MediaPlayer.Create(main, sfxId);
            mpCombatHandler.Start();
            mpCombatHandler.SetVolume(volumeSFX / volDivLeft, volumeSFX / volDivRight);
            mpCombatHandler.Dispose();
        }

        public static async Task PlayDemon(int sfxId, int volDivLeft = 100, int volDivRight = 100)
        {
            mpCombatHandler = MediaPlayer.Create(main, sfxId);
            mpCombatHandler.Start();
            mpCombatHandler.SetVolume(volumeDemon / volDivLeft, volumeDemon / volDivRight);
            mpCombatHandler.Dispose();
        }
    }
}
