using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour {
    public static CombatUI instance;
    private void Awake() => instance = this;

    [SerializeField] float panelSpeed = 0.5f;

    [SerializeField] RectTransform SIRPanel;
    [SerializeField] float SIRPanelShow;
    [SerializeField] float SIRPanelHide;
    [SerializeField] float SIRPSpeed = 0.35f;
    bool SIRSelectionToggle;
    public void _ToggleSIRSelectionPanel() {
        if(!SIRSelectionToggle) {
            SIRPanel.DOAnchorPosX(SIRPanelShow, SIRPSpeed);
            SIRSelectionToggle = true;
        }
        else {
            SIRPanel.DOAnchorPosX(SIRPanelHide, SIRPSpeed);
            SIRSelectionToggle = false;
        }
    }

    internal void UpdateLabelsToEntity(Combat entity) {
        skillPanel.Find("Character Focus Stats").GetComponent<StatBarUI>().UpdateEntityFocusPanel(entity.combatData);
        Button[] panels = skillPanel.Find("Moveset").GetComponentsInChildren<Button>();
        for (int i = 0; i < entity.combatData.movesets.Length; i++) {
            panels[i].gameObject.name = entity.combatData.movesets[i].name;
            panels[i].GetComponentInChildren<TextMeshProUGUI>().SetText(entity.combatData.movesets[i].name);
        }
    }

    [SerializeField] RectTransform skillPanel;
    [SerializeField] float skillPanelShow;
    [SerializeField] float skillPanelHide;
    [SerializeField] float skillPSpeed = 0.35f;
    bool skillToggle;
    public void _ToggleSkillPanel() {
        if(!skillToggle) {
            _ToggleSIRSelectionPanel();
            skillPanel.DOAnchorPosX(skillPanelShow, skillPSpeed);
            skillToggle = true;
        }
        else {
            if(selectedButton != null) {
                selectedButton.DOLocalMoveX(selectedButton.transform.localPosition.x - selectedButtonMoveOffset, skillPSpeed);
                selectedButton.GetComponent<Image>().color = Color.white;
                selectedButton.transform.localScale = Vector3.one;
                selectedButton = null;
            }
            noticePanel.DOAnchorPosY(-12.2f, 0.5f);
            skillPanel.DOAnchorPosX(skillPanelHide, skillPSpeed);
            skillToggle = false;

        }
    }
    Transform selectedButton;
    float selectedButtonMoveOffset = 20;
    [SerializeField] float buttonMoveSpeed = 0.2f;
    [SerializeField] float buttonScaleAmount = 0.25f;
    [SerializeField] float buttonScaleSpeed = 0.5f;
    public void _SelectSkillButton(Transform button) {
        if(selectedButton != null) {
            if(selectedButton == button) {
                selectedButton.DOPunchScale(new Vector3(buttonScaleAmount, -buttonScaleAmount), buttonScaleSpeed, 1, 0);
                return;
            }
            noticePanel.DOAnchorPosY(-12.2f, 0.5f);
            selectedButton.transform.localScale = Vector3.one;
            selectedButton.GetComponent<Image>().color = Color.white;
            selectedButton.DOLocalMoveX(selectedButton.transform.localPosition.x - selectedButtonMoveOffset, buttonMoveSpeed);
            selectedButton = button;
            selectedButton.DOLocalMoveX(selectedButton.transform.localPosition.x + selectedButtonMoveOffset, buttonMoveSpeed);
        }
        else {
            selectedButton = button;
            selectedButton.DOLocalMoveX(selectedButton.transform.localPosition.x + selectedButtonMoveOffset, buttonMoveSpeed);
        }
        selectedButton.GetComponent<Image>().color = Color.white;
    }

    [SerializeField] float pulsateSpeed;
    public IEnumerator PulsatingSkillButton() {
        while(true) {
        DOVirtual.Color(Color.white, new Color(0.6792453f, 0.6183695f, 0.6651971f, 1), pulsateSpeed, (value) => selectedButton.GetComponent<Image>().color = value);
        yield return new WaitForSeconds(pulsateSpeed);
        DOVirtual.Color(new Color(0.6792453f, 0.6183695f, 0.6651971f, 1), Color.white, pulsateSpeed, (value) => selectedButton.GetComponent<Image>().color = value);
        yield return new WaitForSeconds(pulsateSpeed);
        }
    }

    [SerializeField] RectTransform noticePanel;
    public IEnumerator NullTarget() {
        noticePanel.GetComponentInChildren<TextMeshProUGUI>().SetText("Select a target first!");
        noticePanel.DOAnchorPosY(12.75f, 0.25f);
        yield return new WaitForSeconds(1);
        noticePanel.DOAnchorPosY(-12.2f, 0.5f);
    }

    public IEnumerator NoEnergy(string name) {
        noticePanel.GetComponentInChildren<TextMeshProUGUI>().SetText(name + " is too tired...");
        noticePanel.DOAnchorPosY(12.75f, 0.25f);
        yield return new WaitForSeconds(2);
        noticePanel.DOAnchorPosY(-12.2f, 0.5f);
        yield return new WaitForSeconds(1);

    }

    [SerializeField] Transform itemPanel;
    bool itemToggle;
    internal void ToggleUseItemButton() => itemPanel.Find("Button").DOLocalMoveY(-58.5f, panelSpeed);

    public void _ToggleItemPanel() {
        if(!itemToggle) {
            _ToggleSIRSelectionPanel();
            CombatSystem.instance.SetMoveType(CombatSystem.MoveType.Item);
            InventorySystem.instance.UpdateInventoryUI();

            itemPanel.DOLocalMoveY(0, panelSpeed);
            itemToggle = true;
        }

        else {
            itemPanel.Find("Button").DOLocalMoveY(-95, panelSpeed);
            itemPanel.DOLocalMoveY(-100, panelSpeed);
            itemToggle = false;
        }
    }

}