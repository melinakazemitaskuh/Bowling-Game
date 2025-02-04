using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 

public class BallManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> BallList = new List<GameObject>();
    [SerializeField] private List<GameObject> BallIconList = new List<GameObject>();
    [SerializeField] private GameObject BallPrefab;
    [SerializeField] private GameObject BallIconPrefab;
    [SerializeField] private Vector3 BallPosition;
    [SerializeField] private Vector3 BallIconPosition;
    [SerializeField] private float rowSpacing = 2f; // فاصله بین ردیف‌ها
    [SerializeField] private float columnSpacing = 2f; // فاصله بین ستون‌ها
    [SerializeField] private int rows = 2; // تعداد ردیف‌ها
    [SerializeField] private int columns = 3; // تعداد ستون‌ها

    private BowlingGameManager BowlingGameManager;
    private int BallIndex = 0;

    private void Start()
    {
        CreateBalls();
        CreateBallIcons();
    }

    private void CreateBalls()
    {
        int BallCount = 3;
        for (int i = 0; i < BallCount; i++)
        {
            GameObject newBall = Instantiate(BallPrefab, BallPosition, Quaternion.identity);
            newBall.SetActive(false);
            BallList.Add(newBall);
        }
        BallList[0].SetActive(true); // فعال کردن توپ اول
    }

    private void CreateBallIcons()
    {
        // حلقه برای ایجاد ردیف‌ها و ستون‌ها
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // محاسبه موقعیت جدید برای هر پین
                Vector3 newPosition = new Vector3(
                    BallIconPosition.x - (col * columnSpacing), // تغییر مکان در محور X بر اساس ستون‌ها
                    BallIconPosition.y,                        // محور Y ثابت می‌ماند
                    BallIconPosition.z + (row * rowSpacing)    // تغییر مکان در محور Z بر اساس ردیف‌ها
                );

                // ایجاد پین در موقعیت جدید
                GameObject newBallIcon = Instantiate(BallIconPrefab, newPosition, Quaternion.identity);
                BallIconList.Add(newBallIcon);
            }
        }
    }
    
    public void SetActiveBall()
    {
        if (BallIndex < BallList.Count - 1)
        {
            if (GameObject.FindGameObjectsWithTag("Pin") == null)
            {
                SceneManager.LoadScene("MainMenu");
            }
            else {
            // غیرفعال کردن توپ قبلی
            BallList[BallIndex].SetActive(false);

            BallIndex++; // افزایش ایندکس توپ
            BallList[BallIndex].SetActive(true);

            // حذف یکی از آیکن‌های توپ
            if (BallIconList.Count > 0)
            {
                Destroy(BallIconList[0]);
                BallIconList.RemoveAt(0);
            }
            }
        }
        else
        {
            if(GameObject.FindGameObjectsWithTag("Pin") != null)
            {
                SceneManager.LoadScene("LoseScene");
            }
        }
    }
    public void ResetBalls()
{
    foreach (GameObject ball in BallList)
    {
        ball.SetActive(false);
    }

    BallIndex = 0;
       if (BallList.Count > 0)
    {
        BallList[0].SetActive(true);
    }

    // بازنشانی آیکن‌های توپ
    foreach (GameObject ballIcon in BallIconList)
    {
        Destroy(ballIcon);
    }
    BallIconList.Clear();
    CreateBallIcons();
}
}
