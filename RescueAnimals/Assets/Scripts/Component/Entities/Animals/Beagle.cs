public class Beagle : Animal, IAnimalBehaviour
{
    private float _reinforceTime = 3f;

    public void OnResqueMove()
    {
        this.gameObject.SetActive(false);
    }

    public void OnResqueEffect()
    {
        GameManager.Instance.BeagleTime();
    }
}