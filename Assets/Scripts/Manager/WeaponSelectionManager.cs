using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectionManager : MonoBehaviour,IGameStateListener
{
    [Header("Settings")]
    [SerializeField] private Transform containerP;
    [SerializeField] private WeaponSelectionContainer prefabs; 
    [Header("Ref")]
    [SerializeField] private WeaponDataSO[] dataSOs; 
    [SerializeField] private PlayerWeapon playerWeapon; 
    [SerializeField] private Button Next; 
    [Header("DOTween Settings")]
    [SerializeField] private float slideDistance = 1000f; // Khoảng cách đặt ở ngoài màn hình bên trái
    [SerializeField] private float duration = 0.4f;       // Thời gian bay của 1 container
    [SerializeField] private float delayBetween = 0.1f;    // Độ trễ giữa các container (hiệu ứng gợn sóng)
    private WeaponDataSO selectedWeapon; 
    private int initialLevel;
    public void GameStateChangeCallBack(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GAME :
            case GameState.SHOP :
            case GameState.WAVETRANS :
                if(selectedWeapon == null) return;
                playerWeapon.tryAddWeapon(selectedWeapon,initialLevel);
                selectedWeapon =null;
                break;
            case GameState.WEAPONCHOSE :
                configure();
                break;
        }
    }
    [Button]
    private void configure()
    {
        // 1. Dọn dẹp card cũ
        foreach (Transform child in containerP) 
        {
            child.DOKill();
            Destroy(child.gameObject);
        } 

        // 2. Chạy Coroutine sinh card và tạo Animation
        StartCoroutine(GenerateAndAnimateRoutine());
    }

    private IEnumerator GenerateAndAnimateRoutine()
    {
        List<WeaponSelectionContainer> createdContainers = new List<WeaponSelectionContainer>();

        // Bước A: Sinh ra 3 card
        for (int i = 0; i < 3; i++)
        {
            WeaponSelectionContainer container = Instantiate(prefabs, containerP);   
            WeaponDataSO weaponDataSO = dataSOs[UnityEngine.Random.Range(0, dataSOs.Length)];

            int level = UnityEngine.Random.Range(0, 4);
            int capturelv = level;
            container.Configure(weaponDataSO.Sprite, weaponDataSO.WeaponName, capturelv, weaponDataSO);
            
            container.button.onClick.RemoveAllListeners();
            container.button.onClick.AddListener(() => WeaponSelectionCallback(container, weaponDataSO, capturelv));
            container.button.onClick.AddListener(() => Next.interactable = true);

            createdContainers.Add(container);
        }

        // Bước B: Ép Unity tính toán vị trí Layout Group ngay lập tức
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(containerP as RectTransform);

        // Chờ 1 frame nhỏ để Unity chắc chắn cập nhật xong vị trí cho các Child
        yield return new WaitForEndOfFrame();

        // Bước C: Chạy DOTween sau khi Layout đã dàn hàng 3 cột chuẩn vị trí
        for (int i = 0; i < createdContainers.Count; i++)
        {
            var container = createdContainers[i];
            RectTransform rectTransform = container.GetComponent<RectTransform>();
            
            CanvasGroup canvasGroup = container.GetComponent<CanvasGroup>();
            if (canvasGroup == null) canvasGroup = container.gameObject.AddComponent<CanvasGroup>();

            // Lấy vị trí đích ĐÚNG (sau khi Layout đã xếp hàng)
            Vector2 targetPos = rectTransform.anchoredPosition;

            // Đặt vị trí ban đầu lùi về bên trái
            rectTransform.anchoredPosition = targetPos - new Vector2(slideDistance, 0);
            canvasGroup.alpha = 0f;

            // Chạy Animation bay vào
            float delay = i * delayBetween;
            
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(delay);
            seq.Append(rectTransform.DOAnchorPos(targetPos, duration).SetEase(Ease.OutBack));
            seq.Join(canvasGroup.DOFade(1f, duration));
        }
    }
    private void WeaponSelectionCallback(WeaponSelectionContainer w,WeaponDataSO d,int lv)
    {
        selectedWeapon = d;
        initialLevel = lv;
        foreach (WeaponSelectionContainer c in containerP.GetComponentsInChildren<WeaponSelectionContainer>())
        {
            if(c == w)
            {
                c.Select();
            }else c.DeSelect();
        }
    }
}
