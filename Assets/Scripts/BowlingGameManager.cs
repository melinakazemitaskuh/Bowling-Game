using UnityEngine;
using UnityEngine.SceneManagement; // برای مدیریت صحنه‌ها

public class BowlingGameManager : MonoBehaviour
{
    private CameraFollowController CameraFollowController;
    [SerializeField] private float pinDestroyDelay = 5f;
    [SerializeField] private float ballDestroyDelay = 2f;
    
    private Rigidbody BallRigidbody;
    private BallManager BallManager;
    [SerializeField] private int totalPins = 10;
    
    private int currentStage = 0;
    [SerializeField] private int maxStages = 5;

    private bool ballThrown = false;
    private bool pinHit = false;
    private float throwStartTime = 0;
    private int throwCount = 0; // شمارش تعداد پرتاب‌ها
    private int maxThrows = 3; // حداکثر تعداد پرتاب‌ها

    private void Start()
    {
        GetComponentValues();
    }

    private void Update()
    {
        CheckBallTimeout();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Pin"))
        {
            pinHit = true;

            // حذف بولینگ‌های افتاده و توپ پس از تاخیر
            Invoke(nameof(RemovePinsAndBall), pinDestroyDelay);
        }
    }

    public void ThrowBall()
    {
        ballThrown = true;
        throwCount++; // افزایش شمارش پرتاب‌ها
        throwStartTime = Time.time;

        // // بررسی باخت پس از هر پرتاب
        // if (throwCount >= maxThrows)
        // {
        //     CheckGameOverCondition();
        // }
    }

    private void CheckBallTimeout()
    {
        if (ballThrown && !pinHit && Time.time - throwStartTime > ballDestroyDelay)
        {
            Destroy(gameObject);
            BallManager.SetActiveBall();
        }
    }

    private void RemovePinsAndBall()
    {
        GameObject[] pins = GameObject.FindGameObjectsWithTag("Pin");
        foreach (GameObject pin in pins)
        {
            if (pin.transform.position.y < -12f)
            {
                Destroy(pin);
            }
        }

        RemoveBall();
        BallManager.SetActiveBall();
    }

    private void RemoveBall()
    {
        Destroy(gameObject);
    }

    // public void CheckGameOverCondition()
    // {
    //     // بررسی باخت: اگر تعداد پرتاب‌ها به حداکثر رسیده و پین‌ها باقی مانده‌اند
    //     if (ArePinsRemaining())
    //     {
    //         GameOver();
    //     }
    // }

    // public bool ArePinsRemaining()
    // {
    //     GameObject[] pins = GameObject.FindGameObjectsWithTag("Pin");
    //     return pins.Length > 0; // اگر بولینگ‌ها هنوز باقی مانده‌اند
    // }

    // public void GameOver()
    // {
    //     Debug.Log("Game Over! Loading Lose Scene...");
    //     SceneManager.LoadScene("LoseScene"); // لود صحنه شکست
    // }

    private void GetComponentValues()
    {
        BallRigidbody = GetComponent<Rigidbody>();
        BallManager = GameObject.FindObjectOfType<BallManager>();
    }
}
