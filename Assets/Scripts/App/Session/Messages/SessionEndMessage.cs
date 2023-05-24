namespace App.Session.Messages
{
    public readonly struct SessionEndMessage
    {
        public readonly bool IsWon;
        
        public SessionEndMessage(bool isWon)
        {
            IsWon = isWon;
        }
    }
}