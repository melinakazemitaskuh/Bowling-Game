using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody BallRigidbody;
    private BallManager BallManager;
    private CameraFollowController CameraFollowController; // اضافه کردن کنترلر دوربین
    [SerializeField] private float maxMoveSpeed = 40f;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float powerMultiplier = 0.1f;
    [SerializeField] private LineRenderer aimIndicator;
    [SerializeField] private float maxIndicatorLength = 6f;
    [SerializeField] private float aimLerpSpeed = 5f; // سرعت لرپ نشانگر پرتاب

    private bool isDragging;
    private bool canShoot;
    private float currentPower;
    private Vector3 shootDirection = Vector3.forward;
    private Vector3 initialMousePosition;
    private Vector3 currentMousePosition;
    private float currentAngle;

    private void Start()
    {
        GetComponentValues();
        InitializeAimIndicator();
    }

    private void Update()
    {
        HandleInput();
        UpdateAimIndicator();
    }

    private void FixedUpdate()
    {
        Shoot();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            initialMousePosition = Input.mousePosition;
            aimIndicator.enabled = true;
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            currentMousePosition = Input.mousePosition;

            // تنظیم زاویه توپ بر اساس حرکت افقی موس
            float horizontalDelta = currentMousePosition.x - initialMousePosition.x;
            float rotationAmount = horizontalDelta * rotationSpeed * Time.deltaTime;
            currentAngle += rotationAmount;

            // محدود کردن زاویه به بازه -60 تا 60
            currentAngle = Mathf.Clamp(currentAngle, -60f, 60f);

            // به‌روزرسانی جهت توپ با توجه به زاویه محدودشده
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, currentAngle, 0), aimLerpSpeed * Time.deltaTime);
            shootDirection = transform.forward; // به‌روزرسانی جهت پرتاب

            // تنظیم قدرت پرتاب بر اساس حرکت عمودی موس
            float verticalDelta = initialMousePosition.y - currentMousePosition.y; // حرکت به عقب موس قدرت را افزایش می‌دهد
            currentPower = Mathf.Clamp(verticalDelta * powerMultiplier, 0, maxMoveSpeed);
        }

        if (Input.GetMouseButtonUp(0))
        {
            // رها کردن موس برای پرتاب
            isDragging = false;
            canShoot = true;
            aimIndicator.enabled = false; // غیرفعال کردن نشانگر
        }
    }

    private void Shoot()
    {
        if (canShoot)
        {
            
            BallRigidbody.AddForce(shootDirection * currentPower, ForceMode.Impulse);
            canShoot = false;

            // ثبت پرتاب توپ
            GetComponent<BowlingGameManager>().ThrowBall();

            // اتصال دوربین به توپ
            CameraFollowController.FollowBall(transform);
        
        }

    }

    private void GetComponentValues()
    {
        BallRigidbody = GetComponent<Rigidbody>();
        BallManager = FindObjectOfType<BallManager>();
        CameraFollowController = FindObjectOfType<CameraFollowController>();
    }

 private void InitializeAimIndicator()
    {
  // بررسی LineRenderer
        if (!aimIndicator)
        {
            Debug.LogError("LineRenderer is not assigned!");
            return;
        }

        aimIndicator.positionCount = 3; // نشانگر مثلثی سه نقطه دارد
        aimIndicator.enabled = false; // در ابتدا غیرفعال است
    }

    private void UpdateAimIndicator()
    {
        if (isDragging)
        {
            // طول نشانگر بر اساس قدرت پرتاب
            float indicatorLength = Mathf.Lerp(0, maxIndicatorLength, currentPower / maxMoveSpeed);

            // نقاط مثلث را مشخص کنید
            Vector3 basePosition = transform.position; // موقعیت پایه (توپ)
            Vector3 forwardPoint = basePosition + shootDirection * indicatorLength; // نوک مثلث
            Vector3 leftPoint = basePosition + Quaternion.Euler(0, -30, 0) * shootDirection * (indicatorLength / 2); // گوشه چپ
            Vector3 rightPoint = basePosition + Quaternion.Euler(0, 30, 0) * shootDirection * (indicatorLength / 2); // گوشه راست

            // تنظیم نقاط LineRenderer
            aimIndicator.SetPosition(0, leftPoint);
            aimIndicator.SetPosition(1, forwardPoint);
            aimIndicator.SetPosition(2, rightPoint);
            aimIndicator.SetPosition(0, leftPoint); // بستن مثلث
        }
    }
}
