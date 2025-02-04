using UnityEngine;

public class PinSpawnController : MonoBehaviour
{
    [SerializeField] private GameObject PinPrefab; // Prefab پین
    [SerializeField] private Vector3 startPosition = new Vector3(88, -10, 28); // موقعیت اولیه برای اولین پین
    [SerializeField] private float rowSpacing = 2f; // فاصله بین ردیف‌ها
    [SerializeField] private float columnSpacing = 2f; // فاصله بین ستون‌ها
    [SerializeField] private int rows = 2; // تعداد ردیف‌ها
    [SerializeField] private int columns = 3; // تعداد ستون‌ها

    void Start()
    {
        GeneratePinGrid();
    }

    public void GeneratePinGrid()
    {
        // حلقه برای ایجاد ردیف‌ها و ستون‌ها
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // محاسبه موقعیت جدید برای هر پین
                Vector3 newPosition = new Vector3(
                    startPosition.x - (col * columnSpacing), // تغییر مکان در محور X بر اساس ستون‌ها
                    startPosition.y,                        // محور Y ثابت می‌ماند
                    startPosition.z + (row * rowSpacing)    // تغییر مکان در محور Z بر اساس ردیف‌ها
                );

                // ایجاد پین در موقعیت جدید
                Instantiate(PinPrefab, newPosition, PinPrefab.transform.rotation);
            }
        }
    }
}
