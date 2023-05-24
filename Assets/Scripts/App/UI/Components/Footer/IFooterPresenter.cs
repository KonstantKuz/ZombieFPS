namespace App.UI.Components.Footer
{
    public interface IFooterPresenter
    {
        void OnCurrentScreenUpdated(string screenName);
        void SetActive(bool isActive);
    }
}