using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speedX = 3f;
    [SerializeField] private float learpSpeedX = 2f;

    [SerializeField] private float pressSpeedX = 5f;
    [SerializeField] private float speedY = 2f;
    [SerializeField] private Transform footTrans;
    private PlayerInput input;
    private Vector3 footStartLocalScale;
    private Vector3 footStartLocalPos;
    private Collider2D col;
    private Camera mainCamera;
    private Vector2 velocity;
    private bool isDead;
    private bool isGameStart;

    void Awake()
    {
        Init();
        Subscribe();
    }

    void OnDestroy()
    {
        Unsubscribe();
    }
    private void Init()
    {
        input = PlayerInput.Instance;
        mainCamera = Camera.main;
        col = GetComponent<Collider2D>();
        footStartLocalPos = footTrans.localPosition;
        footStartLocalScale = footTrans.localScale;
    }

    private void Subscribe()
    {
        EventCenter.AddListener(Event.GameStart, HandleGameStart);
    }
    private void Unsubscribe()
    {
        EventCenter.RemoveListener(Event.GameStart, HandleGameStart);
    }

    private void HandleGameStart()
    {
        isGameStart = true;
    }
    
    void Update()
    {
        if (!isGameStart) return;
        if (isDead) return;
        int dir = input.Direction;

        if (dir > 0)
        {
            footTrans.localPosition = footStartLocalPos;
            footTrans.localScale = footStartLocalScale;
        }
        else
        {
            Vector3 tempPos = footStartLocalPos;
            tempPos.x = -tempPos.x;
            Vector3 tempScale = footStartLocalScale;
            tempScale.x = -tempScale.x;
            footTrans.localPosition = tempPos;
            footTrans.localScale = tempScale;
        }
        
        float targetX = (input.IsPressing ? pressSpeedX : speedX) * dir;

        velocity.x = Mathf.Lerp(velocity.x, targetX, Time.deltaTime * learpSpeedX);
        velocity.y = Mathf.Lerp(velocity.y, -speedY, Time.deltaTime * learpSpeedX);

        transform.Translate(velocity * Time.deltaTime);

        CheckScreenEdge();
    }

    private void CheckScreenEdge()
    {
        float halfWidth = mainCamera.orthographicSize * mainCamera.aspect;

        float screenLeft  = mainCamera.transform.position.x - halfWidth;
        float screenRight = mainCamera.transform.position.x + halfWidth;

        if (col.bounds.min.x <= screenLeft || col.bounds.max.x >= screenRight)
            Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        Debug.Log("死亡");
    }
}