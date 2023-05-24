namespace App.InteractableItems
{
    public interface IInteractableItem
    {
        string ObjectId { get; }
        void Interact();
    }
}

