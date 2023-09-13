namespace Component.Entities.Animals
{
    public class GilNyang : Animal, IAnimalBehaviour
    {
        public void OnResqueMove()
        {
        }

        public void OnResqueEffect()
        {
            GameManager.Instance.SatelliteEffect();
        }
    }
}