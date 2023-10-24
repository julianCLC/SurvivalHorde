using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    Canvas canvas;
    RectTransform rect;
    [SerializeField] Slider xpBar;
    [SerializeField] Slider hpBar;
    

    // Start is called before the first frame update
    void Start() {
        canvas = GetComponentInChildren<Canvas>();
        rect = GetComponentInChildren<RectTransform>();
        xpBar.value = 0;
    }

    // Update is called once per frame
    void Update() {
        XpBarUpdate();
        HpBarUpdate();
    }

    void XpBarUpdate(){
        float xpProgress = GameManager.instance._playerXp / GameManager.instance._playerLvlXpThresh;
        xpBar.value = xpProgress;
    }

    void HpBarUpdate(){

        // hp bar follow player
        Vector2 screenPos = GameManager.instance.cam.WorldToScreenPoint(GameManager.instance._playerPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPos, null,  out Vector2 canvasPoint);
        hpBar.transform.localPosition = new Vector3(canvasPoint.x, canvasPoint.y - 30, 0);

        float currentHp = (float)GameManager.instance._playerGameFunctions._health / GameManager.instance._playerGameFunctions._maxHealth;
        hpBar.value = currentHp;
    }
        
}
