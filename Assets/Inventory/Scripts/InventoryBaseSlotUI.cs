﻿using uItem;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace uInventory
{
    [RequireComponent (typeof (Image))]
    public abstract class InventoryBaseSlotUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler
    {
        public delegate void SlotInteractionDelegate (InventoryBaseSlot slot);
        public event SlotInteractionDelegate OnLeftClickSlot = delegate { };
        public event SlotInteractionDelegate OnRightClickSlot = delegate { };
        public event SlotInteractionDelegate OnMouseEnterSlot = delegate { };
        public event SlotInteractionDelegate OnMouseExitSlot = delegate { };
        public event SlotInteractionDelegate OnMouseDragSlot = delegate { };

        protected InventoryBaseSlot slot;

        public Image BackgroundImage;
        public Image ItemImage;

        protected virtual void Awake ()
        {
            Assert.IsNotNull (BackgroundImage, "Inventory slot doesn't have a image component");
            Assert.IsNotNull (ItemImage, "Inventory slot doesn't have a child component called 'ItemImage' with image component");

            ItemImage.enabled = false;
        }

        protected virtual void OnEnable ()
        {
            // @todo: better to set before becoming visiable
            if (slot != null)
            {
                UpdateUI (slot.Item, slot.ItemAmount);
            }
        }

        public virtual void SetSlot (InventoryBaseSlot slot)
        {
            Assert.IsNotNull (slot, "null inventory slot model is passed to inventory slot ui");

            this.slot = slot;
            this.slot.OnItemChanged += UpdateUI;
        }

        protected virtual void UpdateUI (Item item, int amount)
        {
            if (item != null)
            {
                ItemImage.sprite = item.icon;
                ItemImage.enabled = true;
            }
            else
            {
                ItemImage.enabled = false;
                ItemImage.sprite = null;
            }
        }

        public void OnPointerDown (PointerEventData eventData)
        {
            if (slot == null) { return; }

            PointerEventData.InputButton button = eventData.button;
            if (button == PointerEventData.InputButton.Left)
            {
                OnLeftClickSlot (slot);
            }
            else if (button == PointerEventData.InputButton.Right)
            {
                OnRightClickSlot (slot);
            }
        }

        public virtual void OnPointerEnter (PointerEventData eventData)
        {
            OnMouseEnterSlot (slot);
        }

        public virtual void OnPointerExit (PointerEventData eventData)
        {
            OnMouseExitSlot (slot);
        }

        public virtual void OnDrag (PointerEventData eventData)
        {
            OnMouseDragSlot (slot);
        }
    }
}