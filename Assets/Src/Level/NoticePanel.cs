using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class NoticePanel : MonoBehaviour {
    public static NoticePanel instance;
    private void Awake() => instance = this;
    [SerializeField] RectTransform noticePanel;
    
    public IEnumerator ShowNotice(string message) { //me too
        noticePanel.GetComponentInChildren<TextMeshProUGUI>().SetText(message);
        if(noticePanel.anchoredPosition.y != 12.75f) {
            noticePanel.DOAnchorPosY(12.75f, 0.25f);
            yield return new WaitForSeconds(4);
            noticePanel.DOAnchorPosY(-47.7f, 0.5f);
        }
    }
}
