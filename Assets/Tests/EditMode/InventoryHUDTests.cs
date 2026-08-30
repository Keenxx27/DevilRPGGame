using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using RPG;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace RPG.Tests
{
    public class InventoryHUDTests
    {
        private readonly List<GameObject> createdObjects = new List<GameObject>();
        private readonly List<Object> createdAssets = new List<Object>();

        [TearDown]
        public void TearDown()
        {
            foreach (GameObject createdObject in createdObjects)
            {
                if (createdObject != null)
                {
                    Object.DestroyImmediate(createdObject);
                }
            }

            foreach (Object createdAsset in createdAssets)
            {
                if (createdAsset != null)
                {
                    Object.DestroyImmediate(createdAsset);
                }
            }

            createdObjects.Clear();
            createdAssets.Clear();
        }

        [Test]
        public void Refresh_SelectsOnlyCurrentBorderAndHidesEmptyIcons()
        {
            InventoryHUD hud = CreateConfiguredHud(out _, out Image[] borders, out Image[] icons);

            Assert.That(hud.Refresh(), Is.True);
            Assert.That(borders[0].enabled, Is.True);
            Assert.That(borders[1].enabled, Is.False);
            Assert.That(borders[2].enabled, Is.False);
            Assert.That(icons[0].enabled, Is.False);
            Assert.That(icons[1].enabled, Is.False);
            Assert.That(icons[2].enabled, Is.False);
        }

        [Test]
        public void Refresh_ShowsPickedUpItemsSpriteAndColor()
        {
            InventoryHUD hud = CreateConfiguredHud(out PlayerInventory inventory, out _, out Image[] icons);
            WorldItem card = CreateItemWithSprite(Color.blue);
            inventory.RegisterNearby(card);
            inventory.TryPickUpNearest();

            hud.Refresh();

            Assert.That(icons[0].enabled, Is.True);
            Assert.That(icons[0].sprite, Is.SameAs(card.Icon));
            Assert.That(icons[0].color, Is.EqualTo(Color.blue));
        }

        [Test]
        public void Refresh_SelectsOnlyThirdBorderWhenThirdSlotIsSelected()
        {
            InventoryHUD hud = CreateConfiguredHud(out PlayerInventory inventory,
                out Image[] borders, out _);
            FieldInfo slotsField = typeof(PlayerInventory).GetField(
                "slots", BindingFlags.Instance | BindingFlags.NonPublic);
            var slots = (InventorySlots<WorldItem>)slotsField.GetValue(inventory);
            slots.Select(2);

            hud.Refresh();

            Assert.That(borders[0].enabled, Is.False);
            Assert.That(borders[1].enabled, Is.False);
            Assert.That(borders[2].enabled, Is.True);
        }

        [Test]
        public void Refresh_InvalidConfigurationReportsErrorOnlyOnce()
        {
            GameObject hudObject = CreateGameObject("Unconfigured HUD", false);
            InventoryHUD hud = hudObject.AddComponent<InventoryHUD>();
            LogAssert.Expect(LogType.Error,
                "InventoryHUD requires one inventory, three borders, and three icons.");

            Assert.That(hud.Refresh(), Is.False);
            Assert.That(hud.Refresh(), Is.False);
            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void ActivatingHudBeforeInventoryAwake_DoesNotReadUninitializedSlots()
        {
            GameObject player = CreateGameObject("Uninitialized Player");
            player.AddComponent<CircleCollider2D>();
            player.AddComponent<MainCharacterMovement>();
            PlayerInventory inventory = player.AddComponent<PlayerInventory>();
            typeof(PlayerInventory).GetField("slots", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(inventory, null);

            GameObject hudObject = CreateGameObject("Early HUD", false);
            InventoryHUD hud = hudObject.AddComponent<InventoryHUD>();
            SetPrivateField(hud, "inventory", inventory);
            SetPrivateField(hud, "selectionBorders", CreateImages("Early Border"));
            SetPrivateField(hud, "itemIcons", CreateImages("Early Icon"));

            Assert.DoesNotThrow(() => hudObject.SetActive(true));
            LogAssert.NoUnexpectedReceived();
        }

        private InventoryHUD CreateConfiguredHud(
            out PlayerInventory inventory, out Image[] borders, out Image[] icons)
        {
            GameObject player = CreateGameObject("Player");
            player.AddComponent<CircleCollider2D>();
            player.AddComponent<MainCharacterMovement>();
            inventory = player.AddComponent<PlayerInventory>();
            EditModeTestLifecycle.Invoke(inventory, "Awake");

            GameObject hudObject = CreateGameObject("HUD", false);
            InventoryHUD hud = hudObject.AddComponent<InventoryHUD>();
            borders = CreateImages("Border");
            icons = CreateImages("Icon");
            SetPrivateField(hud, "inventory", inventory);
            SetPrivateField(hud, "selectionBorders", borders);
            SetPrivateField(hud, "itemIcons", icons);
            hudObject.SetActive(true);
            return hud;
        }

        private Image[] CreateImages(string prefix)
        {
            var images = new Image[3];
            for (int index = 0; index < images.Length; index++)
            {
                images[index] = CreateGameObject(prefix + " " + index).AddComponent<Image>();
            }

            return images;
        }

        private WorldItem CreateItemWithSprite(Color color)
        {
            GameObject itemObject = CreateGameObject("Card");
            WorldItem item = itemObject.AddComponent<WorldItem>();
            EditModeTestLifecycle.Invoke(item, "Awake");
            var texture = new Texture2D(2, 2);
            Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, 2f, 2f), new Vector2(0.5f, 0.5f));
            createdAssets.Add(sprite);
            createdAssets.Add(texture);
            SpriteRenderer renderer = itemObject.GetComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.color = color;
            return item;
        }

        private GameObject CreateGameObject(string name, bool active = true)
        {
            var gameObject = new GameObject(name);
            gameObject.SetActive(active);
            createdObjects.Add(gameObject);
            return gameObject;
        }

        private static void SetPrivateField<T>(object target, string name, T value)
        {
            FieldInfo field = target.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(target, value);
        }
    }
}
