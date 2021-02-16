using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class ShipInputHandler : MonoBehaviour
    {
        public InputActionReference shipMovementReference, shipLookReference, shipFireReference;


        private void OnEnable()
        {
            SetInputReferencesActive(true, shipMovementReference, shipLookReference, shipFireReference);
        }

        private void OnDisable()
        {
            SetInputReferencesActive(false, shipMovementReference, shipLookReference, shipFireReference);
        }

        public void SetInputReferencesActive(bool value, params InputActionReference[] references)
        {
            foreach (InputActionReference @ref in references)
            {
                if (value)
                    @ref.action.Enable();
                else
                    @ref.action.Disable();
            }
        }


        public Vector2 GetMovementInput => shipMovementReference.action.ReadValue<Vector2>();
        public Vector2 GetMousePositionInput => shipLookReference.action.ReadValue<Vector2>();
        public bool GetFireButton => shipFireReference.action.ReadValue<float>() > 0;
    }
}