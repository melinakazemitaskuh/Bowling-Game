using UnityEngine;

public class CameraFollowController : MonoBehaviour
{
[SerializeField] private Transform defaultPosition; // موقعیت اولیه دوربین
[SerializeField] private Vector3 offset = new Vector3(0, 5, -10); // فاصله دوربین از توپ
[SerializeField] private float followSpeed = 5f; // سرعت دنبال کردن توپ
[SerializeField] private float resetSpeed = 3f; // سرعت بازگشت دوربین به موقعیت اولیه

private Transform targetBall; // توپی که دوربین آن را دنبال می‌کند
private bool isFollowing = false; // آیا دوربین توپ را دنبال می‌کند؟
private bool isResetting = false; // آیا دوربین در حال بازگشت به موقعیت اولیه است؟

private void LateUpdate()
{
    if (isFollowing && targetBall != null)
    {
        // دنبال کردن توپ با حرکت نرم
        Vector3 targetPosition = targetBall.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
    else if (isResetting)
    {
        // بازگشت دوربین به موقعیت اولیه با حرکت نرم
        transform.position = Vector3.Lerp(transform.position, defaultPosition.position, resetSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, defaultPosition.rotation, resetSpeed * Time.deltaTime);

        // بررسی اتمام بازگشت به موقعیت اولیه
        if (Vector3.Distance(transform.position, defaultPosition.position) < 0.1f &&
            Quaternion.Angle(transform.rotation, defaultPosition.rotation) < 0.1f)
        {
            isResetting = false;
        }
    }
}

public void FollowBall(Transform ballTransform)
{
    targetBall = ballTransform;
    isFollowing = true;
    isResetting = false;
}

public void ResetCamera()
{
    // بازگشت دوربین به موقعیت اولیه با شروع حرکت نرم
    isFollowing = false;
    isResetting = true;
    targetBall = null;
}

}
