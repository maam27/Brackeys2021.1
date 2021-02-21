using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable_Asset_Definitions;
using Scriptable_Asset_Definitions.Modifiers;
using Ship;
using Ship.Weapons;
using UnityEngine;
using TMPro;
using Utility.Attributes;
using Random = UnityEngine.Random;

namespace Level
{
    public class ConvictAlly : MonoBehaviour
    {
        public List<string> dialogPresets = new List<string>();
        public TMP_Text dialogArea;
        [Space] [Expose] public WeaponModifier weaponModifiers;
        [Expose] public Weapon newWeapon;
        [Expose] public ShipModifier ShipModifier;

        public bool hasBeenAlreadyHelped { private set; get; }


        private void OnEnable()
        {
            hasBeenAlreadyHelped = false;
            var parent = dialogArea.transform.parent;
            parent.gameObject.SetActive(false);
        }

        private void ApplyModification(WeaponSystems systems)
        {
            systems.currentModifiers.Add(weaponModifiers);
        }


        public void HelpPlayer(ShipController player)
        {
            hasBeenAlreadyHelped = true;
            ApplyModification(player.GetComponent<WeaponSystems>());
            StartCoroutine(DisplayDialog());
        }

        private IEnumerator DisplayDialog()
        {
            var parent = dialogArea.transform.parent;
            parent.gameObject.SetActive(true);
            dialogArea.text = dialogPresets[Random.Range(0, dialogPresets.Count)];
            yield return new WaitForSeconds(2f);
            parent.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        public Vector3 GetRandomPositionWithinLevel(Vector2 positionOffset, float size)
        {
            Vector3 result = Vector3.zero;
            while (result == Vector3.zero || !LevelManager.PublicAccess.IsPositionWithinZone(result))
            {
                result = positionOffset.ToXZ() + Random.insideUnitCircle.ToXZ() * size;
            }


            return result;
        }
    }
}