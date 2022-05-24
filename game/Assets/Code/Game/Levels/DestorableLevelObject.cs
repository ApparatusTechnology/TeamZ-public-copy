namespace TeamZ.Code.Game.Levels
{
    public class DestorableLevelObject : LevelObject
    {
        protected override void StrengthTooLow()
        {
            Destroy(this.gameObject);
        }
    }
}
