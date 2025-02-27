using UnityEngine;

namespace STM
{
    public class InputSystem : SingletonBase<InputSystem>
    {
        public bool Jump { get; private set; }
        public Vector2 MoveInput { get; private set; }
        public bool Interact { get; private set; }
        public bool UpArrow { get; private set; }
        public bool OpenInventory { get; private set; }
        public bool PickupItem { get; private set; }
        public bool NextContext { get; private set; }
        // Update is called once per frame
        void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");

            Jump = Input.GetKeyDown(KeyCode.Space);
            MoveInput = new Vector2(horizontal, 0);
            Interact = Input.GetKeyDown(KeyCode.F);
            UpArrow = Input.GetKeyDown(KeyCode.UpArrow);
            OpenInventory = Input.GetKeyDown(KeyCode.I);
            PickupItem = Input.GetKeyDown(KeyCode.Z);
            NextContext = Input.GetKeyDown(KeyCode.Return);

        }
    }
}