namespace Game
{
    public static class EventCodePhoton
    {
        public const byte InstantiateNewPlayerComplete = 0;
        public const byte ReadyStatusChange = 1;
        public const byte NewPlayerEnterRoom = 2;
        public const byte ReadyStatusRefreshSend = 3;
        public const byte EnemyReceiveDamage = 4;
        public const byte PlayerPickedUpBonus = 5;
        public const byte ChosenBonusSend = 6;
    }
}